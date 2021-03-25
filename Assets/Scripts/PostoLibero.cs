using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class PostoLibero : MonoBehaviour
{
    CarAgent agent;
    BoxCollider carSpotCollider, agentCollider;
    Rigidbody agentRigidbody;
    public bool frontHitted = false, backHitted = false;
    private bool firstTimeHit = true;

    // Start is called before the first frame update
    void Start()
    {
        agent = transform.parent.parent.parent.GetComponentInChildren<CarAgent>();
        agentCollider = agent.gameObject.GetComponent<BoxCollider>();
        agentRigidbody = agent.gameObject.GetComponent<Rigidbody>();
        carSpotCollider = this.GetComponent<BoxCollider>();
    }

    
    private void FixedUpdate()
    {


        if (frontHitted)
        {
            if (firstTimeHit)
            {
                agent.getReward(0.1f, false);
                firstTimeHit = false;
            }
            if (backHitted)
            {

                float angle = Quaternion.Angle(transform.parent.rotation, agent.transform.rotation);
                if (angle < 10f)
                {
                    Debug.Log("Parcheggio corretto!");
                    agent.getReward(1f, true);
                }
            }
        }
          
    }

    /*
    void OnTriggerStay(Collider other)
    {

        if (carSpotCollider.bounds.Contains(agentCollider.bounds.min) && carSpotCollider.bounds.Contains(agentCollider.bounds.max))
        {
            if (Mathf.Abs(agentRigidbody.velocity.x) < .1)
            {
                Debug.Log("Parcheggio corretto!");
                agent.GetComponent<CarAgent>().getReward(0.1f, true);
            }
        }
    }*/
}
