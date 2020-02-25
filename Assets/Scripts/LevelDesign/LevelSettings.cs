using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class LevelSettings : MonoBehaviour
{
    public static LevelSettings instance;

    //Zlines
    public static float mainZLine=0;
    public static float secondZLine=-1;
    public float mainZLineInspector=0;
    public float secondZLineInspector=-1;
    public GameObject checkpoint;

    //scene
    static bool loadingScene;

    void Awake()
    {
        if (!instance) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        loadingScene = false;
        mainZLine = mainZLineInspector;
        secondZLine = secondZLineInspector;
    }

    public static void RestartScene(bool OnCheckpoint=true){
        if (instance.checkpoint && OnCheckpoint){
            Checkpoint3 c = instance.checkpoint.GetComponent<Checkpoint3>();
            if (c){
                c.ResetOnCheckpoint();
                return;
            }
        }
        instance.StartCoroutine(LoadSceneIndexCoroutine(SceneManager.GetActiveScene().buildIndex, false));
    }

    public static void LoadSceneIndex(int index, bool resetCheckpoint=true){
        instance.StartCoroutine(LoadSceneIndexCoroutine(index, resetCheckpoint));
    }

    private static IEnumerator LoadSceneIndexCoroutine(int index, bool resetCheckpoint=true){
        if (!loadingScene){
            loadingScene=true;
            SaveManager.instance.Save();
            AsyncOperation async = SceneManager.LoadSceneAsync(index);
            while (!async.isDone){
                //do loop on loading
                yield return null;
            }
            if (resetCheckpoint && instance.checkpoint) Destroy(instance.checkpoint);
            else instance.SetupOnCheckpoint();
        }
    }

    void OnApplicationQuit()
    {
        SaveManager.instance.Save();
    }

    public void SetCheckpoint(GameObject g){
        if (checkpoint) Destroy(checkpoint);
        checkpoint = g;
        checkpoint.transform.parent = null;
        DontDestroyOnLoad(checkpoint);
    }

    public void SetupOnCheckpoint(){
        if (checkpoint){
            if (checkpoint.GetComponent<Checkpoint>()){
                //
            }
            else if (checkpoint.GetComponent<Checkpoint2>()){
                StartCoroutine(checkpoint.GetComponent<Checkpoint2>().LoadPlayers());
            }
        }
    }
}
