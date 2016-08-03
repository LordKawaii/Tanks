using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	[HideInInspector]
	public static GameController instance = null;

	const string PLAYER_TURN_PREFIX = "Player:";
	public const float floorHeight = -2f;
	public const float playerDistanceFromCenter = 2;

	//Set in Inspector
	public Text playerTurnText;
	public Text powerSliderText;
	public Text winnerText;
	public Slider powerSlider;
	public GameObject mainMenu;
	public GameObject inGameUI;
	public GameObject playerPrefab;
	public GameObject winningUI;
	///////////

	[HideInInspector]
	public int currentPlayer = 0;
	[HideInInspector]
	public int numPlayers = 0;
	[HideInInspector]
	public List<PlayerControler> players;
	[HideInInspector]
	public GameState gameState;

	void Awake ()
	{
		if (this != GameController.instance && GameController.instance != null)
			Destroy(this);

		instance = this;

		gameState = new GameState();
    }

	void Start ()
	{
			if (powerSlider == null)
			Debug.LogError("powerSlider is null!");
	}
	
	void Update ()
	{
		if (gameState.GetState()[(int)States.isInGame])
		{
			UpdatePowerSlider();
			StartCoroutine("CheckForWin");
        }
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

	void UpdatePowerSlider()
	{
		if (Input.GetMouseButtonUp(0))
		{
			EventSystem.current.SetSelectedGameObject(null);
		}

		try
		{
			powerSliderText.text = players[currentPlayer].GetAttackPowerValue().ToString();
		}
		catch
		{ }
    }

	IEnumerator CheckForWin()
	{
		if (numPlayers == 1)
		{
			gameState.SetGameOver(true);

			yield return new WaitForEndOfFrame();

			if (winningUI != null)
				winningUI.SetActive(true);
			else
				Debug.LogError("Winning UI is null");

			Debug.Log("player count: " + players.Count);

			if (winnerText != null)
				winnerText.text = players[0].name + " is the winner!";
			else
				Debug.LogError("Winner text is null");

			yield return new WaitForSeconds(3f);

			SceneManager.LoadScene(0);
        }		
	}

	public void StartGame()
	{
		if (mainMenu != null)
			mainMenu.SetActive(false);
		else
			Debug.LogError("MainMenu is null");

		if (inGameUI != null)
			inGameUI.SetActive(true);
		else
			Debug.LogError("GameUI is null");

		GameObject player1 = Instantiate(playerPrefab, new Vector2(playerDistanceFromCenter, floorHeight), playerPrefab.transform.rotation) as GameObject;
		player1.name = "Player 1";
		AddPlayer(player1);
		GameObject player2 = Instantiate(playerPrefab, new Vector2(-playerDistanceFromCenter, floorHeight), playerPrefab.transform.rotation) as GameObject;
		player2.name = "Player 2";
		AddPlayer(player2);

		if (playerTurnText != null)
			playerTurnText.text = PLAYER_TURN_PREFIX + " " + (currentPlayer + 1);
		else
			Debug.LogError("PlayerTurnText is null!");
		

		gameState.SetInGame(true);

		players[currentPlayer].StartTurn();
	}

	void AddPlayer(GameObject player)
	{
		players.Add(player.GetComponent<PlayerControler>());
		players[numPlayers].playerNumber = numPlayers + 1;
		numPlayers++;
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
