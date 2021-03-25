using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetroPostoLibero: MonoBehaviour
{
    // Start is called before the first frame update


    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            transform.parent.GetComponent<PostoLibero>().backHitted = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            transform.parent.GetComponent<PostoLibero>().backHitted = false;
        }
    }
}
