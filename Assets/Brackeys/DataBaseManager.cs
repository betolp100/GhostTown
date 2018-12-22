using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DatabaseControl; // << Remember to add this reference to your scripts which use DatabaseControl

public class DataBaseManager : MonoBehaviour {

    private LevelManager lm;
    [SerializeField]
    private Renderer playerModel;
    [SerializeField]
    private Renderer playerModel2;
    //All public variables bellow are assigned in the Inspector

    [SerializeField]
    private GameObject PLayerStartValues;
    
    
    //These are the GameObjects which are parents of groups of UI elements. The objects are enabled and disabled to show and hide the UI elements.
    public GameObject loginParent;
    public GameObject registerParent;
    public GameObject loggedInParent;
    public GameObject loadingParent;

    //These are all the InputFields which we need in order to get the entered usernames, passwords, etc
    public InputField Login_UsernameField;
    public InputField Login_PasswordField;
    public InputField Register_UsernameField;
    public InputField Register_PasswordField;
    public InputField Register_ConfirmPasswordField;
    public InputField LoggedIn_DataInputField;
    public InputField LoggedIn_DataOutputField;

    //These are the UI Texts which display errors
    public Text Login_ErrorText;
    public Text Register_ErrorText;

    //This UI Text displays the username once logged in. It shows it in the form "Logged In As: " + username
    public Text LoggedIn_DisplayUsernameText;

    private static string currentName = "";

    //Called at the very start of the game
    void Awake()
    {

        ResetAllUIElements();
        lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        playerModel.enabled = false;
        playerModel2.enabled = false;

    }

    public static string GetUsername()
    {
        return currentName;
    }
    //Called by Button Pressed Methods to Reset UI Fields
    void ResetAllUIElements ()
    {
        //This resets all of the UI elements. It clears all the strings in the input fields and any errors being displayed
        Login_UsernameField.text = "";
        Login_PasswordField.text = "";
        Register_UsernameField.text = "";
        Register_PasswordField.text = "";
        Register_ConfirmPasswordField.text = "";
        LoggedIn_DataInputField.text = "";
        LoggedIn_DataOutputField.text = "";
        Login_ErrorText.text = "";
        Register_ErrorText.text = "";
        LoggedIn_DisplayUsernameText.text = "";
    }

    //Called by Button Pressed Methods. These use DatabaseControl namespace to communicate with server.
    IEnumerator LoginUser(string _playerUsername, string _playerPassword)
    {
        IEnumerator e = DCF.Login(_playerUsername, _playerPassword); // << Send request to login, providing username and password
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Success")
        {
            //Username and Password were correct. Stop showing 'Loading...' and show the LoggedIn UI. And set the text to display the username.
            ResetAllUIElements();
            loadingParent.gameObject.SetActive(false);
            loggedInParent.gameObject.SetActive(true);
            playerModel.enabled = true;
            playerModel2.enabled = true;
            LoggedIn_DisplayUsernameText.text = "Logged In As: " + UserAccount.LoggedIn_Username;
            
            LoggedIn_LoadDataButtonPressed();
        }
        else
        {
            //Something went wrong logging in. Stop showing 'Loading...' and go back to LoginUI
            loadingParent.gameObject.SetActive(false);
            loginParent.gameObject.SetActive(true);
            UserAccount.instance.LogOut();
            if (response == "UserError")
            {
                //The Username was wrong so display relevent error message
                Login_ErrorText.text = "Error: Username not Found";
            } else
            {
                if (response == "PassError")
                {
                    //The Password was wrong so display relevent error message
                    Login_ErrorText.text = "Error: Password Incorrect";
                } else
                {
                    //There was another error. This error message should never appear, but is here just in case.
                    Login_ErrorText.text = "Error: Unknown Error. Please try again later.";
                }
            }
        }
    }
    IEnumerator RegisterUser(string _playerUsername, string _playerPassword)
    {
        IEnumerator e = DCF.RegisterUser(_playerUsername, _playerPassword, "NameMePlz!"); // << Send request to register a new user, providing submitted username and password. It also provides an initial value for the data string on the account, which is "Hello World".
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Success")
        {
            //Username and Password were valid. Account has been created. Stop showing 'Loading...' and show the loggedIn UI and set text to display the username.
            ResetAllUIElements();
            loadingParent.gameObject.SetActive(false);
            loggedInParent.gameObject.SetActive(true);
            playerModel.enabled = true;
            playerModel2.enabled = true;
            LoggedIn_DisplayUsernameText.text = "Logged In As: " + _playerUsername;
            LoggedIn_LoadDataButtonPressed();
        }
        else
        {
            //Something went wrong logging in. Stop showing 'Loading...' and go back to RegisterUI
            loadingParent.gameObject.SetActive(false);
            registerParent.gameObject.SetActive(true);
            UserAccount.instance.LogOut();
            if (response == "UserError")
            {
                //The username has already been taken. Player needs to choose another. Shows error message.
                Register_ErrorText.text = "Error: Username Already Taken";
            } else
            {
                //There was another error. This error message should never appear, but is here just in case.
                Login_ErrorText.text = "Error: Unknown Error. Please try again later.";
            }
        }
    }
    IEnumerator GetData (string _playerUsername, string _playerPassword)
    {
        IEnumerator e = DCF.GetUserData(_playerUsername,_playerPassword); // << Send request to get the player's data string. Provides the username and password
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Error")
        {
            
                //There was another error. Automatically logs player out. This error message should never appear, but is here just in case.
                ResetAllUIElements();
                UserAccount.instance.LogOut();
                loginParent.gameObject.SetActive(true);
                loadingParent.gameObject.SetActive(false);
                Login_ErrorText.text = "Error: Unknown Error. Please try again later.";
         
        }
        else
        {
            
            //The player's data was retrieved. Goes back to loggedIn UI and displays the retrieved data in the InputField
            loadingParent.gameObject.SetActive(false);
            loggedInParent.gameObject.SetActive(true);
            playerModel.enabled = true;
            playerModel2.enabled = true;
            string[] responsePart = response.Split('¬');
            currentName = responsePart[0];
            UserAccount.GetCurrentNickName(currentName);
            LoggedIn_DataOutputField.text = currentName;
            if (responsePart.Length > 0)
            {
                string slidersValues = responsePart[1];
                Debug.Log("Sliders values: " + slidersValues);
                string[] _numbers = slidersValues.Split('°');
                Debug.Log("ValueA: " + _numbers[0] + "ValueB: " + _numbers[1] + "ValueC: " + _numbers[2]);

                PlayerModel pm = GetComponent<PlayerModel>();
                float _a, _b, _c;
                _a = System.Convert.ToSingle(_numbers[0]);
                _b = System.Convert.ToSingle(_numbers[1]);
                _c = System.Convert.ToSingle(_numbers[2]);
                pm.A.value = _a;
                pm.B.value = _b;
                pm.C.value = _c;
                pm.SetSlidersColor();
            }        
        }
    }
    
    IEnumerator SetData (string data)
    {
        IEnumerator e = DCF.SetUserData(UserAccount.LoggedIn_Username, UserAccount.LoggedIn_Password, data); // << Send request to set the player's data string. Provides the username, password and new data string
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; // << The returned string from the request

        if (response == "Success")
        {
           
                //The data string was set correctly. Goes back to LoggedIn UI
                loadingParent.gameObject.SetActive(false);
                loggedInParent.gameObject.SetActive(true);
                playerModel2.enabled = true;
                playerModel.enabled = true;
           
        }
        else
        {
            
                //There was another error. Automatically logs player out. This error message should never appear, but is here just in case.
                ResetAllUIElements();
                UserAccount.instance.LogOut();
                loginParent.gameObject.SetActive(true);
                loadingParent.gameObject.SetActive(false);
                Login_ErrorText.text = "Error: Unknown Error. Please try again later.";
           
        }
    }

    //UI Button Pressed Methods
    public void Login_LoginButtonPressed ()
    {
        //Called when player presses button to Login

        //Get the username and password the player entered
        string _playerUsername = Login_UsernameField.text;
        string _playerPassword = Login_PasswordField.text;

        //Check the lengths of the username and password. (If they are wrong, we might as well show an error now instead of waiting for the request to the server)
        if (_playerUsername.Length > 3)
        {
            if (_playerPassword.Length > 5)
            {
                //Username and password seem reasonable. Change UI to 'Loading...'. Start the Coroutine which tries to log the player in.
                loginParent.gameObject.SetActive(false);
                loadingParent.gameObject.SetActive(true);
                UserAccount.instance.LogIn(_playerUsername,_playerPassword);
                StartCoroutine(LoginUser(_playerUsername,_playerPassword));
            }
            else
            {
                //Password too short so it must be wrong
                Login_ErrorText.text = "Error: Password Incorrect";
            }
        } else
        {
            //Username too short so it must be wrong
            Login_ErrorText.text = "Error: Username Incorrect";
        }
    }
    public void Login_RegisterButtonPressed ()
    {
        //Called when the player hits register on the Login UI, so switches to the Register UI
        ResetAllUIElements();
        loginParent.gameObject.SetActive(false);
        registerParent.gameObject.SetActive(true);
    }
    public void Register_RegisterButtonPressed ()
    {
        //Called when the player presses the button to register

        //Get the username and password and repeated password the player entered
        string _playerUsername = Register_UsernameField.text;
        string _playerPassword = Register_PasswordField.text;
        string _confirmedPassword = Register_ConfirmPasswordField.text;

        //Make sure username and password are long enough
        if (_playerUsername.Length > 3)
        {
            if (_playerPassword.Length > 5)
            {
                //Check the two passwords entered match
                if (_playerPassword == _confirmedPassword)
                {
                    //Username and passwords seem reasonable. Switch to 'Loading...' and start the coroutine to try and register an account on the server
                    registerParent.gameObject.SetActive(false);
                    loadingParent.gameObject.SetActive(true);
                    UserAccount.instance.LogIn(_playerUsername, _playerPassword);
                    StartCoroutine(RegisterUser(_playerUsername, _playerPassword));
                }
                else
                {
                    //Passwords don't match, show error
                    Register_ErrorText.text = "Error: Password's don't Match";
                }
            }
            else
            {
                //Password too short so show error
                Register_ErrorText.text = "Error: Password too Short";
            }
        }
        else
        {
            //Username too short so show error
            Register_ErrorText.text = "Error: Username too Short";
        }
    }
    public void Register_BackButtonPressed ()
    {
        //Called when the player presses the 'Back' button on the register UI. Switches back to the Login UI
        ResetAllUIElements();
        loginParent.gameObject.SetActive(true);
        registerParent.gameObject.SetActive(false);
    }
    public void LoggedIn_SaveDataButtonPressed ()
    {
        //Called when the player hits 'Set Data' to change the data string on their account. Switches UI to 'Loading...' and starts coroutine to set the players data string on the server
        loadingParent.gameObject.SetActive(true);
        playerModel.enabled = false;
        playerModel2.enabled = false;
        loggedInParent.gameObject.SetActive(false);
        PlayerModel _playerColor = GetComponent<PlayerModel>();
        float _a, _b, _c;
        _a = _playerColor.A.value;
        _b = _playerColor.B.value;
        _c = _playerColor.C.value;
        Color _currentColor = _playerColor.temp;
        
        Debug.Log("GUARDANDO: COLOR _a = "+_a.ToString()+" COLOR _b = "+_b.ToString()+"  Color _c = "+_c.ToString());
        currentName = LoggedIn_DataInputField.text;




        //UserAccount.GetCurrentColor(_currentColor);

        //UserAccount.GetCurrentNickName(currentName);




        GameObject PSV = Instantiate(PLayerStartValues) as GameObject;
        PSV.name = "PSV_Values";
        PSV.GetComponent<PlayerStartValues>().Nickname = currentName;
        PSV.GetComponent<PlayerStartValues>().startColor = _currentColor;
        PSV.GetComponent<PlayerStartValues>().startColor.a = 1;
        DontDestroyOnLoad(PSV);

        string _data = currentName + "¬" + _a.ToString() +"°"+ _b.ToString() +"°"+ _c.ToString();
        StartCoroutine(SetData(_data));
    }
    public void LoggedIn_LoadDataButtonPressed ()
    {
        //Called when the player hits 'Get Data' to retrieve the data string on their account. Switches UI to 'Loading...' and starts coroutine to get the players data string from the server
        loadingParent.gameObject.SetActive(true);
        playerModel.enabled = false;
        playerModel2.enabled = false;
        loggedInParent.gameObject.SetActive(false);
        StartCoroutine(GetData(UserAccount.LoggedIn_Username , UserAccount.LoggedIn_Password ));
    }
    public void LoggedIn_LogoutButtonPressed ()
    {
        //Called when the player hits the 'Logout' button. Switches back to Login UI and forgets the player's username and password.
        //Note: Database Control doesn't use sessions, so no request to the server is needed here to end a session.
        ResetAllUIElements();
        loginParent.gameObject.SetActive(true);
        playerModel.enabled = false;
        playerModel2.enabled = false;
        loggedInParent.gameObject.SetActive(false);
        UserAccount.instance.LogOut();
    }

    public void ChangeLevel(bool areSettingsChanged)
    {
        if (areSettingsChanged==true)
        {
            LoggedIn_SaveDataButtonPressed();
        }
        lm.LoadLevel("Menu");
    }

    private void OnApplicationQuit()
    {
        UserAccount.instance.LogOut();
    }
}
