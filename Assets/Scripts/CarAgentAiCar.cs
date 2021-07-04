using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class CarAgentAICar : Agent
{
    private float DirHorizontal;
    private float DirVertical;
    private float steerAngle;
    public WheelCollider WheelCFR, WheelCFL, WheelCBR, WheelCBL;
    public Transform WheelTFR, WheelTFL, WheelTBR, WheelTBL;
    public float maxSteerAngle = 30;
    public float motorForce = 300;
    public float brakes = 0;

    private Rigidbody rBody;
    public GameObject checkpoint;

    public GameObject pedone;
    public WaypointAICar[] waypoints;
    private WaypointNavigatorAICar waypointNavigator;

    private float maxZdist;
    private float xDist, zDist;
    private bool xCheck, zCheck;
    private Vector3 toTarget;
    private bool manualBrake = false, pedoneIncontrato, pedoneSuperato;

    public bool isTraining;
    private float distanza = 3.5f;

    public GameObject redLight, greenLight;

    public override void Initialize()
    {
        Time.timeScale = 1;
        rBody = GetComponent<Rigidbody>();
        waypointNavigator = pedone.GetComponent<WaypointNavigatorAICar>();
        redLight.SetActive(false);
        greenLight.SetActive(true);
    }

    public override void OnEpisodeBegin()
    {
        ResetCar();
        ResetPedone();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //+3 Vector3(x, y, z)
        sensor.AddObservation(transform.localPosition);
        //+4 Quaternion(x, y, z , w)
        sensor.AddObservation(transform.localRotation);
        //+3 Vector3(x, y, z)
        sensor.AddObservation(rBody.velocity);
        //+3 Vector3(x, y, z)
        sensor.AddObservation(checkpoint.transform.localPosition);
        //+3 Vector3(x, y, z)
        sensor.AddObservation(pedone.transform.localPosition);
    }

    public override void Heuristic(float[] actionsOut)
    {
        DirHorizontal = Input.GetAxis("Horizontal");
        DirVertical = Input.GetAxis("Vertical");

        actionsOut[0] = DirHorizontal;
        actionsOut[1] = DirVertical;

        //ferma la macchina
        if (Input.GetKey(KeyCode.Space))
        {
            actionsOut[2] = 1f;
        }
        else
            actionsOut[2] = 0f;

    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // vectorAction[0] -> destra/sinistra
        // vectorAction[1] -> avanti/dietro
        // vectorAction[2] -> frena

        //Accelera
        if (vectorAction[1] == 0 || Input.GetKey(KeyCode.Space))
        {
            brakes = 15000;
        }
        else if (vectorAction[1] >= 0)
        {
            brakes = 0;
            WheelCFR.motorTorque = vectorAction[1] * motorForce;
            WheelCFL.motorTorque = vectorAction[1] * motorForce;
            WheelCBR.motorTorque = vectorAction[1] * motorForce;
            WheelCBL.motorTorque = vectorAction[1] * motorForce;
            AddReward(0.1f);
        }

        //Controllo se incrocia un pedone; true=l'ha incrociato, false=non l'ha incrociato
        pedoneIncontrato = CheckPedone();

        //Controllo se ha superato il pedone; true=l'ha superato, false=non l'ha superato
        pedoneSuperato = CheckPedoneSuperato();

        //Controllo se la macchina va all'indietro nel caso in cui non ha incrociato un pedone. Se sì, aggiungo un reward negativo
        if (vectorAction[1] < 0 && !pedoneIncontrato) AddReward(-5f);
        else AddReward(0.1f);

        //Controllo se la macchina va avanti nel caso in cui non ha incrociato un pedone. Se sì, aggiungo un reward positivo
        if (vectorAction[1] > 0 && !pedoneIncontrato) AddReward(10f);

        //Azioni da eseguire se si incrocia un pedone
        if (pedoneIncontrato)
        {
            //Se il pedone è sull'asse x vicino alla macchina e Se il pedone è sull'asse z AVANTI alla macchina allora la macchina deve fermarsi
            if ((pedone.transform.position.x - transform.position.x) >= -9.5f && (pedone.transform.position.x - transform.position.x) <= 4.6f && pedone.transform.position.z >= transform.position.z)
            {
                if (isTraining)
                {
                    vectorAction[2] = 1;
                    vectorAction[1] = 0;
                    //Debug.Log("Frenata prevista");
                    AddReward(10f);

                    motorForce = 0;
                    rBody.velocity = Vector3.zero;
                }

                if (vectorAction[1] == 0 || rBody.velocity.z == 0)
                {
                    //Debug.Log("Bravo!");
                    AddReward(100f);
                }
                else
                {
                    AddReward(-70f);
                }
            }

            //Se lo vedi ma non rischi di colpirlo
            else
            {
                vectorAction[2] = 0;
                motorForce = 500;
            }

            if (GetVelocitySpeed() <= 0.1f && GetVelocitySpeed() > -1)
            {
                //Debug.Log("Correct Speed");
                AddReward(maxZdist * 100f);
            }
            else
            {
                AddReward(-70f);
            }
        }
        else
        {
            motorForce = 500;
            if (GetVelocitySpeed() < 3) AddReward(-100f);
        }

        //Frena
        if (vectorAction[2] >= 0.5)
        {
            manualBrake = true;
            //vectorAction[1] = 0;
            Frena(1f);
        }
        else
        {
            manualBrake = false;
            brakes = 0;
        }

        //Se frena nella posizione sbagliata (lontano dal pedone)
        if (!pedoneIncontrato && manualBrake)
        {
            //Debug.Log("Non l'ha incontrato e ha frenato (ERRORE)");
            AddReward(-600f);
        }

        //Se frena nella posizione corretta (vicino al pedone)
        if (pedoneIncontrato && manualBrake)
        {
            //Debug.Log("L'ha incontrato e ha frenato (Giusto)");
            AddReward(100f);
        }


        //Controllo per freezare il pedone
        if (pedoneSuperato && (pedone.transform.position.x - transform.position.x) >= -4.8f && (pedone.transform.position.x - transform.position.x) <= 6.0f)
        {
            if (Vector3.Distance(transform.position, pedone.transform.position) >= 6.5)
            {
                pedone.GetComponent<Animator>().SetBool("isWalking", true);
                pedone.GetComponent<CharacterNavigationController>().movementSpeed = 2f;
                redLight.SetActive(false);
                greenLight.SetActive(true);
            }
            else
            {
                //Debug.Log("pedone riposizionato");
                pedone.GetComponent<Animator>().SetBool("isWalking", false);
                pedone.GetComponent<CharacterNavigationController>().movementSpeed = 0f;
                redLight.SetActive(true);
                greenLight.SetActive(false);
            }
        }
        else
        {
            //pedone.GetComponent<Animator>().enabled = true;
            pedone.GetComponent<Animator>().SetBool("isWalking", true);
            pedone.GetComponent<CharacterNavigationController>().movementSpeed = 2f;
        }

        //Se frena subito dopo aver superato il pedone non va bene perchè se sta troppo tempo ferma si scontrerebbe con il pedone che torna
        if (pedoneSuperato && manualBrake)
        {
            //Debug.Log("Superato ma non deve frenare");
            AddReward(-200f);
        }

        if (pedoneSuperato && !manualBrake)
        {
            if (GetVelocitySpeed() >= 3)
            {
                //Debug.Log("Pedone superato, non sta frenando e ha velocità corretta");
                AddReward(100f);
            }
            else
            {
                //Debug.Log("Pedone superato, non sta frenando ma è lenta");
                AddReward(-80f);
            }
        }
    }


    // --------- CONTROLLI MACCHINA ---------

    public void steerControl()
    {
        steerAngle = maxSteerAngle * DirHorizontal;
        WheelCFR.steerAngle = steerAngle;
        WheelCFL.steerAngle = steerAngle;
    }

    public void wheelPositionControl()
    {
        wheelPositionControls(WheelCFR, WheelTFR);
        wheelPositionControls(WheelCFL, WheelTFL);
        wheelPositionControls(WheelCBR, WheelTBR);
        wheelPositionControls(WheelCBL, WheelTBL);
    }

    public void wheelPositionControls(WheelCollider collide, Transform trans)
    {
        Vector3 wheelPosition = trans.position;
        Quaternion wheelQuant = trans.rotation;

        collide.GetWorldPose(out wheelPosition, out wheelQuant);

        trans.position = wheelPosition;
        trans.rotation = wheelQuant;
    }

    private bool CheckPedone()
    {

        maxZdist = 0;
        bool pedOnTrajectory = false;
        toTarget = (pedone.transform.position - transform.position).normalized;

        //if (Vector3.Dot(toTarget, transform.forward) > 0)
        if (transform.position.z + distanza < pedone.transform.position.z)
        {
            xDist = (pedone.transform.position.x - transform.position.x) + 10;
            zDist = Mathf.Abs(pedone.transform.position.z - transform.position.z);

            xCheck = (-2 < xDist) && (xDist < 14);
            zCheck = (0 < zDist) && (zDist < 10);

            if (xCheck && zCheck)
            {
                //Debug.Log("pedastrian " + pedone.name + " is in a risk point for " + gameObject.name + " .");
                Debug.DrawLine(transform.position, pedone.transform.position, Color.blue);
            }

            if ((xCheck && zCheck) && !pedOnTrajectory)
            {
                if (zDist > maxZdist) maxZdist = zDist;
                pedOnTrajectory = true;
            }
        }

        return pedOnTrajectory;
    }

    private bool CheckPedoneSuperato()
    {
        bool pedSuperato = false;

        //Se True vuol dire che ho superato il pedone
        if (transform.position.z + distanza >= pedone.transform.position.z)
        {
            //Debug.DrawLine(transform.position, pedone.transform.position, Color.red);
            Debug.DrawLine(new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), new Vector3(pedone.transform.position.x, pedone.transform.position.y + 2f, pedone.transform.position.z), Color.red);
            pedSuperato = true;
        }
        return pedSuperato;
    }

    protected float GetVelocitySpeed()
    {
        //Debug.Log("velocity: " + transform.InverseTransformDirection(rBody.velocity).z);
        return transform.InverseTransformDirection(rBody.velocity).z;
    }

    protected void Frena(float decrement)
    {
        WheelCFR.brakeTorque = brakes;
        WheelCFL.brakeTorque = brakes;
        WheelCBR.brakeTorque = brakes;
        WheelCBL.brakeTorque = brakes;
        DiminuisciVelocita(1f, decrement);
    }

    protected void DiminuisciVelocita(float toCheck, float decrement)
    {
        if ((rBody.velocity.x > toCheck || rBody.velocity.x < toCheck * (-1)) || (rBody.velocity.y > toCheck || rBody.velocity.y < toCheck * (-1))
            || (rBody.velocity.z > toCheck || rBody.velocity.z < toCheck * (-1)))
        {
            if (rBody.velocity.x > toCheck)
                rBody.velocity = new Vector3(rBody.velocity.x - decrement, rBody.velocity.y, rBody.velocity.z);
            if (rBody.velocity.x < toCheck * (-1))
                rBody.velocity = new Vector3(rBody.velocity.x + decrement, rBody.velocity.y, rBody.velocity.z);
            if (rBody.velocity.y > toCheck)
                rBody.velocity = new Vector3(rBody.velocity.x, rBody.velocity.y - decrement, rBody.velocity.z);
            if (rBody.velocity.y < toCheck * (-1))
                rBody.velocity = new Vector3(rBody.velocity.x, rBody.velocity.y + decrement, rBody.velocity.z);
            if (rBody.velocity.z > toCheck)
                rBody.velocity = new Vector3(rBody.velocity.x, rBody.velocity.y, rBody.velocity.z - decrement);
            if (rBody.velocity.z < toCheck * (-1))
                rBody.velocity = new Vector3(rBody.velocity.x, rBody.velocity.y, rBody.velocity.z + decrement);
        }
    }

    // --------- FINE CONTROLLI MACCHINA ---------

    public void FixedUpdate()
    {
        steerControl();
        wheelPositionControl();
        debug_drawDestination();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Checkpoint")
        {
            AddReward(100.0f);
            EndEpisode();
        }

        if (other.tag == "People")
        {
            AddReward(-1000f);
            EndEpisode();
        }

        if (other.tag == "Wall")
        {
            AddReward(-0.1f);
            EndEpisode();
        }

        if (other.tag == "Strisce")
        {
            AddReward(0.1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Strisce")
        {
            AddReward(100f);
        }
    }

    private void ResetCar()
    {
        transform.localPosition = new Vector3(2.72f, 0.28f, 0f);
        transform.rotation = Quaternion.identity;
        rBody.velocity = Vector3.zero;
        rBody.angularVelocity = Vector3.zero;
    }

    private void ResetPedone()
    {
        waypointNavigator.currentWaypoint = waypoints[Random.Range(0, waypoints.Length - 1)];
        pedone.transform.position = waypointNavigator.currentWaypoint.transform.position;
    }

    protected void debug_drawDestination()
    {
        Debug.DrawLine(transform.position, checkpoint.transform.position, Color.yellow);
    }
}