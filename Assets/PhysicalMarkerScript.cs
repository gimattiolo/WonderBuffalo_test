using UnityEngine;

using Leap;
using Leap.Unity;
using Leap.Unity.Interaction;

using System.Collections;

public class PhysicalMarkerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //interactionManager = (InteractionManager)FindObjectOfType(typeof(InteractionManager));

        //interactionBehaviour = gameObject.AddComponent<InteractionBehaviour>();
        //interactionBehaviour.OnGraspBeginEvent += OnGrasped;
        //interactionBehaviour.OnGraspEndEvent += OnReleased;

        //interactionManager.RegisterInteractionBehaviour(interactionBehaviour);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnCollisionEnter(Collision collision)
    {
        //if (IsPartOfHand(collision.collider))
        //{
           // collision.collider.gameObject.GetComponent<>();
            //interactionManager.GraspWithHand(, interactionBehaviour);    
        //}
    }

    private bool IsPartOfHand(Collider collider)
    {
        return (collider.GetComponent<InteractionBrushBone>() != null || collider.GetComponentInParent<IHandModel>() != null);
    }

    //private bool IsBeingGrasped(Collider collider)
    //{
    //    //InteractionBehaviour interactionBehaviour = collider.GetComponent<InteractionBehaviour>();
    //    //if (interactionBehaviour == null) return false;
    //    //return interactionBehaviour.IsBeingGrasped;
    //}

    private void OnGrasped()
    {
    }

    private void OnReleased()
    {
    }


    //private InteractionManager interactionManager;
    //private InteractionBehaviour interactionBehaviour;

}
