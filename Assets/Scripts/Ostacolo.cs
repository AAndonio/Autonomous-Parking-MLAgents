using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ostacolo : MonoBehaviour
{
    private CarAgent agent;
    private bool endEpisode = true;
    private float penalty = -0.5f;

    public void Awake()
    {
        agent = transform.parent.parent.parent.GetComponentInChildren<CarAgent>();

    }

    public void Start()
    {
        if (this.gameObject.tag == "Auto")
        {
            penalty = -0.05f;
            endEpisode = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Ostacolo: contatto avvenuto");
            agent.getReward(penalty, endEpisode);
        }  
    }

}
