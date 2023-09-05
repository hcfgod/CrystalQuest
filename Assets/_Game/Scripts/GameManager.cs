using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager instance;
	public static GameManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<GameManager>();
				if (instance == null)
				{
					GameObject go = new GameObject("GameManager");
					instance = go.AddComponent<GameManager>();
				}
			}
			return instance;
		}
	}

	[SerializeField] private GameEvent OnGameStartedEvent;
	[SerializeField] private GameEvent OnGamePausedEvent;
	[SerializeField] private GameEvent OnGameUnPausedEvent;
	[SerializeField] private GameEvent OnGameStoppedEvent;
	
	private bool isGamePaused = false;
	
	private void Awake()
	{
		if (Instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
	}
	
	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(!isGamePaused)
			{
				PauseGame();
			}
			else
			{
				UnPauseGame();
			}
		}
	}
	
	public void StartGame()
	{
		GameEventManager.Instance.TriggerEvent(OnGameStartedEvent, "Game Started.");
	}
	
	public void PauseGame()
	{
		isGamePaused = true;
		Time.timeScale = 0;
		GameEventManager.Instance.TriggerEvent(OnGamePausedEvent, "Game Paused.");
	}
	
	public void UnPauseGame()
	{
		isGamePaused = false;
		Time.timeScale = 1;
		CursorManager.Instance.LockCursor();
		GameEventManager.Instance.TriggerEvent(OnGameUnPausedEvent, "Game UnPaused.");
	}
	
	public void StopGame()
	{
		GameEventManager.Instance.TriggerEvent(OnGameStoppedEvent, "Game Stopped.");
	}
	
	public void SetIsGamePaused(bool value)
	{
		isGamePaused = value;
	}
}
