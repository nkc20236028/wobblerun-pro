using UnityEngine;

public class KnockbackTrigger : MonoBehaviour
{
    [Header("ノックバック設定")]
    public float power = 12f;        // 水平方向の強さ
    public float upward = 1.2f;      // 上向き成分
    public float cooldown = 0.2f;    // 同一プレイヤーへの再ヒット抑制

    // 直近で吹っ飛ばしたルートを記録（簡易クールダウン）
    private Transform lastRoot;
    private float lastTime;

    private void OnTriggerEnter(Collider other)
    {
        // 1) まず、そのコライダーに紐づく Rigidbody を取得
        var hitRb = other.attachedRigidbody;
        if (hitRb == null) return;

        // 2) ルートを取得し、ルート側の Rigidbody を使う
        Transform root = hitRb.transform.root;

        // 3) 連続ヒット抑制（同じルートに短時間で多重加速しない）
        if (root == lastRoot && Time.time - lastTime < cooldown) return;

        // 4) ルートにある Rigidbody を探す（なければ子のを使う）
        Rigidbody rootRb = root.GetComponent<Rigidbody>();
        if (rootRb == null) rootRb = hitRb;
        if (rootRb.isKinematic) return;

        // 5) 速度リセットで安定化（任意）
        rootRb.linearVelocity = Vector3.zero;
        rootRb.angularVelocity = Vector3.zero;

        // 6) トリガー中心 → ルート位置 の方向 + 上向き成分
        Vector3 dir = (root.position - transform.position).normalized;
        dir.y += upward;
        dir.Normalize();

        // 7) Mass 非依存で速度を与える
        rootRb.AddForce(dir * power, ForceMode.VelocityChange);

        // 8) 記録
        lastRoot = root;
        lastTime = Time.time;
    }
}
