using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class Player : Character, IDamageable
{
    /// <summary>
    /// プレイヤーのステートの状況等を管理します。
    /// </summary>
    private StateManager<Player> m_stateManager;

    /// <summary>
    /// プレイヤーの移動を管理します。
    /// このプロパティは、読み取り専用です。
    /// </summary>
    public PlayerMove Move { get; private set; }

    /// <summary>
    /// プレイヤーのカメラを管理します。
    /// このプロパティは、読み取り専用です。
    /// </summary>
    public PlayerCamera Camera { get; private set; }

    /// <summary>
    /// プレイヤーのUIを管理します。
    /// このプロパティは、読み取り専用です。
    /// </summary>
    public PlayerUI UI { get; private set; }

    /// <summary>
    /// プレイヤーのサウンドを管理します。
    /// このプロパティは読み取り専用です。
    /// </summary>
    public PlayerSound Sound { get; private set; }

    /// <summary>
    /// カメラの注視点オブジェクトを取得します。
    /// このプロパティは、読み取り専用です。
    /// </summary>
    public GameObject CameraLookAtPos { get; private set; }

    /// <summary>
    /// プレイヤーの入力を取得するPlayerInputを参照します。
    /// このプロパティは読み取り専用です。
    /// </summary>
    public PlayerInput MyPlayerInput { get; private set; }

    /// <summary>
    /// プレイヤーの物理演算を管理します。
    /// このプロパティは、読み取り専用です。
    /// </summary>
    public Rigidbody Rigidbody { get; private set; }

    /// <summary>
    /// プレイヤーのオブジェクトを取得します。
    /// このプロパティは、読み取り専用です。
    /// </summary>
    public GameObject PlayerObj { get { return this.gameObject; } }

    public Sword Weapon { get; private set; }

    /// <summary>
    /// 現在のデバイスの名前を取得します。
    /// このプロパティは読み取り専用です。
    /// </summary>
    public string CurrentDevice { get; private set; }

    /// <summary>
    /// プレイヤーの最大体力を管理します。
    /// このプロパティは、読み取り専用です。
    /// </summary>
    public float MaxHP { get; private set; }

    /// <summary>
    /// プレイヤーの現在の体力を管理します。
    /// このプロパティは、読み取り専用です。
    /// </summary>
    public float CurrentHP { get; private set; }

    /// <summary>
    /// プレイヤーの攻撃力を管理します。
    /// このプロパティは、読み取り専用です。
    /// </summary>
    public float AttackDamage { get; private set; }

    public HitBox MyHitBox { get; private set; }

    /// <summary>
    /// プレイヤーの初期設定を行います。
    /// </summary>
    private void Awake()
    {
        MyPlayerInput = this.GetComponent<PlayerInput>();
        Move = this.AddComponent<PlayerMove>();                                 // PlayerMoveをコンポーネントで追加し、Moveに格納
        Anime = this.AddComponent<CharacterAnime>();
        Camera = FindObjectOfType<PlayerCamera>();                              // シーン内のPlayerCameraを探し、Cameraに格納
        UI = FindObjectOfType<PlayerUI>();                                      // シーン内のPlayerUIを探し、UIに格納
        Weapon = GetComponentInChildren<Sword>();
        Sound = GetComponent<PlayerSound>();
        CameraLookAtPos = gameObject.transform.GetChild(0).gameObject;          // カメラの注視点オブジェクトを取得
        Rigidbody = gameObject.GetComponent<Rigidbody>();                       // Rigidbodyを取得
        MaxHP = SaveDataManager.Instance.CurrentSaveData.Get<float>("MaxHP");                 // 最大HPをセーブデータから取得
        CurrentHP = MaxHP;                                                                                                      // 現在のHPを最大HPに設定
        AttackDamage = SaveDataManager.Instance.CurrentSaveData.Get<float>("AttackDamage");    // 攻撃力をセーブデータから取得
        MyHitBox = gameObject.GetComponentInChildren<HitBox>();

        m_stateManager = new StateManager<Player>();                            // PlayerStateManagerをコンポジションで格納
        m_stateManager.Init(new PlayerMoveState(), this);                       // stateManagerの初期化

        UI.SetName();
    }

    /// <summary>
    /// ダメージを受けたときの処理です。
    /// このメソッドは、IDamageableインターフェイスを経由して呼び出されます。
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public void Damage(float damage)
    {
        if (m_stateManager.CurrentState is PlayerHitState) return;
        CurrentHP = Mathf.Clamp(CurrentHP - damage, 0, MaxHP);
        UI.UpdateHPBar(CurrentHP / MaxHP);
        m_stateManager.CurrentState.Damaged(this);
    }

    public void Death()
    {
        m_stateManager.ChangeState(new PlayerDeathState(), this);
    }

    /// <summary>
    /// プレイヤーの更新処理を管理します。
    /// 通常のUpdate関数が使用されます。
    /// </summary>
    /// <param name="input"></param>
    public void Execute(InputInfo input)
    {
        CurrentDevice = MyPlayerInput.currentControlScheme;

        if (CurrentHP <= 0)
        {
            Death();
            return;
        }

        m_stateManager.CurrentState.Execute(this, input);
    }

    /// <summary>
    /// カメラの更新処理を管理します。
    /// LateUpdate関数が使用されます。
    /// </summary>
    /// <param name="input"></param>
    public void CameraExecute(InputInfo input)
    {
        m_stateManager.CurrentState.LateExecute(this, input);
        UI.UpdateMiniMapCamera(this.gameObject.transform.position);
    }

    /// <summary>
    /// プレイヤーの物理に関する更新処理を管理します。
    /// FixedUpdate関数が使用されます。
    /// </summary>
    /// <param name="input"></param>
    public void FixedExecute(InputInfo input)
    {
        m_stateManager.CurrentState.FixedExecute(this, input);
    }

    /// <summary>
    /// 現在のステートから別のステート変更します。
    /// </summary>
    /// <param name="newState">変更先のステート</param>
    public void ChangeState(StateBase<Player> newState)
    {
        m_stateManager.ChangeState(newState, this);
    }
}
