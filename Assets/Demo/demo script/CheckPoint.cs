using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    bool activated = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DemoPlayer player = other.GetComponent<DemoPlayer>();
            if (player != null)
            {
               // player.SetRespawnPoint(transform.position);
                Debug.Log("セーブポイント更新");
                GetComponent<Renderer>().material.color = Color.green;
            }
        }

        if (activated) return;
        activated = true;
    }
}
