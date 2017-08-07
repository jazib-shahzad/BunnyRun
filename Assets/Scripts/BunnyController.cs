using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BunnyController : MonoBehaviour {

	private Rigidbody2D myRigidBody;
	private Animator myAnim;
	private Collider2D myCollider;
	public float bunnyJumpForce = 500f;
	private float bunnyHurtTime = -1;
	private int jumpsLeft = 2;

	public Text scoreText;
	private float startTime;

	public AudioSource jumpSfx;
	public AudioSource deathSfx;

	// Use this for initialization
	void Start () {

		myRigidBody = GetComponent<Rigidbody2D> ();
		myAnim = GetComponent<Animator> ();
		myCollider = GetComponent<Collider2D> ();

		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Escape)) {

			SceneManager.LoadScene ("Title");
		}

		if (bunnyHurtTime == -1) {

			if (Input.GetButtonUp ("Jump") && jumpsLeft > 0) {

				if (myRigidBody.velocity.y < 0) {

					myRigidBody.velocity = Vector2.zero;
				}

				if (jumpsLeft == 1) {

					myRigidBody.AddForce (transform.up * bunnyJumpForce * 0.75f);

				} else {

					myRigidBody.AddForce (transform.up * bunnyJumpForce);
				

				}

				jumpsLeft--;

				jumpSfx.Play ();
			}

			myAnim.SetFloat ("vVelocity", myRigidBody.velocity.y);
			scoreText.text = (Time.time - startTime).ToString ("0.0");
		} else {

			if(Time.time > bunnyHurtTime + 2){
				SceneManager.LoadScene ("main");
			}
		}


	}

	void OnCollisionEnter2D(Collision2D collision){

		if (collision.collider.gameObject.layer == LayerMask.NameToLayer ("Enemy")) {

			foreach (PrefabsSpawner spawner in FindObjectsOfType<PrefabsSpawner>()) {

				spawner.enabled = false;
			}

			foreach (MoveLeft moveLeft in FindObjectsOfType<MoveLeft>()) {

				moveLeft.enabled = false;
			}

			bunnyHurtTime = Time.time;
			myAnim.SetBool ("bunnyHurt", true);

			myRigidBody.velocity = Vector2.zero;
			myRigidBody.AddForce (transform.up * bunnyJumpForce);
			myCollider.enabled = false;

			deathSfx.Play ();

		} else if(collision.collider.gameObject.layer == LayerMask.NameToLayer ("Ground")) {
			jumpsLeft = 2;

		}
	}
}