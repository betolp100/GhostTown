using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    //Static Area Values
    public static float wood, grass;
    public AudioClip[] clips;
    public static AudioClip sound_W_grass, sound_R_grass, sound_W_wood, sound_R_wood;
    public static float timer;
    public GameObject pm;

    //Start
    public static bool GameStarted, GameOver;
    public static float TimeToBegin;
    public static int playersConnected;
    public Transform KioscoCenter;
    public Vector3 center, size;

    public List<GameObject> playersInMap = new List<GameObject>();
    private static Map instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start ()
    {
        GameStarted = false;
        GameOver = false;
        pm.SetActive(false);
        
        sound_W_grass = clips[0];
        sound_W_wood = clips[1];
        sound_R_grass = clips[2];
        sound_R_wood = clips[3];

        timer = 0;
	}

    void Update ()
    {
        timer += Time.deltaTime; // With this we can have the exact time seconds playing from the very begining of the game
        if (GameStarted == true)
        {
            foreach (GameObject player in playersInMap)
            {
                SpawnPlayer(player);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(center, size);
    }

    public void SpawnPlayer(GameObject player)
    {
        Vector3 pos = center + new Vector3(Random.Range(-size.x/2,size.x/2), Random.Range(-size.y / 2, size.y / 2), Random.Range(-size.z / 2, size.z / 2));

        
    



        player.transform.position = pos;
        player.transform.LookAt(KioscoCenter);
        player.name = "Player_" + playersConnected.ToString();
        player.SetActive(true);
    }
}
