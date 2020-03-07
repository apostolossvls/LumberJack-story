using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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
    public int resolutionIndex;
    Resolution[] resolutions;
    public bool isFullscreen;
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
    public int languageIndex;
    //controls
    [Header("Controls")]
    public GameObject[] controlsPages;
    public GameObject[] controlsPageIndicator;
    public Toggle[] controlsTogglesKeyboard;
    public Toggle[] controlsTogglesGamepad;
    public GameObject controlBarrier;
    public List<string> controlsPaths = new List<string>();
    public List<string> controlsID = new List<string>();
    public InputBinding testbinding;
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
        loading = false;
    }

    void Start () {
        onPause = false;
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
        controlBarrier.SetActive(false);
    }

    public static void Pause(){
        onPause = true;
        myTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        SaveManager.instance.Load();
        instance.menuParent.gameObject.SetActive(true);
        instance.FullcreenCheck();
        instance.ResetButtonAndPlanes();
        instance.RefreshResolutionDropDown();
        if (instance.FirstSelected) instance.FirstSelected.Select();
        if (!loading) AudioManager.instance.Play("MenuClick");
    }

    public void MainMenuSetup(){
        SaveManager.instance.Load();
        instance.FullcreenCheck();
        instance.ResetButtonAndPlanes();
        instance.RefreshResolutionDropDown();
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

    //Graphics
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

    public void SetResolution (int resIndex)
    {
        resolutionIndex = resIndex;
        if (resolutionIndex>=0 && resolutions!=null)
        {
            Resolution res = resolutions[resolutionIndex];
            Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        }
    }
    void RefreshResolutionDropDown()
    {
        resolutions = Screen.resolutions;

        graphicsdropdowns[1].ClearOptions();

        resolutionIndex = 0;
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++){
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                resolutionIndex = i;
            }
        }

        graphicsdropdowns[1].AddOptions(options);
        graphicsdropdowns[1].value = resolutionIndex;
        graphicsdropdowns[1].RefreshShownValue();
    }

    public void SetFullcreen (bool full)
    {
        isFullscreen = full;
        Screen.fullScreen = isFullscreen;
    }
    public void FullcreenCheck()
    {
        graphicsToggles[0].isOn = isFullscreen;
        Screen.fullScreen = isFullscreen;
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
        graphicsToggles[1].isOn = isShowFPS;
    }
    //end Graphics

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

    //Controls
    public void ControlPage(int index){
        for (int i = 0; i < controlsPages.Length; i++)
        {
            controlsPages[i].SetActive(i==index? true:false);
            controlsPageIndicator[i].SetActive(i==index? true:false);
        }
    }

    public void GetInputPath(){
        /*
        InputMaster.PlayerActions p = InputManager.controls.Player;
        int deviceIndex = 0;
        while (deviceIndex < 2){
            int actionIndex = 0;
            while (actionIndex < controlsPaths.GetLength(1)){
                InputAction action = null;
                InputActionAsset asset = InputManager.controls.asset;
                switch (asset.actionMaps[actionIndex].name)
                {
                    case "Jump":
                        action = p.Jump;
                        break;
                    case "Action":
                        action = p.Action;
                        break;
                    case "Interact":
                        action = p.Interact;
                        break;
                    case "Release":
                        action = p.Release;
                        break;
                    default:
                        break;
                }
                for (int i = 0; i < 4; i++)
                {
                    if (action !=null){
                        if (action.bindings[i] != null){
                            controlsPaths[deviceIndex,actionIndex,i] = action.bindings[i].overridePath;
                        }
                    }
                }
                actionIndex++;
            }
            deviceIndex++;
        }

        int counter = 0;
        for (int i = 0; i + counter < InputManager.controls.Player.Get().bindings.Count; i++)
        {
            if (!InputManager.controls.Player.Get().bindings[i].isComposite){
                counter++;
            }
            else {
            }

        }
        */
        System.Guid[] a = new System.Guid[controlsID.Count];
        for (int i = 0; i < a.Length; i++)
        {
            a[i] = System.Guid.Parse(controlsID[i]);
        }
        for (int i = 0; i < controlsID.Count; i++)
        {
            //InputManager.controls.Player.Get().FindAction(a[i]);
            InputBinding bind = InputManager.controls.Player.Get().bindings[InputManager.controls.Player.Get().bindings.IndexOf(b => b.id.Equals(a[i]))];
            InputBinding[] bi = InputManager.controls.Player.Get().bindings.ToArray();
            for (int j = 0; j < bi.Length; j++)
            {
                if (bi[j].id.Equals(a[i]))
                {
                    bi[j].overridePath = controlsPaths[i];
                    break;
                }
            }
            bind.overridePath = controlsPaths[i];
            Debug.Log("binding name: "+bind+" , overridePath: "+bind.overridePath +" , cPath: "+controlsPaths[i]);
        }
        for (int j = 0; j < 2; j++)
        {
            Toggle[] group;
            if (j==0) group = controlsTogglesKeyboard;
            else group = controlsTogglesGamepad;
            int l = j==0? controlsTogglesKeyboard.Length : controlsTogglesGamepad.Length;

            for (int z = 0; z < l; z++)
            {
                if (group[z]){
                    InputButton inputButton = group[z].GetComponent<InputButton>();
                    if (inputButton){
                        
                        if (inputButton.id!=null){
                            int bindIndex =  InputManager.controls.Player.Get().bindings.IndexOf(b => b.id.Equals(inputButton.id));
                            if (bindIndex==null) continue;
                            if (bindIndex<0) continue;
                            InputBinding bind = InputManager.controls.Player.Get().bindings[bindIndex];
                            if (bind!=null){
                                inputButton.path = bind.overridePath;
                            }
                        }
                        //inputButton.RefreshDisplay();
                    }
                }
            }
        }
    }

    public void SetInputPathDisplay(){
        /*
        for (int j = 0; j < 2; j++)
        {
            Toggle[] group;
            if (j==0) group = controlsTogglesKeyboard;
            else group = controlsTogglesGamepad;
            int l = j==0? controlsTogglesKeyboard.Length : controlsTogglesGamepad.Length;

            for (int i = 0; i < l; i++)
            {
                if (group[i]){
                    InputButton inputButton = group[i].GetComponent<InputButton>();
                    if (inputButton){
                        bool found = false;
                        for (int z = 0; z < controlsID.Count; z++)
                        {
                            //InputManager.controls.Player.Get().FindAction(a[i]);
                            InputBinding bind = InputManager.controls.Player.Get().bindings[InputManager.controls.Player.Get().bindings.IndexOf(b => b.id.Equals(a[i]))];
                            bind.overridePath = controlsPaths[i];
                            Debug.Log("binding name: "+bind);
                        }
                        if (inputButton.id.Equals(bind.id)){
                            inputButton.path = bind.overridePath;
                        }
                        inputButton.RefreshDisplay();
                    }
                    /*
                    string s1 = group[i].gameObject.name;
                    string[] s2 = s1.Split('/');
                    int index = InputManager.controls.Player.Get().bindings.IndexOf(
                        b => b.action.Equals(s2[0]) && b.ToString().Contains(s2[1]) && s2.Length>2? b.path.Equals(s2[2]) : true);
                    if (index>=0) {
                        InputBinding bind = InputManager.controls.Player.Get().bindings[index];
                        string display = bind.ToDisplayString();
                        string[] d = display.Split(' ');
                        display = d[d.Length-1];
                        group[i].transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = display;
                        Debug.Log("display bind: "+bind+" /display: "+bind.ToDisplayString());
                    }
                    
                }
            }
        }
        /*
        controlsPaths = new string[2,32,4];
        int bindingsCount = InputManager.controls.Player.Get().bindings.Count;
        for (int j = 0; j < 2; j++)
        {
            Toggle[] group;
            if (j==0) group = controlsTogglesKeyboard;
            else group = controlsTogglesGamepad;
            int l = j==0? controlsTogglesKeyboard.Length : controlsTogglesGamepad.Length;

            for (int i = 0; i < l; i++)
            {
                if (group[i]){
                    string[] split = group[i].name.Split('/');
                    string s = controlsPaths[j, i, int.Parse(split[split.Length-1])-1];
                    if (!string.IsNullOrEmpty(s)){
                        s = s.Substring(s.LastIndexOf("/")+1);
                        s = s.ToUpper();
                    }
                    group[i].transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = s;
                    /*
                    //Debug.Log("bindings ("+j+") : "+InputManager.controls.Player.Get().bindings[j]);
                    string target = group[i].gameObject.name;
                    string[] t = target.Split('/');
                    target =  null;
                    for (int w = 0; w < bindingsCount; w++)
                    {
                        string s = InputManager.controls.Player.Get().bindings[w].ToString();
                        if (s.Contains(t[0]) && s.Contains(t[1])){
                            string display = InputManager.controls.Player.Get().bindings[i].ToDisplayString();
                            string[] d = display.Split(' ');
                            display = d[d.Length-1];
                            group[i].transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = display;
                            break;
                        }
                    }
                    /*
                    int result = InputManager.controls.Player.Get().bindings.IndexOf(b => b.ToString().Contains(t[0]) && b.ToString().Contains(t[1]));
                    if (result<0){
                        Debug.LogWarning("result < 0: "+i);
                    }
                    else {
                        string display = InputManager.controls.Player.Get().bindings[i].ToDisplayString();
                        string[] d = display.Split(' ');
                        display = d[d.Length-1];
                        group[i].transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = display;
                    }
                    
                }
            }
        }
        */
    }
    public void SetInputPathToggle(Toggle t){
        StartCoroutine(SetInputPathToggleIEnumerator(t));
    }
    IEnumerator SetInputPathToggleIEnumerator(Toggle t){
        InputButton inputButton = t.GetComponent<InputButton>();
        if (!inputButton) yield return null;

        InputAction action = inputButton.action;
        int deviceIndex = -1;
        int actionIndex = -1;
        int bindingIndex = -1;
        InputMaster.PlayerActions p = InputManager.controls.Player;

        string[] info = t.gameObject.name.Split('/');

        if (info.Length > 2){
            switch (info[0])
            {
                case "Keyboard":
                    deviceIndex = 0;
                    break;
                case "Controller":
                    deviceIndex = 1;
                    break;
                default:
                    break;
            }
            switch (info[1])
            {
                case "Jump":
                    action = p.Jump;
                    actionIndex = 8;
                    break;
                case "Action":
                    action = p.Action;
                    actionIndex = 9;
                    break;
                case "Interact":
                    action = p.Interact;
                    actionIndex = 10;
                    break;
                case "Release":
                    action = p.Release;
                    actionIndex = 11;
                    break;
                default:
                    break;
            }
            switch (info[2])
            {
                case "Up":
                    bindingIndex = 0;
                    break;
                case "Down":
                    bindingIndex = 1;
                    break;
                case "Left":
                    bindingIndex = 2;
                    break;
                case "Right":
                    bindingIndex = 3;
                    break;
                case "Positive":
                    bindingIndex = 0;
                    break;
                case "Negative":
                    bindingIndex = 1;
                    break;
                case "1":
                    bindingIndex = 0;
                    break;
                case "2":
                    bindingIndex = 1;
                    break;
                default:
                    bindingIndex = 1;
                    break;
            }
        }

        if (action == null || deviceIndex == -1 || actionIndex == -1) yield return null;

        if (!loading) AudioManager.instance.Play("MenuClick");    

        action.Disable();
        controlBarrier.SetActive(true);
        InputActionRebindingExtensions.RebindingOperation operation = 
            action.PerformInteractiveRebinding()
            .WithBindingGroup(deviceIndex==0? "Keyboard and Mouse" : "Gamepad")
            .WithTargetBinding(inputButton.index)
            .WithControlsExcluding("Mouse");
        operation.Start();
        while (!operation.completed){
            yield return null;
        }
        string bID = operation.action.bindings[inputButton.index].id.ToString();
        string path = operation.action.bindings[inputButton.index].overridePath;

        bool found = false;
        for (int i = 0; i < controlsID.Count; i++)
        {
            if (controlsID[i] == bID){
                controlsPaths[i] = operation.action.bindings[inputButton.index].overridePath;
                found = true;
                break;
            }
        }
        if (!found) {
            Debug.Log("id not found, path:"+path);
            controlsID.Add(bID);
            controlsPaths.Add(path);
        }

        inputButton.path = operation.action.bindings[inputButton.index].overridePath;
        inputButton.RefreshDisplay();
        operation.Dispose();
        controlBarrier.SetActive(false);
        t.Select();
        action.Enable();

        //test
        
        
        /*
        List<InputBinding>[] bindings = new List<InputBinding>[2];
        bindings[0] = new List<InputBinding>{};
        bindings[1] = new List<InputBinding>{};
        for (int i = 0; i < action.bindings.Count; i++)
        {
            if (action.bindings[i].isComposite) continue;
            //Debug.Log("groups: "+action.bindings[i].groups); //vector2
            if (action.bindings[i].groups=="Keyboard and Mouse")
                bindings[0].Add(action.bindings[i]);
            else if (action.bindings[i].groups=="Gamepad")
                bindings[1].Add(action.bindings[i]);
        }

        string path = bindings[deviceIndex][bindingIndex].overridePath; //vector2
        Debug.Log("deviceIndex: "+deviceIndex+ " actionIndex: "+actionIndex+ " bindingIndex: "+bindingIndex);
        bool found = false;
        string bID = bindings[deviceIndex][bindingIndex].id.ToString();
        Debug.Log("BID: "+bID);
        
        for (int i = 0; i < controlsID.Count; i++)
        {
            if (controlsID[i] == bID){
                controlsPaths[i] = path;
                found = true;
                break;
            }
        }
        
        if (!found) {
            Debug.Log("id not found, path:"+path);
            controlsID.Add(bID);
            controlsPaths.Add(path.ToString());
            //controlsPaths.Insert(0, path.ToString());
        }
        
        
        //controlsPaths[deviceIndex, actionIndex,bindingIndex] = path;
        //bindingsID[deviceIndex, actionIndex,bindingIndex] = bindings[deviceIndex][bindingIndex].id.ToString();

        path = path.Substring(path.LastIndexOf("/")+1);
        path = path.ToUpper();
        t.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = path;

        Debug.Log("Action:" +action.name+" , "+bindings[deviceIndex][bindingIndex].effectivePath);
        /*
        for (int j = 0; j < bindings.Length; j++)
        {
            Debug.Log("--- device "+j+" ---");
            for (int i = 0; i < bindings[j].Count; i++)
            {
                Debug.Log(bindings[j][i].path +" : "+bindings[j][i].overridePath);
                
            }
        }
        */

        yield return null;
    }
    //end Controls

    public void ExitToMainMenu(int index){
        SaveManager.instance.Save();
        Time.timeScale = myTimeScale;
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
        resolutionIndex = data.qualityResolutionIndex;
        isFullscreen = data.qualityIsFullscreen;
        isShowFPS = data.qualityShowFPS;
        FullcreenCheck();
        SetResolution(resolutionIndex);
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

        //Controls
        controlsID = new List<string>(){};
        controlsPaths = new List<string>(){};
        
        
        if (data.controlsInputPaths !=null)
            controlsPaths = data.controlsInputPaths;
        else controlsPaths = new List<string>(){};
        if (data.controlsInputID != null)
            controlsID = data.controlsInputID;
        else controlsID = new List<string>(){};
        
        
        Debug.Log("---list1---");
        for (int i = 0; i < controlsID.Count; i++)
        {
            Debug.Log(controlsPaths[i]);
            Debug.Log(controlsID[i]);
        }
        
        //controlsPaths = data.controlsInputPaths.Count>0? data.controlsInputPaths : new List<string>(){};
        //controlsID = data.controlsInputID.Count>0? data.controlsInputID : new List<string>(){};
        GetInputPath();
        //SetInputPathDisplay();

        loading = false;
    }
}
