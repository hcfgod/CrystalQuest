using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lightbug.CharacterControllerPro.Implementation;

public class Player : MonoBehaviour
{
	[SerializeField] private GameEvent OnGamePausedEvent;
	[SerializeField] private GameEvent OnGameUnPausedEvent;
	
	[SerializeField] private PlayerData _playerData;
	
	[SerializeField] private GameObject CharacterObject;
	
	private NormalMovement _playerMovment;
	private Camera3D _playerCamera;
	
	private void Awake()
	{
		_playerMovment = GetComponentInChildren<NormalMovement>();
		_playerCamera = GetComponentInChildren<Camera3D>();
	}
	
	private void Start()
	{
		CursorManager.Instance.LockCursor();
		
		GameEventManager.Instance.Subscribe(OnGamePausedEvent, GamePaused);
		GameEventManager.Instance.Subscribe(OnGameUnPausedEvent, GameUnPaused);
	}
	
	private void Update()
	{
		UpdatePlayerData();
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
	
	private void UpdatePlayerData()
	{
		_playerData.isIdle = _playerMovment.IsIdle();
		_playerData.isWalking = _playerMovment.IsWalking();
		_playerData.isRunning = _playerMovment.IsRunning();
		_playerData.isCrouching = _playerMovment.IsCrouched();
	}
}
