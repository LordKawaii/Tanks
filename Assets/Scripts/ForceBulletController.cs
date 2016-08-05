using UnityEngine;
using System.Collections;

public class ForceBulletController : BulletControler
{
	const int FORCE_MULTIPLYER = 50;
	Rigidbody2D rb2d;

	void Awake()
	{
		rb2d = gameObject.GetComponent<Rigidbody2D>();
	}

	protected override void bulletEffect(GameObject other)
	{

		PlayerControler player = other.GetComponent<PlayerControler>();
		Rigidbody2D playerRB2d = other.GetComponent<Rigidbody2D>();

		if (rb2d.velocity.y < 0)
		{
			playerRB2d.AddForce(new Vector2(rb2d.velocity.x, rb2d.velocity.y * -1) * FORCE_MULTIPLYER);
        }
		else
			playerRB2d.AddForce(new Vector2(rb2d.velocity.x, rb2d.velocity.y) * FORCE_MULTIPLYER);


		Destroy(gameObject);
	}

}
