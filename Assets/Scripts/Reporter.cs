using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Reporter : MonoBehaviour
{
    private int _id;
    private string _extension = ".csv";
    
    private string _path = @".\Assets\Reports\report_";

    [SerializeField] private Data data;
    [SerializeField] private UIController uiController;

    private void DOSearchIDEmpty()
    {
        int n = 1;

        while (File.Exists(_path + n + _extension))
            n++;

        _id = n;
    }

    public void CreateReport()
    {
        DOSearchIDEmpty();

        string path = _path + _id + _extension;
        string comma = ",";

        FileStream fs = File.Create(path);
        {
            Byte[] info =
                new UTF8Encoding(true).GetBytes(
                    "NEpisode" + comma + "EpisodeLenght" + comma + "EpisodeSteps" + comma + "Accident" + comma +
                    "TotalNGoal" + comma + "NGoalNorth" + comma + "NGoalSouth" + comma + "TotalNCarGenerated" + comma +
                    "NCarGeneratedNorth" + comma +
                    "NCarGeneratedSouth" + comma + "TotalNCarBeenWaiting" + comma + "NCarBeenWaitingNorth" + comma +
                    "NCarBeenWaitingSouth" +
                    comma + "TotalAVGTimeToPass" + comma + "AVGTimeToPassNorth" + comma + "AVGTimeToPassSouth" + comma +
                    "TotalAVGTimeWaited" + comma + "AVGTimeWaitedNorth" + comma + "AVGTimeWaitedSouth" + "\n");
            fs.Write(info, 0, info.Length);

            Debug.Log("File has been created");
        }
        fs.Close();
    }

    public void WriteData()
    {
        string comma = ",";
        string path = _path + _id + _extension;

        using (FileStream fs = File.Open(path, FileMode.Append, FileAccess.Write, FileShare.None))
        {
            Byte[] info = new UTF8Encoding(true).GetBytes(data.NEpisode + comma +
                                                          data.EpisodeLenght.ToString("0.##") +
                                                          comma + data.EpisodeSteps + comma +
                                                          data.Accident + comma + data.TotalNGoal + comma +
                                                          data.NGoalNorth + comma + data.NGoalSouth + comma +
                                                          data.TotalNCarGenerated + comma + data.NCarGeneratedNorth +
                                                          comma + data.NCarGeneratedSouth + comma +
                                                          data.TotalNCarBeenWaiting + comma +
                                                          data.NCarBeenWaitingNorth + comma +
                                                          data.NCarBeenWaitingSouth + comma +
                                                          data.TotalAvgTimeToPass.ToString("0.##") + comma +
                                                          data.AvgTimeToPassNorth.ToString("0.##") + comma +
                                                          data.AvgTimeToPassSouth.ToString("0.##") + comma +
                                                          data.TotalAvgTimeWaited.ToString("0.##") + comma +
                                                          data.AvgTimeWaitedNorth.ToString("0.##") + comma +
                                                          data.AvgTimeWaitedSouth.ToString("0.##") + "\n");
            fs.Write(info, 0, info.Length);
            fs.Close();

            StartCoroutine(ShowReportCreatedPopUp());
            Debug.Log("File wrote");
        }

        createGraph();
    }

    private void createGraph()
    {
        Process otherProcess = new Process();
        otherProcess.StartInfo.FileName = Directory.GetCurrentDirectory() + @"\venv\Scripts\python";
        otherProcess.StartInfo.Arguments =
            string.Format("{0} {1}", Directory.GetCurrentDirectory() + @"\Assets\Scripts\plottingCSV.py", _path + _id + _extension);
        otherProcess.StartInfo.CreateNoWindow = true;
        otherProcess.StartInfo.UseShellExecute = false;

        otherProcess.StartInfo.RedirectStandardInput = true;
        otherProcess.StartInfo.RedirectStandardOutput = true;

        otherProcess.Start();

        otherProcess.StandardOutput.ReadToEnd();
    }

    private IEnumerator ShowReportCreatedPopUp()
    {
        uiController.reportCreatedPopUpValue.text = "Report Created in " + _path + _id + _extension;
        uiController.reportCreatedPopUp.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        uiController.reportCreatedPopUp.SetActive(false);
        uiController.reportCreatedPopUpValue.text = "";
    }
}