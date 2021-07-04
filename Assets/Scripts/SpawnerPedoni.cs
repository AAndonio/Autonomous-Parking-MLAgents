using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPedoni : MonoBehaviour
{
    public List<GameObject> pedoniPrefabs;
    //public GameObject pedonePrefab;
    public int pedoniDaSpawnare;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnPedoni());
    }

    IEnumerator SpawnPedoni()
    {
        int count = 0;
        while (count < pedoniDaSpawnare)
        {
            float num = Random.Range(0f, 1f);
            GameObject obj;
            if(num >= 0.9f)
                obj = Instantiate(pedoniPrefabs[1]);
            else
                obj = Instantiate(pedoniPrefabs[0]);

            Transform child = transform.GetChild(Random.Range(0, transform.childCount - 1));
            obj.GetComponent<WaypointNavigatorAICar>().currentWaypoint = child.GetComponent<WaypointAICar>();
            obj.transform.position = child.position;

            yield return new WaitForEndOfFrame();

            count++;
        }
    }
}
