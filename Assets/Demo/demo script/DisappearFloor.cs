using UnityEngine;
using System.Collections;

public class DisappearFloor : MonoBehaviour
{
    Collider col;
    Renderer rend;
    bool triggered = false;

    void Start()
    {
        col = GetComponent<Collider>();
        rend = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(Disappear());
        }
    }

    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(1f);

        col.enabled = false;
        rend.enabled = false;
    }
}
