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
    List<GameObject> humanInstances;
    List<GameObject> dogInstances;
    GameObject[] instances;

    void Start()
    {
        reached = false;
        isLastCheckpoint = false;
    }

    void OnTriggerEnter(Collider other){
        if (other.tag == "PlayerHuman" && !reached){
            reached = true;
            isLastCheckpoint = true;
            InstantiatePlayers();
            InstantiateObjects();
        }
    }

    void Update(){
        if (isLastCheckpoint){
            if (Input.GetKeyUp(KeyCode.P)){
                LoadPlayers();
                LoadObjects();
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

    void InstantiatePlayers(){
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
            //dogInstances.Clear();
        }
        InstantiatePlayers();
    }
}
