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
    private const float MoveSpeed = 1.7f;
    private const float SprintSpeed = 3.3f;
    private CharacterController characterController;
    private float speed;
    public float SpeedChangeRate = 10.0f;
    private float animationBlend;
    private float targetRotation;
    private float rotationVelocity;
    private float RotationSmoothTime = 0.12f;
    private bool rotateOnMove = true;
    private float verticalVelocity;
    private GameObject mainCamera;
    private Animator animator;


    private void Awake()
    {
        input = GetComponent<PlayerInputManager>();
        playerCameraRoot = GetComponentInChildren<PlayerCameraRoot>().transform;
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    private void Move()
    {
        float targetSpeed = input.sprint ? SprintSpeed : MoveSpeed;

        if (input.move == Vector2.zero) targetSpeed = 0.0f;

        float currentHorizontalSpeed =
            new Vector3(
                characterController.velocity.x,
                0.0f,
                characterController.velocity.z
            ).magnitude;

        float speedOffset = 0.1f;

        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed,
                Time.deltaTime * SpeedChangeRate);

            speed = Mathf.Round(speed * 1000f) / 1000f;
        }
        else
        {
            speed = targetSpeed;
        }
        animationBlend = Mathf.Lerp(animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (animationBlend < 0.01f) animationBlend = 0f;


        Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

        if (input.move != Vector2.zero)
        {
            targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity,
                RotationSmoothTime);

            if (rotateOnMove)
            {
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;

        // move the player

        characterController.Move(targetDirection.normalized * (speed * Time.deltaTime)+
                    new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
        PlayAnimationOnServerRpc(animationBlend);
    }

    [ServerRpc]
    private void PlayAnimationOnServerRpc(float animationBlend)
    {
        PlayeAnimationOnClientRpc(animationBlend);
    }

    [ClientRpc]
    private void PlayeAnimationOnClientRpc(float animationBlend)
    {
        animator.SetFloat("moveSpeed", animationBlend);
        
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

    private void Update()
    {
        if(IsOwner)
        { 
            Move(); 
        }
    }

    private void LateUpdate()
    {
        if (IsOwner) 
        {
            Rotate();
        }
    }
}
