using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goalpoint : MonoBehaviour
{
    public string goalSceneName = "goal scene";
    public GameObject gameClearUI;
    public float delay = 2f;

    bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
       triggered = true;

        //プレイヤー操作停止
        DemoPlayer player = other.GetComponent<DemoPlayer>();
        if (player != null)
        {
            player.DisableControl();
        }

        //クリアの表示
        if(gameClearUI != null)
        {
            gameClearUI.SetActive(true);
        }

        //シーン切り替え
        StartCoroutine(ChangeGoalSc());
    }

    IEnumerator ChangeGoalSc()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(goalSceneName);
    }
}
