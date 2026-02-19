using UnityEngine;
using UnityEngine.SceneManagement;

public class Goalpoint : MonoBehaviour
{
    public string goalSceneName = "goal scene";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ゴールポイントに到達");
            SceneManager.LoadScene(goalSceneName);
        }
    }
}
