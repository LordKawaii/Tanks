using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	[HideInInspector]
	public static GameController instance = null;

	const string PLAYER_TURN_PREFIX = "Player:";

	public Text playerTurnText;
	public Text powerSliderText;
	public Slider powerSlider;

	[HideInInspector]
	public int currentPlayer = 0;
	[HideInInspector]
	public int numPlayers = 0;
	[HideInInspector]
	public List<PlayerControler> players;

	void Awake ()
	{
		if (this != GameController.instance && GameController.instance != null)
			Destroy(this);

		instance = this;
	}

	void Start () {
		//For testing
		foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
		{
			players.Add(player.GetComponent<PlayerControler>());
			players[numPlayers].playerNumber = numPlayers + 1;
			numPlayers++;
		}
		/////////////

		if (powerSlider == null)
			Debug.LogError("powerSlider is null!");

		if (playerTurnText != null)
			playerTurnText.text = PLAYER_TURN_PREFIX + " " + (currentPlayer + 1);
		else
			Debug.LogError("PlayerTurnText is null!");
		players[currentPlayer].StartTurn();
	}
	
	void Update () {
		if (Input.GetMouseButtonUp(0))
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
		powerSliderText.text = players[currentPlayer].GetAttackPowerValue().ToString();
    }

	public PlayerControler getCurrentPlayer()
	{
		return players[currentPlayer];
	}

	public void ChangeTurns()
	{
		if (currentPlayer + 1 < numPlayers)
		{
			currentPlayer++;
		}
		else
			currentPlayer = 0;

		playerTurnText.text = PLAYER_TURN_PREFIX + " " + (currentPlayer + 1);
		players[currentPlayer].StartTurn();
	}
}
