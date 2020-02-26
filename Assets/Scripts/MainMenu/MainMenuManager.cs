using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;
    public Transform target;
    public int pointer=0;
    public Transform[] canvas; //start=0 , levels=1
    public Transform optionCanvas;
    void Awake(){
        if (Instance!=null && Instance!=this){
            Destroy(this.gameObject);
        }
        else {
            Instance = this;
        }
    }

    void Start(){
        pointer = 0;
    }

    public void GoToPanel(int index){
        pointer = index;
        target.SetPositionAndRotation(canvas[pointer].position, canvas[pointer].rotation);
    }

    public void GoToOptions(){
        if (PauseMenuManager.instance){
            PauseMenuManager.instance.MainMenuSetup();

            pointer = 2;
            target.SetPositionAndRotation(optionCanvas.position, optionCanvas.rotation);
        }
    }
    public void LeaveOptions(int index){
        if (PauseMenuManager.instance){
            SaveManager.instance.Save();

            pointer = index;
            target.SetPositionAndRotation(canvas[pointer].position, canvas[pointer].rotation);
        }
    }

    public void LoadScene(int index){
        SceneManager.LoadSceneAsync(index);
    }

    public void QuitGame(){
        Application.Quit();
    }
}
