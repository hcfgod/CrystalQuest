using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lightbug.CharacterControllerPro.Implementation;

public class Player : MonoBehaviour
{
	[SerializeField] private GameEvent OnGamePausedEvent;
	[SerializeField] private GameEvent OnGameUnPausedEvent;
	
	[SerializeField] private GameObject CharacterObject;
	
	private Camera3D _playerCamera;
	
	private void Awake()
	{
		_playerCamera = GetComponentInChildren<Camera3D>();
	}
	
	private void Start()
	{
		CursorManager.Instance.LockCursor();
		
		GameEventManager.Instance.Subscribe(OnGamePausedEvent, GamePaused);
		GameEventManager.Instance.Subscribe(OnGameUnPausedEvent, GameUnPaused);
	}
	
	private void OnDisable()
	{
		GameEventManager.Instance.Unsubscribe(OnGamePausedEvent, GamePaused);
		GameEventManager.Instance.Unsubscribe(OnGameUnPausedEvent, GameUnPaused);
	}
	
	private void GamePaused(object eventData)
	{
		_playerCamera.enabled = false;
		CharacterObject.SetActive(false);
	}
	
	private void GameUnPaused(object eventData)
	{
		CharacterObject.SetActive(true);
		_playerCamera.enabled = true;
	}
}
