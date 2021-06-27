using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class PepperAgent : Agent
{
    Rigidbody rb;
    float currentSteering;
    public WheelCollider ruotaDestra;
    public WheelCollider ruotaSinistra;
    public WheelCollider ruotaPosteriore;

    public Transform wheelDestra;
    public Transform wheelSinistra;
    public Transform wheelPosteriore;
    public float speed;

    public int maxSteps;

    private int currentSteps;

    public GameObject target;

    RaycastHit raycast;

    Vector3 startingPosition;

    Vector3 startingDistance;

    Vector3 currentDistance;

    public Transform directionArrow;

    public EnvScript env;

    float multi;
    public float brake;

    private EnvironmentParameters param;
    private  float maxSpeed;
    private float currentSpeed;
    
    //inizializzazione delle varabili
    public override void Initialize(){

        //variabili component
        param = Academy.Instance.EnvironmentParameters;
        rb = GetComponent<Rigidbody>();

        //variabili normali
        startingPosition = gameObject.transform.localPosition;
        startingDistance = new Vector3(Mathf.Abs(target.transform.localPosition.x - gameObject.transform.localPosition.x), 0, Mathf.Abs(target.transform.localPosition.z - gameObject.transform.localPosition.z));
        currentDistance = startingDistance;
        currentSteps = 0;
        ruotaPosteriore.brakeTorque = 0;
        ruotaSinistra.brakeTorque = 0;
        ruotaDestra.brakeTorque = 0;
        currentSpeed = rb.velocity.magnitude * 3.6f;
        maxSpeed = 10;

    }

    //osservazioni vettoriali dell'agente
    public override void CollectObservations(VectorSensor sensor)
    {
        //velocità di movimento (su ogni asse)
        sensor.AddObservation(rb.velocity);

        //distanza dal target
        sensor.AddObservation(Vector3.Distance(target.transform.position, transform.position));

        //direzione di pepper rispetto al target
        sensor.AddObservation((target.transform.position - transform.position).normalized);
        
        //direzione delle ruota posteriore
        sensor.AddObservation(wheelPosteriore.forward);

        //direzione di pepper
        sensor.AddObservation(transform.forward);

        //velocità di rotazione di pepper (sull'asse y)
        sensor.AddObservation(rb.angularVelocity.y);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        //output della rete neurale
        var motor = vectorAction[0];
        var steering = vectorAction[1];

        //una "freccia" posta sopra all'agente che punta sempre verso il target
        //usata sotto nelle ricompense
        directionArrow.LookAt(target.transform);

        //codice che gestisce il movimento
        //se il valore di motor è > 0 si ha un accellerazione, altrimenti si ha una frenata 
        if (motor > 0)
        {
            ruotaPosteriore.brakeTorque = 0;
            ruotaSinistra.brakeTorque = 0;
            ruotaDestra.brakeTorque = 0;

            ruotaPosteriore.motorTorque = motor * speed;
            ruotaSinistra.motorTorque = motor * speed;
            ruotaDestra.motorTorque = motor * speed;
        }
        else{
            ruotaPosteriore.brakeTorque = brake * speed/2;
            ruotaSinistra.brakeTorque = brake * speed/2;
            ruotaDestra.brakeTorque = brake * speed/2;
        }
        
        //codice che gestisce la rotazione

        //controllo che ci assicura che la rotazione sia graduale e non oltre un certo grado
         if (Mathf.Abs(currentSteering + steering) <= 90 && Mathf.Abs(steering) <= 1){
            currentSteering += steering;
        }
        //la ruota posteriore ruota in maniera inversa a quelle anteriori
        ruotaPosteriore.steerAngle = -currentSteering;
        //controllo che limita le due ruote anteriori e che le blocca ad un massimo di 30° di rotazione
        if (currentSteering > 30)
        {
            ruotaSinistra.steerAngle = 30;
            ruotaDestra.steerAngle = 30;
        }
        else if (currentSteering < -30){
            ruotaSinistra.steerAngle = -30;
            ruotaDestra.steerAngle = -30;
        }
        else
        {
            ruotaSinistra.steerAngle = currentSteering;
            ruotaDestra.steerAngle = currentSteering;
        }

        //ricompense

        //piccola ricompensa se le ruote sono allineate
        if (Mathf.Abs(currentSteering) <= 10){
            AddReward(1f/maxSteps);
        }

        //piccola ricompensa se l'agente guarda verso il target (con 10° di errore) o una penalità nel caso opposto 
        if  (Mathf.Abs(directionArrow.eulerAngles.y - transform.eulerAngles.y) % 360 <= 5 ||  Mathf.Abs(directionArrow.eulerAngles.y - transform.eulerAngles.y) % 360 >= 355) {
            AddReward(0.1f / maxSteps);
        }
        else{
            AddReward(-0.01f / maxSteps);
        }

        //piccola ricompensa se l'agente si è avvicinato al target rispetto allo scorso step, penalità nel caso opposto
        if (Mathf.Abs(target.transform.localPosition.x - gameObject.transform.localPosition.x) < currentDistance.x || Mathf.Abs(target.transform.localPosition.z - gameObject.transform.localPosition.z) < currentDistance.z) {
            AddReward(0.1f / maxSteps);
            currentDistance = new Vector3(Mathf.Abs(target.transform.localPosition.x - gameObject.transform.localPosition.x), startingDistance.y, Mathf.Abs(target.transform.localPosition.z - gameObject.transform.localPosition.z));
        }
        else {
            AddReward(-0.1f / maxSteps);
        }

        //termina la sessione se abbiamo raggiunto il numero massimo di step
        if (currentSteps >= maxSteps) {
            EndEpisode();
        }
        
        //conta che un nuovo step è stato eseguito
        currentSteps++;
    }

    //inizializza una nuova sessione
    public override void OnEpisodeBegin()
    {
        //parametri del curriculum learning
        multi = param.GetWithDefault("multi", 5f);
        env.setStep((int) param.GetWithDefault("position", 1), (int) param.GetWithDefault("obstacles", 3));
        maxSteps = (int) param.GetWithDefault("steps", 5000);
        env.setRadius(param.GetWithDefault("radius", 4));

        //variabili
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        gameObject.transform.localPosition = startingPosition;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        currentSteering = 0;
        currentSteps = 0;
        currentDistance = startingDistance;
    }

    //controlli manuali dell'agente
    public override void Heuristic(float[] actionsOut)
    {
        //freccia superiore per accellerare, freccia inferiore per frenare  
        actionsOut[0] = Input.GetAxis("Vertical");
        //fraccia sinistra e destra per girare le ruote
        actionsOut[1] = Input.GetAxis("Horizontal");

        //si possono usare in modo analogo i tasti "wasd"
    }

    //funzione chiamata quando viene rilevata una collisione
    private void OnCollisionEnter(Collision collision)
    {
        //se è una collisione col target
        if (collision.transform.CompareTag("target"))
        {
            //grande ricompensa per aver raggiunto il target
            AddReward(10f * multi);

            //il curriculum learning stabilisce se bisogna iniziare una nuova sessione oppure se bisogna cambiare la posizione di target/ostacoli
            if (param.GetWithDefault("position", 1) != 1){
                EndEpisode();
            }
            else{
                env.setStep( (int) param.GetWithDefault("position", 1), 0);
            }
        }

        //se è una collisione con un ostacolo
        if (collision.transform.CompareTag("object"))
        {
            //grande penalità alla collisione con un ostacolo
            AddReward(-10f);
        }
    }
}
