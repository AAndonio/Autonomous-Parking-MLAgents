using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private Transform nextWayPoint;

    private void OnTriggerEnter(Collider other)
    {
        CarMover thisCar = other.GetComponentInParent<CarMover>();

        if (thisCar.tag.Equals(gameObject.tag))
        {
            var directionToNextWaypoint = nextWayPoint.position - transform.position;

            directionToNextWaypoint.y = 0f; // To prevent car fly away...

            thisCar.Direction = directionToNextWaypoint.normalized;
            thisCar.transform.LookAt(nextWayPoint.position);
        }
    }
}
