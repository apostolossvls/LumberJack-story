using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager Instance;
    //public AudioManager audioManager;
    public int previousGameState=0;
    public Button[] menuButtons;
    public GameObject[] planes;
    public Toggle[] toggles;
    public Slider[] sliders;
    public TMP_Dropdown[] dropdowns;
    public GameObject FPStext;
    public bool isShowFPS;
    public float[] Volumes = new float[6];
    public bool[] AudioMute = new bool[1];
    static float myTimeScale;

    void Awake(){
        if (Instance!=null && Instance!=this){
            Destroy(this.gameObject);
        }
        else {
            Instance = this;
        }
    }
    
    void OnEnable(){
        for (int i = 0; i < planes.Length; i++)
        {
            planes[i].SetActive(false);
            menuButtons[i].interactable = true;
        }
        myTimeScale = Time.timeScale;
    }

    public static void Pause(){
        myTimeScale = Time.timeScale;
        Time.timeScale = 0;
        Instance.gameObject.SetActive(true);
    }
    
    public void Unpause(){
        /*if (SceneManager.GetActiveScene().buildIndex==1){
            //if (previousGameState!=0) h.GameState=previousGameState;
            //else h.GameState=1;
            //h.LightsParent.GetComponent<Animator>().speed=1;
            //h.ChangeGameStateOnEnemies();
        }*/
        //audioManager.UIaudio1[1].Play();
        this.gameObject.SetActive(false);
        SaveOptions();
        Time.timeScale = myTimeScale;
    }

    public void ShowPlane(int index){
        for (int i = 0; i < planes.Length; i++)
        {
            if (i==index){
                menuButtons[i].interactable = false;
                planes[i].SetActive(true);
            }
            else {
                menuButtons[i].interactable = true;
                planes[i].SetActive(false);
            }
        }
        //audioManager.UIaudio1[0].Play();
    }

    public void SaveOptions(){
        //GameObject.Find("GameData").GetComponent<GameData>().SaveGameDataOptions(this);
    }

    /*public void LoadOptions(GameData gd){
        QualitySettings.SetQualityLevel(gd.OptionsQualityIndex, true);
        //Debug.Log("options quality: "+gd.OptionsQualityIndex+" level: "+QualitySettings.names[gd.OptionsQualityIndex]);
        isShowFPS=gd.OptionsShowFPS;
        isShowKickButton=gd.OptionsShowKickButton;
        toggles[0].isOn = gd.OptionsShowFPS;
        toggles[1].isOn = gd.OptionsShowTutorialCircle;
        toggles[2].isOn = gd.OptionsShowKickButton;
        dropdowns[0].value = QualitySettings.names.Length-1 - gd.OptionsQualityIndex;
        sliders[0].value = gd.OptionsaccelerationSqrMagnitude;
        if (gd.OptionsVolumes.Length>=6) Volumes = gd.OptionsVolumes;
        //AudioMute = gd.OptionsAudioMutes;
        for (int i = 1; i < 8; i++)
        {
            if (Volumes.Length>=i){
                float v = Volumes[i-1];
                if (v<0) v/=4f;
                Volumes[i-1] = v;
                sliders[i].value = Volumes[i-1];
            }
        }
        for (int i = 3; i < 3+gd.OptionsAudioMutes.Length; i++)
        {
            AudioMute[i-3] = gd.OptionsAudioMutes[i-3];
            toggles[i].isOn = AudioMute[i-3];
            SetMasterAudioMute();
        }
        if (SceneManager.GetActiveScene().buildIndex!=0){
            h.ShowTutorialCircle = gd.OptionsShowTutorialCircle;
            KickButton.SetActive(gd.OptionsShowKickButton);
            FPStext.SetActive(gd.OptionsShowFPS);
        }
    }
    */

    public void ChangeQuality(TMP_Dropdown dr){
        QualitySettings.SetQualityLevel(QualitySettings.names.Length-1 - dr.value, true);
        //Debug.Log(QualitySettings.GetQualityLevel().ToString());
        //Debug.Log(QualitySettings.names[QualitySettings.GetQualityLevel()].ToString());
    }
    
    public void ShowFPS (Toggle tog){
        if (SceneManager.GetActiveScene().buildIndex==1){
            FPStext.SetActive(tog.isOn);
        }
        isShowFPS=tog.isOn;
    }

    public void SetVolume (Slider s){
        /*
        for (int i = 1; i < 8; i++)
        {
            if (sliders[i]==s){
                float v = s.value;
                if (v<0) v*=4f;
                Volumes[i-1] = v;
                if (sliders[1]!=s || toggles[3].isOn) audioManager.audioMixer.SetFloat(s.name, Volumes[i-1]);
                break;
            }
        }
        */
    }

    public void SetMasterAudioMute(){
        /*
        for (int i = 3; i < 4; i++)
        {
            string s="MasterVolume";
            if (i==3) s = "MasterVolume";
            if (!toggles[i].isOn){
                audioManager.audioMixer.SetFloat(s, -80);
            }
            else {
                audioManager.audioMixer.SetFloat(s, Volumes[0]);
            }
            AudioMute[i-3]=toggles[i].isOn;
        }
        */
    }
}
