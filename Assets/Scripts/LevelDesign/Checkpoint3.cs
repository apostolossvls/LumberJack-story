﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class Checkpoint3 : MonoBehaviour
{
    bool reached;
    public static Checkpoint3 LastCheckpoint;
    public Transform human;
    public Transform dog;
    public Transform HumanResetPos;
    public Transform DogResetPos;
    public GameObject humanInstance;
    public GameObject dogInstance;

    void Start()
    {
        reached = false;
        human = GameObject.FindWithTag("PlayerHuman").transform;
        dog = GameObject.FindWithTag("PlayerDog").transform;
    }

    void OnTriggerEnter(Collider other){
        if (other.tag == "PlayerHuman" && !reached){
            Debug.Log("Checkpoint: "+transform.name);
            reached = true;
            LastCheckpoint = this;
            StartCoroutine(SetupOthersOnCheckpoint());
            InstantiatePlayers();
            //InstantiatePlayerParts();
            InstantiateObjects();
        }
    }

    void Update(){
        if (LastCheckpoint == this){
            if (Input.GetKeyUp(KeyCode.P)){
                ResetOnCheckpoint();
            }
        }
    }

    public void ResetOnCheckpoint(){
        //LoadPlayerParts();
        LoadPlayers();
        LoadObjects();
        StartCoroutine(SetupOthersOnLoad());
    }

    void InstantiateObjects(){
        //Resetable[] res = Resources.FindObjectsOfTypeAll(typeof(Resetable)) as Resetable[];
        GameObject[] all = SceneManager.GetActiveScene().GetRootGameObjects();
        for (int j = 0; j < all.Length; j++)
        {
            //Debug.Log("-----"+ all[j].name+"-----");
            foreach (Resetable res in all[j].GetComponentsInChildren<Resetable>(true))
            {
                //Debug.Log("name: "+ res.name);
                if (res){
                    if (res.original && res.active){
                        GameObject g = GameObject.Instantiate(res.gameObject, res.transform.position, res.transform.rotation);
                        g.transform.parent = res.transform.parent;
                        g.name = res.name;
                        g.GetComponent<Resetable>().original = false;
                        g.SetActive(false);
                    }
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

    IEnumerator SetupOthersOnCheckpoint(){
        //objects
        GameObject[] all = SceneManager.GetActiveScene().GetRootGameObjects();
        for (int j = 0; j < all.Length; j++)
        {
            foreach (Resetable res in all[j].GetComponentsInChildren<Resetable>(true))
            {
                if (res){
                    if (!res.original){
                        res.active = false;
                        Destroy(res.gameObject);
                    }
                }
            }
        }

        //players
        if (humanInstance){
            humanInstance.gameObject.SetActive(false);
            Destroy(humanInstance.gameObject);
        }
        if (dogInstance) {
            dogInstance.gameObject.SetActive(false);
            Destroy(dogInstance.gameObject);
        }
        humanInstance = null;
        dogInstance = null;

        //links
        foreach (NavMeshLinkPoints link in Object.FindObjectsOfType<NavMeshLinkPoints>())
        {
            link.AlighPoints();
        }
        foreach (NavMeshLink link in Object.FindObjectsOfType<NavMeshLink>())
        {
            link.enabled = false;
            yield return new WaitForEndOfFrame();
            link.enabled = true;
        }

        LevelSettings.instance.checkpoint = gameObject;
    }

    void LoadObjects(){
        GameObject[] all = SceneManager.GetActiveScene().GetRootGameObjects();
        for (int j = 0; j < all.Length; j++)
        {
            foreach (Resetable res in all[j].GetComponentsInChildren<Resetable>(true))
            {
                if (res){
                    if (res.original || !res.active){
                        res.active = false;
                        Destroy(res.gameObject);
                    }
                    else {
                        res.original = true;
                        res.gameObject.SetActive(true);
                    }
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

        foreach (NavMeshLinkPoints link in Object.FindObjectsOfType<NavMeshLinkPoints>())
        {
            link.AlighPoints();
        }

        foreach (Checkpoint3 c in Object.FindObjectsOfType<Checkpoint3>())
        {
            if (c != this) {
                c.human = human;
                c.dog = dog;
            }
        }


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
