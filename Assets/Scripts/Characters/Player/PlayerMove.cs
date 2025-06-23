using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの移動を制御するクラス。
/// 地上移動、空中移動、回転、ジャンプ、速度の設定・取得などを管理します。
/// </summary>
public class PlayerMove : MonoBehaviour
{
    public float CurrentSpeed { get; private set; }

    private float runSpeed = 5;
    private float walkSpeed = 2;

    private float rotationSmoothTime = 0.1f;
    private float rotationVelocity;

    private void Awake()
    {
        runSpeed = SaveDataManager.Instance.CurrentSaveData.Get<float>("RunSpeed");
    }

    public void Movement(Player player, InputInfo input)
    {
        Vector3 direction = CalculateMoveDirection(player, input);
        MovePlayer(player, direction, input);
        RotatePlayer(player, direction);
    }

    private Vector3 CalculateMoveDirection(Player player, InputInfo input)
    {
        Vector3 forward = player.Camera.CameraTransform.forward;
        Vector3 right  = player.Camera.CameraTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 movement = forward * input.Move.z + right * input.Move.x;
        movement = Vector3.ClampMagnitude(movement, 1.0f);

        return movement;
    }

    private void MovePlayer(Player player, Vector3 direction, InputInfo input)
    {
        if (SettingDataManager.Instance.CurrentSettingData.Get<bool>("AutoDash"))
        {
            CurrentSpeed = direction.magnitude * (input.AutoDash ? runSpeed : walkSpeed);
        }
        else
        {
            CurrentSpeed = direction.magnitude * (input.Dash ? runSpeed : walkSpeed);
        }
        player.Rigidbody.MovePosition(transform.position + direction * CurrentSpeed * Time.fixedDeltaTime);
    }

    private void RotatePlayer(Player player, Vector3 direction)
    {
        Vector3 forward = direction;

        // 方向が0でない場合のみ回転
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Quaternion.Lerpを使って回転をスムージング
            player.Rigidbody.MoveRotation(targetRotation);
        }
    }
}
