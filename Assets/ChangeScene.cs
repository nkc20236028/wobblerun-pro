using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScene : MonoBehaviour
{
   public void ChangeingScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
