using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// classe per salvare posizione e verso dell'auto parcheggiata
public class parkedCar
{
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
}


public class Parcheggio : MonoBehaviour {

    [SerializeField]
    private GameObject freeSpotObject = null;

    private int numberOfCarsToHide = 0;

    private Dictionary<int, parkedCar> parkedCarsOriginal = new Dictionary<int, parkedCar>();

    private Component[] cars;

    private List<GameObject> freeSpots = new List<GameObject>();

    public void Awake()
    {
        cars = transform.GetComponentsInChildren<Transform>().Where(c => c.gameObject.tag == "Auto").ToArray();  
    
        // cache all car positions and rotations
        foreach (Component car in cars)
        {
               parkedCarsOriginal.Add(car.GetInstanceID(),
               new parkedCar
               {
                   Position = car.transform.position,
                   Rotation = car.transform.rotation
               });
        }
    }

    public int getFreeSpots(){
        return numberOfCarsToHide;
    }

    public void setFreeSpots(int freeSpots){
        numberOfCarsToHide = freeSpots;
    }

    private int[] setCarsToHide(int numbersOfCarsToHide)
    {
        int[] carsToHide = new int[numbersOfCarsToHide];
        int numberToHide;
        int carKey;
        for(int i = 0; i < numbersOfCarsToHide; i++)
        {
            do
            {
                //Debug.Log("setCarsToHide: numero auto " + parkedCarsOriginal.Count);
                numberToHide = Random.Range(1, parkedCarsOriginal.Count);
                //Debug.Log("setCarsToHide: numero auto da nascondere generato: " + numberToHide);
                carKey = parkedCarsOriginal.ElementAt(numberToHide).Key;
            } while (carsToHide.Contains(carKey));

            carsToHide[i] = carKey;
        }

        return carsToHide;
    }

    public void resetParkingArea()
    {

        //Distruggo posti liberi generati precedentemente
        foreach (GameObject freeSpot in freeSpots)
        {
            Destroy(freeSpot);
        }

        //variabile contenente id auto da nascondere
        int[] carsToHide = setCarsToHide(numberOfCarsToHide);

        //ripristino posizione auto
        foreach (Component car in cars)
        {
            var originalCar = parkedCarsOriginal[car.GetInstanceID()];

            car.GetComponent<Rigidbody>().velocity = Vector3.zero;
            car.transform.SetPositionAndRotation(originalCar.Position, originalCar.Rotation);

            if (carsToHide.Contains(car.GetInstanceID()))
            {
                car.gameObject.SetActive(false);

                GameObject freeSpot = Instantiate(freeSpotObject, Vector3.zero, Quaternion.identity) as GameObject;
               
                freeSpot.transform.position = new Vector3(car.gameObject.transform.position.x, 0.157f, car.gameObject.transform.position.z);
                freeSpot.transform.parent = car.transform.parent;

                freeSpot.transform.localRotation = Quaternion.identity;

                freeSpots.Add(freeSpot);

            }
            else
            {
                car.gameObject.SetActive(true);
            }
        }
    }

}
