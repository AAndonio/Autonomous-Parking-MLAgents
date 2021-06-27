using UnityEngine;

public class CriticalZone : MonoBehaviour
{
    private const string NorthGenerated = "north";
    private const string SouthGenerated = "south";

    private AgentManager _manager;

    [SerializeField] private Data data;
    [SerializeField] private UIController uiController;

    public bool Accident { get; set; }
    public int TotalCarPassed { get; set; }

    void Awake()
    {
        Reset();

        _manager = GetComponentInParent<AgentManager>();
    }

    public void Reset()
    {
        TotalCarPassed = 0;
        Accident = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        CarMover mover = other.GetComponent<CarMover>();

        if (mover != null)
        {
            mover.RedZone = this;
            mover.Semaphore.CarsFromThisSemaphore++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CarMover mover = other.GetComponent<CarMover>();

        if (mover != null)
        {
            TotalCarPassed++;
            mover.Semaphore.CarsFromThisSemaphore--;

            _manager.OnGoal();
        }
    }
}