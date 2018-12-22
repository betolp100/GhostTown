using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    public GameObject content, playerText, desc,effect; 
    private Animator anim;
    private LevelManager lm;
    public Text head;
    private bool canPressStart, done;
    public float speed;
    void Start ()
    {
        done = false;
        desc.SetActive(false);
        canPressStart = false;
        StartCoroutine(TextAppear());
        lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        anim = content.GetComponent<Animator>();
        StartCoroutine(AnimateText("Name in progress..."));        
        anim.SetBool("go", false);
    }

    // Update is called once per frame
    void Update ()
    {
        float rdm = Random.Range(speed,speed*2);
        effect.transform.Rotate(0, 0, rdm * Time.deltaTime, Space.Self);
        if (done == false && canPressStart == true && Input.GetButton("Start"))
        {
            Debug.Log("PressEnter");
            canPressStart = false;
            done = true;
            PressStart();
        }
        else return;   
    }

    IEnumerator TextAppear()
    {
        yield return new WaitForSeconds(2);
        desc.SetActive(true);
    }

 

    public void PressStart()
    {
        lm.LoadLevel("DataBase");
    }

    IEnumerator AnimateText(string strComplete)
    {
        Debug.Log("hola");
        yield return new WaitForSeconds(.75f);
        int i = 0;
        head.text = "";
        head.gameObject.SetActive(true);
        while (i < strComplete.Length)
        {
            head.text += strComplete[i++];
            if (i == strComplete.Length)
            {
                yield return new WaitForSeconds(0.45f);
                head.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(.175f);

        }
        
        canPressStart = true;
    }
   
}
