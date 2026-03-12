using UnityEngine;

public class ShowImage : MonoBehaviour
{
    public GameObject imageUI;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            imageUI.SetActive(true);
        }
    }

    void Update()
    {
        if (imageUI.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            imageUI.SetActive(false);
        }
    }
}