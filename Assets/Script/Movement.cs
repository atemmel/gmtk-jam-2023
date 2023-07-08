using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    const float gravityCE = -9.82f;

	private Animator animator;
	private SpriteRenderer sprite;

	bool lookLeft = false;

    CharacterController characterController;
    float charVelocity;

    Vector3 direction = Vector3.zero;
    Vector2 input;

    [SerializeField] private float gravityMul = 3f;
    [SerializeField] private float speed = 7;
    [SerializeField] private float jumpHeight = 5;
    [SerializeField] private GameObject leader;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        applyGravity();
        moveObj();
		var delta = direction * speed * Time.deltaTime;
        characterController.Move(delta);
		animator.SetInteger("Direction", (int)direction.x);
		sprite.flipX = lookLeft;
	}

    //Follower
    float getDir()
    {
        float difference = transform.position.x - leader.transform.position.x;

        if(difference < -transform.localScale.x)
        {
			lookLeft = false;
            return 1*speed;
        }
        else if(difference > transform.localScale.x)
        {
			lookLeft = true;
            return -1*speed;
        }
        return 0;
    }

    bool getJump()
    {
        if(transform.position.y + transform.localScale.y/2 < leader.transform.position.y)
        {
            return true;
        }
        return false;
    }

    void moveObj()
    {
        direction.x = getDir();

        if(getJump())
        {
            jump();
        }
    }

    //Gravitas
    void jump()
    {
        if(characterController.isGrounded)
        {
            charVelocity += jumpHeight;
        }
    }
    void applyGravity()
    {
        if(characterController.isGrounded && charVelocity < 0)
        {
            charVelocity = -1.0f;
        }
        else
        {
            charVelocity += gravityCE * gravityMul * Time.deltaTime;
        }
        direction.y = charVelocity;
    }

    // User Controls
    // public void userControls(InputAction.CallbackContext context)
    // {
    //     input = context.ReadValue<Vector2>();
    //     Debug.Log(input);
    //     direction.x = input.x;
    //     if(input.y == 1)
    //     {
    //         jump();
    //     }
    // }
}
