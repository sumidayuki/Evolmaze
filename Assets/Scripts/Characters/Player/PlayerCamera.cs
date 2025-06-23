using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerCamera : MonoBehaviour
{
    private float m_mouseSensitivity = 4.0f;
    private float m_controllerSensitivity = 20f;

    private readonly Vector3 OFFSET = new Vector3(0, 1, -2);

    private float rotationX;
    private float rotationY;

    private LayerMask obstacleLayer;
    private RaycastHit hit;
    private Vector3 hitPos;

    private GameObject LockOnTarget;

    public Transform CameraTransform { get; private set; }

    public void Awake()
    {
        CameraTransform = transform;

        Cursor.lockState = CursorLockMode.Locked;

        // 障害物レイヤーを設定
        obstacleLayer = LayerMask.GetMask("Wall", "Ground", "Roof");

        SetMouseSensitivity(SettingDataManager.Instance.CurrentSettingData.Get<float>("MouseSensitivity"));
        SetControllerSensitivity(SettingDataManager.Instance.CurrentSettingData.Get<float>("ControllerSensitivity"));
    }

    public void SetMouseSensitivity(float amount)
    {
        m_mouseSensitivity = amount;
    }

    public void SetControllerSensitivity(float amount)
    {
        m_controllerSensitivity = amount;
    }

    public void NormalCamera(Player player, InputInfo input)
    {
        if (m_mouseSensitivity != SettingDataManager.Instance.CurrentSettingData.Get<float>("MouseSensitivity"))
        {
            SetMouseSensitivity(SettingDataManager.Instance.CurrentSettingData.Get<float>("MouseSensitivity"));
        }

        if (m_controllerSensitivity != SettingDataManager.Instance.CurrentSettingData.Get<float>("ControllerSensitivity"))
        {
            SetControllerSensitivity(SettingDataManager.Instance.CurrentSettingData.Get<float>("ControllerSensitivity"));
        }

        float sensitivity = (player.CurrentDevice is "Gamepad") ? m_controllerSensitivity : m_mouseSensitivity;

        if (input.LockOn)
        {
            LockOnCheck();

            if (LockOnTarget == null)
            {
                input.LockOn = false;
                return;
            }

            Vector3 playerPos = player.CameraLookAtPos.transform.position;
            Vector3 enemyPos = LockOnTarget.transform.position + Vector3.up * 1.5f;

            // 敵→プレイヤーの方向ベクトル
            Vector3 dirToPlayerFromEnemy = (playerPos - enemyPos).normalized;

            // 敵から見た、プレイヤーの背後方向にカメラを置く
            Vector3 desiredCameraPos = playerPos + dirToPlayerFromEnemy * OFFSET.magnitude;

            // Yオフセットを調整
            desiredCameraPos.y += OFFSET.y;

            // 障害物チェック（必要なら）
            if (ObstacleCheck(playerPos, desiredCameraPos))
            {
                transform.position = hitPos;
            }
            else
            {
                transform.position = desiredCameraPos;
            }

            // プレイヤーを中心にして、敵の方向を向く
            transform.LookAt((playerPos + enemyPos) * 0.5f);
        }
        else
        {
            // マウス入力に基づきカメラの回転を計算
            rotationX += input.Look.x * sensitivity / 10;
            rotationY = Mathf.Clamp(rotationY - input.Look.y * sensitivity / 10, -90, 40);

            // 現在の回転角をもとにカメラを回転
            Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);
            Vector3 desiredPosition = player.CameraLookAtPos.transform.position + rotation * OFFSET;

            // 障害物があるかチェック
            if (ObstacleCheck(player.CameraLookAtPos.transform.position, desiredPosition))
            {
                // 障害物がある場合、衝突点にカメラを移動
                transform.position = hitPos;
            }
            else
            {
                // 障害物がない場合、指定された位置にカメラを配置
                transform.position = desiredPosition;
            }

            // プレイヤーの周囲を回転
            transform.RotateAround(player.CameraLookAtPos.transform.position, Vector3.up, rotationY);

            // カメラをプレイヤーに向ける
            transform.LookAt(player.CameraLookAtPos.transform.position);
        }
    }

    private bool ObstacleCheck(Vector3 targetPos, Vector3 desiredPos)
    {
        // Raycastを使って障害物があるか判定
        if (Physics.Raycast(targetPos, desiredPos - targetPos, out hit, Vector3.Distance(targetPos, desiredPos), obstacleLayer, QueryTriggerInteraction.Ignore))
        {
            hitPos = hit.point;
            return true;
        }
        return false;
    }

    private void LockOnCheck()
    {
        int layerMask = ~LayerMask.GetMask("Player"); // Playerレイヤーを無視する

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, layerMask))
        {
            if(hit.collider.gameObject.CompareTag("Enemy"))
            {
                LockOnTarget = hit.collider.gameObject;
            }
        }
    }
}