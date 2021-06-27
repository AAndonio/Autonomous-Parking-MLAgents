using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CameraSwitch : MonoBehaviour
{
    public Camera[] cameras;
    public TMP_Text ResumeText;
    public TMP_Text CurrentCameraText;
    public TMP_Text PauseText;

    private int currentCameraIndex;

    public bool isRunning;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckPause();
    }


    private void CheckPause()
    {
        if (isRunning)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

}