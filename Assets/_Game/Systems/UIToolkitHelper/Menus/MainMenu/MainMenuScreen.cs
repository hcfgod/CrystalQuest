using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System;
using UIToolkitHelper;
using UnityEngine.SceneManagement;

public class MainMenuScreen : MenuBase
{
	public static Action PlayButtonPressed;
	public static Action SettingsButtonPressed;
	public static Action ExitButtonPressed;
	
	[SerializeField] private string gameTitle;
	[SerializeField] private Texture2D gameBackgroundImage;
	[SerializeField] private Color controlsPanelbackgroundColor;
	[SerializeField] private Color titleTextColor;
	
	protected void Awake()
	{
		MenuManager.Instance.RegisterMenu(nameof(MainMenuScreen), this);
	}
	
	public void SceneTransitionFinished()
	{
		StartInitializingGUI();
	}
	
	public override IEnumerator GenerateGUI()
	{
		yield return null;
    
		// Background Image
		var backgroundImage = Create<Image>("backgroundImage", "background-image");
		backgroundImage.style.backgroundImage = gameBackgroundImage;
		root.Add(backgroundImage);
    
		// Left Panel
		var leftPanel = Create("leftPanel", "left-panel");
		leftPanel.style.backgroundColor = controlsPanelbackgroundColor;
		root.Add(leftPanel);

		// Main container
		var container = Create("mainContainer", "main-container");
		root.Add(container);
    
		// Title
		var title = Create<Label>("gameTitle", "game-title");
		title.text = gameTitle;
		title.style.color = titleTextColor;
		container.Add(title);
		
		// Play Button
		var playButton = Create<Button>("playBtn", "play-button");
		playButton.text = "Play";
		leftPanel.Add(playButton);  // Add to leftPanel
		playButton.clicked += () =>
		{
			PlayButtonPressed?.Invoke();
		};
    
		// Settings Button
		var settingsButton = Create<Button>("settingsBtn", "settings-button");
		settingsButton.text = "Settings";
		leftPanel.Add(settingsButton);  // Add to leftPanel
		settingsButton.clicked += () => 
		{
			SettingsButtonPressed?.Invoke();
		};

		// Exit Button
		var exitButton = Create<Button>("exitBtn", "exit-button");
		exitButton.text = "Exit";
		leftPanel.Add(exitButton);  // Add to leftPanel
		exitButton.clicked += () => 
		{
			ExitButtonPressed?.Invoke();
		};
		
		var startPos = GetDefaultStartPosition(root, -1080, StartPositionDirection.UP);
		var targetPos = GetDefaultTargetPosition(root);
		
		AnimateElement(root, startPos, targetPos, 2000);
	}
}
