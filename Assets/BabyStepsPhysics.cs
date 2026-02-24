using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class BabyStepsPhysics : MonoBehaviour
{
    [Header("関節の割り当て")]
    public ConfigurableJoint leftHip; public ConfigurableJoint leftKnee; public ConfigurableJoint leftFoot;
    public ConfigurableJoint rightHip; public ConfigurableJoint rightKnee; public ConfigurableJoint rightFoot;
    public ConfigurableJoint spineJoint;

    [Header("精密操作（角度の蓄積）")]
    public float adjustSpeed = 120f;   // WADで足を動かす速さ
    public float moveSpeed = 10f;     // 物理的な追従速度
    public float kneeMaxBend = 70f;    // 上げている時の膝の曲がり

    // 足ごとの「狙い」を保存する変数
    private float lTargetX, lTargetZ, rTargetX, rTargetZ;

    [Header("自立と重心設定")]
    public float uprightForce = 150000f;
    public float uprightDamper = 30000f;
    public float sideLeanAngle = 15f; // 片足立ちの時の傾き
    public float forwardLeanWeight = 10f; // W押し込み時の前傾

    private Rigidbody rb;

    void Start() { rb = GetComponent<Rigidbody>(); Cursor.lockState = CursorLockMode.Locked; }

    void Update()
    {
        bool L = Mouse.current.leftButton.isPressed;
        bool R = Mouse.current.rightButton.isPressed;
        bool W = Keyboard.current.wKey.isPressed;
        bool A = Keyboard.current.aKey.isPressed;
        bool D = Keyboard.current.dKey.isPressed;

        // --- 左足の狙い調整 ---
        if (L)
        {
            if (W) lTargetX = Mathf.MoveTowards(lTargetX, 70f, adjustSpeed * Time.deltaTime);
            if (A) lTargetZ = Mathf.MoveTowards(lTargetZ, -40f, adjustSpeed * Time.deltaTime);
            if (D) lTargetZ = Mathf.MoveTowards(lTargetZ, 40f, adjustSpeed * Time.deltaTime);
        }
        else
        {
            // クリックを離したら、次に備えてゆっくりリセット（または接地後に0へ）
            lTargetX = Mathf.Lerp(lTargetX, 0, Time.deltaTime * 2f);
            lTargetZ = Mathf.Lerp(lTargetZ, 0, Time.deltaTime * 2f);
        }

        // --- 右足の狙い調整 ---
        if (R)
        {
            if (W) rTargetX = Mathf.MoveTowards(rTargetX, 70f, adjustSpeed * Time.deltaTime);
            if (A) rTargetZ = Mathf.MoveTowards(rTargetZ, -40f, adjustSpeed * Time.deltaTime);
            if (D) rTargetZ = Mathf.MoveTowards(rTargetZ, 40f, adjustSpeed * Time.deltaTime);
        }
        else
        {
            rTargetX = Mathf.Lerp(rTargetX, 0, Time.deltaTime * 2f);
            rTargetZ = Mathf.Lerp(rTargetZ, 0, Time.deltaTime * 2f);
        }

        UpdateLeg(leftHip, leftKnee, leftFoot, L, lTargetX, lTargetZ, true);
        UpdateLeg(rightHip, rightKnee, rightFoot, R, rTargetX, rTargetZ, false);

        if (spineJoint != null) spineJoint.targetRotation = Quaternion.identity;
    }

    void FixedUpdate()
    {
        // 垂直維持とダイナミックな重心移動
        bool L = Mouse.current.leftButton.isPressed;
        bool R = Mouse.current.rightButton.isPressed;

        float roll = 0;
        if (L && !R) roll = -sideLeanAngle; // 左足を上げたら右へ傾く
        if (R && !L) roll = sideLeanAngle;  // 右足を上げたら左へ傾く

        float pitch = (Keyboard.current.wKey.isPressed) ? forwardLeanWeight : 0;

        Quaternion targetRot = Quaternion.Euler(pitch, transform.eulerAngles.y, roll);
        Quaternion deltaRot = targetRot * Quaternion.Inverse(transform.rotation);
        deltaRot.ToAngleAxis(out float angle, out Vector3 axis);
        if (angle > 180f) angle -= 360f;
        rb.AddTorque(axis.normalized * angle * uprightForce - rb.angularVelocity * uprightDamper);
    }

    void UpdateLeg(ConfigurableJoint h, ConfigurableJoint k, ConfigurableJoint f, bool isInput, float tx, float tz, bool isL)
    {
        if (h == null) return;
        var hd = h.slerpDrive; var kd = k.slerpDrive;
        float speed = isInput ? moveSpeed : moveSpeed * 0.4f;

        if (isInput)
        {
            // 【上げている時】バネを弱くして操作しやすくする
            hd.positionSpring = 8000f; kd.positionSpring = 8000f;

            // 左右反転修正（右足の時はZ軸の命令を逆にする）
            float actualZ = isL ? tz : -tz;

            h.targetRotation = Quaternion.Lerp(h.targetRotation, Quaternion.Euler(tx, 0, actualZ), Time.deltaTime * speed);
            k.targetRotation = Quaternion.Lerp(k.targetRotation, Quaternion.Euler(-kneeMaxBend, 0, 0), Time.deltaTime * speed);
        }
        else
        {
            // 【下ろしている時/軸足】バネを爆上げしてガッチリ固定
            hd.positionSpring = 150000f; kd.positionSpring = 150000f;

            // 真下へ足を伸ばして着地する
            h.targetRotation = Quaternion.Lerp(h.targetRotation, Quaternion.Euler(tx * 0.2f, 0, 0), Time.deltaTime * speed);
            k.targetRotation = Quaternion.Lerp(k.targetRotation, Quaternion.identity, Time.deltaTime * speed);
        }
        h.slerpDrive = hd; k.slerpDrive = kd;
        f.targetRotation = Quaternion.identity;
    }
}