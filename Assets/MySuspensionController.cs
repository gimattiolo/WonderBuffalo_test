using UnityEngine;
using UnityEngine.Assertions;

namespace Leap.Unity.Interaction
{

    /**
    * The SuspensionControllerDefault class turns off rendering of suspended objects
    * and restores rendering when the suspension times out or the interaction 
    * simulation resumes.
    * @since 4.1.4
    */
    public class MySuspensionController : ISuspensionController
    {
        [SerializeField]
        private float _maxSuspensionTime = 4;

        private Renderer[] _renderers;

        private GameObject marker;

        private StickingObject stickingObject;

        protected override void Init(InteractionBehaviour obj)
        {
            base.Init(obj);

            stickingObject = obj.gameObject.GetComponent<StickingObject>();


            _renderers = obj.GetComponentsInChildren<Renderer>();
        }

        /** The timeout period. */
        public override float MaxSuspensionTime
        {
            get
            {
                return _maxSuspensionTime;
            }
        }

        /** Resumes rendering of the object. */
        public override void Resume()
        {
            //Debug.Log("Resume from suspension");

            _obj.rigidbody.useGravity = true;
            _obj.rigidbody.isKinematic = false;
            setRendererState(true);
            //if(stickingObject == null)
            //{
            //    return;
            //}
            //stickingObject.Grasp();
        }

        /** Suspends rendering of the object and sets the IsKinematic property of its rigid body to true. */
        public override void Suspend()
        {
            //Debug.Log("Go into suspension");

            //if (stickingObject != null)
            //{
            //    stickingObject.Ungrasp();
            //}


            //_obj.gameObject.transform.position.Set(0.0f, 10.0f, 0.0f);
            _obj.rigidbody.useGravity = false;
            _obj.rigidbody.isKinematic = true;
            setRendererState(false);
        }

        /** Resumes rendering of the object. */
        public override void Timeout()
        {
            _obj.rigidbody.useGravity = true;
            _obj.rigidbody.isKinematic = false;


            //Debug.Log("Timeout suspension");

            setRendererState(true);
        }

        private void setRendererState(bool visible)
        {
            for (int i = 0; i < _renderers.Length; i++)
            {
                _renderers[i].enabled = visible;
            }
        }

        /** Validates that the object remains kinematic when it is suspended. */
        public override void Validate()
        {
            base.Validate();

            if (_obj.UntrackedHandCount != 0)
            {
                Assert.IsTrue(_obj.rigidbody.isKinematic,
                              "Object must be kinematic when suspended.");
            }
        }
    }
}

