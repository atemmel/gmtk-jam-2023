using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    const float gravityCE = -9.82f;


	[SerializeField] AudioClip[] sounds; 
	public GameObject lowerRay;
	public GameObject upperRay;
	public ParticleSystem deathSplat;

	private Animator animator;
	private SpriteRenderer sprite;
	private AudioSource aud;

	bool running = false;
	bool alive = true;

	float timeOfDeath = 0;

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
		aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
		if(alive)
		{
			bool falling = body.velocity.y < -0.0f;
			animator.SetBool("falling", falling);
			sprintToggle();
			if(!running) {
				return;
			}

			if(!aud.isPlaying)
			{
				aud.clip = sounds[0];
				aud.Play();
			}
			moveObj();

			var dx = direction * speed;
			var v = body.velocity;
			body.velocity = new Vector2(dx.x, v.y);
			animator.SetInteger("Direction", (int)dx.x);
			sprite.flipX = lookDir == Direction.Left;
		}
		else{
			if(!aud.isPlaying)
			{
				aud.clip = sounds[1];
				aud.Play();
			}
			if(Time.time - timeOfDeath > 3)
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
		}
	}

	void sprintToggle()
	{
		if(Input.GetKeyDown("space")){
			running = !running;
			animator.SetBool("running", running);
			if(!running){aud.Stop();}
		}
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

		if (shortHit.collider == null || shortHit.collider.tag == "Exit") {
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
		aud.Stop();
		var v = body.velocity;
		v.y = jumpVelocity;
		body.velocity = v;
    }

	public void Slay() {
		if(!alive) return;
		aud.Stop();
		running = false;
		alive = false;
		body.velocity = Vector3.zero;
		animator.SetBool("Dying", true);
		animator.SetBool("falling", false);
		StartCoroutine(splat());
		timeOfDeath = Time.time;
	}

	IEnumerator splat()
    {
        yield return new WaitForSeconds(1);
		var obj = Instantiate(deathSplat, transform);
		Destroy(obj, 60);
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

    void OnCollisionEnter2D(Collision2D collision)
    {
		/*
		var contact = collision.GetContact(0);
		var contactPoint = contact.point;
		var ownCenter = contact.otherCollider.bounds.center;
		Debug.Log(body.velocity.y);
		if(contactPoint.y < ownCenter.y && body.velocity.y < -7f) {
			Debug.Log("Ska dö");
		} else {
			Debug.Log("Ska inte dö");
		}
		*/
		Debug.Log(collision.relativeVelocity.ToString());
		if(collision.relativeVelocity.y > 6f) {
			Slay();
		}
    }
}
