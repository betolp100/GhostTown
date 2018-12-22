using UnityEngine;
using UnityEngine.UI;

public class PlayerNameplate : MonoBehaviour
{
    [SerializeField]
    private Text username;

    [SerializeField]
    private Player player;

    private void Update()
    {
        username.text = player.nickName;
        Camera cam = Camera.main;
        if (cam != null)
        {
            transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
            
        }
    }

}
