using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Color activeColor = Color.green;
    bool activated = false;

    void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            RespawnManager rm = other.GetComponent<RespawnManager>();

            if (rm != null)
            {
                rm.SetRespawnPoint(transform);
                activated = true;
            }

            // 色変更（見た目）
            GetComponent<Renderer>().material.color = activeColor;
        }
    }
}
