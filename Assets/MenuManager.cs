using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private LevelManager lm;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    public void ChangeLevel(string level)
    {
        lm.LoadLevel(level);
    }
}
