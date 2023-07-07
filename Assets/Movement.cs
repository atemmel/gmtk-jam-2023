using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    const int LEFT = 0;
    const int RIGHT = 1;
    const int UP = 2;
    const int DOWN = 3;

    CharacterController characterController;
    float charVelocity = 0f;

    Vector3 direction;
    Vector2 input;

    [SerializeField] private float speed;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        characterController.Move(direction*speed*Time.deltaTime);
    }

    //User Controls
    public void userControls(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        Debug.Log(input);
        direction = new Vector3(input.x, 0, 0);
    }

    //Movement Controller
    // void makeMove(int dir)
    // {
    //     switch(dir)
    //     {
    //         case LEFT:
    //             moveLeft(1);
    //             break;
    //         case RIGHT:
    //             moveRight(1);
    //             break;
    //         case UP:
    //             moveUp(1);
    //             break;
    //         case DOWN:
    //             moveDown(1);
    //             break;
    //         default:
    //             break;
    //     }
    // }

    // //Basic Movement actions
    // void moveLeft(float mul)
    // {
    //     characterController.Move()
    // }

    // void moveRight(float mul)
    // {
    //     transform.position += new Vector3(1*mul, 0, 0);
    // }

    // void moveUp(float mul)
    // {
    //     transform.position += new Vector3(0, 1, 0);
    // }

    // void moveDown(float mul)
    // {
    //     transform.position += new Vector3(0, -1*mul, 0);
    // }
}
