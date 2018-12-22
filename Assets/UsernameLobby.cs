using UnityEngine;
using UnityEngine.UI;

public class UsernameLobby : MonoBehaviour {

    Text userNameText;
    [SerializeField]
    private GameObject logout;

	void Start ()
    {
        userNameText = GetComponent<Text>();
        if (UserAccount.IsLoggedIn == true)
        {
            Debug.Log("UserName: "+ UserAccount.LoggedIn_CurrentNickName);
            userNameText.text = UserAccount.LoggedIn_CurrentNickName;
            logout.SetActive(true);
        }
        else
        {
            userNameText.text = "No user at the moment";
            logout.SetActive(false);
        }
	}

    public void LogOut()
    {
        UserAccount.instance.LogOut();
    }
}
