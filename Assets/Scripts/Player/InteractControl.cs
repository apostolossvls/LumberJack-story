using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractControl : MonoBehaviour
{
    public bool IsHuman = true;
    public bool canGrabItem = true;
    public bool canGrabGrabbable = true;
    public bool canGrabDraggable = true;
    public InteractCollider interactCollider;
    public Transform[] HandPivot;
    public Transform[] GrabPivots; //0.front , 1.back , 2.front up, 3.down
    public bool[] GrabPivotsUsing;
    Transform[] ItemParent = new Transform[2];
    List<Transform> possibleinteracts;
    string[] interactTags;
    public bool leftHandGrabbing;
    public bool rightHandGrabbing;
    public Transform leftGrab;
    public Transform rightGrab;
    public float grabForce=300f;
    RigidbodyConstraints rigidbodyConstraints;
    public GameObject indicator;

    //TimersTarget
    public float HoldReleaseTimerTarget=1f;

    //timers
    float HoldReleaseTimer;

    void Start()
    {
        ReleaseHand();
    }

    void Update(){
        possibleinteracts = interactCollider.possibleinteracts;
        interactTags = interactCollider.tags;
        int index = GetShortDistance();
        if (index!=-1 && Input.GetButtonUp("Interact")){
            if (possibleinteracts[index].tag==interactTags[0] && canGrabItem){ //item
                //grab item
                Debug.Log("grabbing item (InteractControl)");
                if (IsHuman){
                    if (leftHandGrabbing && rightHandGrabbing){
                        ReleaseHand(true, false);
                        GrabItem(possibleinteracts[index], true);
                    }
                    else if (rightHandGrabbing && !leftHandGrabbing){
                        GrabItem(possibleinteracts[index], false);
                    }
                    else {
                        GrabItem(possibleinteracts[index], true);
                    }
                }
                else {
                    ReleaseHand(true, false);
                    GrabItem(possibleinteracts[index], true);
                }
            }
            else if (possibleinteracts[index].tag==interactTags[1] && IsHuman && canGrabGrabbable){ //fixedJoint , grabbable
                Debug.Log("grabbing grabbable (InteractControl)");
                ReleaseHand();
                GrabGrabbable(possibleinteracts[index]);
            } 
            else if (possibleinteracts[index].tag==interactTags[2] && IsHuman && canGrabDraggable){ //pushdrag , draggable
                Debug.Log("grabbing draggable (InteractControl)");
                ReleaseHand();
                GrabDraggable(possibleinteracts[index]);
            }
            else if (possibleinteracts[index].tag==interactTags[3] && IsHuman){ //interact
                Debug.Log("interacting (InteractControl)");
                //ReleaseHand();
                //GrabDraggable(possibleinteracts[index]);
                Interact(possibleinteracts[index]);
            }
        }

        if (Input.GetButtonUp("Release")){
            if (HoldReleaseTimer < HoldReleaseTimerTarget){
                ReleaseHand();
            }
            else {
                //HoldRelease
                Throw();
            }
            HoldReleaseTimer=0;
        }

        if (Input.GetButton("Release")){
            HoldReleaseTimer+=Time.deltaTime;
        }

        //second Cross
        if (Mathf.Abs(Input.GetAxisRaw("SecondHorizontal")) > 0.3f || Mathf.Abs(Input.GetAxisRaw("SecondVertical")) > 0.3f){
            //Debug.Log("second cross movement");
            Vector2 inp;
            inp.x = Input.GetAxisRaw("SecondHorizontal");
            inp.y = Input.GetAxisRaw("SecondVertical");
            if (leftGrab && rightGrab && leftGrab == rightGrab && IsHuman){
                if (rightGrab.tag=="Grabbable" && !GrabbablePositionMatch(GrabbablePosition(inp))){
                    //GrabOverTime(rightGrab, GrabPivots[GrabbablePosition(inp)], 0.5f);
                    //StopCoroutine("GrabItemOverTime");
                    //StartCoroutine(GrabOverTime(rightGrab, GrabPivots[GrabbablePosition(inp)], 0.5f));
                    ChangeGrabbablePosition(GrabbablePosition(inp));
                }
            }
        }
        /*movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical"); //Horizontal */

        if (possibleinteracts.Count>0 && index>=0){
            if (!indicator.activeSelf) indicator.SetActive(true);
            Vector3 tempT = possibleinteracts[index].transform.position;
            indicator.transform.position = tempT;
        }
        else {
            indicator.SetActive(false);
        }
    }

    void OnDisable(){
        if (indicator.activeSelf) indicator.SetActive(false);
    }

    int GetShortDistance(){
        int f = -1;
        float dist = float.MaxValue;
        for (int i = 0; i < interactCollider.possibleinteracts.Count; i++)
        {
            if (possibleinteracts[i]){
                if (Vector3.Distance(transform.position, possibleinteracts[i].position)< dist && !IsOnHand(possibleinteracts[i])){
                    dist = Vector3.Distance(transform.position, possibleinteracts[i].position);
                    f = i;
                }
            }
        }
        return f;
    }

    int GrabbablePosition(Vector2 inp){
        int pos = -1;
        if (Mathf.Abs(inp.x)>Mathf.Abs(inp.y)){
            if (inp.x>0) pos = 0;
            else pos = 1;
        }
        else if (inp.y!=0){
            if (inp.y>0) pos = 2;
            else pos = 3;
        }
        return pos;
    }

    bool GrabbablePositionMatch(int j) {
        for (int i = 0; i < GrabPivotsUsing.Length; i++)
        {
            if (GrabPivotsUsing[i]){
                if (i==j) return true;
                else return false;
            } 
        }
        return false;
    }

    bool IsOnHand(Transform t){
        if (t==rightGrab) return true;
        else if (t==leftGrab) return true;
        else return false;
    }

    void ReleaseHand(bool rRight = true, bool rLeft = true){
        if (rRight && rightHandGrabbing){
            rightHandGrabbing = false;
            if (rightGrab.tag=="Item"){
                ReleaseItem(rightGrab, true);
            }
            else if (rightGrab.tag=="Grabbable"){
                ReleaseGrabbable(rightGrab);
            }
            else if (rightGrab.tag=="Draggable"){
                ReleaseDraggable(rightGrab);
            }
            else if (rightGrab.tag=="HumanInteractable"){
                //do something
            }
            rightGrab=null;
        }
        if (rLeft && leftHandGrabbing){
            leftHandGrabbing = false;
            if (leftGrab.tag=="Item"){
                ReleaseItem(leftGrab, false);
            }
            else if (leftGrab.tag=="Grabbable"){
                ReleaseGrabbable(leftGrab);
            }
            if (leftGrab.tag=="Draggable"){
                ReleaseDraggable(leftGrab);
            }
            if (leftGrab.tag=="HumanInteractable"){
                //do something
            }
            leftGrab=null;
        }

        /*if (script){
            if (script is GrabItem){
                Debug.Log("Script is GrabItem");
                GrabItem c = script as GrabItem;
                c.ResetHodling(); 
            }
            else if (script is GrabFixedJoint){
                Debug.Log("Script is GrabFixedJoint");
                GrabFixedJoint c = script as GrabFixedJoint;
                c.ResetHolding(); 
            }
            else if (script is PushDrag){
                Debug.Log("Script is PushDrag");
                PushDrag c = script as PushDrag;
                c.ResetHolding(); 
            }
            else {
                Debug.Log("Script error on GrabControl");
            }
        }
        script = null;*/
    }

    /*void DropItem(){
        //Debug.Log("posItem: "+possibleItem);
        //Debug.Log(previousAnchorPivot);
        item.SetParent(previousAnchorPivot);
        foreach (Collider c in item.GetComponents<Collider>())
        {
            if (!c.isTrigger) c.enabled = true;
        }
        if (item.GetComponent<Rigidbody>()) item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
        //Destroy(AnchorPivot.Find("itemPivot").gameObject);
        // myObjList.Where(x => x.name == yourname).SingleOrDefault();
        //if (previousAnchorPivot) item.SetParent(previousAnchorPivot);
        //else item.DetachChildren();
        grabbing=false;
        Debug.Log("Not grabbing");
    }
    */

    void GrabItem(Transform obj, bool OnRight){
        int hand=0;
        if (OnRight){
            rightHandGrabbing=true;
            rightGrab=obj;
            hand=0;
        }
        else{
            leftHandGrabbing=true;
            leftGrab=obj;
            hand=1;
        }

        //Debug.Log("Grab");
        ItemParent[hand] = obj.parent;
        //GameObject g = new GameObject();
        //g.name = "itemPivot";
        //g.transform.parent = AnchorPivot.transform;
        //item.transform.parent = g.transform;
        obj.parent = HandPivot[hand];
        /*
        obj.localPosition = Vector3.zero;
        obj.localRotation = Quaternion.identity;
        */
        StopCoroutine("GrabItemOverTime");
        StartCoroutine(GrabOverTime(obj, HandPivot[hand], 10f, hand));

        if (obj.GetComponent<Rigidbody>()) obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        foreach (Collider c in obj.GetComponents<Collider>())
        {
            if (!c.isTrigger) c.enabled = false;
        }
    }

    private Vector3 posVel = Vector3.zero;

    IEnumerator GrabOverTime(Transform t, Transform newT, float speed, int handIndex=0){
        float timer=0;
        while (timer<1/speed){
            t.localRotation = Quaternion.Slerp(t.localRotation, newT.localRotation, Time.deltaTime * speed);
            t.localPosition = Vector3.SmoothDamp(t.localPosition, newT.localPosition, ref posVel, 1/speed);
            timer+=Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        //t.parent = HandPivot[handIndex];
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        yield return null;
    }

    void GrabGrabbable(Transform obj){
        rightHandGrabbing=true;
        leftHandGrabbing=true;
        rightGrab=obj;
        leftGrab=obj;

        ItemParent[0] = obj.parent;
        ItemParent[1] = obj.parent;

        obj.parent = GrabPivots[0];
        StopCoroutine("GrabItemOverTime");
        StartCoroutine(GrabOverTime(rightGrab, GrabPivots[0], 10f));

        if (obj.GetComponent<Rigidbody>()) obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        foreach (Collider c in obj.GetComponents<Collider>())
        {
            if (!c.isTrigger) c.enabled = false;
        }

        /*
        //Debug.DrawRay( transform.position, transform.right, Color.black, 1f);
        FixedJoint fj = obj.gameObject.AddComponent<FixedJoint>();
        fj.breakForce = grabForce;
        fj.connectedBody = GetComponent<Rigidbody>();
        

        if (obj.GetComponent<Rigidbody>()){
            //m_Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;
            rigidbodyConstraints = obj.GetComponent<Rigidbody>().constraints;
            //holdingGameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
        */
    }

    void ChangeGrabbablePosition(int pos){
        rightGrab.parent = GrabPivots[pos];

        for (int i = 0; i < GrabPivotsUsing.Length; i++)
        {
            if (i==pos) GrabPivotsUsing[i] = true;
            else GrabPivotsUsing[i] = false;
        }

        StopCoroutine("GrabItemOverTime");
        StartCoroutine(GrabOverTime(rightGrab, GrabPivots[pos], 10f));
    }

    void GrabDraggable(Transform obj){
        rightHandGrabbing=true;
        leftHandGrabbing=true;
        rightGrab=obj;
        leftGrab=obj;

        //Debug.DrawRay( transform.position, transform.right, Color.black, 1f);
        SpringJoint sj = obj.gameObject.AddComponent<SpringJoint>();
        sj.spring = 50;
        sj.damper = 10;
        //holding.maxDistance = 0.3f;
        sj.tolerance = 2f;
        sj.enableCollision = true;
        sj.breakForce = grabForce;
        sj.connectedBody = GetComponent<Rigidbody>();
        /*if (holdingGameObject.GetComponent<Rigidbody>()){
            //m_Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationZ;
            rigidbodyConstraints = holdingGameObject.GetComponent<Rigidbody>().constraints;
            //holdingGameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }*/

        if (GetComponent<Jump>()) GetComponent<Jump>().enabled =false;
    }

    void Interact(Transform t){
        Debug.Log("interacting function");
        //do something
        t.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
    }

    void ReleaseItem(Transform t, bool OnRight){
        //Debug.Log("posItem: "+possibleItem);
        //Debug.Log(previousAnchorPivot);
        StopCoroutine("GrabItemOverTime");
        t.SetParent(ItemParent[OnRight? 0 : 1]);
        foreach (Collider c in t.GetComponents<Collider>())
        {
            if (!c.isTrigger) c.enabled = true;
        }
        if (t.GetComponent<Rigidbody>()) t.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
        //Destroy(AnchorPivot.Find("itemPivot").gameObject);
        // myObjList.Where(x => x.name == yourname).SingleOrDefault();
        //if (previousAnchorPivot) item.SetParent(previousAnchorPivot);
        //else item.DetachChildren();
    }

    void ReleaseGrabbable(Transform t){
        //Destroy(t.GetComponent<FixedJoint>());

        t.parent = ItemParent[0];
        ItemParent[0] = null;
        ItemParent[1] = null;

        foreach (Collider c in t.GetComponents<Collider>())
        {
            if (!c.isTrigger) c.enabled = true;
        }
        if (t.GetComponent<Rigidbody>()) t.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;

        //if (holdingGameObject.GetComponent<Rigidbody>()) holdingGameObject.GetComponent<Rigidbody>().constraints = rigidbodyConstraints;
        if (t.position.z!=0) 
            t.position = new Vector3(t.position.x, t.position.y, 0);
    }

    void ReleaseDraggable(Transform t){
        Destroy(t.GetComponent<SpringJoint>());
        //if (holdingGameObject.GetComponent<Rigidbody>()) holdingGameObject.GetComponent<Rigidbody>().constraints = rigidbodyConstraints;
        if (t.position.z!=0) 
            t.position = new Vector3(t.position.x, t.position.y, 0);
        
        if (GetComponent<Jump>()) GetComponent<Jump>().enabled =true;
    }

    void Throw(){
        //Debug.Log("Throw");
        if (rightGrab.tag=="Item"){
            float f = 10f;
            if (!IsHuman) f = 5;
            rightGrab.GetComponent<Rigidbody>().AddForce((transform.forward+transform.up)*f, ForceMode.Impulse);
            ReleaseHand(true, false);
        }
        else if (rightGrab.tag=="Grabbable"){
            rightGrab.GetComponent<Rigidbody>().AddForce((transform.forward+transform.up)*0.5f, ForceMode.Impulse);
            ReleaseHand();
        }
        else if (rightGrab.tag=="Draggable"){
            rightGrab.GetComponent<Rigidbody>().AddForce(new Vector3(Mathf.Sign(rightGrab.position.x-transform.position.x), 0, 0) * 4f, ForceMode.Impulse);
            ReleaseHand();
        }
    }
}
