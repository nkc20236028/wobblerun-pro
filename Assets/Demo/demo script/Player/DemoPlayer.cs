using UnityEngine;

public class DemoPlayer : MonoBehaviour
{
   //プレイヤー初期設定
    //public float moveSpeed = 5f;
    //public float jumpPower = 5f;
    public float gravity = -9.81f;
    //public float mouseSensitivity = 3f;

    CharacterController controller;
    Vector3 velocity;
   
    //吹っ飛び用　変数
    public float knockbackPower = 10f;
    public float knockUpPower = 4f;

    //動く床用変数
    MoveGround currentGround;

    //リスポーン用変数
    RespawnManager respawn;
    public float fally = -10f;

    bool canControl = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;

       controller = GetComponent<CharacterController>();
        respawn = GetComponent<RespawnManager>();
    }

    void Update()
    {
        if (!canControl) return;

        // マウス左右
        //float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
        //transform.Rotate(Vector3.up * mouseX);

       // WASD移動
       //float x = Input.GetAxis("Horizontal");
       //float z = Input.GetAxis("Vertical");
       //Vector3 move = transform.right * x + transform.forward * z;
       //controller.Move(move * moveSpeed * Time.deltaTime);

        //動く床　床の移動量を取得
        Vector3 groundDelta = Vector3.zero;
        if (currentGround != null)
        {
            groundDelta = currentGround.GetDelta();
        }

        // 接地
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // ジャンプ
        //if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        //    velocity.y = Mathf.Sqrt(jumpPower * -2f * gravity);

        // 重力
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //動く床　移動
        //Vector3 totalMove = move * moveSpeed + velocity + groundDelta / Time.deltaTime;
        //controller.Move(totalMove * Time.deltaTime);

        //接地確認
        Debug.Log(controller.isGrounded);

        //横方向の慣性を抑制
        if (controller.isGrounded)
        {
            if (velocity.y < 0)
            {
                velocity.y = -2f;
                velocity.x = 0f;
                velocity.z = 0f;
            }
        }

        //接地中に床を取得
        if (controller.isGrounded)
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 1.2f))
            {
                currentGround = hit.collider.GetComponent<MoveGround>();
            }
            else
            {
                currentGround = null;
            }
        }
        else
        {
            currentGround = null;
        }

        //落下処理
        if (transform.position.y < fally)
        {
            respawn.Respawn();
            velocity = Vector3.zero;
        }
    }

   　　 // 床判定＆吹っ飛び棒
    void OnTriggerEnter(Collider other)
    { 
          //吹っ飛び棒用
        if (other.CompareTag("Obstacle"))
        {
            //障害物　プレイヤー　方向
            Vector3 dir = (transform.position - other.transform.position).normalized;

            //横方向をメインに
            dir.y = 0f;

            //吹っ飛び
            velocity = dir * knockbackPower;
            velocity.y = knockUpPower;

        }
    }

    public void DisableControl()
    {
        canControl = false;
        velocity = Vector3.zero;
    }

}
