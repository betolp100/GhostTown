using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class LevelManager : MonoBehaviour
{
    protected Scene currentScene;
    public static LevelManager instance = null;
    public int frameRate;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)   //Singleton Algorythm
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        QualitySettings.vSyncCount = 0;
    }

    private void Update()
    {
        if (frameRate != Application.targetFrameRate) Application.targetFrameRate = frameRate;
    }

    public void QuitLevel()
    {
        Debug.Log("Game Finished");
        Application.Quit();   //End game
    }

    public void LoadLevel(string name)
    {
        StartCoroutine(LoadAsynchronously(name));
        Debug.Log("starting game");
    }   //Load level

    IEnumerator LoadAsynchronously(string name)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(name);
        while (!op.isDone)
        {
            yield return null;
        }
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);  //Load next level
    }

    public int GetLevel()
    {
        return (SceneManager.GetActiveScene().buildIndex+1);
    }

}
