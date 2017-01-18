using UnityEngine;
using System;
using Leap;
using Leap.Unity;
using Leap.Unity.Interaction;
using UnityEngine.UI;

public class StickingObject : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        GameObject textGameObject = GameObject.Find("Text");

        screenText = textGameObject.GetComponent<Text>();

        manager = (InteractionManager)FindObjectOfType(typeof(InteractionManager));

        controller = new Controller();
        
        //controller.FrameReady += new EventHandler<FrameEventArgs>(OnFrameReady);
        //controller.InternalFrameReady += new EventHandler<InternalFrameEventArgs>(OnInternalFrameReady);

        riggedHand = null;
        graspingHand = null;
        grasped = false;

        graspedObject = gameObject;

        behaviour = graspedObject.GetComponent<IInteractionBehaviour>();

        isCurrentGraspingHandValid = false;
        isPreviousGraspingHandValid = true;

        done = false;

        graspStartTime = -1.0f;
   }

    private bool isGraspingHandValid()
    {
        if(graspingHand == null)
        {
            return false;
        }

        if (controller.IsConnected)
        {
            //controller is a Controller object 
            Frame frame = controller.Frame();
            for (int i = 0; i < frame.Hands.Count; ++i)
            {
                if (frame.Hands[i].IsLeft == graspingHand.IsLeft)
                {
                    return true;
                }
            }

        }
        return false;
    }

        // Update is called once per frame
    void Update()
    {
        // time out for testing
        float now = Time.time;
        if(graspStartTime >= 0.0f)
        {
            //screenText.text = "" + (now - start);

            if (now - graspStartTime >= 5.0f)
            {
                //screenText.text = "Expired";
                //Ungrasp();
                graspStartTime = -1.0f;
            }
        }

        //grasp object at startup for debugging purposes
        //if (!grasped)
        //{
        //    if (!done)
        //    {
        //        if (controller.IsConnected)
        //        {
        //            Frame frame = controller.Frame();
        //            //Frame previous = controller.Frame(1); //The previous frame

        //            for (int i = 0; i < frame.Hands.Count; ++i)
        //            {
        //                OnHandGrasp(frame.Hands[i]);
        //                done = true;
        //                isCurrentGraspingHandValid = true;
        //                break;
        //            }
        //        }
        //    }
        //}

        if (!grasped)
        {
            return;
        }

        isPreviousGraspingHandValid = isCurrentGraspingHandValid;
        isCurrentGraspingHandValid = isGraspingHandValid();

        if (isCurrentGraspingHandValid && !isPreviousGraspingHandValid)
        {
            if (controller.IsConnected)
            {
                //controller is a Controller object 
                Frame frame = controller.Frame();
                Frame previous = controller.Frame(1); //The previous frame

                Hand hand = null;

                for (int i = 0; i < frame.Hands.Count; ++i)
                {
                    hand = frame.Hands[i];
                    if (hand.IsLeft == graspingHand.IsLeft)
                    {
                        Debug.Log("*************Regrasping with " + frame.Id + "-" + hand.Id);

                        graspingHand = hand;
                        Grasp();
                    }
                }
            }
        }
    }

    private void OnFrameReady(object sender, FrameEventArgs args)
    {
        //string str = "Frame " + args.frame.Id + " : ";
        //for (int i = 0; i < args.frame.Hands.Count; ++i)
        //{
        //    str += "[ " + args.frame.Hands[i].Id + " ]";
        //}
        //Debug.Log(str);
    }
    
    private void OnInternalFrameReady(object sender, InternalFrameEventArgs args)
    {
        //Debug.Log("On Internal Frame - frame " + args.frame.tracking_id);
    }
    
    public void Ungrasp()
    {
        if (!grasped)
        {
            return;
        }

        if (manager == null)
        {
            return;
        }

        if(graspingHand == null)
        {
            return;
        }
        
        bool released = manager.ReleaseHand(graspingHand.Id);

        if (riggedHand != null)
        {
            riggedHand.PoseIsFrozen = false;
        }
    }

    public void Grasp()
    {
        if (manager == null)
        {
            return;
        }

        if(graspingHand == null)
        {
            return;
        }

        bool released = manager.ReleaseHand(graspingHand.Id);
        Debug.Log("Released " + released);

        manager.RegisterInteractionBehaviour(behaviour);

        try
        {
            manager.GraspWithHand(graspingHand, behaviour);

        }
        catch(InvalidOperationException e)
        {
            Debug.Log("Grasp Exception");
        }
        finally
        {
            Debug.Log("Being grasped " + behaviour.IsBeingGraspedByHand(graspingHand.Id));
            if (!behaviour.IsBeingGraspedByHand(graspingHand.Id))
            {
                isCurrentGraspingHandValid = false;
            }
            else
            {
                //behaviour.NotifyHandGrasped()graspin;

            }
        }
    }

    public void OnHandGrasp(Hand hand)
    {
        if (grasped)
        {
            return;
        }

        Debug.Log("OnHandGrasp");
        screenText.text = "OnHandGrasp";


        graspingHand = hand;

        grasped = true;

        Grasp();

        graspStartTime = Time.time;

        UnityEngine.Object[] objs = FindObjectsOfType(typeof(RiggedHand));


        for (int i = 0; i < objs.Length; ++i)
        {
            RiggedHand h = (RiggedHand)(objs[i]);
            if (h == null)
            {
                continue;
            }
            if (graspingHand.IsLeft && h.Handedness == Chirality.Left)
            {
                riggedHand = h;
                break;
            }
            riggedHand = h;
            break;
        }

        if (riggedHand != null)
        {
            riggedHand.PoseIsFrozen = true;
        }
    }

    public void OnHandRelease(Hand hand)
    {
        Debug.Log("OnHandRelease");
        screenText.text = "OnHandRelease";

        //grasped = false;
        //graspingHand = null;
        //riggedHand = null;
    }

    public bool grasped;

    private RiggedHand riggedHand;
    private Hand graspingHand;
    private Controller controller;
    private InteractionManager manager;

    private GameObject graspedObject;

    private IInteractionBehaviour behaviour;

    private bool isCurrentGraspingHandValid;
    private bool isPreviousGraspingHandValid;

    private bool done;

    private float graspStartTime;

    private Text screenText;
}