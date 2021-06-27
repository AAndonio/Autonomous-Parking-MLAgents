using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.MLAgents;
public class ParcheggioGenerale : MonoBehaviour {

    List<Parcheggio> parcheggi;
    int total_free_spots;

    [SerializeField] int totalFreeSpots;

    public void resetFreeSpots()
    {
        parcheggi = transform.GetComponentsInChildren<Parcheggio>().ToList();
        total_free_spots = (int) Academy.Instance.EnvironmentParameters.GetWithDefault("free_spots",totalFreeSpots);

        Debug.Log(total_free_spots);

         foreach (Parcheggio p in parcheggi){
            p.setFreeSpots(0);
        }

        int value;        
        while(total_free_spots > 0) {

            foreach (Parcheggio p in parcheggi){
                value = (int) (Random.Range(1,101) % 2) + 1;

                if(total_free_spots >= value){
                    if((p.getFreeSpots() + value) < 8){
                        p.setFreeSpots(p.getFreeSpots() + value);
                        total_free_spots = total_free_spots - value;
                    }
                }
            }
        }
    }

}
