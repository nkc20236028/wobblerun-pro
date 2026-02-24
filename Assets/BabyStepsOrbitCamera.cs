using UnityEngine;
using UnityEngine.InputSystem;

public class BabyStepsOrbitCamera : MonoBehaviour
{
    [Header("追従対象 (Hipsをドラッグ)")]
    public Transform target;

    [Header("カメラの設定")]
    public float distance = 2.5f;       // キャラからの距離
    public float sensitivity = 0.10f;   // マウス感度
    public float heightOffset = 1.1f;   // キャラを見下ろす高さ
    public float minPitch = -5f;        // 下限
    public float maxPitch = 60f;        // 上限

    private float yaw;                  // 左右角度
    private float pitch = 20f;          // 上下角度

    void Start()
    {
        if (target == null) return;
        yaw = target.eulerAngles.y; // キャラの初期向きに合わせる
    }

    void LateUpdate() // 物理演算の後にカメラを動かす
    {
        if (target == null) return;

        // マウス移動量の取得
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        yaw += mouseDelta.x * sensitivity;
        pitch -= mouseDelta.y * sensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // 円状の回転を計算
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // キャラの腰の位置を基準にカメラを配置
        Vector3 focusPoint = target.position + Vector3.up * heightOffset;
        Vector3 position = focusPoint - (rotation * Vector3.forward * distance);

        transform.rotation = rotation;
        transform.position = position;
    }
}