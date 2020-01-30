using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint2 : MonoBehaviour
{
    bool reached;
    bool isLastCheckpoint;
    public Transform human;
    public Transform dog;
    public Transform HumanResetT;
    public Transform DogResetT;

    void Start()
    {
        reached = false;
        isLastCheckpoint = false;
    }


    void Update(){
        if (isLastCheckpoint){
            if (Input.GetKeyUp(KeyCode.P)){
                Debug.Log("checkpoint reset");
                LevelSettings.RestartScene();
            }
        }
    }

    void OnTriggerEnter(Collider other){
        if (other.tag == "PlayerHuman" && !reached){
            reached = true;
            isLastCheckpoint = true;
            LevelSettings.instance.SetCheckpoint(gameObject);
        }
    }

    public IEnumerator LoadPlayers(){
        human = GameObject.FindWithTag("PlayerHuman").transform;
        dog = GameObject.FindWithTag("PlayerDog").transform;
        //Camera cam = (Camera)FindObjectOfType(typeof(Camera));
        //Cinemachine.CinemachineTransposer vcam = (Cinemachine.CinemachineTransposer)FindObjectOfType(typeof(Cinemachine.CinemachineTransposer));
        Cinemachine.CinemachineVirtualCamera vcam = Object.FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
        CameraFollowPivot p = Object.FindObjectOfType<CameraFollowPivot>();
        vcam.gameObject.SetActive(false);

        human.SetPositionAndRotation(HumanResetT.position, HumanResetT.rotation);
        dog.SetPositionAndRotation(DogResetT.position, DogResetT.rotation);
        p.transform.SetPositionAndRotation(HumanResetT.position, HumanResetT.rotation);
        //float x = vcam.m_XDamping;
        //float y = vcam.m_YDamping;
        //vcam.m_XDamping = 0;
        //vcam.m_YDamping = 0;
        //vcam.gameObject.transform.SetPositionAndRotation(HumanResetT.position, HumanResetT.rotation);
        //yield return new WaitForEndOfFrame();
        vcam.gameObject.SetActive(true);
        //vcam.m_XDamping = x;
        //vcam.m_YDamping = y;
        yield return null;
    }
}
