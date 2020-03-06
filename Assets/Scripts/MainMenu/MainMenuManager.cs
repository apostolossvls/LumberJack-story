using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;
    public Transform target;
    public int pointer=0;
    public Transform[] canvas; //start=0 , levels=1
    public Transform optionCanvas;
    public Button[] levelButtons;
    public Selectable[] firstSelectables;
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
        firstSelectables[0].Select();
        PauseMenuManager.instance.FullcreenCheck();
    }

    public void GoToPanel(int index){
        pointer = index;
        firstSelectables[index+1].Select();
        target.SetPositionAndRotation(canvas[pointer].position, canvas[pointer].rotation);
        AudioManager.instance.Play("MenuClick");
    }

    public void GoToOptions(){
        if (PauseMenuManager.instance){
            PauseMenuManager.instance.MainMenuSetup();

            pointer = 2;
            target.SetPositionAndRotation(optionCanvas.position, optionCanvas.rotation);
        }
        AudioManager.instance.Play("MenuClick");
    }
    public void LeaveOptions(int index){
        if (PauseMenuManager.instance){
            SaveManager.instance.Save();

            pointer = index;
            firstSelectables[index+1].Select();
            target.SetPositionAndRotation(canvas[pointer].position, canvas[pointer].rotation);
        }
        AudioManager.instance.Play("MenuClick");
    }

    public void SetLevelInfo(){
        for (int i = 0; i < levelButtons.Length; i++)
        {
            LevelData level = LevelDataManager.instance.levelDatas[i];
            levelButtons[i].GetComponentInChildren<Toggle>().isOn = level.cleared;
        }
    }

    public void LoadLevel(int pointer){
        int index = -1;
        for (int i = 0; i < LevelDataManager.instance.levelDatas.Length; i++)
        {
            if (LevelDataManager.instance.levelDatas[i].buildIndex == pointer){
                index = LevelDataManager.instance.levelDatas[i].buildIndex;
            }
        }

        LevelDataManager.instance.levelIndex = index;

        SceneManager.LoadSceneAsync(index);
    }

    public void QuitGame(){
        Application.Quit();
    }
}
