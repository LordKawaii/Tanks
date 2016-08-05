using UnityEngine;
using System.Collections;

public class BulletControler : MonoBehaviour {
	public int damage = 2;

	[HideInInspector]
	public PlayerControler owner;

	protected void OnTriggerEnter2D(Collider2D other)
	{

		if (owner == null && other.tag == "Player")
		{
			owner = other.GetComponent<PlayerControler>();
		}


		if (other.tag != "Player")
		{
			Destroy(gameObject);
		}

		if (other.tag == "Player" && other.GetComponent<PlayerControler>() != owner && owner != null)
		{
			bulletEffect(other.gameObject);
        }
	}

	protected virtual void bulletEffect(GameObject other)
	{
		PlayerControler player = other.GetComponent<PlayerControler>();

		if (player.health - damage <= 0)
		{
			GameController.instance.numPlayers--;
			GameController.instance.players.Remove(player);
			Destroy(other.gameObject);
			GameController.instance.players.TrimExcess();
			GameController.instance.players.Sort();
		}
		else
		{
			player.health -= damage;
		}

		Destroy(gameObject);
	}
}
