using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    bool reached;
    bool isLastCheckpoint;
    public Transform human;
    public Transform dog;
    public Transform HumanResetPos;
    public Transform DogResetPos;
    public GameObject[] humanResetParents;
    public GameObject[] dogResetParents;
    public GameObject[] resetObjects;
    GameObject humanInstance;
    GameObject dogInstance;
    List<GameObject> humanInstances;
    List<GameObject> dogInstances;
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
                SetupOthersOnLoad();
            }
        }
    }

    void InstantiateObjects(){
        instances = new GameObject[resetObjects.Length];
        for (int i = 0; i < resetObjects.Length; i++)
        {
            if (resetObjects[i]){
                instances[i] = GameObject.Instantiate(resetObjects[i], resetObjects[i].transform.position, resetObjects[i].transform.rotation);
                instances[i].transform.parent = resetObjects[i].transform.parent;
                instances[i].name = resetObjects[i].name;
                instances[i].SetActive(false);
            }
        }
    }

    void InstantiatePlayers() {
        if (humanInstance) Destroy(humanInstance);
        if (dogInstance) Destroy(dogInstance);
        humanInstance = GameObject.Instantiate(human.gameObject, HumanResetPos.position, HumanResetPos.rotation);
        humanInstance.transform.parent = human.transform.parent;
        humanInstance.name = human.name+"Checkpoint";
        humanInstance.tag = "Untagged";
        humanInstance.SetActive(false);
        dogInstance = GameObject.Instantiate(dog.gameObject, DogResetPos.position, DogResetPos.rotation);        
        dogInstance.transform.parent = dog.transform.parent;
        dogInstance.name = dog.name+"Checkpoint";
        dog.tag = "Untagged";
        dogInstance.SetActive(false); 
    }

    void InstantiatePlayerParts(){
        if (human){
            //humanInstances = new List<GameObject>{};
            for (int i = 0; i < humanResetParents.Length; i++)
            {
                int c = humanResetParents[i].transform.childCount;
                List<GameObject> toCopy = new List<GameObject>{};
                //if (i==0) Debug.Log("ChildCount:" + c);
                for (int j = 0; j < c; j++)
                {
                    GameObject g1 = humanResetParents[i].transform.GetChild(j).gameObject;
                    if (!g1.name.Contains("Checkpoint") && !g1.name.Contains("GettingDestroyed")){
                        toCopy.Add(g1);
                    }
                    //humanInstances.Add(g2);
                }
                for (int j = 0; i < toCopy.Count; i++)
                {
                    GameObject g1 = toCopy[j];
                    GameObject g2 = GameObject.Instantiate(g1, g1.transform.position, g1.transform.rotation);
                    g2.transform.parent = g1.transform.parent;
                    g2.name = g1.name+"Checkpoint";
                    g2.SetActive(false);
                }
            }
        }
        if (dog){
            //dogInstances = new List<GameObject>{};
            for (int i = 0; i < dogResetParents.Length; i++)
            {
                int c = dogResetParents[i].transform.childCount;
                List<GameObject> toCopy = new List<GameObject>{};
                for (int j = 0; j < c; j++)
                {
                    GameObject g1 = dogResetParents[i].transform.GetChild(j).gameObject;
                    if (!g1.name.Contains("Checkpoint") && !g1.name.Contains("GettingDestroyed")){
                        toCopy.Add(g1);
                        /*GameObject g2 = GameObject.Instantiate(g1, g1.transform.position, g1.transform.rotation);
                        g2.transform.parent = g1.transform.parent;
                        g2.name = g1.name+"Checkpoint";
                        g2.SetActive(false);*/
                    }
                    //dogInstances.Add(g2);
                }
                for (int j = 0; i < toCopy.Count; i++)
                {
                    GameObject g1 = toCopy[j];
                    GameObject g2 = GameObject.Instantiate(g1, g1.transform.position, g1.transform.rotation);
                    g2.transform.parent = g1.transform.parent;
                    g2.name = g1.name+"Checkpoint";
                    g2.SetActive(false);
                }
            }
        }
    }

    void LoadObjects(){
        for (int i = 0; i < resetObjects.Length; i++)
        {
            if (resetObjects[i]){
                Destroy(resetObjects[i]);
            }
            if (instances[i]){
                instances[i].SetActive(true);
            }
        }
        resetObjects = instances;
        InstantiateObjects();
    }

    void LoadPlayers(){
        human.gameObject.SetActive(false);
        dog.gameObject.SetActive(false);
        Destroy(human.gameObject);
        Destroy(dog.gameObject);
        
        human = humanInstance.transform;
        dog = dogInstance.transform;

        human.tag = "PlayerHuman";
        human.name = human.name.Replace("Checkpoint", "");

        dog.tag = "PlayerDog";
        dog.name = dog.name.Replace("Checkpoint", "");

        human.gameObject.SetActive(true);
        dog.gameObject.SetActive(true);
    }

    void LoadPlayerParts(){
        if (human){
            human.position = HumanResetPos.position;
            human.rotation = HumanResetPos.rotation;
            
            for (int i = 0; i < humanResetParents.Length; i++)
            {
                int c = humanResetParents[i].transform.childCount;
                List<GameObject> toDestroy = new List<GameObject>{};
                for (int j = 0; j < c; j++)
                {
                    GameObject g = humanResetParents[i].transform.GetChild(j).gameObject;
                    if (g.name.Contains("Checkpoint")) {
                        g.name = g.name.Replace("Checkpoint", "");
                        g.SetActive(true);
                    }
                    else {
                        toDestroy.Add(g);
                    }
                }
                for (int j = 0; j < toDestroy.Count; j++)
                {
                    toDestroy[j].name = "GettingDestroyed";
                    Destroy(toDestroy[j]);
                }
            }
            //humanInstances.Clear();
        }
        if (dog){
            dog.position = DogResetPos.position;
            dog.rotation = DogResetPos.rotation;

            for (int i = 0; i < dogResetParents.Length; i++)
            {
                int c = dogResetParents[i].transform.childCount;
                List<GameObject> toDestroy = new List<GameObject>{};
                for (int j = 0; j < c; j++)
                {
                    GameObject g = dogResetParents[i].transform.GetChild(j).gameObject;
                    if (g.name.Contains("Checkpoint")) {
                        g.name = g.name.Replace("Checkpoint", "");
                        g.SetActive(true);
                    }
                    else {
                        toDestroy.Add(g);
                    }
                }
                for (int j = 0; j < toDestroy.Count; j++)
                {
                    toDestroy[j].name = "GettingDestroyed";
                    Destroy(toDestroy[j]);
                }
            }
            //dogInstances.Clear();
        }
        InstantiatePlayerParts();
    }

    void SetupOthersOnLoad(){
        Object.FindObjectOfType<ControlManager>().SetPlayers();
        Object.FindObjectOfType<PositionMatchPosition>().changeTarget(human);
        if (dog) {
            if (dog.GetComponent<NavMeshMovement>()) dog.GetComponent<NavMeshMovement>().target = human;
        }
    }
}
