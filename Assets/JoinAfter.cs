using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class JoinAfter : NetworkBehaviour
{
    [SerializeField]
    private Renderer headMesh;

    [SerializeField]
    private Renderer bodyMesh;

    [SerializeField]
    private Player player;


    [SyncVar]
    private Color localColor;

    [SyncVar]
    private string localNickName;

    bool alreadySync=false;
    public bool logout = false;
    void Start()
    {
        player = GetComponent<Player>();
        Debug.Log("Esperando respuesta");

        StartCoroutine(SyncScoreLoop());

    }
    
    IEnumerator SyncScoreLoop()
    {/*
        while (true)
        {
            if (alreadySync == false)

            SyncNow();

            yield return new WaitForSeconds(2);
            Debug.Log("Tratando de sincronizar");
            SyncNow();
            alreadySync = true;
        }*/
     //Funciona en ciclos, pero se resetea al inicio del ciclo
        while (true)
        {
            //if (alreadySync == false)
            {

                yield return new WaitForSeconds(5);
                SyncNow();
                alreadySync = true;
                
            }
        }
    }

    private void OnApplicationQuit()
    {
        logout = true;
    }

    private void OnDisable()
    {
        logout = true;
    }

    /*void HadSyncAlready()
    {
        CmdSetUserName(transform.name, localNickName, localColor);
    }*/

    private void OnDestroy()
    {
        if (player != null)
            SyncNow();
    }

    void SyncNow()
    {
        Debug.Log("El usuario esta conectado?");
        if (UserAccount.IsLoggedIn)
        {
            Debug.Log("El usuario si esta conectado y esperando que nos abra la puerta");
            PlayerStartValues _psv = GameObject.Find("PSV_Values").GetComponent<PlayerStartValues>();
            if (_psv != null||isLocalPlayer)
            {
                localNickName = "LoadingLOLOLOL";
                localColor = Color.green;
                if (UserAccount.IsLoggedIn == true)
                {
                    localNickName = _psv.Nickname;
                    Debug.Log("PLAYER SETUP: NICKNAME: " + localNickName);
                    localColor = _psv.startColor;
                }
                else
                {
                    localNickName = transform.name;
                }

                CmdSetUserName(transform.name, localNickName, localColor);
            }
            else
            {   
                string _username = "LoadingLELELEL";
                Color _color = Color.blue;
                CmdSetUserName(transform.name, _username, _color);
            }
        }
    }


    [Command]
    void CmdSetUserName(string _playerID, string _username, Color _color)
    {
        Player player = GameManager.GetPlayer(_playerID);
        if (player != null)
        {
            RpcSetUserName(_playerID, _username, _color);
            /*
            //solo hacerlo para jugadores que ya estaban antes de la partida y hacerlo una vez
                Player[] _players = GameManager.GetAllPlayers();
                foreach (Player _player in _players)
                {
                    _player.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = _player.nickName;
                    _player.transform.GetChild(1).GetChild(0).GetComponent<Renderer>().material.color = _player.playerColor;
                    _player.transform.GetChild(1).GetChild(1).GetComponent<Renderer>().material.color = _player.playerColor;
                }
               */ 
            
        }
    }

    [ClientRpc]
    void RpcSetUserName(string _playerID, string username, Color _color)
    {
        Player player = GameManager.GetPlayer(_playerID);
        if (player != null)
        {
            player.nickName = username;
            player.playerColor = _color;
            headMesh.material.color = player.playerColor;
            bodyMesh.material.color = player.playerColor;
            Debug.Log(player.playerColor);
        }
        
    }
}
