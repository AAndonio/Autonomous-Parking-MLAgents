using UnityEngine;

public class Data : MonoBehaviour
{
    public int NEpisode { get; set; }
    public int EpisodeSteps { get; set; }
    public float EpisodeLenght { get; set; }

    public bool Accident { get; set; }

    public int TotalNGoal { get; set; }
    public int NGoalNorth { get; set; }
    public int NGoalSouth { get; set; }

    public int TotalNCarGenerated { get; set; }
    public int NCarGeneratedNorth { get; set; }
    public int NCarGeneratedSouth { get; set; }

    public int TotalNCarBeenWaiting { get; set; }
    public int NCarBeenWaitingNorth { get; set; }
    public int NCarBeenWaitingSouth { get; set; }

    public float TotalAvgTimeToPass { get; set; }
    public float AvgTimeToPassNorth { get; set; }
    public float AvgTimeToPassSouth { get; set; }

    public float TotalAvgTimeWaited { get; set; }
    public float AvgTimeWaitedNorth { get; set; }
    public float AvgTimeWaitedSouth { get; set; }

    public void Reset()
    {
        NEpisode++;
        EpisodeSteps = 0;
        EpisodeLenght = 0f;

        Accident = false;

        TotalNGoal = 0;
        NGoalNorth = 0;
        NGoalSouth = 0;

        TotalNCarGenerated = 0;
        NCarGeneratedNorth = 0;
        NCarGeneratedSouth = 0;

        TotalNCarBeenWaiting = 0;
        NCarBeenWaitingNorth = 0;
        NCarBeenWaitingSouth = 0;

        TotalAvgTimeToPass = 0f;
        AvgTimeToPassNorth = 0f;
        AvgTimeToPassSouth = 0f;

        TotalAvgTimeWaited = 0f;
        AvgTimeWaitedNorth = 0f;
        AvgTimeWaitedSouth = 0f;
    }
}