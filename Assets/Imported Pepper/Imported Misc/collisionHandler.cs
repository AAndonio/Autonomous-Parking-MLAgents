using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class collisionHandler : MonoBehaviour
{
    public Animator anim;


    float tempo = 0.0F;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        tempo = tempo + Time.deltaTime;
        if (tempo>3.1F) 
            anim.SetBool("opened", false);

    }

    private void OnTriggerEnter(Collider other)
    {
      //  anim.SetBool("opened", false);
    }

    //When the Primitive exits the collision, it will change Color
    private void OnTriggerExit(Collider other)
    {
        anim.SetBool("opened", true);

    }


  


}
