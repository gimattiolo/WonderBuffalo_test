using UnityEngine;
using Leap;
using Leap.Unity;
using Leap.Unity.Interaction;

public class LeapLevelScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        manager = (InteractionManager)FindObjectOfType(typeof(InteractionManager));
        controller = (LeapHandController)FindObjectOfType(typeof(LeapHandController));

        //controller.GraphicsEnabled = false;
        //controller.PhysicsEnabled = false;

        marker = GameObject.Find("Capsule");
        done = false;




    }

    // Update is called once per frame
    void Update () {
	
	}

    public void OnHandGrasp(Hand hand)
    {
        //print("OnHandGrasp");
        if (done)
        {
            return;
        }

        done = true;

        manager.ReleaseHand(hand.Id);
        manager.GraspWithHand(hand, marker.GetComponent<IInteractionBehaviour>());

        Object[] objs = FindObjectsOfType(typeof(RiggedHand));

        RiggedHand riggedHand = null;
        for (int i = 0; i < objs.Length; ++i)
        {
            RiggedHand h = (RiggedHand)(objs[i]);
            if (hand.IsLeft && h.Handedness == Chirality.Left)
            {
                riggedHand = h;
                break;
            }
            riggedHand = h;
            break;
        }

        if(riggedHand != null)
        {
            riggedHand.PoseIsFrozen = true;
        }
    }
    public void OnHandRelease(Hand hand)
    {
        //print("OnHandRelease");
    }

    private LeapHandController controller;
    private InteractionManager manager;
    private GameObject marker;
    private bool done;
}
    