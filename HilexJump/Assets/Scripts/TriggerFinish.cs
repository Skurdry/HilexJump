using UnityEngine;
using UnityEngine.Events;

public class TriggerFinish : MonoBehaviour
{

    public static UnityEvent onFinishTriggered;

    private void Awake()
    {
        if (onFinishTriggered == null)
        {
            onFinishTriggered = new UnityEvent();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Finish Triggered!");   
            onFinishTriggered?.Invoke();

            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
