using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
public class PlayerInputManager : NetworkBehaviour
{
    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool sprint;
    public bool aim;
    public bool shoot;
    public bool primary;
    public bool secondary;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    private InputController inputActions;

    public void InitInputSystem()
    {
        // 确保只初始化一次
        if (inputActions != null) return;
        
        Debug.Log("Initializing Input System");
        
        // 创建输入控制器实例
        inputActions = new InputController();
        
        // 注册移动输入回调
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;
        
        // 注册视角输入回调
        inputActions.Player.Look.performed += OnLookPerformed;
        inputActions.Player.Look.canceled += OnLookCanceled;
        
        // 注册跳跃输入回调
        inputActions.Player.Jump.performed += OnJumpPerformed;
        inputActions.Player.Jump.canceled += OnJumpCanceled;
        
        // 注册射击输入回调
        inputActions.Player.Shoot.performed += OnShootPerformed;
        inputActions.Player.Shoot.canceled += OnShootCanceled;
        
        // 注册冲刺输入回调
        inputActions.Player.Sprint.performed += OnSprintPerformed;
        inputActions.Player.Sprint.canceled += OnSprintCanceled;
        
        // 注册主武器切换回调
        inputActions.Player.PrimaryWeapon.performed += OnPrimaryWeaponPerformed;
        
        // 注册副武器切换回调
        inputActions.Player.SecondaryWeapon.performed += OnSecondaryWeaponPerformed;
        
        // 启用输入系统
        inputActions.Enable();
        
        Debug.Log("Input System Initialized");
    }

    // 统一的输入系统清理方法
    public void CleanupInputSystem()
    {
        if (inputActions == null) return;
        
        Debug.Log("Cleaning up Input System");
        
        // 取消所有回调注册
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        
        inputActions.Player.Look.performed -= OnLookPerformed;
        inputActions.Player.Look.canceled -= OnLookCanceled;
        
        inputActions.Player.Jump.performed -= OnJumpPerformed;
        inputActions.Player.Jump.canceled -= OnJumpCanceled;
        
        inputActions.Player.Shoot.performed -= OnShootPerformed;
        inputActions.Player.Shoot.canceled -= OnShootCanceled;
        
        inputActions.Player.Sprint.performed -= OnSprintPerformed;
        inputActions.Player.Sprint.canceled -= OnSprintCanceled;
        
        inputActions.Player.PrimaryWeapon.performed -= OnPrimaryWeaponPerformed;
        inputActions.Player.SecondaryWeapon.performed -= OnSecondaryWeaponPerformed;
        
        // 禁用输入系统
        inputActions.Disable();
        inputActions = null;
        
        Debug.Log("Input System Cleaned up");
    }

    #region 输入回调处理函数

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        move = Vector2.zero;
    }

    private void OnLookPerformed(InputAction.CallbackContext ctx)
    {
        look = ctx.ReadValue<Vector2>();
    }

    private void OnLookCanceled(InputAction.CallbackContext ctx)
    {
        look = Vector2.zero;
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        jump = true;
    }

    private void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        jump = false;
    }

    private void OnShootPerformed(InputAction.CallbackContext ctx)
    {
        shoot = true;
    }

    private void OnShootCanceled(InputAction.CallbackContext ctx)
    {
        shoot = false;
    }

    private void OnSprintPerformed(InputAction.CallbackContext ctx)
    {
        sprint = true;
    }

    private void OnSprintCanceled(InputAction.CallbackContext ctx)
    {
        sprint = false;
    }

    private void OnPrimaryWeaponPerformed(InputAction.CallbackContext ctx)
    {
        primary = !primary;
    }

    private void OnSecondaryWeaponPerformed(InputAction.CallbackContext ctx)
    {
        secondary = !secondary;
    }

    #endregion
    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
