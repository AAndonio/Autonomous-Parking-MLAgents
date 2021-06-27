using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Random = UnityEngine.Random;

public class AgentCamera2 : Agent
{
    Rigidbody rbody;
    float tempo = 0.0f;
    bool trovato;
    public Transform target;
    ///private int direction = 0;
    Camera cam;

    public override void Initialize()
    {
        rbody = GetComponent<Rigidbody>();
        cam = GetComponent<Camera>();
        
    }

    public override void OnEpisodeBegin()
    {
        rbody.angularVelocity = Vector3.zero;
        //transform.localRotation = Quaternion.Euler(0, 180, 0);
        tempo = 0.0f;
        trovato = false;
        //zoom = false;
        target.localPosition = new Vector3(Random.value * 8 - 4, Random.value * 8 - 4, Random.value * 8 - 4);
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
        if (target.transform.position.y < 0)
        {
            EndEpisode();

        }

        rbody.constraints = RigidbodyConstraints.FreezeRotationZ;
        /*  var rotateDir = Vector3.zero;

          direction = Mathf.FloorToInt(vectorAction[0]);

          Debug.Log(direction);

           switch (direction)
           {
               case 0:

                   rotateDir = -transform.up;
                   break;
               case 1 :
                   rotateDir = transform.up;
                   break;
              case 2:
                      rotateDir = -transform.right;
                  break;
              case 3 :
                  rotateDir = transform.right;
                  break;


           }



            transform.Rotate(rotateDir, Time.fixedDeltaTime * 50);
          */



        //----------------------------SOLUZIONE OTTIMA------------------------------------------------//
        Vector3 controlSignal = Vector3.zero;
          controlSignal.y = vectorAction[0];
          controlSignal.x = vectorAction[1];

        transform.Rotate(controlSignal, Time.fixedDeltaTime * 50);
      

        //-----------------------------------------------------------------------------------------------//


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
          

            if (Vector3.Distance(this.transform.position, target.position) > 5f)
            {
               
                transform.LookAt(target);
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 10f, Time.deltaTime * 10f);

            }
            else if (Vector3.Distance(this.transform.position, target.position) <= 4.90f)
            {

               
                transform.LookAt(target);
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60f, Time.deltaTime * 60f);

            }

            if (tempo > 4)
            {
                Debug.Log("trovato="+trovato);
                cam.fieldOfView = 60f;
                EndEpisode();
            }


        }

        

        if (tempo > 5f && !trovato)
        {
            Debug.Log("troppo tempo "+tempo);
            tempo = 0f;
            AddReward(-0.02f);

        }


       /*if (this.transform.rotation.y < -0.60f || this.transform.rotation.y > 0.60f)
        {
            Debug.Log("Hai girato troppo..Penalita");
            AddReward(-0.02f);

        }



        Debug.Log("posizione" + rbody.transform.rotation.x);
       
     if(rbody.transform.rotation.x< -0.537103f || rbody.transform.rotation.x > 0.4707886f)
        {
            Debug.Log("if");
        }
      */

     
    }



    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");

        /*
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            actionsOut[0] = 0;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            actionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            actionsOut[0] = 2;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            actionsOut[0] = 3;
        }

        */
    }

}
