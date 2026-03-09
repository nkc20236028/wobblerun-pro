using UnityEngine;

public class playercontroller : MonoBehaviour
{
    // 重力
    public float gravity = -9.81f;

    CharacterController controller;
    Vector3 velocity;

    // 吹っ飛び用
    public float knockbackPower = 10f;
    public float knockUpPower = 4f;

    // 動く床
    MoveGround currentGround;

    // リスポーン
    //RespawnManager respawn;
    // public float fally = -10f;

    bool canControl = true;

    void Start()
    {
       // controller = GetComponent<CharacterController>();
       // respawn = GetComponent<RespawnManager>();
    }

    void Update()
    {
        if (!canControl) return;

        Vector3 groundDelta = Vector3.zero;
        if (currentGround != null)
        {
            groundDelta = currentGround.GetDelta();
        }

        // 接地処理
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // 重力
        velocity.y += gravity * Time.deltaTime;

        // 移動（重力＋床移動）
        Vector3 totalMove = velocity + groundDelta / Time.deltaTime;
        controller.Move(totalMove * Time.deltaTime);

        // 横方向の慣性を抑える
        if (controller.isGrounded)
        {
            velocity.x = 0f;
            velocity.z = 0f;
        }

        // 床取得
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

        // 落下リスポーン
        //if (transform.position.y < fally)
        //{
        //    respawn.Respawn();
        //    velocity = Vector3.zero;
        //}
    }

    // 吹っ飛び
    //void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    Debug.Log("衝突");

    //    if (hit.collider.CompareTag("Obstacle"))
    //    {
    //        Debug.Log("吹き飛ばし");
    //        Vector3 dir = (transform.position - hit.transform.position).normalized;
    //        dir.y = 0f;

    //        velocity = dir * knockbackPower;
    //        velocity.y = knockUpPower;
    //    }
    //}

    public void DisableControl()
    {
        canControl = false;
        velocity = Vector3.zero;
    }
}