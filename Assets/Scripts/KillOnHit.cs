using UnityEngine;

public class KillOnHit : MonoBehaviour
{
    [SerializeField] private Generator generatorAssociated;

    private void OnTriggerEnter(Collider other)
    {
        CarMover mover = other.GetComponent<CarMover>();
        
        if (mover != null)
        {
            if (mover.IsLastOne)
                generatorAssociated.OnMaxCarGenerated();
            
            Destroy(mover.gameObject);
        }
    }
}