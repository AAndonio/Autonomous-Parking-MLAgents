using UnityEngine;

public class NoticePoint : MonoBehaviour
{
    private const string CarStopper = "Observer";

    private SemaphoreController _semaphore;

    private void Awake()
    {
        _semaphore = GetComponentInParent<SemaphoreController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other != null && other.name.Equals(CarStopper))
            other.GetComponentInParent<CarMover>().ObserveTrafficLight(_semaphore);
    }
}