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
        currentCameraIndex = 0;
        CurrentCameraText.text = "Current camera: " + (currentCameraIndex + 1);

        //Turn all cameras off, except the first default one
        for (int i = 1; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        if (cameras.Length > 0)
        {
            cameras[0].gameObject.SetActive(true);
        }
        ResumeText.gameObject.SetActive(true);
        PauseText.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            currentCameraIndex++;
            if (currentCameraIndex < cameras.Length)
            {
                cameras[currentCameraIndex - 1].gameObject.SetActive(false);
                cameras[currentCameraIndex].gameObject.SetActive(true);
            }
            else
            {
                cameras[currentCameraIndex - 1].gameObject.SetActive(false);
                currentCameraIndex = 0;
                cameras[currentCameraIndex].gameObject.SetActive(true);
            }
            CurrentCameraText.text = "Current camera: " + (currentCameraIndex + 1);
        }

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

        if (Input.GetKeyDown(KeyCode.V))
        {
            if (isRunning)
            {
                isRunning = false;
                ResumeText.gameObject.SetActive(true);
                PauseText.gameObject.SetActive(false);
            }
            else
            {
                ResumeText.gameObject.SetActive(false);
                PauseText.gameObject.SetActive(true);
                isRunning = true;
            }
        }
    }

}