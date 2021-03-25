using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System.Linq;


public class CarAgent : Agent
{

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private CarController carController;

    private Rigidbody carControllerRigidBody;

    [SerializeField] bool useVecObs;

    Parcheggio parcheggio1, parcheggio2;

    List<Parcheggio> parcheggi;

    float[] xCoordinates = new float[3]{0f,-14f,14f};
    float[] zCoordinates = new float[3]{0f,8.5f,-9.5f};

    public override void Initialize()
    {
        //salvo posizioni originali dell'agente (per il reset)
        originalPosition = this.transform.position;
        originalRotation = this.transform.rotation;

        //ottengo il controller per permettere il movimento dell'agente
        carController = GetComponent<CarController>();
        carControllerRigidBody = carController.GetComponent<Rigidbody>();

        //ottengo oggetti dei parcheggi
        parcheggi = transform.parent.GetComponentsInChildren<Parcheggio>().ToList();
    }

    public override void OnEpisodeBegin()
    {
        ResetArea();
    }

    // Update is called once per frame
    public override void CollectObservations(VectorSensor sensor)
    {
        if (useVecObs)
        {
            //sensor.AddObservation(carControllerRigidBody.position);
            sensor.AddObservation(carControllerRigidBody.rotation);
            sensor.AddObservation((float) System.Math.Round(carControllerRigidBody.velocity.magnitude,0)); 
        }
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        carController.verticalMovement = vectorAction[0];
        carController.horizontalMovement = vectorAction[1];
        //AddReward(-4f / MaxStep);
    }

    public void getReward(float rewardAmount, bool endEpisode)
    {
        AddReward(rewardAmount);
        if (endEpisode)
            EndEpisode();
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Vertical");
        actionsOut[1]= Input.GetAxis("Horizontal");

        /*
        if(Input.GetKey(KeyCode.Space))
            actionsOut[2] = 1;
        else
            actionsOut[2] = 0; */
    }

    private void ResetArea()
    {
        //Debug.Log("carAgent resetArea(): position: (" + originalPosition.x + " " + originalPosition.y + " " + originalPosition.z + ")");

        carControllerRigidBody.velocity = Vector3.zero;
        carControllerRigidBody.angularVelocity = Vector3.zero;
        this.transform.TransformPoint(Vector3.zero);

        Vector3 agentPosition = new Vector3(originalPosition.x+xCoordinates[Random.Range(0, xCoordinates.Length)], originalPosition.y, originalPosition.z+zCoordinates[Random.Range(0, xCoordinates.Length)]);

        transform.SetPositionAndRotation(agentPosition, originalRotation);

        foreach (Parcheggio p in parcheggi){
            p.resetParkingArea();
        }

    }

}
