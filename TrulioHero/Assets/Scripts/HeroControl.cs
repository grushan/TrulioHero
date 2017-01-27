using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroControl : MonoBehaviour {

	Rigidbody2D heroBody;
	Animator animator;
	public PhysicsMaterial2D forGroundMaterial;
	public PhysicsMaterial2D forWallMaterial;
	//public vars
	public float maxSpeed = 10f;
	public float jumpForce = 10f;
	public Transform groundCheck;
	public Transform wallCheck;
	public float groundRadius = 0.2f;
	public LayerMask whatIsGround;

	//moving
	float heroMove = 0f;

	//animation
	bool facingRight = true;

	//states
	public bool onGround;
	public bool onWall;
	public int jumpValue = 0;
	public int jumpMax = 15;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		heroBody = GetComponent<Rigidbody2D>();
		animator.SetBool ("IDLE", true);
	}

	// Update is called once per frame
	void Update () {

		Vector2 heroVel = heroBody.velocity;

		if ((facingRight && heroMove < 0) || heroMove > 0 && !facingRight) {
			Flip ();
		} 
		if (onGround) {
			if (Input.GetKeyDown (KeyCode.Z)) {
				heroBody.AddForce (new Vector2 (0f, jumpForce * 10), ForceMode2D.Impulse);
				jumpValue = 1;
			}
		}   
		if (Input.GetKey (KeyCode.Z) && jumpValue > 0 && jumpValue < jumpMax) {
			if (onGround) {
				jumpValue = 0;
			}
			heroBody.AddForce (new Vector2 (0f, jumpForce * 10), ForceMode2D.Force);
			jumpValue++;
		}
		if (jumpValue > jumpMax-1) {
			jumpValue = 0;
		}
	}

	void Flip (){
		facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
	private void FixedUpdate(){
		heroMove = Input.GetAxis ("Horizontal");
		heroBody.velocity = new Vector2(heroMove * maxSpeed, heroBody.velocity.y);
		onGround = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		onWall = Physics2D.OverlapCircle (wallCheck.position, 0.4f, whatIsGround);
		heroBody.sharedMaterial = (!onWall || !onGround) ? forWallMaterial : forGroundMaterial;

		UpdateAnimation ();

	}

	private void UpdateAnimation (){
		animator.SetBool ("IDLE", (heroMove == 0 && onGround));
		animator.SetBool ("WALK", (Mathf.Abs (heroMove) > 0 && !animator.GetBool ("WALK") && onGround));
		animator.SetBool ("JUMP", (!onGround && !animator.GetBool ("JUMP")));
	}

}
