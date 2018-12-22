using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject sceneCamera;

    public static GameManager instance = null;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("GameManager: Detected more than 1 GameManager, closing newest");
            Destroy(this);
        }
        instance = this;

    }

    public void SetSceneCameraActive(bool isActive)
    {
        if (sceneCamera == null) return;
        sceneCamera.SetActive(isActive);
    }

    public static Player[] GetAllPlayers()
    {
        return players.Values.ToArray(); 
    }
    [SerializeField]
    public MatchSettings matchSettings;
    #region Player Tracking

    private const string PLAYER_ID_PREFIX = "Player ";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string _netID, Player _player/*, string _customName, Color _customColor*/)
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
        
    }

    
    public static void UnRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }

    public static Player GetPlayer(string _playerID)
    {
        return players[_playerID];
    }

    /*private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200, 200, 200, 500));

        foreach (string _playerID in players.Keys)
        {
            GUILayout.Label(_playerID + " - " + players[_playerID].transform.name);
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }*/
    #endregion
}

