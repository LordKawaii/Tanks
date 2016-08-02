using UnityEngine;
using System.Collections;

public class PlayerControler : MonoBehaviour {
	public float speed = 1f;
	public float aimSpeed = 1f;
	public float maxDistance = 2f;

	public Transform reticleCircle;
	public GameObject Bullet;
	public float maxAttackPower = 500f;

	[HideInInspector]
	public int playerNumber = 0;
	[HideInInspector]
	public bool isCurrentPlayer;
	[HideInInspector]
	public bool hasFired = false;

	float attackPower = 0f;
	Vector2 startingLocation;

	void Awake()
	{
		hasFired = false;
		isCurrentPlayer = false;
		maxAttackPower = 800f;
	}

	void Start()
	{
		
	}

	void Update () {
		if (isCurrentPlayer)
		{
			Movement();
			Aiming();
		}
	}

	void Movement ()
	{
		if (Input.GetButton("Horizontal"))
		{
			if (Input.GetAxis("Horizontal") > 0 && transform.position.x - startingLocation.x < maxDistance)
				transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
			if (Input.GetAxis("Horizontal") < 0 && startingLocation.x - transform.position.x < maxDistance)
				transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);

		}
	}

	void Aiming()
	{
		if (Input.GetButton("Vertical"))
		{
			if (Input.GetAxis("Vertical") > 0)
				reticleCircle.Rotate(Vector3.forward * aimSpeed * Time.deltaTime);
			if (Input.GetAxis("Vertical") < 0)
				reticleCircle.Rotate(Vector3.forward * -aimSpeed * Time.deltaTime);
			Debug.Log("Is current player: " + isCurrentPlayer);
		}

		if (Input.GetButtonDown("Fire1") && !hasFired)
		{
			StartCoroutine("Shootin");
        }
	}

	IEnumerator Shootin()
	{
		hasFired = true;

		attackPower = GameController.instance.powerSlider.value;
		GameObject tempBullet = Instantiate(Bullet, transform.position, reticleCircle.rotation) as GameObject;
		tempBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right * maxAttackPower * attackPower);
		yield return new WaitForEndOfFrame();
		EndTurn();
	}

	public void StartTurn()
	{
		hasFired = false;
		isCurrentPlayer = true;
		startingLocation = transform.position;
		GameController.instance.powerSlider.value = attackPower;
    }

	void EndTurn()
	{
		isCurrentPlayer = false;
		GameController.instance.ChangeTurns();
	}

	public int GetAttackPowerValue()
	{
		attackPower = GameController.instance.powerSlider.value;
		return Mathf.RoundToInt(attackPower * maxAttackPower);
	}
}
