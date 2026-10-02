using UnityEngine;

public class Plateform : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * 4f, ForceMode.Impulse);

            GameManager.Instance.ResetCombo();
        }
    }
}
