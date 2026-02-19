using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class DisappearFloor : MonoBehaviour
{
    bool triggered = false;

    Renderer[] rends;
    Collider[] cols;

    public float disappearDelay = 0.1f;
    public float respawnTime = 3f;

    public float shakeTime = 0.5f;
    public float shakePower = 0.05f;
    

    Vector3 originalPos;

    void Start()
    {
        originalPos = transform.parent.position;

        rends = transform.parent.GetComponentsInChildren<Renderer>();
        cols = transform.parent.GetComponentsInChildren<Collider>();

        if (shakeTime <= 0f)
        {
            shakeTime = 0.3f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(DisappearAndRespawn());
        }
    }

    IEnumerator DisappearAndRespawn()
    {

        float elapsed = 0f;

        while (elapsed < shakeTime)
        {
            Vector3 offset = Random.insideUnitSphere * shakePower;
            offset.y = 0;
            transform.parent.position = originalPos + offset;

            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        //Œ³‚ÌˆÊ’u‚É–ß‚·
        transform.parent.position = originalPos;

        yield return new WaitForSeconds(disappearDelay);

        // Á‚·
        foreach (Renderer r in rends) r.enabled = false;
        foreach (Collider c in cols) c.enabled = false;

        // •œŠˆ‘Ò‚¿
        yield return new WaitForSeconds(respawnTime);

        // •œŠˆ
        foreach (Renderer r in rends) r.enabled = true;
        foreach (Collider c in cols) c.enabled = true;

        triggered = false;
    }

    
}

