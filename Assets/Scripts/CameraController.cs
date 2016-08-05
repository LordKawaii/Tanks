using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	Camera mainCamera;

	void Awake ()
	{
		mainCamera = Camera.main;
	}	
	
	void Update ()
	{
		if (GameController.instance.gameState.GetState()[(int)States.isInGame])
		{ 
			if (!GameController.instance.players[GameController.instance.currentPlayer].hasFired)
				mainCamera.transform.position = new Vector3 (GameController.instance.players[GameController.instance.currentPlayer].gameObject.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
			else if (GameController.instance.players[GameController.instance.currentPlayer].tempBullet != null)
				mainCamera.transform.position = new Vector3(GameController.instance.players[GameController.instance.currentPlayer].tempBullet.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
		}
	}
}
