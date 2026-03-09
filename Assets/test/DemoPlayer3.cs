using UnityEngine;

public class DemoPlayer3 : MonoBehaviour
{
    // 重力
    public float gravity = -9.81f;

    // ノックバック用
    public float knockbackPower = 10f;
    public float knockUpPower = 4f;

    // 落下リスポーン
    public float fally = -10f;

    CharacterController controller;
    Vector3 velocity;

    // 動く床
    MoveGround currentGround;

    // リスポーン
    RespawnManager respawn;

    // 操作制御
    bool canControl = true;

    // ノックバック中フラグ（★重要）
    bool isKnockback = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        respawn = GetComponent<RespawnManager>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!canControl) return;

        // ===== 動く床の移動量 =====
        Vector3 groundDelta = Vector3.zero;
        if (currentGround != null)
        {
            groundDelta = currentGround.GetDelta();
        }

        // ===== 接地処理（ノックバック中は無効）=====
        if (controller.isGrounded && velocity.y < 0 && !isKnockback)
        {
            velocity.y = -2f;   // 地面に吸着
            velocity.x = 0f;
            velocity.z = 0f;
        }

        // ===== 重力（接地中かつ上昇してない時はかけない）=====
        if (!controller.isGrounded || velocity.y > 0)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // ===== 移動 =====
        Vector3 totalMove = velocity * Time.deltaTime + groundDelta;
        controller.Move(totalMove);

        // ===== ノックバック終了判定 =====
        if (controller.isGrounded && velocity.y < 0)
        {
            isKnockback = false;
        }

        // ===== 接地中の床取得 =====
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

        // ===== 落下リスポーン =====
        if (transform.position.y < fally)
        {
            respawn.Respawn();
            velocity = Vector3.zero;
            isKnockback = false;
        }
    }

    // ===== 吹っ飛び棒判定 =====
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Vector3 dir = (transform.position - other.transform.position).normalized;
            dir.y = 0f;

            velocity = dir * knockbackPower;
            velocity.y = knockUpPower;

            isKnockback = true;
        }
    }

    public void DisableControl()
    {
        canControl = false;
        velocity = Vector3.zero;
    }
}