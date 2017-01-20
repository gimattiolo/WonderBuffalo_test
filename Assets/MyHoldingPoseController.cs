using UnityEngine;
using Leap.Unity.Interaction;
using Leap;
using Leap.Unity;


public class MyHoldingPoseController : IHoldingPoseController
{
    [Tooltip("Offset along hand normal on grab.")]
    [SerializeField]
    private float bodyHandNormalOffset = 0.0f;

    [Tooltip("Offset along hand grasp direction on grab.")]
    [SerializeField]
    private float bodyHandGraspOffset = 0.0f;


    /**
    * Add the specified hand to the pose calculation.
    * @param hand The Leap.Hand object containing the reported tracking data.
    */
    public override void AddHand(Hand hand)
    { }
    /**
    * Reports that a hand has been re-identified and that you should replace the
    * old hand with the new hand data.
    * @param oldId the previous Leap.Hand.Id value
    * @param newId the replacement Leap.Hand.Id
    */
    public override void TransferHandId(int oldId, int newId)
    { }
    /**
    * Remove the specified hand from the pose calculation.
    * @param hand The Leap.Hand object to be removed.
    */
    public override void RemoveHand(Hand hand)
    { }
    /**
    * Calculate the best holding pose given the current state of the hands and 
    * interactable object.
    * @param hands the list of hands with the current tracking data.
    * @param position A Vector3 object to be filled with the disred object position.
    * @param rotation A Quaternion object to be filled with the desired rotation.
    */
    public override void GetHoldingPose(ReadonlyList<Hand> hands, out Vector3 position, out Quaternion rotation)
    {
        position = Vector3.zero;
        rotation = Quaternion.identity;

        if (hands.Count <= 0)
        {
            return;
        }

        Hand h = hands[0];

        Vector3 handNormal = UnityVectorExtension.ToVector3(h.PalmNormal);
        //Debug.Log("hand normal " + handNormal);

        Vector3 handDirection = UnityVectorExtension.ToVector3(h.Direction);
        //Debug.Log("hand direction " + handDirection);

        Vector3 handGraspDirection = Vector3.Cross(handNormal, handDirection);
        //Debug.Log("hand grasp " + handGraspDirection);

        Vector3 bodyPosition = UnityVectorExtension.ToVector3(h.PalmPosition);
        bodyPosition += bodyHandNormalOffset * handNormal;
        bodyPosition += bodyHandGraspOffset * handGraspDirection;

        Quaternion bodyRotation = Quaternion.FromToRotation(Vector3.up, handGraspDirection);

        position = bodyPosition;
        rotation = bodyRotation;

        //Debug.Log("Hold " + rotation);
    }
}
