using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    RectTransform sprintBarFill;
    private PlayerController playerController;

    void SetStaminaAmount(float _stamina)
    {
        sprintBarFill.localScale = new Vector3(1,_stamina, 1);
    }

    public void SetPlayerController(PlayerController _playerController)
    {
        playerController = _playerController;
    }

    void Start()
    {
        PauseMenu.isMenuOn = false;
    }

    private void Update()
    {
        SetStaminaAmount((playerController.GetStaminaAmount()));

        if (Input.GetButtonDown("Start"))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.isMenuOn = pauseMenu.activeSelf;
    }


}
