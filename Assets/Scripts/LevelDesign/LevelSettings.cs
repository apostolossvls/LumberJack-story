using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSettings : MonoBehaviour
{
    public static LevelSettings self;

    //Zlines
    public static float mainZLine=0;
    public static float secondZLine=-1;
    public float mainZLineInspector=0;
    public float secondZLineInspector=-1;
    // Start is called before the first frame update
    void Awake()
    {
        if (!self) self = this;
        else Destroy(this);
        mainZLine = mainZLineInspector;
        secondZLine = secondZLineInspector;
    }

    public static void RestartScene(){
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
