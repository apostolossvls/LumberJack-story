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

    //scene
    static bool reloadingScene;

    void Awake()
    {
        if (!self) self = this;
        else Destroy(this);
        reloadingScene = false;
        mainZLine = mainZLineInspector;
        secondZLine = secondZLineInspector;
    }

    public static void RestartScene(){
        if (!reloadingScene){
            reloadingScene=true;
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
