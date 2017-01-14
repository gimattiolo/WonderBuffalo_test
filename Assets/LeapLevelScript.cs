using UnityEngine;
using System.Collections;
using Leap;
using Leap.Unity.Interaction;

public class LeapLevelScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        manager = (InteractionManager)FindObjectOfType(typeof(InteractionManager));
        marker = GameObject.Find("Capsule");

        //IInteractionBehaviour b = marker.GetComponent<IInteractionBehaviour>();
        //manager.GraspWithHand(manager., marker.GetComponent<InteractionManager>());

        done = false;

    }

    // Update is called once per frame
    void Update () {
	
	}

    public void OnHandGrasp(Hand hand)
    {
        print("grasped");
        if (done)
        {
            return;
        }
        done = true;

        manager.ReleaseHand(hand.Id);



        //marker.transform.parent = ;

        //ma

        //Vector3 handNormal = new Vector3(h.PalmNormal.x, h.PalmNormal.y, h.PalmNormal.z);
        //Vector3 handDirection = new Vector3(h.Direction.x, h.Direction.y, h.Direction.z);

        //Vector3 bodyPosition = new Vector3(h.PalmPosition.x, h.PalmPosition.y, h.PalmPosition.z);// + 1.0f * offset;//;_obj.warper.RigidbodyPosition;

        //Quaternion bodyRotation = Quaternion.LookRotation(handDirection, Vector3.Cross(handNormal, handDirection));//= _obj.warper.RigidbodyRotation;


        manager.GraspWithHand(hand, marker.GetComponent<IInteractionBehaviour>());
    }
    public void OnHandRelease(Hand hand)
    {
        print("released");
        //manager.ReleaseHand(hand.Id);

        //manager.GraspWithHand(hand, marker.GetComponent<IInteractionBehaviour>());

    }


    private InteractionManager manager;
    private GameObject marker;

    bool done;
}
    