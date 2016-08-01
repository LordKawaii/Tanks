using UnityEngine;
using System.Collections;

public class PlayerControler : MonoBehaviour {
	public float speed = 1f;
	public float aimSpeed = 1f;
	public float maxDistance = 2f;

	[HideInInspector]
	public float AttackPower = 500f;

	public Transform reticleCircle;
	public GameObject Bullet;

	Vector2 startingLocation;


	void Start()
	{
		StartTurn();
        AttackPower = 500f;
}

	void Update () {
		Movement();
		Aiming();
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

		}

		if (Input.GetButtonDown("Fire1"))
		{
			Shootin();
        }
	}

	void Shootin()
	{
		GameObject tempBullet = Instantiate(Bullet, transform.position, reticleCircle.rotation) as GameObject;
		tempBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right * AttackPower);
		Debug.Log("Bullet force: " +  AttackPower);

	}

	void StartTurn()
	{
		startingLocation = transform.position;
    }
}
