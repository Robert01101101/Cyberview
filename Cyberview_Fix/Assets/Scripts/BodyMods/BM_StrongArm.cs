using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BM_StrongArm : AbstractBodyMod
{
    // Strong arm body mod. Picks up box by manually setting its position to be player Pos + blockOffset. Collisions are handled by
    // enabling the player's blockColliderHelper. Otherwise, the player could push the box into the wall, as setting the position manually
    // overrides in-game physics (and therefore the box collider's). Script also releases hold on the box if stuck on a ledge.

    bool holdingBox;
    bool droppingBox;
    GameObject heavyBox;
    GameObject blockColliderHelper;
    private static float offsetX = 4.3f;
    Vector3 blockOffset = new Vector3(offsetX, 0, 0);
    float originalBodyBoneYPos;
    private float helperOrigYPos = -0.35f;
    private float helperFinalYPos = 0.15f;
    int framesStuckOnLedge;
    private AudioSource audio;

    void Start()
    {
        originalBodyBoneYPos = owner.bodyBone.transform.position.y;
        audio = GetComponent<AudioSource>();
    }

    //------------------------------------------------------ Released Hold Box Button
    public override void DisableBodyMod()
    {
        if (holdingBox && !droppingBox) droppingBox = true;
        if (!holdingBox && droppingBox) droppingBox = false;
    }

    //------------------------------------------------------ Pressed Hold Box Button
    public override void EnableBodyMod()
    {
        if (holdingBox && droppingBox)
        {
                                                            //drop box
            GotoState(BodyModState.INACTIVE);
            holdingBox = false;
            //reset colliders
            heavyBox.transform.parent = null;
            blockColliderHelper.SetActive(false);
            //heavyBox.GetComponent<BoxCollider2D>().enabled = true;
            heavyBox.GetComponent<Rigidbody2D>().simulated = true;
            heavyBox = null;
            owner.GetComponentInChildren<Animator>().SetBool("grab", false);
            owner.strongArmsInUse = false;
            framesStuckOnLedge = 0;

        } else if (!heavyBox && !droppingBox)
        {
                                                            //Pick up box
            //clear heavy box
            heavyBox = null;

            List<GameObject> interactables = new List<GameObject>();
            interactables = owner.GetInteractables();

            //get all interactables in range
            List<GameObject> boxes = new List<GameObject>();

            //sort out HeavyBoxes
            foreach (GameObject go in interactables)
            {
                if (go.tag == "HeavyBlock" || go.tag == "Orb") boxes.Add(go);
            }

            //get closest HeavyBox
            if (boxes.Count > 1)
            {
                //box detected
                bool boxesLeft = false;
                bool boxesRight = false;
                foreach (GameObject box in boxes)
                {
                    //check if there are boxes on both sides
                    if (box.transform.position.x - owner.gameObject.transform.position.x > 0) { boxesRight = true; } else { boxesLeft = true; }
                }
                if (boxesLeft && boxesRight)
                {
                    //discard all boxes not on player's facing side
                    if (owner.gameObject.transform.localScale.x > 0) {
                        //player facing right
                        for (int i=boxes.Count-1; i>=0; i--)
                        {
                            if (boxes[i].transform.position.x - owner.gameObject.transform.position.x < 0) boxes.Remove(boxes[i]);
                        }
                    } else {
                        //player facing left
                        for (int i = boxes.Count-1; i >= 0; i--)
                        {
                            if (boxes[i].transform.position.x - owner.gameObject.transform.position.x > 0) boxes.Remove(boxes[i]);
                        }
                    }
                }
                float closestYDistance = 10f;
                foreach (GameObject box in boxes)
                {
                    //get the one at 241's height
                    float Ydistance = Vector2.Distance(new Vector2(0, owner.gameObject.transform.position.y), new Vector2(0, box.transform.position.y));
                    if (Ydistance < closestYDistance) { heavyBox = box; closestYDistance = Ydistance; }
                }
                setUpBoxForTracking();
                
            }
            else if (boxes.Count == 1)
            {
                //no box detected
                heavyBox = boxes[0];
                setUpBoxForTracking();
            }
        }  
    }

    private void setUpBoxForTracking ()
    {
        //enable helper collider
        if (blockColliderHelper == null) blockColliderHelper = FindObject(owner.gameObject, "BlockColliderHelper");
        blockColliderHelper.transform.localPosition = new Vector3(blockColliderHelper.transform.localPosition.x, helperOrigYPos, blockColliderHelper.transform.localPosition.z);
        blockColliderHelper.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(MoveCollider());

        //disable box collider
        //heavyBox.GetComponent<BoxCollider2D>().enabled = false;
        heavyBox.GetComponent<Rigidbody2D>().simulated = false;

        audio.Stop();
        audio.Play();

        GotoState(BodyModState.ACTIVE);
        holdingBox = true;

        owner.DecreaseHealth(energyCostPerTick, false);

        owner.GetComponentInChildren<Animator>().SetBool("grab", true);
        owner.strongArmsInUse = true;

        heavyBox.transform.SetParent(owner.bodyBone.gameObject.transform);

        if (heavyBox.gameObject.tag == "Orb") GameObject.Find("FinalBossCutscene").GetComponent<FinalBossCutscene>().PickedUpOrb();
    }
    
    IEnumerator MoveCollider()
    {
        Vector3 vecA = new Vector3(blockColliderHelper.transform.localPosition.x, helperOrigYPos, blockColliderHelper.transform.localPosition.z);
        Vector3 vecB = new Vector3(blockColliderHelper.transform.localPosition.x, helperFinalYPos, blockColliderHelper.transform.localPosition.z);
        //floorAvatar.position 
        float step = (3 / (vecA - vecB).magnitude) * Time.fixedDeltaTime;
        float t = 0;
        while (t <= 1.0f)
        {
            t += step; // Goes from 0 to 1, incrementing by step each time
            blockColliderHelper.transform.localPosition = Vector3.Lerp(vecA, vecB, t); // Move objectToMove closer to b
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
        blockColliderHelper.transform.localPosition = vecB;
    }


    //(usually, we could use: GameObject.Find(name), but that only returns active gameobjects. This also gets inactive ones.)
    //(from: https://answers.unity.com/questions/890636/find-an-inactive-game-object.html )
    public GameObject FindObject(GameObject parent, string name)
    {
        Transform[] trs = parent.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }

    //------------------------------------------------------ Holding Box
    private void Update()
    {
        if (macroState == BodyModState.ACTIVE && heavyBox != null)
        {
            //flip if necessary
            if (owner.transform.localScale.x > 0) blockOffset = new Vector3(offsetX, 1, 0);
            else if (owner.transform.localScale.x < 0) blockOffset = new Vector3(-offsetX, 1, 0);

            //position box
            heavyBox.transform.position = (owner.bodyBone.transform.position + blockOffset);//(owner.transform.position + blockOffset);

            //decrease health every tick
            if (!tickDelay) StartCoroutine(DecreaseHealthAfterTick());

            //let go of box if stuck on ledge
            if (!owner.isGrounded && Mathf.Abs(owner.gameObject.GetComponent<Rigidbody2D>().velocity.y) < 1f)
            {
                framesStuckOnLedge += 5;
            }
            if (framesStuckOnLedge > 100) EnableBodyMod();
        }
        if (framesStuckOnLedge > 0) framesStuckOnLedge--;
    }

    public override void EquipBodyMod()
    {
    }

    public override void UnequipBodyMod()
    {
    }

    public GameObject GetBox() { return heavyBox; }

    public void ResetArm()
    {
        if (blockColliderHelper == null) blockColliderHelper = FindObject(owner.gameObject, "BlockColliderHelper");

        GotoState(BodyModState.INACTIVE);
        holdingBox = false;
        //reset colliders
        if (heavyBox!=null) heavyBox.transform.parent = null;
        blockColliderHelper.SetActive(false);
        //heavyBox.GetComponent<BoxCollider2D>().enabled = true;
        if (heavyBox != null) heavyBox.GetComponent<Rigidbody2D>().simulated = true;
        heavyBox = null;
        owner.GetComponentInChildren<Animator>().SetBool("grab", false);
        owner.strongArmsInUse = false;
    }
}
