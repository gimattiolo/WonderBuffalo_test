using UnityEngine;
using Leap.Unity.Interaction;
using Leap;
using System.Collections.Generic;
using LeapInternal;
using Leap.Unity.Interaction.CApi;


/**
* IHoldingPoseController defines the interface used by the Interaction Engine
* to request the best pose for an object when it is held.
*
* The Interaction Engine provides the HoldingPoseControllerKabsh implementation for
* this controller.
* @since 4.1.4
*/
public class MyHoldingPoseController : IHoldingPoseController
{
    /**
    * Add the specified hand to the pose calculation.
    * @param hand The Leap.Hand object containing the reported tracking data.
    * @since 4.1.4
    */
    public override void AddHand(Hand hand)
    { }
    /**
    * Reports that a hand has been re-identified and that you should replace the
    * old hand with the new hand data.
    * @param oldId the previous Leap.Hand.Id value
    * @param newId the replacement Leap.Hand.Id
    * @since 4.1.4
    */
    public override void TransferHandId(int oldId, int newId)
    { }
    /**
    * Remove the specified hand from the pose calculation.
    * @param hand The Leap.Hand object to be removed.
    * @since 4.1.4
    */
    public override void RemoveHand(Hand hand)
    { }
    /**
    * Calculate the best holding pose given the current state of the hands and 
    * interactable object.
    * @param hands the list of hands with the current tracking data.
    * @param position A Vector3 object to be filled with the disred object position.
    * @param rotation A Quaternion object to be filled with the desired rotation.
    * @since 4.1.4
    */
    public override void GetHoldingPose(ReadonlyList<Hand> hands, out Vector3 position, out Quaternion rotation)
    {
        if(hands.Count <= 0)
        {
            //return;
        }

        Hand h = hands[0];

        Vector3 handNormal = new Vector3(h.PalmNormal.x, h.PalmNormal.y, h.PalmNormal.z);
        Vector3 handDirection = new Vector3(h.Direction.x, h.Direction.y, h.Direction.z);

        Vector3 bodyPosition = new Vector3(h.PalmPosition.x, h.PalmPosition.y, h.PalmPosition.z);// + 1.0f * offset;//;_obj.warper.RigidbodyPosition;

        Quaternion bodyRotation = Quaternion.LookRotation(handDirection, Vector3.Cross(handNormal, handDirection));//= _obj.warper.RigidbodyRotation;
        position = bodyPosition;
        rotation = bodyRotation;
    }
}
