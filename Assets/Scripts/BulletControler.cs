using UnityEngine;
using System.Collections;

public class BulletControler : MonoBehaviour {
	[HideInInspector]
	public PlayerControler owner;

	void OnTriggerEnter2D(Collider2D other)
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
			GameController.instance.numPlayers--;
			GameController.instance.players.Remove(other.GetComponent<PlayerControler>());
			Destroy(other.gameObject);
			GameController.instance.players.TrimExcess();
			GameController.instance.players.Sort();
			Destroy(gameObject);
		}

		if (owner != null)
			Debug.Log("Owner name: " + owner.name + " Others name: " + other.name);
	}
	
}
