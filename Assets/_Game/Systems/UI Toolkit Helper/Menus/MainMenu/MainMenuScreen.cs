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
	
	[SerializeField] private bool hasBackgroundImage;
	
	[SerializeField] private Texture2D gameBackgroundImage;
	[SerializeField] private Color controlsPanelbackgroundColor;
	[SerializeField] private Color titleTextColor;
	[SerializeField] private Color buttonBackgroundColor;
	[SerializeField] private Color buttonTextColor;
	
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
		if(hasBackgroundImage)
		{
			var backgroundImage = Create<Image>("backgroundImage", "background-image");
			backgroundImage.style.backgroundImage = gameBackgroundImage;
			root.Add(backgroundImage);
		}
    
		// Left Panel
		var leftPanel = Create("leftPanel", "left-panel");
		leftPanel.style.backgroundColor = controlsPanelbackgroundColor;
		root.Add(leftPanel);

		// Main container
		var container = Create("mainContainer", "main-container");
		root.Add(container);
		
		// Title Background 
		var titleBackground = Create("titleBackground", "title-background");
		titleBackground.style.backgroundColor = buttonBackgroundColor;
		container.Add(titleBackground);

		// Title
		var title = Create<Label>("gameTitle", "game-title");
		title.text = gameTitle;
		title.style.color = titleTextColor;
		titleBackground.Add(title);

		// Play Button
		var playButton = Create<Button>("playBtn", "play-button");
		playButton.text = "Play";
		playButton.style.backgroundColor = buttonBackgroundColor;
		playButton.style.color = buttonTextColor;
		leftPanel.Add(playButton);  // Add to leftPanel
		playButton.clicked += () =>
		{
			PlayButtonPressed?.Invoke();
		};
    
		// Settings Button
		var settingsButton = Create<Button>("settingsBtn", "settings-button");
		settingsButton.text = "Settings";
		settingsButton.style.backgroundColor = buttonBackgroundColor;
		settingsButton.style.color = buttonTextColor;
		leftPanel.Add(settingsButton);  // Add to leftPanel
		settingsButton.clicked += () => 
		{
			SettingsButtonPressed?.Invoke();
		};

		// Exit Button
		var exitButton = Create<Button>("exitBtn", "exit-button");
		exitButton.text = "Exit";
		exitButton.style.backgroundColor = buttonBackgroundColor;
		exitButton.style.color = buttonTextColor;
		leftPanel.Add(exitButton);  // Add to leftPanel
		exitButton.clicked += () => 
		{
			ExitButtonPressed?.Invoke();
		};
		
		var startPos = GetDefaultStartPosition(root, -1080, StartPositionDirection.RIGHT);
		var targetPos = GetDefaultTargetPosition(root);
		
		AnimateElement(root, startPos, targetPos, 2000);
	}
}
