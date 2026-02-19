using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    Vector3 startPoint;
    Vector3 respawnPoint;
    bool hasCheckpoint = false;

    public float respawnHeightOfset = 1.2f;

    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        //ゲーム開始位置を保存
        startPoint = transform.position;
    }

    //チェックポイントを踏んだとき
    public void SetRespawnPoint(Vector3 point)
    {
        respawnPoint = point;
        hasCheckpoint = true;

        Debug.Log("リスポーンポイントを更新" + point);
    }

    //落下時
    public void Respawn()
    {
        controller.enabled = false;

        Vector3 pos;

        if (hasCheckpoint)
        {
            pos = respawnPoint;
        }
        else
        {
            pos = startPoint;
        }

        pos.y += respawnHeightOfset;

        transform.position = pos;

        controller.enabled = true;
    }
}
