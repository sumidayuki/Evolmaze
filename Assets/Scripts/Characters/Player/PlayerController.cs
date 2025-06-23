using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputInfo m_inputInfo;
    private Player m_player;

    private void Awake()
    {
        m_inputInfo = new InputInfo();

        m_player = GetComponent<Player>();
    }

    private void Update()
    {
        m_player.Execute(m_inputInfo);

        InputClear();
    }

    private void LateUpdate()
    {
        m_player.CameraExecute(m_inputInfo);
    }

    private void FixedUpdate()
    {
        m_player.FixedExecute(m_inputInfo);
    }

    public void OnMove(InputValue value)
    {
        var input = value.Get<Vector2>();
        m_inputInfo.Move = new Vector3(input.x, 0, input.y);
    }

    // Dashƒ{ƒ^ƒ“‚ª‰Ÿ‚³‚ê‚Ä‚¢‚éŠÔ‚¾‚¯ Dash ‚ð true ‚É
    public void OnDash(InputValue value)
    {
        var input = value.Get<float>();
        m_inputInfo.Dash = input > 0 ? true : false;
    }

    public void OnAutoDash()
    {
        m_inputInfo.AutoDash = !m_inputInfo.AutoDash;
    }

    public void OnLook(InputValue value)
    {
        var input = value.Get<Vector2>();
        m_inputInfo.Look = input;
    }

    public void OnAttackA()
    {
        m_inputInfo.AttackA = true;
    }

    public void OnAttackB()
    {
        m_inputInfo.AttackB = true;
    }

    public void OnDiveRoll()
    {
        m_inputInfo.DiveRoll = true;
    }

    public void OnPause()
    {
        GameManager.Instance.Pause();
    }

    public void OnLockOn()
    {
        m_inputInfo.LockOn = !m_inputInfo.LockOn;
    }

    private void InputClear()
    {
        m_inputInfo.AttackA = false;
        m_inputInfo.AttackB = false;
        m_inputInfo.DiveRoll = false;
    }
}
