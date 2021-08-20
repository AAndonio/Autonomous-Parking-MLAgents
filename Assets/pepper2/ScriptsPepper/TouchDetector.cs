using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDetector : MonoBehaviour
{
    public GameObject bott;
    public string touchTargetTag;
    RobotController robotController;
    public bool hasTouchedTarget = false;
    public GameObject robot;
    BottleRandomPosition randomPos;
    float[] defaultRotations = { 0.0f, 0.0f, 0.0f};
        

    public void Start()
    {

        robotController = robot.GetComponent<RobotController>();
        randomPos = bott.GetComponent<BottleRandomPosition>();
        randomPos.Move();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PincherPalm") || true)
        {
            Debug.Log("Touch Detected!");
            hasTouchedTarget = true;
            randomPos.Move();
            robotController.ForceJointsToRotations(defaultRotations);
            
        }
    }
}
