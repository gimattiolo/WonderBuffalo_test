using UnityEngine;

using System;
using System.Collections;
using Leap;
using Leap.Unity;
using Leap.Unity.Interaction;

public class InteractivePlaneScript : GridScript {

	// Use this for initialization
	void Start () {
        // from GridScript
        Init();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateRipples();
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (GetActiveRippleNumber() >= maxRipples)
        {
            return;
        }

        if (IsPartOfHand(collision.collider))
        {
            if(collision.contacts.Length <= 0)
            {
                // it should never happen
                return;
            }

            ContactPoint contact = collision.contacts[0];
            print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
            Debug.DrawRay(contact.point, contact.normal, Color.white, 3.0f);
            bool forced = false;
            bool ans = CreateRipple(contact.point, 2.0f, 2f, (float)Math.PI, 0.0f, 1.0f / (-10.0f), 1.0f / (-0.25f), forced);
        }
    }

    private void OnCollisionExit(Collision collision)
    {

    }

    private bool IsPartOfHand(Collider collider)
    {
        return (collider.GetComponent<InteractionBrushBone>() != null || collider.GetComponentInParent<IHandModel>() != null);
    }
}
