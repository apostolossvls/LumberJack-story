using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [HideInInspector] public RigidbodyConstraints[] rigidbodyConstraints = new RigidbodyConstraints[2];

    //UI
    public GameObject indicator;
    public GameObject ThrowSlider;
    public GameObject holdingInteractIndicator;
    public GameObject HoldingReleaseIndicator;

    //HoldInteract
    bool holdingInteract;

    //throw
    bool IsThrowing;
    public float throwPower=1f;
    public float throwingAngleX=0;
    [HideInInspector] public Vector3 throwingAngle;
    public float throwForce=0;

    //TimersTarget
    public float HoldInteractTimerTarget=1f;
    public float HoldReleaseTimerTarget=1f;

    //timers
    float HoldInteractTimer;
    float HoldReleaseTimer;

    void Start()
    {
        ReleaseHand();
        IsThrowing=false;
        holdingInteract = false;
        if (IsHuman) rigidbodyConstraints = new RigidbodyConstraints[2];
        else rigidbodyConstraints = new RigidbodyConstraints[1];
    }

    void Update(){
        possibleinteracts = interactCollider.possibleinteracts;
        interactTags = interactCollider.tags;
        int index = GetShortDistance();
        if (index!=-1){
            if (Input.GetButtonUp("Interact")){
                bool returnHoldInteract = false;
                if (HoldInteractTimer >= HoldInteractTimerTarget){
                    holdingInteract=true;
                    returnHoldInteract = HoldInteract(possibleinteracts[index]);
                }
                if (HoldInteractTimer < HoldInteractTimerTarget || !returnHoldInteract){
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
                    else if (IsHuman){
                        if (possibleinteracts[index].tag==interactTags[1] && canGrabGrabbable){ //fixedJoint , grabbable
                            Debug.Log("grabbing grabbable (InteractControl)");
                            ReleaseHand();
                            GrabGrabbable(possibleinteracts[index]);
                        } 
                        else if (possibleinteracts[index].tag==interactTags[2] && canGrabDraggable){ //pushdrag , draggable
                            Debug.Log("grabbing draggable (InteractControl)");
                            ReleaseHand();
                            GrabDraggable(possibleinteracts[index]);
                        }
                        else if (possibleinteracts[index].tag==interactTags[3]){ //interact
                            //Debug.Log("interacting (InteractControl)");
                            //ReleaseHand();
                            //GrabDraggable(possibleinteracts[index]);
                            Interact(possibleinteracts[index]);
                        }
                    }
                    else { //dog
                        if (possibleinteracts[index].tag==interactTags[1]){ //interact
                            Interact(possibleinteracts[index]);
                        }
                    }
                }
                HoldInteractTimer=0;
                holdingInteractIndicator.SetActive(false);
            }
            if (Input.GetButton("Interact")) {
                HoldInteractTimer+=Time.deltaTime;
                if (HoldInteractTimer>=HoldInteractTimerTarget/5){
                    holdingInteractIndicator.SetActive(true);
                    holdingInteractIndicator.GetComponentInChildren<Image>().fillAmount = HoldInteractTimer/HoldInteractTimerTarget;
                }
            }
        }
        else {
            HoldInteractTimer=0;
            holdingInteract=false;
        }

        if (Input.GetButtonUp("Release")  && (rightGrab || leftGrab)){
            HoldingReleaseIndicator.SetActive(false);
            if (HoldReleaseTimer < HoldReleaseTimerTarget){
                ReleaseHand();
            }
            else if (rightGrab){
                //HoldRelease
                Throw();
                IsThrowing = false;
            }
            HoldReleaseTimer=0;
        }

        if (Input.GetButton("Release") && (rightGrab || leftGrab)){
            HoldReleaseTimer+=Time.deltaTime;
            if (HoldReleaseTimer>=HoldReleaseTimerTarget/5 && rightGrab){
                HoldingReleaseIndicator.SetActive(true);
                HoldingReleaseIndicator.GetComponentInChildren<Image>().fillAmount = HoldReleaseTimer/HoldReleaseTimerTarget;
            }
            if (HoldReleaseTimer >= HoldReleaseTimerTarget && !IsThrowing && rightGrab){
                IsThrowing = true;
                throwingAngleX = transform.forward.x>=0? 1 : -1 * Mathf.PI/4;
                throwForce = 1;
            }
        }

        //swapHandInput
        if (Input.GetButtonUp("SwapHand") && IsHuman){
            Inventory inv = GetComponent<Inventory>();
            if (inv){
                if (inv.inputHold < inv.inputHoldNeeded) {
                    SwapHands();
                }
            }
            else {
                SwapHands();
            }
        }

        //second Cross
        if (Input.GetAxisRaw("SecondHorizontal")!=0 || Input.GetAxisRaw("SecondVertical")!=0){
            Vector2 inp;
            inp.x = Input.GetAxisRaw("SecondHorizontal");
            inp.y = Input.GetAxisRaw("SecondVertical");
            if (IsThrowing){
                if (Mathf.Abs(inp.x) > 0) {
                    SetThrowForce(inp.x);
                }
                if (Mathf.Abs(inp.y) > 0) {
                    SetThrowAngle(inp.y);
                }
            }
            else if (Mathf.Abs(inp.x) > 0.3f || Mathf.Abs(inp.y) > 0.3f){
                //if (leftGrab && rightGrab && leftGrab == rightGrab && IsHuman){
                if (rightGrab){
                    if (rightGrab.tag=="Grabbable" && !GrabbablePositionMatch(GrabbablePosition(inp))){
                        //GrabOverTime(rightGrab, GrabPivots[GrabbablePosition(inp)], 0.5f);
                        //StopCoroutine("GrabItemOverTime");
                        //StartCoroutine(GrabOverTime(rightGrab, GrabPivots[GrabbablePosition(inp)], 0.5f));
                        ChangeGrabbablePosition(GrabbablePosition(inp));
                    }
                }
            }
        }
        /*movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical"); //Horizontal */

        if (IsThrowing && (rightGrab.tag=="Item" || rightGrab.tag=="Grabbable")){
            if (!ThrowSlider.activeSelf) ThrowSlider.SetActive(true);
            throwingAngle = new Vector3(throwingAngleX, Mathf.Cos(throwingAngleX), 0).normalized;
            Debug.DrawRay(transform.position, throwingAngle, Color.magenta);
            ThrowSlider.transform.rotation = Quaternion.LookRotation(throwingAngle);
            ThrowSlider.GetComponentInChildren<Slider>().value = throwForce;
        }
        else if (ThrowSlider.activeSelf){
            HoldingReleaseIndicator.SetActive(false); 
            ThrowSlider.SetActive(false);
        } 

        if (possibleinteracts.Count>0 && index>=0){
            if (!indicator.activeSelf) indicator.SetActive(true);
            Vector3 tempT = possibleinteracts[index].transform.position;
            indicator.transform.position = tempT;
        }
        else {
            indicator.SetActive(false);
            holdingInteractIndicator.SetActive(false);
        }
    }

    void OnDisable(){
        if (indicator.activeSelf) indicator.SetActive(false);
        if (holdingInteractIndicator.activeSelf) holdingInteractIndicator.SetActive(false);
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

    public bool IsOnHand(Transform t){
        if (t==rightGrab) return true;
        else if (t==leftGrab) return true;
        else return false;
    }

    public void ReleaseHand(bool rRight = true, bool rLeft = true, bool fromThrow=false){
        Debug.Log("release");
        if (rRight && rightHandGrabbing){
            rightHandGrabbing = false;
            rightGrab.SendMessage("OnRelease", new MessageArgs(transform), SendMessageOptions.DontRequireReceiver);          
            if (rightGrab.tag=="Item"){
                Transform obj = rightGrab;
                ReleaseItem(rightGrab, true);
                if (!fromThrow) {
                    obj.SendMessage("OnReleaseInventory", new MessageArgs(transform), SendMessageOptions.DontRequireReceiver);
                }
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
            leftGrab.SendMessage("OnRelease", new MessageArgs(transform), SendMessageOptions.DontRequireReceiver);
            if (leftGrab.tag=="Item"){
                Transform obj = leftGrab;
                ReleaseItem(leftGrab, false);
                if (!fromThrow) {
                    obj.SendMessage("OnReleaseInventory", new MessageArgs(transform), SendMessageOptions.DontRequireReceiver);
                }
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

    public void GrabItem(Transform obj, bool OnRight){

        //test if obj is hand of other playable cahracter
        if (obj.GetComponentInParent<InteractControl>()){
            InteractControl i = obj.GetComponentInParent<InteractControl>();
            if (i.rightGrab == obj) i.SendMessageReleaseHand(true, false);
            else if (i.leftGrab == obj) i.SendMessageReleaseHand(false, true);
        }


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

        rightGrab.SendMessage("OnGrab", new MessageArgs(transform), SendMessageOptions.DontRequireReceiver);

        StopCoroutine("GrabOverTime");
        StartCoroutine(GrabOverTime(obj, HandPivot[hand], 10f, hand));

        if (obj.GetComponent<Rigidbody>()) {
            rigidbodyConstraints[hand] = obj.GetComponent<Rigidbody>().constraints;
            obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        /*foreach (Collider c in obj.GetComponents<Collider>())
        {
            //if (!c.isTrigger) c.enabled = false; //test
            if (!c.isTrigger) c.isTrigger = true;
        }*/
        SetLayer (obj, "IgnorePlayer");
        //SetIgnoreCollision(obj.GetComponentInChildren<Collider>(), true);
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

    public void GrabGrabbable(Transform obj){
        rightHandGrabbing=true;
        leftHandGrabbing=true;
        rightGrab=obj;
        leftGrab=obj;

        ItemParent[0] = obj.parent;
        ItemParent[1] = obj.parent;

        obj.parent = GrabPivots[0];
        GrabPivotsUsing[0] = true;

        rightGrab.SendMessage("OnGrab", new MessageArgs(transform), SendMessageOptions.DontRequireReceiver);

        StopCoroutine("GrabOverTime");
        StartCoroutine(GrabOverTime(rightGrab, GrabPivots[0], 10f));

        if (obj.GetComponent<Rigidbody>()) {
            rigidbodyConstraints[0] = obj.GetComponent<Rigidbody>().constraints;
            rigidbodyConstraints[1] = obj.GetComponent<Rigidbody>().constraints;
            obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        
        /*foreach (Collider c in obj.GetComponents<Collider>())
        {
            //if (!c.isTrigger) c.enabled = false; //test
            if (!c.isTrigger) c.isTrigger = true;

        }*/
        SetLayer (obj, "IgnorePlayer");
        //SetIgnoreCollision(obj.GetComponentInChildren<Collider>(), true);

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

        StopCoroutine("GrabOverTime");
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

        rightGrab.SendMessage("OnGrab", new MessageArgs(transform), SendMessageOptions.DontRequireReceiver);

        if (GetComponent<Jump>()) GetComponent<Jump>().enabled =false;
    }

    void Interact(Transform t){
        Debug.Log("interacting function");
        //do something
        t.SendMessage("OnInteract", new MessageArgs(transform), SendMessageOptions.DontRequireReceiver);
    }

    void ReleaseItem(Transform t, bool OnRight){
        //Debug.Log("posItem: "+possibleItem);
        //Debug.Log(previousAnchorPivot);
        StopCoroutine("GrabItemOverTime");
        t.SetParent(ItemParent[OnRight? 0 : 1]);
        /*foreach (Collider c in t.GetComponents<Collider>())
        {
            //if (!c.isTrigger) c.enabled = true; //test
            if (c.isTrigger) c.isTrigger = false;
        }*/
        if (t.GetComponent<Rigidbody>()) t.GetComponent<Rigidbody>().constraints = rigidbodyConstraints[OnRight? 0 : 1];
        //Destroy(AnchorPivot.Find("itemPivot").gameObject);
        // myObjList.Where(x => x.name == yourname).SingleOrDefault();
        //if (previousAnchorPivot) item.SetParent(previousAnchorPivot);
        //else item.DetachChildren();
    }

    void ReleaseGrabbable(Transform t){
        //Destroy(t.GetComponent<FixedJoint>());
        rightGrab=null;
        leftGrab=null;
        rightHandGrabbing=false;
        leftHandGrabbing=false;

        t.parent = ItemParent[0];
        ItemParent[0] = null;
        ItemParent[1] = null;

        /*foreach (Collider c in t.GetComponents<Collider>())
        {
            //if (!c.isTrigger) c.enabled = true; //test
            if (c.isTrigger) c.isTrigger = false;
        }*/
        //if (t.GetComponent<Rigidbody>()) t.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
        if (t.GetComponent<Rigidbody>()) t.GetComponent<Rigidbody>().constraints = rigidbodyConstraints[0];

        //if (holdingGameObject.GetComponent<Rigidbody>()) holdingGameObject.GetComponent<Rigidbody>().constraints = rigidbodyConstraints;
        if (t.position.z!=0) 
            t.position = new Vector3(t.position.x, t.position.y, 0);
    }

    void ReleaseDraggable(Transform t){
        rightGrab=null;
        leftGrab=null;
        rightHandGrabbing=false;
        leftHandGrabbing=false;
        SpringJoint[] sj = t.GetComponents<SpringJoint>();
        foreach (var joint in sj)
        {
            if (joint.connectedBody == GetComponent<Rigidbody>()){
                Destroy(joint);
            }
        }
        //if (holdingGameObject.GetComponent<Rigidbody>()) holdingGameObject.GetComponent<Rigidbody>().constraints = rigidbodyConstraints;
        if (t.position.z!=0) 
            t.position = new Vector3(t.position.x, t.position.y, 0);
        
        if (GetComponent<Jump>()) GetComponent<Jump>().enabled =true;
    }

    bool HoldInteract(Transform obj){
        MessageArgs msg = new MessageArgs(transform);
        obj.SendMessage("OnHoldInteract", msg, SendMessageOptions.DontRequireReceiver);
        return msg.received;
    }

    void Throw(){
        MessageArgs msg = new MessageArgs(transform);
        rightGrab.SendMessage("OnThrow", msg, SendMessageOptions.DontRequireReceiver);
        //Debug.Log("Throw");
        //Vector3 angle = (new Vector3(throwingAngle, Mathf.Pow((1-Mathf.Pow(throwingAngle, 2)), 1/2), 0));
        //throwingAngle = new Vector3(throwingAngleX, Mathf.Cos(throwingAngleX), 0).normalized;
        //Debug.DrawRay(transform.position, throwingAngle, Color.magenta, 3f);
        Transform obj = rightGrab;
        if (obj){
            if (obj.tag=="Item"){
                if (!msg.received){
                    //rightGrab.GetComponent<Rigidbody>().AddForce((transform.forward+transform.up)*f, ForceMode.Impulse);
                    ReleaseHand(true, false, true);
                    obj.GetComponent<Rigidbody>().AddForce(throwingAngle * throwPower * throwForce, ForceMode.Impulse);
                }
            }
            else if (obj.tag=="Grabbable"){
                //rightGrab.GetComponent<Rigidbody>().AddForce((transform.forward+transform.up)*0.5f, ForceMode.Impulse);
                ReleaseHand(true, true, true);
                obj.GetComponent<Rigidbody>().AddForce(throwingAngle * throwPower * throwForce, ForceMode.Impulse);
            }
            else if (obj.tag=="Draggable"){
                ReleaseHand(true, true, true);
                obj.GetComponent<Rigidbody>().AddForce(new Vector3(Mathf.Sign(rightGrab.position.x-transform.position.x), 0, 0) * 4f, ForceMode.Impulse);
            }
        }
    }

    void SwapHands(){
        Transform r = rightGrab;
        Transform l = leftGrab;
        
        bool flag = false;
        if (r){
            if (r.tag==interactTags[0]) flag = true;
        }
        else if (l){
            if (l.tag==interactTags[0]) flag = true;
        }
        if (flag){
            rightGrab = l;
            leftGrab = r;
            rightHandGrabbing = l;
            leftHandGrabbing = r;

            Transform temp = ItemParent[0];
            ItemParent[0] = ItemParent[1];
            ItemParent[1] = temp;
            
            if (rightGrab){
                rightGrab.parent = HandPivot[0];
                StartCoroutine(GrabOverTime(rightGrab, HandPivot[0], 10f, 1));
            }
            if (leftGrab){
                leftGrab.parent = HandPivot[1];
                StartCoroutine(GrabOverTime(leftGrab, HandPivot[1], 10f, 0));
            }

            RigidbodyConstraints cons = rigidbodyConstraints[0];
            rigidbodyConstraints[0] = rigidbodyConstraints[1];
            rigidbodyConstraints[1] = cons;

        }
        /*
        ItemParent[hand] = obj.parent;
        obj.parent = HandPivot[hand];

        rightGrab.SendMessage("OnGrab", new MessageArgs(transform), SendMessageOptions.DontRequireReceiver);

        StopCoroutine("GrabOverTime");
        StartCoroutine(GrabOverTime(obj, HandPivot[hand], 10f, hand));

        if (obj.GetComponent<Rigidbody>()) {
            rigidbodyConstraints[hand] = obj.GetComponent<Rigidbody>().constraints;
            obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        SetLayer (obj, "IgnorePlayer");
        //SetIgnoreCollision(obj.GetComponentInChildren<Collider>(), true);
        */
    }

    void SetThrowAngle(float inpY, bool fixAdding=true){
        //Debug.Log("Cross Y");
        throwingAngleX = Mathf.Clamp(throwingAngleX + Time.deltaTime * Mathf.Sign(inpY), -Mathf.PI/2, Mathf.PI/2);
    }

    void SetThrowForce(float inpX, bool fixAdding=true){
        //Debug.Log("Cross X");
        throwForce =  Mathf.Clamp(throwForce + Time.deltaTime * Mathf.Sign(inpX), 0, 1);
    }

    //called from other playable character when they take item from hand
    public void SendMessageReleaseHand(bool rRight = true, bool rLeft = true){
        ReleaseHand(rRight, rLeft);
    }

    void OnTriggerExit(Collider collider)
    {
        if (this.isActiveAndEnabled && collider.gameObject.layer == LayerMask.NameToLayer("ObjectSafeArea") && !IsOnHand(collider.transform)){
            InteractControl i = collider.GetComponentInParent<InteractControl>();
            if (!i){
                SetLayer(collider.transform.parent.transform, "Default");
            }
        }
    }

    void SetLayer (Transform obj, string value){
        obj.gameObject.layer = LayerMask.NameToLayer(value);
    }
}
