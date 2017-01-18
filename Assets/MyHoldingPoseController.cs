using UnityEngine;
using Leap.Unity.Interaction;
using Leap;


public class MyHoldingPoseController : IHoldingPoseController
{
    [Tooltip("Object spaceRotation axis on grab.")]
    [SerializeField]
    private Vector3 bodyRotationAxis = new Vector3(0.0f, 1.0f, 0.0f);

    [Tooltip("Rotation degrees angle on grab.")]
    [SerializeField]
    private float bodyRotationAngle = 0.0f;

    [Tooltip("Offset along hand direction on grab.")]
    [SerializeField]
    private float bodyOffset = 0.0f;

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
        position = new Vector3();
        rotation = new Quaternion(0.0f, 1.0f, 0.0f, 0.0f);

        if (hands.Count <= 0)
        {
            return;
        }

        Hand h = hands[0];

        Vector3 handNormal = new Vector3(h.PalmNormal.x, h.PalmNormal.y, h.PalmNormal.z);
        Vector3 handDirection = new Vector3(h.Direction.x, h.Direction.y, h.Direction.z);


        Vector3 bodyPosition = new Vector3(h.PalmPosition.x, h.PalmPosition.y, h.PalmPosition.z);// + offset * handNormal;
        //Vector3 bodyPosition = new Vector3(h.WristPosition.x, h.WristPosition.y, h.WristPosition.z);// + bodyOffset * handDirection;
                                                                                                 //Debug.Log("Caught");
        Quaternion bodyRotation = Quaternion.LookRotation(handDirection, Vector3.Cross(handNormal, handDirection));

        //if(bodyRotation.w < 1.0f)
        //{
        //    Debug.Log("Caught");
        //}

        position = bodyPosition;
        bodyRotationAxis.Normalize();
        rotation = bodyRotation;// Quaternion.AngleAxis(bodyRotationAngle, bodyRotationAxis);

        //Debug.Log("Hold " + rotation);
    }
}
