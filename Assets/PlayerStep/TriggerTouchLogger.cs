using UnityEngine;

public class TriggerTouchLogger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[TriggerEnter] name={other.name}, tag={other.tag}, layer={other.gameObject.layer}");
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log($"[TriggerStay] name={other.name}");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"[TriggerExit] name={other.name}");
    }
}