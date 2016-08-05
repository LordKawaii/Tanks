using UnityEngine;
using System.Collections;

public class PlayerControler : MonoBehaviour {
	const float TIME_TILL_END_TURN = 2f;

	public float speed = 1f;
	public float aimSpeed = 1f;
	public float maxDistance = 2f;
	public int health = 5;

	public Transform reticleCircle;
	public GameObject bullet1Prefab;
	public GameObject bullet2Prefab;
	public float maxAttackPower = 500f;

	[HideInInspector]
	public int playerNumber = 0;
	[HideInInspector]
	public bool isCurrentPlayer;
	[HideInInspector]
	public bool hasFired = false;
	[HideInInspector]
	public GameObject tempBullet;
	[HideInInspector]
	public bool usingWeapon1 = true;

	float attackPower = 0f;
	Vector2 startingLocation;
	bool hasStartedEndTurn = false;
	bool hasSetUp = false;
	bool isFacingRight = true;

	void Awake()
	{
		hasStartedEndTurn = false;
		hasFired = false;
		isCurrentPlayer = false;
		maxAttackPower = 800f;
	}

	void Start()
	{
		
	}

	void Update () {
		if (GameController.instance.gameState.GetState()[(int)States.isInGame])
		{
			if (!hasSetUp)
			{
				SetupPlayer();
                hasSetUp = true;
            }

			if (isCurrentPlayer)
			{
				Movement();
				Aiming();
			}
		}

		if (tempBullet == null && hasFired && !hasStartedEndTurn)
		{
			Debug.Log("Starting End turn");
			StartCoroutine("EndTurn");
		}
	}

	void SetupPlayer()
	{
		int numPlayersToLeft = 0;
		foreach (PlayerControler player in GameController.instance.players)
		{
			if (player.transform.position.x - transform.position.x < 0 && player != this)
				numPlayersToLeft++;
		}

		if (numPlayersToLeft >= GameController.instance.players.Count / 2)
		{
			reticleCircle.transform.Rotate(Vector3.forward * 180);
			isFacingRight = false;
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
			if (isFacingRight)
			{ 
				if (Input.GetAxis("Vertical") > 0)
				{
					reticleCircle.Rotate(Vector3.forward * -aimSpeed * Time.deltaTime);
				}
				if (Input.GetAxis("Vertical") < 0)
				{
					reticleCircle.Rotate(Vector3.forward * aimSpeed * Time.deltaTime);
				}
			}
			if (!isFacingRight)
			{
				if (Input.GetAxis("Vertical") > 0)
				{
					reticleCircle.Rotate(Vector3.forward * aimSpeed * Time.deltaTime);
				}
				if (Input.GetAxis("Vertical") < 0)
				{
					reticleCircle.Rotate(Vector3.forward * -aimSpeed * Time.deltaTime);
				}
			}

			if (reticleCircle.transform.eulerAngles.z >= 80 && !isFacingRight)
				isFacingRight = true;
			if (reticleCircle.transform.eulerAngles.z <= 80 && isFacingRight)
				isFacingRight = false;
		}

		if (Input.GetButtonDown("Fire1") && !hasFired)
		{
			Shootin();
        }
	}

	void Shootin()
	{
		hasFired = true;

		attackPower = GameController.instance.powerSlider.value;
		if (usingWeapon1)
			tempBullet = Instantiate(bullet1Prefab, transform.position, reticleCircle.rotation) as GameObject;
		else
			tempBullet = Instantiate(bullet2Prefab, transform.position, reticleCircle.rotation) as GameObject;
		tempBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right * maxAttackPower * attackPower);
	}

	public void StartTurn()
	{
		hasStartedEndTurn = false;
		hasFired = false;
		isCurrentPlayer = true;
		startingLocation = transform.position;
		GameController.instance.powerSlider.value = attackPower;

		GameController.instance.healthBar.value = health;

		if (usingWeapon1)
			GameController.instance.Weapon1btn();
		else
			GameController.instance.Weapon2btn();
    }

	public IEnumerator EndTurn()
	{
		hasStartedEndTurn = true;
		yield return new WaitForSeconds(2f);
		Debug.Log("Ending Turn");
		isCurrentPlayer = false;
		GameController.instance.ChangeTurns();
	}

	public int GetAttackPowerValue()
	{
		attackPower = GameController.instance.powerSlider.value;
		return Mathf.RoundToInt(attackPower * maxAttackPower);
	}

}
