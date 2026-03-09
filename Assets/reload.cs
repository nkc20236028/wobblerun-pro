using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoBackToTitle : MonoBehaviour
{
    public float delay = 3f;

    void Start()
    {
        Invoke(nameof(BackToTitle), delay);
    }

    void BackToTitle()
    {
        SceneManager.LoadScene("title");
    }
}