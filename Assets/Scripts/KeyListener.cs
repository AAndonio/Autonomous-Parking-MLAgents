using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeyListener : MonoBehaviour
{
    [SerializeField] GameObject autoParcheggio;
    [SerializeField] GameObject aiCar;
    [SerializeField] GameObject smartTrafficCars;
    [SerializeField] GameObject smartTrafficAI;

    private bool autoParcheggioStatus = true;
    private bool aiCarStatus = true;
    private bool smartTrafficCarsStatus = true;
    private bool smartTrafficAIStatus = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            smartTrafficAI.SetActive(!smartTrafficAIStatus);
            smartTrafficCars.SetActive(!smartTrafficCarsStatus);
            smartTrafficCarsStatus = !smartTrafficCarsStatus;
            smartTrafficAIStatus = !smartTrafficAIStatus;
        } 
        
        if (Input.GetKeyDown("2")) {
            autoParcheggio.SetActive(!autoParcheggioStatus);
            autoParcheggioStatus = !autoParcheggioStatus;
        }

        if (Input.GetKeyDown("3")) {
            aiCar.SetActive(!aiCarStatus);
            aiCarStatus = !aiCarStatus;
        }
    }
}
