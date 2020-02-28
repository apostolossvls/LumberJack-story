using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{    
    public static PauseMenuManager instance;
    public bool onMainMenu = false;
    public GameObject menuParent;
    public Button[] menuButtons;
    public GameObject[] planes;
    public Selectable FirstSelected;
    //graphics
    [Header("Graphics")]
    public Toggle[] graphicsToggles;
    public Slider[] graphicsSliders;
    public TMP_Dropdown[] graphicsdropdowns;
    public int qualityIndex;
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
    public Toggle[] gameplayToggles;
    public bool showInteractIndicator;
    public bool showDogVision;
    public bool showHintIndicator;
    //controls
    [Header("Controls")]
    static float myTimeScale;
    static bool onPause;
    static bool loading;
    public bool draggingSelectable = false;

    void Awake(){
        if (instance!=null && instance!=this){
            Destroy(this.gameObject);
        }
        else {
            instance = this;
        }
        onPause = false;
        loading = false;
        SaveManager.instance.Load();
    }

    void Update (){
        if (Input.GetButtonUp("Pause") && !onMainMenu){
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
        SaveManager.instance.Load();
        instance.menuParent.gameObject.SetActive(true);
        instance.ResetButtonAndPlanes();
        if (instance.FirstSelected) instance.FirstSelected.Select();
        if (!loading) AudioManager.instance.Play("MenuClick");
    }

    public void MainMenuSetup(){
        SaveManager.instance.Load();
        instance.ResetButtonAndPlanes();
        if (instance.FirstSelected) instance.FirstSelected.Select();
    }
    
    public void Unpause(){
        onPause = false;
        SaveManager.instance.Save();
        instance.menuParent.gameObject.SetActive(false);
        Time.timeScale = myTimeScale;
        /*if (SceneManager.GetActiveScene().buildIndex==1){
            //if (previousGameState!=0) h.GameState=previousGameState;
            //else h.GameState=1;
            //h.LightsParent.GetComponent<Animator>().speed=1;
            //h.ChangeGameStateOnEnemies();
        }*/
        if (!loading) AudioManager.instance.Play("MenuClick");
    }

    public void ShowPlane(int index){
        for (int i = 0; i < planes.Length; i++)
        {
            if (i==index){
                //menuButtons[i].interactable = false;
                planes[i].SetActive(true);
            }
            else {
                //menuButtons[i].interactable = true;
                planes[i].SetActive(false);
            }
        }
        if (!loading) AudioManager.instance.Play("MenuClick");
    }

    //Quality
    void ChangeQuality(){
        QualitySettings.SetQualityLevel(QualitySettings.names.Length-1 - qualityIndex, true);
        //Debug.Log(QualitySettings.GetQualityLevel().ToString());
        //Debug.Log(QualitySettings.names[QualitySettings.GetQualityLevel()].ToString());
    }
    public void ChangeQualityDropDown(TMP_Dropdown dr){
        qualityIndex = dr.value;
        ChangeQuality();
        if (!loading) AudioManager.instance.Play("MenuClick");
    }
    void ChangeQualityDropDown(){
        graphicsdropdowns[0].value = qualityIndex;
    }
    
    void ShowFPS (){
        if (FPStext) FPStext.SetActive(isShowFPS);
    }
    public void ShowFPSToggle (Toggle t){
        isShowFPS = t.isOn;
        ShowFPS();
        if (!loading) AudioManager.instance.Play("MenuClick");
    }
    void ShowFPSToggle (){
        graphicsToggles[0].isOn = isShowFPS;
    }
    //end Quality

    //Audio
    public void SetVolume (Slider s){
        for (int i = 0; i < audioSliders.Length; i++)
        {
            if (audioSliders[i]==s){
                float v = s.value;
                if (v<0) v*=4f;
                Volumes[i] = v;
                if (!AudioMute[i]) audioMixer.SetFloat(s.name, Volumes[i]);
                break;
            }
        }
        //if (!loading && !draggingSelectable) AudioManager.instance.Play("MenuClick");
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
                AudioMute[i]=!t.isOn;
                break;
            }
        }
        if (!loading) AudioManager.instance.Play("MenuClick");
    }

    public void VolumeAllConfig (){
        for (int i = 0; i < Volumes.Length; i++)
        {
            if (!AudioMute[i]) {
                audioMixer.SetFloat(audioSliders[i].name, Volumes[i]);
            }
            float v = Volumes[i];
            if (v<0) v/=4f;
            audioSliders[i].value = v;
        }
    }

    public void AudioMuteAllConfig (){
        for (int i = 0; i < audioToggles.Length; i++)
        {
            if (AudioMute[i]){
                audioMixer.SetFloat(audioToggles[i].name, -80);
            }
            else
            {
                audioMixer.SetFloat(audioToggles[i].name, Volumes[AudioToggleToSlider(audioToggles[i])]);
            }
            audioToggles[i].isOn = !AudioMute[i];
        }
    }

    int AudioToggleToSlider(Toggle t){
        for (int i = 0; i < audioSliders.Length; i++)
        {
            if (t.name == audioSliders[i].name) return i;
        }
        return -1;
    }
    //end Audio

    //Gameplay
    void InteractIndicator(){
        foreach (InteractIndicator ind in Object.FindObjectsOfType<InteractIndicator>())
        {
            ind.active = showInteractIndicator;
            ind.IsActive();
        }
    }
    public void InteractIndicatorToggle(Toggle t){
        showInteractIndicator = t.isOn;
        if (!onMainMenu) InteractIndicator();
        if (!loading) AudioManager.instance.Play("MenuClick");
    }
    void InteractIndicatorSetToggle(){
        gameplayToggles[0].isOn = showInteractIndicator;
    }

    void DogPPVolume(){
        GameObject.FindGameObjectWithTag("PlayerDog").GetComponentInChildren<UnityEngine.Rendering.PostProcessing.PostProcessVolume>(true).gameObject.SetActive(showDogVision);
    }
    public void DogPPVolumeToggle(Toggle t){
        showDogVision = t.isOn;
        if (!onMainMenu) DogPPVolume();
        if (!loading) AudioManager.instance.Play("MenuClick");
    }
    void DogPPVolumeSetToggle(){
        gameplayToggles[1].isOn = showDogVision;
    }

    void ShowHint(){
        foreach (InteractIndicator ind in Object.FindObjectsOfType<InteractIndicator>())
        {
            ind.showHint = showHintIndicator;
            if (!showHintIndicator) ind.DisplayHint(false);
        }
    }
    public void ShowHintToggle(Toggle t){
        showHintIndicator = t.isOn;
        if (!onMainMenu) ShowHint();
        if (!loading) AudioManager.instance.Play("MenuClick");
    }
    void ShowHintSetToggle(){
        gameplayToggles[2].isOn = showHintIndicator;
    }
    //end Gameplay

    public void ExitToMainMenu(int index){
        LevelSettings.LoadSceneIndex(index);
        Debug.Log("Exit");
    }

    public void OnSliderBeginDrag(){
        if (!loading) AudioManager.instance.Play("MenuClick");
    }
    public void OnSliderdEndDrag(){
        if (!loading) AudioManager.instance.Play("MenuClick");
    }

    public void OnSliderdMove(){
        if (!loading) AudioManager.instance.Play("MenuClick");
    }

    public void SetAll(GameData data){
        loading = true;

        //Audio
        AudioMute = data.audioMutes;
        Volumes = data.audioVolumes;
        VolumeAllConfig();
        AudioMuteAllConfig();

        //Quality
        qualityIndex = data.qualityIndex;
        isShowFPS = data.qualityShowFPS;
        ChangeQuality();
        ChangeQualityDropDown();
        ShowFPS();
        ShowFPSToggle();

        //Gameplay
        showInteractIndicator = data.gameplayShowInteractIndicator;
        showHintIndicator = data.gameplayShowHintIndicator;
        showDogVision = data.gameplayShowDogVision;
        if (!onMainMenu){
            InteractIndicator();
            DogPPVolume();
            ShowHint();
        }
        InteractIndicatorSetToggle();
        DogPPVolumeSetToggle();
        ShowHintSetToggle();

        loading = false;
    }
}
