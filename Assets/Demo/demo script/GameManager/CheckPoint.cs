using UnityEngine;
using UnityEngine.WSA;

public class CheckPoint : MonoBehaviour
{
    Renderer rend;
    bool activated = false;

    void Start()
    {
        rend = GetComponentInChildren<Renderer>();

        if (rend == null)
        {
            Debug.LogError("Renderer not found on CheckPoint");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RespawnManager respawn = other.GetComponent<RespawnManager>();

            if (respawn != null)
            {

                respawn.SetRespawnPoint(transform.position);

                rend.material.color = Color.green;

                activated = true;

                Debug.Log("リスポーンポイントがアクティブ");
            }
        }

        
    }
}

