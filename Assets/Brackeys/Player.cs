using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour
{
    public int kills;
    public int deaths;

    [SyncVar]
    public string nickName="Loading...";
    [SyncVar]
    public Color playerColor = Color.red;

    private bool _isDead = false;
    [SerializeField]
    private GameObject deathEffect;
    [SerializeField]
    private GameObject spawnEffect;

    public bool isDead
    {
        get{ return _isDead; }
        protected set{ _isDead = value; }
    }

    [SerializeField] private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    [SerializeField]
    private GameObject[] disableGameObjectsOnDeath; 
    [SerializeField]
    private bool[] wasEnabled;
    private bool firstSetup = true;


    void Awake()
    {
        SetDefaults();
    }
    /*
    void Update()
    {
        if (!isLocalPlayer) return;
        if (Input.GetKeyDown(KeyCode.K))
            RpcTakeDamage(9999); 
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DeadZone") RpcTakeDamage(9999, "");
    }

    public void SetupPlayer()
    {
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(false);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        }

        CmdBroadCastNewPlayerSetup();
    }

    [Command]
    private void CmdBroadCastNewPlayerSetup()
    {
        RpcPlayerSetupOnAllClients();
    }

    [ClientRpc]
    private void RpcPlayerSetupOnAllClients ()
    {
        if (firstSetup == true)
        {
            wasEnabled = new bool[disableOnDeath.Length];

            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;

            }
            firstSetup = false;
        }
        SetDefaults();
    }

    public void SetDefaults()
    {
        isDead = false;

        currentHealth = maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(true);
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
        {
            _col.enabled = true;
        }

        GameObject _graphicsGFXInstance = (GameObject)Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(_graphicsGFXInstance, 1f);
    }
     
    [ClientRpc]
    public void RpcTakeDamage(int mount, string _sourceID )
    {
        if (isDead == true) return;
        currentHealth -= mount;

        Debug.Log(transform.name + " now has " + currentHealth + " health.");
        if (currentHealth <= 0)
        {
            Die(_sourceID);
        }

    }

    private void Die(string _sourceID)
    {
        isDead = true;

        Player sourcePlayer = GameManager.GetPlayer(_sourceID);
        if (_sourceID != null)
        {
            sourcePlayer.kills++;
        }

        deaths++;

        Collider _col = GetComponent<Collider>();
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;

        }
        for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
        {
            disableGameObjectsOnDeath[i].SetActive(false);
        }

        if (_col != null)
        {
            _col.enabled = true;
        }

        GameObject _graphicsGFXInstance = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_graphicsGFXInstance,1f);
        Debug.Log(transform.name + " is dead... :c");

        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }

        StartCoroutine(Respawn()); 
    }

    private IEnumerator Respawn()  
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);
        
        Transform _spawnpoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnpoint.position;
        transform.rotation = _spawnpoint.rotation;
        yield return new WaitForSeconds(0.15f);
        SetupPlayer();

        SetDefaults();
    }
}
