using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class CarController2 : Agent
{
    Rigidbody rb;
    public WheelCollider rightWheel;
    public WheelCollider leftWheel;
    public float speed;
    
    public override void Initialize(){
        rb = GetComponent<Rigidbody>();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(rb.velocity);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        var motor = vectorAction[0];
        var steering = vectorAction[1];

        leftWheel.steerAngle = steering * speed;
        rightWheel.steerAngle = steering * speed ; 
        leftWheel.motorTorque = -motor * speed;
        rightWheel.motorTorque = -motor * speed;
    }
    public override void OnEpisodeBegin()
    {
        
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Vertical");
        actionsOut[1] = Input.GetAxis("Horizontal");
    }
}
