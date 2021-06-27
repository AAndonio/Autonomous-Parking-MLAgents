using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Values on UI")] [SerializeField]
    private Data data;

    [SerializeField] public TextMeshProUGUI nEpisodeValue;
    [SerializeField] public TextMeshProUGUI nAccidentValue;

    [SerializeField] private TextMeshProUGUI totalNCarGeneratedValue;
    [SerializeField] public TextMeshProUGUI maxTotalNCarGeneratedValue;

    [SerializeField] private TextMeshProUGUI nCarGeneratedNorthValue;
    [SerializeField] public TextMeshProUGUI maxNCarGeneratedNorthValue;

    [SerializeField] private TextMeshProUGUI nCarGeneratedSouthValue;
    [SerializeField] public TextMeshProUGUI maxNCarGeneratedSouthValue;

    [SerializeField] private TextMeshProUGUI totalNGoalValue;
    [SerializeField] private TextMeshProUGUI nGoalNorthValue;
    [SerializeField] private TextMeshProUGUI nGoalSouthValue;

    [SerializeField] private TextMeshProUGUI totalNCarBeenWaitingValue;
    [SerializeField] private TextMeshProUGUI nCarBeenWaitingNorthValue;
    [SerializeField] private TextMeshProUGUI nCarBeenWaitingSouthValue;

    [SerializeField] public TextMeshProUGUI totalAvgTimeToPassValue;
    [SerializeField] public TextMeshProUGUI avgTimeToPassNorthValue;
    [SerializeField] public TextMeshProUGUI avgTimeToPassSouthValue;

    [SerializeField] public TextMeshProUGUI totalAvgTimeWaitedValue;
    [SerializeField] public TextMeshProUGUI avgTimeWaitedNorthValue;
    [SerializeField] public TextMeshProUGUI avgTimeWaitedSouthValue;

    [SerializeField] public Image northSemaphoreValue;
    [SerializeField] public Image southSemaphoreValue;

    [SerializeField] public TextMeshProUGUI reportCreatedPopUpValue;

    [Header("Component on UI")] [SerializeField]
    public GameObject menuHint;

    [SerializeField] public GameObject reportInfo;
    [SerializeField] public GameObject episodeInfo;
    [SerializeField] public GameObject lastEpisodeInfo;
    [SerializeField] public GameObject semaphoresIndicator;
    [SerializeField] public GameObject mainCameraIndicator;
    [SerializeField] public GameObject reportCreatedPopUp;

    private bool _isVisible;

    private void Awake()
    {
        nEpisodeValue.text = "0";
        nAccidentValue.text = "0";
        totalAvgTimeToPassValue.text = "0";
        avgTimeToPassNorthValue.text = "0";
        avgTimeToPassSouthValue.text = "0";
        totalAvgTimeWaitedValue.text = "0";
        avgTimeWaitedNorthValue.text = "0";
        avgTimeWaitedSouthValue.text = "0";
        northSemaphoreValue.color = Color.red;
        southSemaphoreValue.color = Color.red;

        _isVisible = false;

        menuHint.SetActive(true);
        reportInfo.SetActive(false);
        episodeInfo.SetActive(false);
        lastEpisodeInfo.SetActive(false);
        semaphoresIndicator.SetActive(false);
        mainCameraIndicator.SetActive(false);
        reportCreatedPopUp.SetActive(false);

        EpisodeReset();
    }

    public void EpisodeReset()
    {
        totalNCarGeneratedValue.text = "0";
        nCarGeneratedNorthValue.text = "0";
        nCarGeneratedSouthValue.text = "0";

        totalNGoalValue.text = "0";
        nGoalNorthValue.text = "0";
        nGoalSouthValue.text = "0";

        totalNCarBeenWaitingValue.text = "0";
        nCarBeenWaitingNorthValue.text = "0";
        nCarBeenWaitingSouthValue.text = "0";
    }

    private void Update()
    {
        // updating UI...
        totalNCarGeneratedValue.text = data.TotalNCarGenerated.ToString();
        nCarGeneratedNorthValue.text = data.NCarGeneratedNorth.ToString();
        nCarGeneratedSouthValue.text = data.NCarGeneratedSouth.ToString();

        totalNGoalValue.text = data.TotalNGoal.ToString();
        nGoalNorthValue.text = data.NGoalNorth.ToString();
        nGoalSouthValue.text = data.NGoalSouth.ToString();

        totalNCarBeenWaitingValue.text = data.TotalNCarBeenWaiting.ToString();
        nCarBeenWaitingNorthValue.text = data.NCarBeenWaitingNorth.ToString();
        nCarBeenWaitingSouthValue.text = data.NCarBeenWaitingSouth.ToString();

        if (Input.GetKeyDown(KeyCode.Space))
            ShowsUI();
        
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void ShowsUI()
    {
        if (!_isVisible)
        {
            menuHint.SetActive(false);
            reportInfo.SetActive(true);
            episodeInfo.SetActive(true);
            lastEpisodeInfo.SetActive(true);
            semaphoresIndicator.SetActive(true);

            _isVisible = true;
        }
        else
        {
            menuHint.SetActive(true);
            reportInfo.SetActive(false);
            episodeInfo.SetActive(false);
            lastEpisodeInfo.SetActive(false);
            semaphoresIndicator.SetActive(false);

            _isVisible = false;
        }
    }
}