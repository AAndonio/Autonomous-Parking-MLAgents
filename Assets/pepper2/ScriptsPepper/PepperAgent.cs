using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System;

public class PepperAgent : Agent
{
    public GameObject endEffector;
    public GameObject bottle;
    public GameObject robot;
    public GameObject mano;


    RobotController robotController;
    TouchDetector touchDetector;
    BottleRandomPosition tablePositionRandomizer;


    void Start()
    {
        robotController = robot.GetComponent<RobotController>();
        touchDetector = bottle.GetComponent<TouchDetector>();
        tablePositionRandomizer = bottle.GetComponent<BottleRandomPosition>();
    }


    // AGENT

    public override void OnEpisodeBegin()
    {
        float[] defaultRotations = { 0.0f, 0.0f, 0.0f };
        robotController.ForceJointsToRotations(defaultRotations);
        touchDetector.hasTouchedTarget = false;
        tablePositionRandomizer.Move();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (robotController.joints[0].robotPart == null)
        {
            // No robot is present, no observation should be added
            return;
        }
        // current rotations
        float[] rotations = robotController.GetCurrentJointRotations();
        foreach (float rotation in rotations)
        {
            // normalize rotation to [-1, 1] range
            float normalizedRotation = (rotation / 360.0f) % 1f;
            sensor.AddObservation(normalizedRotation);
        }

        foreach (var joint in robotController.joints)
        {
            sensor.AddObservation(joint.robotPart.transform.position - robot.transform.position);
            sensor.AddObservation(joint.robotPart.transform.forward);
            sensor.AddObservation(joint.robotPart.transform.right);
        }

        // relative bottle position
        Vector3 bottPosition = bottle.transform.position - robot.transform.position;
        sensor.AddObservation(bottPosition);

        // relative end position
        Vector3 endPosition = endEffector.transform.position - robot.transform.position;
        sensor.AddObservation(endPosition);
        sensor.AddObservation(bottPosition - endPosition);

    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // move
        for (int jointIndex = 0; jointIndex < vectorAction.Length; jointIndex++)
        {
            RotationDirection rotationDirection = ActionIndexToRotationDirection((int)vectorAction[jointIndex]);
            robotController.RotateJoint(jointIndex, rotationDirection, false);
        }

        // end episode if we touched the cube
        if (touchDetector.hasTouchedTarget)
        {
            SetReward(1f);
            EndEpisode();
        }


        //reward
        float distanceToBottle = Vector3.Distance(endEffector.transform.position, bottle.transform.position); // roughly 0.7f
       
        float distanceFromHandToBott = Vector3.Distance(mano.transform.position, bottle.transform.position); // roughly 0.7f

        var palmMoreDistant = 0f;

        if (distanceFromHandToBott < distanceToBottle)
        {
            palmMoreDistant = -1.0f;
        }
        else
        {
            palmMoreDistant = +1.0f;
        }


        var reward = -distanceToBottle + palmMoreDistant / 100f;

        SetReward(reward * 0.1f);

    }


    // HELPERS

    static public RotationDirection ActionIndexToRotationDirection(int actionIndex)
    {
        return (RotationDirection)(actionIndex - 1);
    }
}
