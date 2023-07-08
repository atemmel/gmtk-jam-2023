using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    const float gravityCE = -9.82f;

	public GameObject lowerRay;
	public GameObject upperRay;

	private Animator animator;
	private SpriteRenderer sprite;

	bool running = true;

    Rigidbody2D body;
    float charVelocity;

	enum Direction {
		Left,
		Right,
	};

	Direction lookDir = Direction.Right;

	enum LookAheadResult {
		Continue,
		Turn,
		Jump,
	};

    Vector2 direction = new Vector2(1f, 0f);
    Vector2 input;

    [SerializeField] private float speed = 1;
    [SerializeField] private float jumpVelocity = 1;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
		if(!running) {
			return;
		}

        moveObj();

		var dx = direction * speed;
		var v = body.velocity;
        body.velocity = new Vector2(dx.x, v.y);
		animator.SetInteger("Direction", (int)dx.x);
		sprite.flipX = lookDir == Direction.Left;
	}

	LookAheadResult lookAhead() {
		var d = direction;
		var lowerRayOffset = lowerRay.transform.localPosition;
		var upperRayOffset = upperRay.transform.localPosition;
		lowerRayOffset.x *= d.x;
		upperRayOffset.x *= d.x;
		var lowerRayOrigin = transform.position + lowerRayOffset;
		var upperRayOrigin = transform.position + upperRayOffset;
		var lowerRayAngle = GetVectorFromAngle(lowerRay.transform.rotation.z);
		var upperRayAngle = GetVectorFromAngle(upperRay.transform.rotation.z);
		lowerRayAngle.x *= d.x;
		upperRayAngle.x *= d.x;
		var shortHit = Physics2D.Raycast(lowerRayOrigin, lowerRayAngle, lowerRay.transform.localScale.x);
		var highHit = Physics2D.Raycast(upperRayOrigin, upperRayAngle, upperRay.transform.localScale.x);
		Debug.DrawRay(lowerRayOrigin, lowerRayAngle * lowerRay.transform.localScale.x, Color.red);
		Debug.DrawRay(upperRayOrigin, upperRayAngle * upperRay.transform.localScale.x, Color.red);

		if (shortHit.collider == null) {
			//Debug.Log("A");
			return LookAheadResult.Continue;
		}

		if (highHit.collider == null) {
			//Debug.Log("B");
			return LookAheadResult.Jump;
		}

		//Debug.Log("C");
		return LookAheadResult.Turn;
	}

    void moveObj()
    {
		var result = lookAhead();
		switch(result) {
			case LookAheadResult.Continue:
				break;
			case LookAheadResult.Turn:
				direction.x = -direction.x;
				switch(lookDir) {
					case Direction.Left:
						lookDir = Direction.Right;
						break;
					case Direction.Right:
						lookDir = Direction.Left;
						break;
				}
				break;
			case LookAheadResult.Jump:
				jump();
				break;
		}
    }

    //Gravitas
    void jump()
    {
		var v = body.velocity;
		v.y = jumpVelocity;
		body.velocity = v;
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

	static Vector3 GetVectorFromAngle(float angle) {
		return new Vector3(
				Mathf.Cos(angle), 
				Mathf.Sin(angle));
	}
}
