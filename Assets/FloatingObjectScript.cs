using Leap.Unity;
using Leap.Unity.Interaction;

using UnityEngine;

public class FloatingObjectScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        floating = false;
        // set gravity on
        rigidBody = gameObject.GetComponent<Rigidbody>();
        rigidBody.useGravity = false;
        rigidBody.freezeRotation = true;

    }

    // Update is called once per frame
    void Update () {
	    if(floating)
        {
            if(rigidBody != null)
            {
                rigidBody.AddForceAtPosition(acceleration * rigidBody.mass * forceDirection, rigidBody.centerOfMass);
            }
        }
	}


    private void OnCollisionEnter(Collision collision)
    {
        if (IsPartOfHand(collision.collider))
        {
            if (collision.contacts.Length <= 0)
            {
                // it should never happen
                return;
            }

            floating = true;
        }
    }


    private bool IsPartOfHand(Collider collider)
    {
        return (collider.GetComponent<InteractionBrushBone>() != null || collider.GetComponentInParent<IHandModel>() != null);
    }

    [Tooltip("Acceleration applied to the object on contact. Gravity is disabled.")]
    public float acceleration;

    [Tooltip("Direction of the force applied to the object on contact")]
    public Vector3 forceDirection;

    private bool floating;
    private Rigidbody rigidBody;
}
