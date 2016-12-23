using UnityEngine;
using System.Collections;

public class Navigation : MonoBehaviour
{
    public float turnSpeed = 50f;

    public

    void MouseAiming()
    {
        var rot = new Vector3(0f, 0f, 0f);
        // rotates Camera Left

        if (Mathf.Abs(Input.GetAxis("Mouse X")) > Mathf.Abs(Input.GetAxis("Mouse Y")))
        {
            if (Input.GetAxis("Mouse X") < 0f)
            {
                rot.y -= 1f;
            }
            // rotates Camera Left
            if (Input.GetAxis("Mouse X") > 0f)
            {
                rot.y += 1f;
            }
        }
        else
        {
            //rotates Camera Up
            if (Input.GetAxis("Mouse Y") < 0f)
            {
                rot.x -= 1f;
            }
            // rotates Camera Down
            if (Input.GetAxis("Mouse Y") > 0f)
            {
                rot.x += 1f;
            }
        }

        transform.Rotate(rot, turnSpeed * Time.deltaTime);
    }

    void KeyboardMovement()
    {
        var sensitivity = 0.01f;
        var movementAmount = 0.5f;
        var movementVector = new Vector3(0f, 0f, 0f);
        var hMove = Input.GetAxis("Horizontal");
        var vMove = Input.GetAxis("Vertical");
        // left arrow
        if (hMove < -sensitivity) movementVector.x = -movementAmount;
        // right arrow
        if (hMove > sensitivity) movementVector.x = movementAmount;
        // up arrow
        if (vMove < -sensitivity) movementVector.z = -movementAmount;
        // down arrow
        if (vMove > sensitivity) movementVector.z = movementAmount;
        // Using Translate allows you to move while taking the current rotation into consideration
        transform.Translate(movementVector);
    }

    void Update()
    {
        MouseAiming();
        KeyboardMovement();
    }
}