using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontePostoLibero: MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            transform.parent.GetComponent<PostoLibero>().frontHitted = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            transform.parent.GetComponent<PostoLibero>().frontHitted = false;
        }
    }
}
