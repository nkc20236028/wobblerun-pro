using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class BabyStepsPhysics2 : MonoBehaviour
{
    [Header("関節")]
    public ConfigurableJoint leftHip, leftKnee, leftFoot;
    public ConfigurableJoint rightHip, rightKnee, rightFoot;
    public ConfigurableJoint spineJoint;

    [Header("足")]
    public float adjustSpeed = 120f;
    public float kneeMaxBend = 70f;

    float lTargetX, lTargetZ;
    float rTargetX, rTargetZ;

    [Header("姿勢")]
    public float uprightForce = 150000f;
    public float uprightDamper = 30000f;
    public float sideLeanAngle = 15f;
    public float forwardLeanWeight = 10f;

    [Header("物理")]
    public float gravity = -30f;
    public float groundCheckDist = 0.8f;

    [Header("ノックバック")]
    public float knockbackPower = 8f;
    public float knockUpPower = 4f;

    [Header("落下")]
    public float fally = -10f;

    Rigidbody rb;
    bool isGrounded;
    bool isKnockback;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        bool L = Mouse.current.leftButton.isPressed;
        bool R = Mouse.current.rightButton.isPressed;
        bool W = Keyboard.current.wKey.isPressed;
        bool A = Keyboard.current.aKey.isPressed;
        bool D = Keyboard.current.dKey.isPressed;

        UpdateTarget(ref lTargetX, ref lTargetZ, L, W, A, D);
        UpdateTarget(ref rTargetX, ref rTargetZ, R, W, A, D);

        if (spineJoint) spineJoint.targetRotation = Quaternion.identity;

        // 落下スポーン
        //if (transform.position.y < fally)
        //{
        //    rb.linearVelocity = Vector3.zero;
        //    rb.angularVelocity = Vector3.zero;
        //    transform.position = Vector3.up * 2f;
        //}
    }

    void FixedUpdate()
    {
        GroundCheck();

        ApplyGravity();

        UpdateLegPhysics(leftHip, leftKnee, leftFoot,
            Mouse.current.leftButton.isPressed,
            lTargetX, lTargetZ, true);

        UpdateLegPhysics(rightHip, rightKnee, rightFoot,
            Mouse.current.rightButton.isPressed,
            rTargetX, rTargetZ, false);

        ApplyUprightTorque();
    }

    // 接地判定
    void GroundCheck()
    {
        isGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            groundCheckDist
        );

        if (isGrounded && rb.linearVelocity.y < 0)
        {
            isKnockback = false;
        }
    }

    // 重力
    void ApplyGravity()
    {
        if (!isGrounded)
        {
            rb.AddForce(Vector3.up * gravity, ForceMode.Acceleration);
        }
    }

    // 姿勢制御
    void ApplyUprightTorque()
    {
        bool L = Mouse.current.leftButton.isPressed;
        bool R = Mouse.current.rightButton.isPressed;

        float roll = 0;
        if (L && !R) roll = -sideLeanAngle;
        if (R && !L) roll = sideLeanAngle;

        float pitch = Keyboard.current.wKey.isPressed ? forwardLeanWeight : 0;

        Quaternion targetRot = Quaternion.Euler(pitch, transform.eulerAngles.y, roll);
        Quaternion delta = targetRot * Quaternion.Inverse(transform.rotation);
        delta.ToAngleAxis(out float angle, out Vector3 axis);
        if (angle > 180) angle -= 360;

        rb.AddTorque(axis * angle * uprightForce - rb.angularVelocity * uprightDamper);
    }

    // 足制御
    void UpdateLegPhysics(ConfigurableJoint h, ConfigurableJoint k, ConfigurableJoint f,
        bool lifting, float tx, float tz, bool isLeft)
    {
        if (!h || !k) return;

        var hd = h.slerpDrive;
        var kd = k.slerpDrive;

        if (lifting)
        {
            hd.positionSpring = 8000;
            kd.positionSpring = 8000;

            float z = isLeft ? tz : -tz;
            h.targetRotation = Quaternion.Euler(tx, 0, z);
            k.targetRotation = Quaternion.Euler(-kneeMaxBend, 0, 0);
        }
        else
        {
            hd.positionSpring = 150000;
            kd.positionSpring = 150000;

            h.targetRotation = Quaternion.Euler(tx * 0.2f, 0, 0);
            k.targetRotation = Quaternion.identity;
        }

        h.slerpDrive = hd;
        k.slerpDrive = kd;
        if (f) f.targetRotation = Quaternion.identity;
    }

    void UpdateTarget(ref float tx, ref float tz, bool active, bool W, bool A, bool D)
    {
        if (active)
        {
            if (W) tx = Mathf.MoveTowards(tx, 70, adjustSpeed * Time.deltaTime);
            if (A) tz = Mathf.MoveTowards(tz, -40, adjustSpeed * Time.deltaTime);
            if (D) tz = Mathf.MoveTowards(tz, 40, adjustSpeed * Time.deltaTime);
        }
        else
        {
            tx = Mathf.Lerp(tx, 0, Time.deltaTime * 3);
            tz = Mathf.Lerp(tz, 0, Time.deltaTime * 3);
        }
    }

    // 吹き飛ぶ
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Vector3 dir = (transform.position - other.transform.position).normalized;
            dir.y = 0;

            rb.AddForce(dir * knockbackPower + Vector3.up * knockUpPower,
                ForceMode.Impulse);

            isKnockback = true;
        }
    }
}