using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using System;
public class PlayerSetup : NetworkBehaviour
{
    private List<Behaviour> components = new List<Behaviour>();

    public event EventHandler OnPlayerCameraSetup;

    private void Awake()
    {
        components.Add(GetComponent<PlayerInputManager>());
        components.Add(GetComponent<PlayerInput>());
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner)
        {
            DisableComponents();
        }
        else
        {
            OnPlayerCameraSetup?.Invoke(this, EventArgs.Empty);
        }

    }

    private void DisableComponents()
    {
        foreach (Behaviour component in components)
        {
            component.enabled = false;
        }
    }
}
