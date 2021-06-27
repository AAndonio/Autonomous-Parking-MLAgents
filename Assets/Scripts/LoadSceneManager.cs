using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{

    public TMP_Text testo;

    public void LoadMainScene()
    {
        SceneManager.LoadScene(1);
    }

    public void sottolinea()
    {
        testo.fontStyle = FontStyles.Underline;
    }

    public void NonSottolineare()
    {
        testo.fontStyle ^= FontStyles.Underline;
    }
}
