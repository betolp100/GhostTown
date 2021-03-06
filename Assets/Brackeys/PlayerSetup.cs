﻿using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player),typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour
{

    [SerializeField]
    private string head="Head";
    [SerializeField]
    private GameObject playerGraphics;

    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    [SerializeField]
    private GameObject playerUIPrefab;

    [HideInInspector]
    public GameObject playerUIInstance;

    void Start()
    {
        if (!isLocalPlayer)
        {

            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            
            SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(head));
            playerUIInstance=Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;

            PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
            if (ui == null)
            {
                Debug.LogError("NO PLAYER UI COMPONENT ON PLAYER UI PREFAB");
            }
            else
            {
                ui.SetPlayerController(GetComponent<PlayerController>());
                GetComponent<Player>().SetupPlayer();

                
                
            }
        }
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();

        GameManager.RegisterPlayer(_netID,_player);
    }

    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    private void OnDisable()
    {
        Destroy(playerUIInstance);

        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
        }

        string _netID = GetComponent<NetworkIdentity>().netId.ToString();

        GameManager.UnRegisterPlayer(transform.name);
    }
}
