using UnityEngine;
using UnityEngine.Serialization;

public class CarRilevator : MonoBehaviour
{
    private const string NorthGenerated = "north";
    private const string SouthGenerated = "south";

    private SemaphoreController _semaphore;

    [SerializeField] private Data data;

    private void Awake()
    {
        _semaphore = GetComponentInParent<SemaphoreController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        CarMover mover = other.GetComponent<CarMover>();

        if (mover != null)
        {
            _semaphore.CarsOnRilevator++;
            _semaphore.TotalCarRilevated++;

            mover.Semaphore = _semaphore;
            mover.tag = _semaphore.gameObject.tag;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CarMover mover = other.GetComponent<CarMover>();

        if (mover != null)
        {
            _semaphore.CarsOnRilevator--;
        }
    }
}