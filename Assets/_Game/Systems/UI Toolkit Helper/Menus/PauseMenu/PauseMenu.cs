using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIToolkitHelper;
using UnityEngine.UIElements;
using System;

public class PauseMenu : MenuBase
{
	public static Action OnResumeClicked;
	public static Action OnOptionsClicked;
	public static Action OnMainMenuClicked;
	public static Action OnQuitToDesktopClicked;
	
	[SerializeField] private GameEvent OnGamePausedEvent;
	[SerializeField] private GameEvent OnGameUnPausedEvent;
	
	[SerializeField] private float _animationDuration;
	[SerializeField] private Color pauseMenuBackgroundColor;
	[SerializeField] [TextArea(20, 10)] private string additionalTextString;

	private VisualElement leftPanel;
	
	private Vector3 startPosition;
	private Vector3 targetPosition;
	
	private void Awake()
	{
		MenuManager.Instance.RegisterMenu(nameof(PauseMenu), this);
	}
	
	private void Start()
	{
		GameEventManager.Instance.Subscribe(OnGamePausedEvent, GamePaused);
		GameEventManager.Instance.Subscribe(OnGameUnPausedEvent, GameUnPaused);
	}

	private void OnDisable()
	{
		GameEventManager.Instance.Unsubscribe(OnGamePausedEvent, GamePaused);
		GameEventManager.Instance.Unsubscribe(OnGameUnPausedEvent, GameUnPaused);
	}
	
	public override IEnumerator GenerateGUI()
	{
		yield return null;
		
		leftPanel = Create("leftPanel", "left-panel");
		leftPanel.style.backgroundColor = pauseMenuBackgroundColor;
		root.Add(leftPanel);
		
		startPosition = GetDefaultStartPosition(leftPanel, -310, StartPositionDirection.RIGHT);
		targetPosition = GetDefaultTargetPosition(leftPanel);

		float animationDurationInMilliSeconds = _animationDuration * 1000;
		int animationDuration = (int)MathF.Round(animationDurationInMilliSeconds);

		AnimateElement(leftPanel, startPosition, targetPosition, animationDuration);
		
		// Header section
		var headerSection = Create("headerSection");
		headerSection.AddToClassList("header-section");
		leftPanel.Add(headerSection);

		var pauseMenuTitle = Create<Label>("pauseMenuTitle", "pauseMenuTitle");
		pauseMenuTitle.text = "Pause Menu";
		headerSection.Add(pauseMenuTitle);

		// Controls section
		var controlsSection = Create("controlsSection");
		controlsSection.AddToClassList("controls-section");
		leftPanel.Add(controlsSection);

		var resumeButton = Create<Button>("resumeButton");
		resumeButton.clicked += () =>
		{
			OnResumeClicked?.Invoke();
			CursorManager.Instance.LockCursor();
			GameEventManager.Instance.TriggerEvent(OnGameUnPausedEvent, "Game Unpaused");
		};
		resumeButton.text = "Resume";
		controlsSection.Add(resumeButton);
    
		var optionsButton = Create<Button>("optionsButton");
		optionsButton.clicked += () => { OnOptionsClicked?.Invoke(); };
		optionsButton.text = "Options";
		controlsSection.Add(optionsButton);
    
		var mainMenuButton = Create<Button>("mainMenupButton");
		mainMenuButton.clicked += () => 
		{
			OnMainMenuClicked?.Invoke();		
			GameEventManager.Instance.TriggerEvent(OnGameUnPausedEvent, "Game Unpaused");
		};	
		mainMenuButton.text = "Main Menu";
		controlsSection.Add(mainMenuButton);
		
		var quitToDesktopButton = Create<Button>("quitToDesktopButton");
		quitToDesktopButton.clicked += () => 
		{
			OnQuitToDesktopClicked?.Invoke();
		};	
		quitToDesktopButton.text = "Quit To Desktop";
		controlsSection.Add(quitToDesktopButton);
    
		// Additional Text section
		var additionalSection = Create("additionalSection");
		additionalSection.AddToClassList("additional-section");
		leftPanel.Add(additionalSection);

		var additionalText = Create<Label>("additionalText", "additionalText");
		additionalText.text = additionalTextString;
		additionalSection.Add(additionalText);
	}
	
	public override IEnumerator HideGUIRoutine()
	{
		float animationDurationInMilliSeconds = _animationDuration * 1000;
		int animationDuration = (int)MathF.Round(animationDurationInMilliSeconds);

		AnimateElement(leftPanel, targetPosition, startPosition, animationDuration);
		
		yield return new WaitForSeconds(_animationDuration);
		
		root.Clear();
	}
	
	private void GamePaused(object eventData)
	{
		StartInitializingGUI();
			
		CursorManager.Instance.UnlockCursor();
	}
	
	private void GameUnPaused(object eventData)
	{
		GameManager.Instance.SetIsGamePaused(false);
		Time.timeScale = 1;
		
		StartHidingGUI();
	}
}
