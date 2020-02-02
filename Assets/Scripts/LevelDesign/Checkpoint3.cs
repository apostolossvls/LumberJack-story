using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint3 : MonoBehaviour
{
    bool reached;
    bool isLastCheckpoint;
    public Transform human;
    public Transform dog;
    public Transform HumanResetPos;
    public Transform DogResetPos;
    public GameObject humanInstance;
    public GameObject dogInstance;
    GameObject[] instances;

    void Start()
    {
        reached = false;
        isLastCheckpoint = false;
        human = GameObject.FindWithTag("PlayerHuman").transform;
        dog = GameObject.FindWithTag("PlayerDog").transform;
    }

    void OnTriggerEnter(Collider other){
        if (other.tag == "PlayerHuman" && !reached){
            reached = true;
            isLastCheckpoint = true;
            InstantiatePlayers();
            //InstantiatePlayerParts();
            InstantiateObjects();
        }
    }

    void Update(){
        if (isLastCheckpoint){
            if (Input.GetKeyUp(KeyCode.P)){
                //LoadPlayerParts();
                LoadPlayers();
                LoadObjects();
                StartCoroutine(SetupOthersOnLoad());
            }
        }
    }

    void InstantiateObjects(){
        Resetable[] res = Resources.FindObjectsOfTypeAll(typeof(Resetable)) as Resetable[];
        for (int i = 0; i < res.Length; i++)
        {
            if (res[i]){
                if (res[i].original && res[i].active){
                    GameObject g = GameObject.Instantiate(res[i].gameObject, res[i].transform.position, res[i].transform.rotation);
                    g.transform.parent = res[i].transform.parent;
                    g.name = res[i].name;
                    g.GetComponent<Resetable>().original = false;
                    g.SetActive(false);
                }
            }
        }
    }

    void InstantiatePlayers() {
        //if (humanInstance) Destroy(humanInstance);
        //if (dogInstance) Destroy(dogInstance);
        humanInstance = GameObject.Instantiate(human.gameObject, HumanResetPos.position, HumanResetPos.rotation);
        humanInstance.transform.parent = human.transform.parent;
        humanInstance.name = human.name+"Checkpoint";
        humanInstance.tag = "Untagged";
        humanInstance.SetActive(false);
        dogInstance = GameObject.Instantiate(dog.gameObject, DogResetPos.position, DogResetPos.rotation);        
        dogInstance.transform.parent = dog.transform.parent;
        dogInstance.name = dog.name+"Checkpoint";
        dogInstance.tag = "Untagged";
        dogInstance.SetActive(false); 
    }

    void LoadObjects(){
        Resetable[] res = Resources.FindObjectsOfTypeAll(typeof(Resetable)) as Resetable[];
        for (int i = 0; i < res.Length; i++)
        {
            if (res[i]){
                if (res[i].original){
                    res[i].active = false;
                    Destroy(res[i].gameObject);
                }
                else {
                    res[i].original = true;
                    res[i].gameObject.SetActive(true);
                }
            }
        }

        InstantiateObjects();
    }

    void LoadPlayers(){
        human.gameObject.SetActive(false);
        dog.gameObject.SetActive(false);
        Destroy(human.gameObject);
        Destroy(dog.gameObject);
        
        human = humanInstance.transform;
        dog = dogInstance.transform;
        humanInstance = null;
        dogInstance = null;

        human.tag = "PlayerHuman";
        human.name = human.name.Replace("Checkpoint", "");

        dog.tag = "PlayerDog";
        dog.name = dog.name.Replace("Checkpoint", "");

        human.gameObject.SetActive(true);
        dog.gameObject.SetActive(true);
        InstantiatePlayers();
    }

    IEnumerator SetupOthersOnLoad(){
        Cinemachine.CinemachineTransposer vcamC = Object.FindObjectOfType<Cinemachine.CinemachineTransposer>();
        float x = vcamC.m_XDamping;
        float y = vcamC.m_YDamping;
        vcamC.m_XDamping = 0;
        vcamC.m_YDamping = 0;

        Object.FindObjectOfType<ControlManager>().SetupPlayers();
        Object.FindObjectOfType<CameraFollowPivot>().ChangeTarget(human); //error
        if (dog) {
            if (dog.GetComponent<NavMeshMovement>()) dog.GetComponent<NavMeshMovement>().target = human;
        }
        vcamC.transform.SetPositionAndRotation(HumanResetPos.position, HumanResetPos.rotation);
        yield return new WaitForFixedUpdate();
        vcamC.m_XDamping = x;
        vcamC.m_YDamping = y;
        yield return null;
    }
}
