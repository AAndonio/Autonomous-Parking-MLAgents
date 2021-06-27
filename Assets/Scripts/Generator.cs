using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generator : MonoBehaviour
{
    private const string NorthGenerator = "north";
    private const string SouthGenerator = "south";

    private bool _hasStopped;
    private int _carsCounter;
    private int maxCarSpawn;

    [SerializeField] private float minSpawnIntervalInSeconds;
    [SerializeField] private float maxSpawnIntervalInSeconds;

    [SerializeField] private List<GameObject> spawnableObjects = new List<GameObject>();

    [SerializeField] private AgentManager manager;
    [SerializeField] private Data data;
    [SerializeField] private UIController uiController;

    private IEnumerator Spawn()
    {
        if (_carsCounter < maxCarSpawn)
        {
            var spawned = Instantiate(GetRandomSpawnableFromList(), transform.position, transform.rotation, transform);

            _carsCounter++;

            CarMover mover = spawned.GetComponent<CarMover>();
            mover.Direction = transform.forward;
            
            if (_carsCounter == maxCarSpawn)
                mover.IsLastOne = true;

            yield return new WaitForSeconds(Random.Range(minSpawnIntervalInSeconds, maxSpawnIntervalInSeconds));
            StartCoroutine();
        }
    }

    private GameObject GetRandomSpawnableFromList()
    {
        int randomIndex = Random.Range(0, spawnableObjects.Count);
        return spawnableObjects[randomIndex];
    }

    private void StopCoroutine()
    {
        StopCoroutine(nameof(Spawn));
    }

    private void StartCoroutine()
    {
        StartCoroutine(nameof(Spawn));
    }

    public void Reset()
    {
        StopCoroutine();

        _carsCounter = 0;

        StartCoroutine();
    }
    
    private void OnTriggerStay(Collider other)
    {
        CarMover thisCar = other.GetComponent<CarMover>();

        if (thisCar != null)
            if (!thisCar.IsMoving && !_hasStopped)
            {
                StopCoroutine();
                _hasStopped = true;
            }
    }

    private void OnTriggerExit(Collider other)
    {
        CarMover thisCar = other.GetComponent<CarMover>();

        if (thisCar != null)
            if (thisCar.IsMoving && _hasStopped)
            {
                StartCoroutine();
                _hasStopped = false;
            }
    }

    public void GettingSettingsFromMenu()
    {
        if (gameObject.tag.Equals(NorthGenerator))
        {
            if (SkipMenuData.NorthGeneratorMaxCarSpawn != 0)
                maxCarSpawn = SkipMenuData.NorthGeneratorMaxCarSpawn;
            else
                maxCarSpawn = 60;

                //uiController.maxNCarGeneratedNorthValue.text = maxCarSpawn.ToString();
        }
        
        if (gameObject.tag.Equals(SouthGenerator))
        {
            if (SkipMenuData.SouthGeneratorMaxCarSpawn != 0)
                maxCarSpawn = SkipMenuData.SouthGeneratorMaxCarSpawn;
            else
                maxCarSpawn = 60;
            
            //uiController.maxNCarGeneratedSouthValue.text = maxCarSpawn.ToString();
        }
    }

    public void OnMaxCarGenerated()
    {
        manager.OnEnd(generator: true);
    }
}