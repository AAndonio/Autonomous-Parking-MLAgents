using UnityEngine;
using Random = UnityEngine.Random;

public class CarMover : MonoBehaviour
{
    private const float CarStoppedSpeed = 0f;
    private const string CarLayerName = "CarLayer";

    private int _carLayer;

    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float minSpeed = 10f;

    [SerializeField] private Transform viewSensor;

    public bool IsMoving { get; set; }
    public bool IsLastOne { get; set; }
    private float Speed { get; set; }

    public float TimeToPass { get; set; }
    public float TimeWaited { get; set; }

    public Vector3 Direction { get; set; }

    public CriticalZone RedZone { get; set; }
    public SemaphoreController Semaphore { get; set; }

    void Awake()
    {
        TimeToPass = 0f;
        TimeWaited = 0f;

        IsLastOne = false;

        _carLayer = LayerMask.GetMask(CarLayerName);

        Throttle(null); // First start
    }

    private void FixedUpdate()
    {
        ObserveInFrontCar();

        transform.Translate(Direction * (Time.deltaTime * Speed), Space.World);
    }

    private void Update()
    {
        TimeToPass += Time.deltaTime;
    }

    public void ObserveTrafficLight(SemaphoreController semaphore)
    {
        if (!semaphore.IsGreen)
            Stop();

        else if (!IsMoving)
        {
            Throttle(null);
            ObserveInFrontCar();
        }
    }

    private void ObserveInFrontCar()
    {
        //Debug.DrawRay(viewSensor.position, viewSensor.forward * 4.5f);

        if (Physics.Raycast(viewSensor.position, viewSensor.forward, out var inFrontRay, 4.5f, _carLayer))
        {
            if (inFrontRay.collider.GetComponent<CarMover>())
            {
                CarMover inFrontCar = inFrontRay.collider.GetComponent<CarMover>();
            
                if (!inFrontCar.IsMoving)
                    Stop();

                else
                    Throttle(inFrontCar.Speed);
            }
        }
    }

    private void Throttle(float? speed)
    {
        Speed = speed ?? Random.Range(minSpeed, maxSpeed);
        IsMoving = true;
    }

    private void Stop()
    {
        Speed = CarStoppedSpeed;
        IsMoving = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<CarMover>() != null && RedZone != null)
        {
            Debug.Log("incidente");
            RedZone.Accident = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<CarRilevator>() && !IsMoving)
            TimeWaited += Time.deltaTime;
    }
}