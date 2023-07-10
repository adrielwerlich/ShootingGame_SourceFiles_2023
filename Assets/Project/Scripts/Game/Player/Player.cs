using DigitalRuby.LightningBolt;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {

    public GameObject model;

    [Header("Movement")]
	[SerializeField] private float movingVelocity;
	[SerializeField] private float jumpingVelocity;
	[SerializeField] private float knockbackForce;
	//public float playerRotatingSpeed;

	[Header("Equipment")]
	public int health = 5;
	public Sword sword;
	public Bow bow;
	public GameObject quiver;
	public int arrows = 15;
	public GameObject bombPrefab;
	public int bombAmount = 5;
	public float throwingSpeed;
	public int orbAmount = 0;

    [SerializeField] private GameObject simpleLightningPrefab;
    [SerializeField] private GameObject lightningPrefab;
    [SerializeField] private int energyLevel = 500;

    public int experience = 0;

	private Rigidbody playerRigidbody;
	private bool canJump;
	private Quaternion targetModelRotation;
	private float knockbackTimer;
	private bool justTeleported;
	private Dungeon currentDungeon;


	public bool JustTeleported {
		get {
			bool returnValue = justTeleported;
			justTeleported = false;
			return returnValue;
		}
	}

	public Dungeon CurrentDungeon {
		get {
			return currentDungeon;
		}
	}

	// Use this for initialization
	void Start () {

		bow.gameObject.SetActive (true);
		quiver.gameObject.SetActive (true);

		sword.gameObject.SetActive (false);
        playerRigidbody = GetComponent<Rigidbody>();


        Enemy.InformPlayer += OnEnemyKilledIncreaseExperience;

        InputManager.FireArrow += FireArrow;
        InputManager.FireBomb += ThrowBomb;
        InputManager.FireLightning += FireLightning;

    }

    private void OnDisable()
    {
        InputManager.FireArrow -= FireArrow;
        InputManager.FireBomb -= ThrowBomb;
        InputManager.FireLightning -= FireLightning;

    }


    // Update is called once per frame
    void Update () {

	}

	

	void ProcessInput () {

		// Check for jumps.
		//if (canJump && Input.GetKeyDown ("space")) {
		//	canJump = false;
		//	playerRigidbody.velocity = new Vector3 (
		//		playerRigidbody.velocity.x,
		//		jumpingVelocity,
		//		playerRigidbody.velocity.z
		//	);
		//}

		// Check equipment interaction.
		//if (Input.GetKeyDown ("z")) {
		//	sword.gameObject.SetActive (true);
		//	bow.gameObject.SetActive (false);
		//	quiver.gameObject.SetActive (false);

		//	sword.Attack ();
		//}

		//if (Input.GetKeyDown ("s")) {
		//	if (arrowAmount > 0) {
		//		sword.gameObject.SetActive (false);
		//		bow.gameObject.SetActive (true);
		//		quiver.gameObject.SetActive (true);

		//		bow.Attack ();
		//		arrowAmount--;
		//	}
		//}

		//if (Input.GetKeyDown ("c")) {
		//	ThrowBomb ();
		//}

		//if (Keyboard.current.)
	}

    private void FireArrow()
    {
        if (arrows > 0)
        {
            //sword.gameObject.SetActive(false);
            //bow.gameObject.SetActive(true);
            bow.Attack();
            arrows--;
        }
    }

	private void FireLightning()
	{
		GameObject lightning = null;

		if (energyLevel > 100)
		{
			lightning = Instantiate(lightningPrefab);
		}
		else if (energyLevel > 0)
		{
			lightning = Instantiate(simpleLightningPrefab);
		}

		if (lightning != null)
        {
            Vector3 intialPosition = getInitialPosition();

			lightning.transform.position = intialPosition;

            var bolt = lightning.gameObject.GetComponent<LightningBoltScript>();
            bolt.StartObject.transform.position = intialPosition + new Vector3(
                -0.150f,
                -1.89f,
                -0.27f
            );
			bolt.EndObject.transform.position = intialPosition + (model.transform.forward * 5);

            StartCoroutine(Wait(lightning));
        }


    }

    IEnumerator Wait(GameObject lightning)
    {
        yield return new WaitForSeconds(.2f);
        Destroy(lightning);

    }

    private Vector3 getInitialPosition()
    {
        return transform.position
            + model.transform.forward
            + (model.transform.up * 2);
    }

    private void ThrowBomb () {
		if (bombAmount <= 0) {
			return;
		}

		GameObject bombObject = Instantiate (bombPrefab);
		bombObject.transform.position = transform.position + model.transform.forward;

		Vector3 throwingDirection = (model.transform.forward + (Vector3.up * .2f)).normalized;

		bombObject.GetComponent<Rigidbody> ().AddForce (throwingDirection * throwingSpeed);

		bombAmount--;
	}

	void OnTriggerEnter (Collider otherCollider) {
		if (otherCollider.GetComponent<EnemyBullet> () != null) {
			Hit ((transform.position - otherCollider.transform.position).normalized);
			Destroy (otherCollider.gameObject);
		} else if (otherCollider.tag == "BombTreasure") {
			bombAmount += 10;
			Destroy (otherCollider.gameObject);
        }
        else if (otherCollider.tag == "ArrowTreasure")
        {
            arrows += 10;
            Destroy(otherCollider.gameObject);
        }
    }

	void OnTriggerStay (Collider otherCollider) {
		if (otherCollider.GetComponent<Dungeon> () != null) {
			currentDungeon = otherCollider.GetComponent<Dungeon> ();
		}
	}

	void OnTriggerExit (Collider otherCollider) {
		if (otherCollider.GetComponent<Dungeon> () != null) {
			Dungeon exitDungeon = otherCollider.GetComponent<Dungeon> ();
			if (exitDungeon == currentDungeon) {
				currentDungeon = null;
			}
		}
	}

	void OnCollisionEnter (Collision collision) {
		if (collision.gameObject.GetComponent<Enemy> ()) {
			Hit ((transform.position - collision.transform.position).normalized);
		}
	}

	private void Hit (Vector3 direction) {
		Vector3 knockbackDirection = (direction + Vector3.up).normalized;
		playerRigidbody.AddForce (knockbackDirection * knockbackForce);
		knockbackTimer = 1f;

		health--;
		if (health <= 0) {
			Destroy (gameObject);
		}
	}

	public void Teleport (Vector3 target) {
		transform.position = target;
		justTeleported = true;
	}

  //  public void EnemyKilled()
  //  {
  //      Debug.Log("another monster killed");

		//experience++;
  //  }

    private void OnEnemyKilledIncreaseExperience(string type, int experienceGain)
    {
        Debug.Log("player event enemy killed. Type is => " + type);
		if (type == "simple")
		{
			experience += 1 + experienceGain;
		} else if (type == "strong")
		{
			experience += 2 + experienceGain;
		} else if (type == "shoting")
		{
			experience += 3 + experienceGain;
		}
    }

	#region WebGL is on mobile check
		//[DllImport("__Internal")]
		//private static extern bool IsMobile();
		//public bool CheckIfMobile()
		//{
		//	#if !UNITY_EDITOR && UNITY_WEBGL
		//	return IsMobile();
		//	#endif

		//	return false;
		//}
    #endregion
}