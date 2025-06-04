using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerCamera : MonoBehaviour
{
    private Transform playerCameraRoot;
    private CinemachineVirtualCamera playerFollowCinemachine;
    private CinemachineVirtualCamera playerAimCinemachine;
    private PlayerSetup playerSetup;

    private void Awake()
    {
        playerSetup = GetComponent<PlayerSetup>();

        playerSetup.OnPlayerCameraSetup += PlayerSetup_OnPlayerCameraSetup;
    }

    private void PlayerSetup_OnPlayerCameraSetup(object sender, System.EventArgs e)
    {
        playerCameraRoot = GetComponentInChildren<PlayerCameraRoot>().transform;
        playerFollowCinemachine = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        playerAimCinemachine = GameObject.Find("PlayerAimCamera").GetComponent<CinemachineVirtualCamera>();

        playerFollowCinemachine.Follow = playerCameraRoot;
        playerAimCinemachine.Follow = playerCameraRoot;
        playerAimCinemachine.gameObject.SetActive(false);
    }

}
