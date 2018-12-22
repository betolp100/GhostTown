using System.Collections;
using UnityEngine;
using System;

public class PlayerScore : MonoBehaviour {
    Player player;
    int lastKills = 0;
    int lastDeaths = 0;
    string nameAndColor = null;
    bool colorAndNameAlreadySet = false;

    void Start()
    {
        player = GetComponent<Player>();

        StartCoroutine(SyncScoreLoop());
    }

    IEnumerator SyncScoreLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            SyncNow();
        }
    }

    private void OnDestroy()
    {
        if(player!=null)
        SyncNow();
    }

    void SyncNow()
    {
        
        if (UserAccount.IsLoggedIn)
        {
           
            UserAccount.instance.GetData(OnDataReceived);
        }
    }

    void OnDataReceived(string data)
    {
        if (colorAndNameAlreadySet == false)
        {
            string _nickName = DataTranslator.DataToName(data);
            float r = DataTranslator.DataToValue(data, 0);
            float g = DataTranslator.DataToValue(data, 1);
            float b = DataTranslator.DataToValue(data, 2);
            Color _playerColor = new Color(r, g, b);
            nameAndColor = _nickName + "¬" + r + "°" + g + "°" + b + "°";
            Debug.Log("SE ENCONTRARON Y AJUSTARON LOS SIGUIENTE DATOS: NICKNAME: " + _nickName + ",   R: " + r + ",    G:" + g + ",   B:" + b);
        }
        if (player.kills <= lastKills && player.deaths <= lastDeaths)
            return;

        int killsSinceLast = player.kills - lastKills;
        int deathsSinceLast = player.deaths - lastDeaths;

        
        int kills = (int) DataTranslator.DataToValue(data,3);
        int deaths = (int) DataTranslator.DataToValue(data,4);

        int newKills = killsSinceLast + kills;
        int newDeaths = deathsSinceLast + deaths;

        string newData = DataTranslator.ValuesToData(nameAndColor, newKills, newDeaths);

        Debug.Log("Syncing: " + newData);

        lastKills = player.kills;
        lastDeaths = player.deaths;

        UserAccount.instance.SendData(newData);
        Debug.Log("WINNER, WINNER, CHIKEN DINNER");
    }
}
