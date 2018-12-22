using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Rigidbody))]
public class PlayerModel : MonoBehaviour
{
    public Text nameField, placeHolder;
    public Slider A, B, C;
    private Material mat;
    private Material mat2;
    public Color temp;
    private Renderer rend;
    private Renderer rend2;
    private LevelManager lm;
    private bool startDone = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        rend = GetComponent<Renderer>();
        rend2 = transform.GetChild(0).GetComponent<Renderer>();
        mat = rend.material;
        mat2 = rend2.material;

        A.value = temp.r;
        B.value = temp.g;
        C.value = temp.b;
        startDone = true;
    }

    public void SetSlidersColor()
    {
        if (startDone == false) return;
        temp.r = A.value;
        temp.g = B.value;
        temp.b = C.value;
        mat.color = temp;
        mat2.color = temp;
    }

    public void ChangeLevel(string level)
    {
        lm.LoadLevel(level);
    }
}
