using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
public class PlayerSetup : NetworkBehaviour
{
    private List<Behaviour> components = new List<Behaviour>();

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

    }

    private void DisableComponents()
    {
        foreach (Behaviour component in components)
        {
            component.enabled = false;
        }
    }
}
