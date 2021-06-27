using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvScript : MonoBehaviour
{
    float rangeX;
    float rangeY;
    public bool show = false;
    public GameObject target;
    private int counter;
    private float x;
    private float y;
    SphereCollider coll;
    int targetNumber;
    int rand;
    public GameObject ostacle1Prefab;
    public GameObject ostacle2Prefab;

    GameObject[] obstacles;
    
    //inizializzazione dell'ambiente
    void Start()
    {
        //estraggo il collider dal target
        coll = target.GetComponent<SphereCollider>();
        //variabili usate per definire un range
        rangeX = 0;
        rangeY = 0;

        //istanzia una lista di ostacoli di diverso tipo
        obstacles = new GameObject[10];

        for(int i = 0; i < 10; i++){

            if (i%2 == 0){
                obstacles[i] = Instantiate(ostacle1Prefab, new Vector3(0, -20, 0), Quaternion.identity);
            }
            else {
                obstacles[i] = Instantiate(ostacle2Prefab, new Vector3(0, -20, 0), Quaternion.identity);
            }

            obstacles[i].transform.parent = gameObject.transform;
        }

    }

    
    //modifica l'ambiente in base ai paremetri step (fase) e obstacle (numero di ostacoli)
    public void setStep(int step, int obstacle){

        //se il flag pubblico è attivo, la modifica dell'ambiente è ignorata con dei paramatri fissi
        //un ambiente "statico" permette di applicare modifiche manuali durante l'esecuzione per osservare l'agente durante la fase di inferenza
        if (show){
            step = -1;
            obstacle = 0;
        }

        //chiama la funzione che gestisce gli ostacoli
        setOstacoli(obstacle);
        
        //gestisce il target in base allo step
        //la scelta è essenzialmente tra il non toccare la posizione prefissata del target e spostarlo in una posizione random
        switch (step){
            case 0:
                break;
            case 1:
                sceltaRandom();
                break;
            default:
                break;
        }
    }

     //individua una posizione random limitata all'ambiente e non troppo vicina ad un ostacolo o all'agente
    private void sceltaRandom(){
        bool flag = true;
       
        while (flag){
            rangeX = UnityEngine.Random.Range(-45, 45);
            rangeY = UnityEngine.Random.Range(-45, 45);
            float positionX;
            float positionY;
            flag = false;

            //controllo: il target deve essere lontano dall'agente
            if (Mathf.Abs(rangeX) > 10 || Mathf.Abs(rangeY) > 10){
                //controllo: il target deve essere lontano dagli ostacoli
                foreach (GameObject ob in obstacles)
                {
                    positionX = ob.transform.localPosition.x;
                    positionY = ob.transform.localPosition.z;

                    if (Mathf.Abs(rangeX - positionX) <  5 && Mathf.Abs(rangeY - positionY) < 5){
                        flag = true;
                    }
                }
            }
        }
        
        Vector3 rVector = new Vector3(rangeX, 0, rangeY);

        target.transform.localPosition = rVector;
    }

    //distribuisce gli ostacoli in posizioni random
    //non possono essere troppo vicini all'agente
    //sono presenti due cicli per distribuire meglio gli ostacoli sull'ambiente (ogni ciclo instanzia su metà dell'ambiente) 
    private void setOstacoli(int obstacle){
        float X;
        float Y;

        //primo ciclo
        for (int i = 0; i < obstacle; i++){
            while (true){
                X = UnityEngine.Random.Range(0, 40);
                Y = UnityEngine.Random.Range(-45, 45);
                if (Mathf.Abs(X) > 10 || Mathf.Abs(Y) > 10){
                    if (Mathf.Abs(X - rangeX) > 2 || Mathf.Abs(Y - rangeY) > 2){
                       break;
                    }
                }
            }

            obstacles[i].transform.localPosition = new Vector3(X, 0, Y);
        }

        //secondo ciclo
        for (int i = 0; i < obstacle; i++){
            while (true){
                X = UnityEngine.Random.Range(-40, 0);
                Y = UnityEngine.Random.Range(-45, 45);
                if (Mathf.Abs(X) > 10 || Mathf.Abs(Y) > 10){
                    if (Mathf.Abs(X - rangeX) > 2 || Mathf.Abs(Y - rangeY) > 2){
                       break;
                    }
                }
            }

            obstacles[i + obstacle].transform.localPosition = new Vector3(X, 0, Y);
        }
    }

    //stabilisce il raggio del target
    public void setRadius(float radius){
        coll.radius = radius;
    }
}
