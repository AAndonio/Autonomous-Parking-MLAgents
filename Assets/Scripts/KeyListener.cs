using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KeyListener : MonoBehaviour
{
    [SerializeField] GameObject autoParcheggio;
    [SerializeField] GameObject aiCar;
    [SerializeField] GameObject smartTrafficCars;
    [SerializeField] Generator smartTrafficCarsNorthGenerator;
    [SerializeField] Generator smartTrafficCarsSouthGenerator;

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
            smartTrafficAIStatus = !smartTrafficAIStatus;
            smartTrafficCarsStatus = !smartTrafficCarsStatus;

            smartTrafficAI.SetActive(smartTrafficAIStatus);
            smartTrafficCars.SetActive(smartTrafficCarsStatus);


            if(smartTrafficAIStatus){
                smartTrafficCarsNorthGenerator.Reset();
                smartTrafficCarsSouthGenerator.Reset();
            }
        }
        
        if (Input.GetKeyDown("2")) {
            autoParcheggioStatus = !autoParcheggioStatus;
            autoParcheggio.SetActive(autoParcheggioStatus);

        }

        if (Input.GetKeyDown("3")) {
            aiCarStatus = !aiCarStatus;
            aiCar.SetActive(aiCarStatus);
        }
    }
}
