using UnityEngine;

public class demoplayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpPower = 5f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 3f;

    CharacterController controller;
    Vector3 velocity;
    Vector3 startPosition; 

    void Start()
    {
        controller = GetComponent<CharacterController>();
        startPosition = transform.position; // スタート地点保存
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // マウス左右
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        // WASD移動
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // 接地
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // ジャンプ
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
            velocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);

        // 重力
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

   　　 // 床判定
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            controller.enabled = false;          // 一旦無効化
            transform.position = startPosition;  // スタートに戻す
            velocity = Vector3.zero;             // 落下リセット
            controller.enabled = true;           // 再有効化
        }
    }
}
