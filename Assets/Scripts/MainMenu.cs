using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private const int DefaultSouthValue = 30;
    private const int DefaultNorthValue = 30;
    
    [SerializeField] private TMP_InputField totalMaxCarSpawn;

    [SerializeField] private TMP_InputField northGeneratorMaxCarSpawn;

    [SerializeField] private TMP_InputField southGeneratorMaxCarSpawn;

    private void Start()
    {
        southGeneratorMaxCarSpawn.onValueChanged.AddListener(delegate { UpdateTotalMaxCarSpawnValue();  });
        northGeneratorMaxCarSpawn.onValueChanged.AddListener(delegate { UpdateTotalMaxCarSpawnValue();  });
    }

    public void PlayGame()
    {

        if (!totalMaxCarSpawn.text.Equals(""))
            SkipMenuData.TotalMaxCarSpawn = Int32.Parse(totalMaxCarSpawn.text);
        else
            SkipMenuData.TotalMaxCarSpawn = DefaultNorthValue + DefaultSouthValue;

        if (!northGeneratorMaxCarSpawn.text.Equals(""))
            SkipMenuData.NorthGeneratorMaxCarSpawn = Int32.Parse(northGeneratorMaxCarSpawn.text);
        else
            SkipMenuData.NorthGeneratorMaxCarSpawn = DefaultNorthValue;

        if (!southGeneratorMaxCarSpawn.text.Equals(""))
            SkipMenuData.SouthGeneratorMaxCarSpawn = Int32.Parse(southGeneratorMaxCarSpawn.text);
        else
            SkipMenuData.SouthGeneratorMaxCarSpawn = DefaultSouthValue;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void UpdateTotalMaxCarSpawnValue()
    {
        int southValue, northValue, totalValue;

        if (southGeneratorMaxCarSpawn.text.Equals(""))
            southValue = DefaultSouthValue;
        else
            southValue = Int32.Parse(southGeneratorMaxCarSpawn.text);

        if (northGeneratorMaxCarSpawn.text.Equals(""))
            northValue = DefaultNorthValue;
        else
            northValue = Int32.Parse(northGeneratorMaxCarSpawn.text);

        totalValue = southValue + northValue;
        
        totalMaxCarSpawn.text = "";
        totalMaxCarSpawn.text = totalValue.ToString();
    }
}