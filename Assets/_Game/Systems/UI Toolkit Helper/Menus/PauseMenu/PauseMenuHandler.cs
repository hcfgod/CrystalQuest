using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuHandler : MonoBehaviour
{
	private void OnEnable()
	{
		PauseMenu.OnResumeClicked += ResumeGame;
		PauseMenu.OnOptionsClicked += OpenOptions;
		PauseMenu.OnMainMenuClicked += GoToMainMenu;
		PauseMenu.OnQuitToDesktopClicked += QuitToDesktop;
	}
	
	private void OnDisable()
	{
		PauseMenu.OnResumeClicked -= ResumeGame;
		PauseMenu.OnOptionsClicked -= OpenOptions;
		PauseMenu.OnMainMenuClicked -= GoToMainMenu;
		PauseMenu.OnQuitToDesktopClicked -= QuitToDesktop;
	}

	private void ResumeGame()
	{
		// Code to resume the game
	}

	private void OpenOptions()
	{
		// Code to open the options menu
	}

	private void GoToMainMenu()
	{
		SceneManager.LoadScene("MainMenuScene");
	}
	
	private void QuitToDesktop()
	{
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
        #endif
        
		Application.Quit();
	}
}
