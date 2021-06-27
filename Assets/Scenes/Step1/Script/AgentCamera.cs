using System;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Random = UnityEngine.Random;

public class AgentCamera : Agent
{
    Rigidbody rbody;
    float tempo = 0.0f;
    bool trovato;
   // bool zoom;
    public Transform target;
   // float RotateSpeed = 300; // con addTourque era 2
    public float forceMultiplier = 1f;
    Camera cam;
    float initialFOV;

    public override void Initialize()
    {
        rbody = GetComponent<Rigidbody>();
        cam = GetComponent<Camera>();
        initialFOV = cam.fieldOfView;
    }

    public override void OnEpisodeBegin()
    {
        rbody.angularVelocity = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0, 180, 0);
        tempo = 0.0f;
        trovato = false;
        //zoom = false;
        target.localPosition = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    }


    public override void CollectObservations(VectorSensor sensor)
    {
     
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(transform.localRotation);
        sensor.AddObservation(rbody.velocity.y);
        sensor.AddObservation(trovato);
 
    }


    public override void OnActionReceived(float[] vectorAction)
    {
        var rotateDir = Vector3.zero;
        var rotateAxis = Mathf.FloorToInt(vectorAction[0]);
        var zoomCam = Mathf.FloorToInt(vectorAction[1]);
        var zoomON = cam.fieldOfView;
        

        switch (rotateAxis)
        {
            case 0:

                rotateDir = -transform.up;
                break;
            case -1:

                rotateDir = transform.up;
                break;
        }

/*        switch (zoomCam)
        {
            case 0:
              
                zoomON += 4f;
                break;
            case -1:
                
                zoomON -= 4f;
                break;
        }
        //initialFOV = Mathf.Lerp(zoomON, 10f, Time.deltaTime * 10f);

        */

        //rbody.AddTorque(rotateDir * RotateSpeed, ForceMode.VelocityChange);
        transform.Rotate(rotateDir, Time.fixedDeltaTime * 50);

        tempo += Time.deltaTime;

        if (!trovato)
        {
            var myTrasform = transform;
            var rayDir = 10.0f * myTrasform.forward;
            Debug.DrawRay(transform.position, rayDir, Color.red, 0.5f, true);
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, 1f, rayDir, out hit, 10f))
            {
                if (hit.collider.CompareTag("Target"))
                {
                    Debug.Log("Trovato");
                    trovato = true;
                    AddReward(1f);

                }
            }
        }

        if (trovato)
        {
            var distanza = Vector3.Distance(this.transform.position, target.position);

            if (Vector3.Distance(this.transform.position, target.position) > 5f)
            {
                Debug.Log(distanza+"dist1");
                transform.LookAt(target);
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 10f, Time.deltaTime * 10f);

            }
            else if (Vector3.Distance(this.transform.position, target.position) <= 4.90f)
            {
                
                Debug.Log("if dentro distance 2_ " + distanza);
                transform.LookAt(target);
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60f, Time.deltaTime * 60f);

            }

            if (tempo > 4)
            {
                cam.fieldOfView = 60f;
                EndEpisode();
            }


        }



        if ( tempo > 5.0f)
        {
          
            AddReward(-0.05f);
 
        }
        if (this.transform.eulerAngles.y < 98 || this.transform.eulerAngles.y > 248)
        {
            Debug.Log("Hai girato troppo..Penalita");
            AddReward(-0.5f);
            
        }

    }

    

    public override void Heuristic(float[] actionsOut)
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            actionsOut[0] = 0;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            actionsOut[0] = -1;
        }





    }




}
