using UnityEngine;
using System.Collections;

public enum States
{
	isInMainMenu,
	isInGame,
	isPaused,
	gameIsOver
}

public class GameState {
	public static int NUM_STATES = 4;
	bool[] states;

	public GameState()
	{
		states = new bool[NUM_STATES];
	}

	public bool[] GetState()
	{
		return states;
	}

	public void SetInMainMenu(bool state)
	{
		if (state)
		{
			states[(int)States.isInGame] = false;
			states[(int)States.gameIsOver] = false;
		}

		states[(int)States.isInMainMenu] = state;
	}

	public void SetIsPaused(bool state)
	{
		if (state)
			states[(int)States.isInGame] = false;

		states[(int)States.isPaused] = state;
	}

	public void SetInGame(bool state)
	{
		if (state)
		{
			states[(int)States.isInMainMenu] = false;
			states[(int)States.isPaused] = false;
			states[(int)States.gameIsOver] = false;
		}

		states[(int)States.isInGame] = state;
	}

	public void SetGameOver(bool state)
	{
		if (state)
		{ 
			states[(int)States.isPaused] = false;
			states[(int)States.isInGame] = false;
		}

		states[(int)States.gameIsOver] = state;
	}

}
