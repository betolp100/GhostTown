using UnityEngine;
using UnityEngine.UI;


public class BuildPlayer : MonoBehaviour
{
    public string  playerName=null;
    public Color color;
    public static BuildPlayer instance = null;
    private GameObject playerToCustomize = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {   //Singleton Algorythm
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
