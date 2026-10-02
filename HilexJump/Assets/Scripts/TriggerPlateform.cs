using System;
using System.Collections;
using UnityEngine;

public class TriggerPlateform : MonoBehaviour
{

    [SerializeField]
    GameObject plateform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.IncreaseScore();
            StartCoroutine(DestroyPlateform());
        }
    }

    //coroutine to destroy the platform after a certain time
    IEnumerator DestroyPlateform()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(plateform);
    }
    
}
