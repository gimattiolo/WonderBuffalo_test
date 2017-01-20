/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap;

namespace Leap.Unity {
  /**
* Manages the orientation of the bones in a model rigged for skeletal animation.
* 
* The class expects that the graphics model bones corresponding to bones in the Leap Motion 
* hand model are in the same order in the bones array.
*/
  public class RiggedFinger : FingerModel {

    /** Allows the mesh to be stretched to align with finger joint positions
     * Only set to true when mesh is not visible
     */
    public bool deformPosition = false;

    public Vector3 modelFingerPointing = Vector3.forward;
    public Vector3 modelPalmFacing = -Vector3.up;

    public Quaternion Reorientation() {
      return Quaternion.Inverse(Quaternion.LookRotation(modelFingerPointing, -modelPalmFacing));
    }

    public RiggedHand riggedHand;

    // relative to the palm
    public Quaternion[] frozenFingerRelativeRotations = null;
    private bool fingerIsFrozen = false;


    public bool FingerIsFrozen
    {
        get
        {
            return fingerIsFrozen;
        }
        set
        {
            //bool freeze = (value == true && fingerIsFrozen != value);
            //fingerIsFrozen = value;
            //if (freeze)
            //{
            //    FreezeFinger();
            //}
            fingerIsFrozen = value;
        }
    }

    public void FreezeFinger()
    {
        frozenFingerRelativeRotations = new Quaternion[bones.Length];

        Quaternion palmRotation = riggedHand.GetRiggedPalmRotation();
        float angle;
        Vector3 axis;
        palmRotation.ToAngleAxis(out angle, out axis);
        Quaternion invPalmRotation = Quaternion.AngleAxis(-angle, axis);

        for (int i = 0; i < bones.Length; ++i)
        {
            if (bones[i] != null)
            {
                Quaternion fingerRotation = GetBoneRotation(i);
                // we store the rotation of each finger relative to the palm rotation
                frozenFingerRelativeRotations[i] = invPalmRotation  * fingerRotation;
            }
        }
    }

    public void FreezeFinger(Quaternion[] rotations)
    {
        frozenFingerRelativeRotations = new Quaternion[bones.Length];

        Debug.Assert(bones.Length == rotations.Length);
        for (int i = 0; i < bones.Length; ++i)
        {
            frozenFingerRelativeRotations[i] = rotations[i];
        }
    }

    /** Updates the bone rotations. */
    public override void UpdateFinger() {
      for (int i = 0; i < bones.Length; ++i) {
        if (bones[i] != null) {
          Quaternion boneRotation = GetBoneRotation(i);
          if (fingerIsFrozen && frozenFingerRelativeRotations != null)
          {

            Quaternion palmRotation = Quaternion.identity;
            if(riggedHand != null)
            {
                palmRotation = riggedHand.GetRiggedPalmRotation();
            }

            boneRotation = palmRotation * frozenFingerRelativeRotations[i];
          }

          bones[i].rotation = boneRotation * Reorientation();

          if (deformPosition) {
            bones[i].position = GetJointPosition(i);
          }
        }
      }
    }
    public void SetupRiggedFinger (bool useMetaCarpals) {
      findBoneTransforms(useMetaCarpals);
      modelFingerPointing = calulateModelFingerPointing();
    }

    private void findBoneTransforms(bool useMetaCarpals) {
      if (!useMetaCarpals || fingerType == Finger.FingerType.TYPE_THUMB) {
        bones[1] = transform;
        bones[2] = transform.GetChild(0).transform;
        bones[3] = transform.GetChild(0).transform.GetChild(0).transform;
      }
      else {
        bones[0] = transform;
        bones[1] = transform.GetChild(0).transform;
        bones[2] = transform.GetChild(0).transform.GetChild(0).transform;
        bones[3] = transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform;

      }
    }
    private Vector3 calulateModelFingerPointing() {
      Vector3 distance = transform.InverseTransformPoint(transform.position) -  transform.InverseTransformPoint(transform.GetChild(0).transform.position);
      Vector3 zeroed = RiggedHand.CalculateZeroedVector(distance);
      return zeroed;
    }
  } 
}
