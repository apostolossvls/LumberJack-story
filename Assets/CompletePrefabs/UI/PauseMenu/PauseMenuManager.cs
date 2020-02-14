using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager Instance;
    //public AudioManager audioManager;
    //public int previousGameState=0;
    public GameObject menuParent;
    public Button[] menuButtons;
    public GameObject[] planes;
    //graphics
    [Header("Graphics")]
    public Toggle[] graphicsToggles;
    public Slider[] graphicsSliders;
    public TMP_Dropdown[] graphicsdropdowns;
    public GameObject FPStext;
    public bool isShowFPS;
    //audio
    [Header("Audio")]
    public AudioMixer audioMixer;
    public Toggle[] audioToggles;
    public Slider[] audioSliders;
    public float[] Volumes = new float[6];
    public bool[] AudioMute = new bool[1];
    //gameplay
    [Header("Gameplay")]
    //controls
    [Header("Controls")]
    static float myTimeScale;
    static bool onPause;

    void Awake(){
        if (Instance!=null && Instance!=this){
            Destroy(this.gameObject);
        }
        else {
            Instance = this;
        }
        onPause = false;
    }

    void Update (){
        if (Input.GetButtonUp("Pause")){
            if (onPause) Unpause();
            else Pause();
        }
    }
    
    void ResetButtonAndPlanes(){
        for (int i = 0; i < planes.Length; i++)
        {
            planes[i].SetActive(false);
            menuButtons[i].interactable = true;
        }
    }

    public static void Pause(){
        onPause = true;
        myTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        Instance.menuParent.gameObject.SetActive(true);
        Instance.ResetButtonAndPlanes();
    }
    
    public void Unpause(){
        onPause = false;
        Instance.menuParent.gameObject.SetActive(false);
        SaveOptions();
        Time.timeScale = myTimeScale;
        /*if (SceneManager.GetActiveScene().buildIndex==1){
            //if (previousGameState!=0) h.GameState=previousGameState;
            //else h.GameState=1;
            //h.LightsParent.GetComponent<Animator>().speed=1;
            //h.ChangeGameStateOnEnemies();
        }*/
        //audioManager.UIaudio1[1].Play();
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
        isShowFPS=tog.isOn;
        FPStext.SetActive(isShowFPS);
    }

    public void SetVolume (Slider s){
        for (int i = 0; i < audioSliders.Length; i++)
        {
            if (audioSliders[i]==s){
                float v = s.value;
                if (v<0) v*=4f;
                Volumes[i] = v;
                audioMixer.SetFloat(s.name, Volumes[i]);
                //if (audioSliders[1]!=s || audioToggles[3].isOn) audioManager.audioMixer.SetFloat(s.name, Volumes[i-1]);
                break;
            }
        }
    }

    public void SetAudioMute(Toggle t){
        for (int i = 0; i < audioToggles.Length; i++)
        {
            if (t == audioToggles[i]){
                if (!t.isOn){
                    audioMixer.SetFloat(t.name, -80);
                }
                else {
                    audioMixer.SetFloat(t.name, Volumes[AudioToggleToSlider(t)]);
                }
                AudioMute[i]=t.isOn;
                break;
            }
        }
    }

    int AudioToggleToSlider(Toggle t){
        for (int i = 0; i < audioSliders.Length; i++)
        {
            if (t.name == audioSliders[i].name) return i;
        }
        return -1;
    }
}
