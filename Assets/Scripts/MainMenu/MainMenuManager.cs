using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;
    public Transform target;
    public Transform startPanelTransform;
    public Transform scenePanelTransform;
    void Awake(){
        if (Instance!=null && Instance!=this){
            Destroy(this.gameObject);
        }
        else {
            Instance = this;
        }
    }

    public void GoToStart(){
        target.SetPositionAndRotation(startPanelTransform.position, startPanelTransform.rotation);
    }

    public void GoToScenePanel(){
        target.SetPositionAndRotation(scenePanelTransform.position, scenePanelTransform.rotation);
    }

    public void LoadScene(int index){
        SceneManager.LoadSceneAsync(index);
    }

    public void QuitGame(){
        Application.Quit();
    }
}
