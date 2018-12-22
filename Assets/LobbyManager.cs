using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LobbyManager : MonoBehaviour {

    //Scene
    public int decision;
    public bool done, isMakingServer, isSelectingHost;
    public GameObject ClientPanel, ClientPanel_2, HostPanel, HostPanel_2;

    private LevelManager lm;

    private void Start()
    {
        done = false;
        isMakingServer = false;
        isSelectingHost = false;
        HostPanel.SetActive(true);
         
        ClientPanel.SetActive(true);
        HostPanel_2.SetActive(false);
        ClientPanel_2.SetActive(false);
        decision = 0;

        lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    public void GoBackToMenu()
    {
        Destroy(GameObject.Find("NetworkManager"));
        lm.LoadLevel("Menu");
    }


    public void B_MAKING_SERVER()
    {
        decision = 1;
        HostPanel_2.transform.parent.GetComponent<Image>().color = HostPanel_2.transform.parent.GetComponent<Button>().colors.highlightedColor;
        HostPanel_2.transform.parent.GetComponent<Button>().enabled = false;
        isSelectingHost = false;
        isMakingServer = true;
        HostPanel.SetActive(false);
        ClientPanel.SetActive(false);
        ClientPanel_2.SetActive(false);
        HostPanel_2.SetActive(true);
    }

    public void B_SELECTING_HOST()
    {
        decision = 2;
        ClientPanel_2.transform.parent.GetComponent<Image>().color = ClientPanel_2.transform.parent.GetComponent<Button>().colors.highlightedColor;
        ClientPanel_2.transform.parent.GetComponent<Button>().enabled = false;
        isMakingServer = false;
        isSelectingHost = true;
        HostPanel.SetActive(false);
        HostPanel_2.SetActive(false);
        ClientPanel.SetActive(false);
        ClientPanel_2.SetActive(true);
    }
    
    public void ReturnToDecision()
    {
        if(decision==1) HostPanel_2.transform.parent.GetComponent<Button>().enabled = true;
        if (decision == 2) ClientPanel_2.transform.parent.GetComponent<Button>().enabled = true;
        decision = 0;
        isMakingServer = false;
        isSelectingHost = false;

        ClientPanel_2.SetActive(false);
        HostPanel_2.SetActive(false);
        ClientPanel.SetActive(true);
        HostPanel.SetActive(true);
    }

}
