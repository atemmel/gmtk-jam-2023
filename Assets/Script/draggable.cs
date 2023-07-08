using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class draggable : MonoBehaviour
{
    const float gravityCE = -9.82f;
    
    bool grabbed = false;
    CharacterController characterController;
    float charVelocity;

    Vector3 direction = Vector3.zero;

    [SerializeField] private float gravityMul = 3f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        applyGravity();
        checkIfGrab();
        moveGrab();
        characterController.Move(direction*Time.deltaTime);
    }

    //Mouse actions
    Vector3 getWorldMouse()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void checkIfGrab()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = getWorldMouse();
            Debug.Log(mousePos);
            float topY = transform.position.y + transform.localScale.y/2;
            float botY = transform.position.y - transform.localScale.y/2;
            float rightX = transform.position.x + transform.localScale.x/2;
            float leftX = transform.position.x - transform.localScale.x/2;

            if(mousePos.y < topY && mousePos.y > botY && mousePos.x < rightX && mousePos.x > leftX)
            {
                grabbed = true;
            }
        }
    }

    void moveGrab()
    {
        if(grabbed && Input.GetMouseButton(0))
        {
            Debug.Log("it gets in");
            Vector3 newPos = getWorldMouse();
            newPos.z = 0;
            characterController.Move(newPos - transform.position);
        }
        else
        {
            grabbed = false;
        }
    }

    //Gravity
    void applyGravity()
    {
        if(grabbed){return;}
        else if(characterController.isGrounded && charVelocity < 0)
        {
            charVelocity = -1.0f;
        }
        else
        {
            charVelocity += gravityCE * gravityMul * Time.deltaTime;
        }
        direction.y = charVelocity;
    }
}
