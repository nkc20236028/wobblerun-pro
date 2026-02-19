using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public Transform currentRespawnPoint;

    public void Respawn()
    {
        if (currentRespawnPoint == null)
        {
            Debug.LogError("Re");
            return;
        }
    }
}
