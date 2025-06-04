using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private PlayerInputManager input;

    /// <summary>
    /// 相机移动相关
    /// </summary>
    private float totalHorizontalRotate;
    private float totalVerticalRotate;
    private const float minRotateLimit = -30f;
    private const float maxRotateLimit = 70f;
    private const float eps = 0.01f;
    public float mouseSensitivity = 0.13f;
    private Transform playerCameraRoot;


    /// <summary>
    /// 玩家移动相关
    /// </summary>
    

    private void Awake()
    {
        input = GetComponent<PlayerInputManager>();
        playerCameraRoot = GetComponentInChildren<PlayerCameraRoot>().transform;
    }

    private void Move()
    {

    }

    private void Rotate()
    {
        if (input.look.sqrMagnitude >= eps)
        {
            totalHorizontalRotate += input.look.x * mouseSensitivity;
            totalVerticalRotate -= input.look.y * mouseSensitivity;
        }
        totalHorizontalRotate = ClampAngle(totalHorizontalRotate, float.MinValue, float.MaxValue);
        totalVerticalRotate = ClampAngle(totalVerticalRotate, minRotateLimit, maxRotateLimit);
        playerCameraRoot.transform.rotation = Quaternion.Euler(totalVerticalRotate, totalHorizontalRotate, 0.0f);

    }
    private float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    private void LateUpdate()
    {
        if (IsOwner) { Rotate(); }
    }
}
