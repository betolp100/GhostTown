using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private static AudioManager instance=null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)   //Singleton Algorythm
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    void Start ()
    {
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
