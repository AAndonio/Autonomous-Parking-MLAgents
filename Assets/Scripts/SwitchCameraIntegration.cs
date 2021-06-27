using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCameraIntegration : MonoBehaviour
{
    [SerializeField] GameObject topCamera;
    [SerializeField] GameObject smartTrafficLightsCamera;
    [SerializeField] GameObject aiCarCamera;
    [SerializeField] GameObject parkingCamera;
    // Start is called before the first frame update
    void Start()
    {
        topCamera.SetActive(true);
        smartTrafficLightsCamera.SetActive(false);
        aiCarCamera.SetActive(false);
        parkingCamera.SetActive(false);   
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("z")){
            topCamera.SetActive(true);
            smartTrafficLightsCamera.SetActive(false);
            aiCarCamera.SetActive(false);
            parkingCamera.SetActive(false);
        }

        if(Input.GetKeyDown("x")){
            topCamera.SetActive(false);
            smartTrafficLightsCamera.SetActive(true);
            aiCarCamera.SetActive(false);
            parkingCamera.SetActive(false);
        }

        if(Input.GetKeyDown("c")){
            topCamera.SetActive(false);
            smartTrafficLightsCamera.SetActive(false);
            aiCarCamera.SetActive(true);
            parkingCamera.SetActive(false);
        }

        if(Input.GetKeyDown("v")){
            topCamera.SetActive(false);
            smartTrafficLightsCamera.SetActive(false);
            aiCarCamera.SetActive(false);
            parkingCamera.SetActive(true);
        }
    }
}