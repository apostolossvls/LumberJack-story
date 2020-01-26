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
    static bool loadingScene;

    void Awake()
    {
        if (!self) self = this;
        else Destroy(this);
        loadingScene = false;
        mainZLine = mainZLineInspector;
        secondZLine = secondZLineInspector;
    }

    public static void RestartScene(){
        if (!loadingScene){
            loadingScene=true;
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public static void LoadSceneIndex(int index){
        if (!loadingScene){
            loadingScene=true;
            SceneManager.LoadSceneAsync(index);
        }
    }
}
