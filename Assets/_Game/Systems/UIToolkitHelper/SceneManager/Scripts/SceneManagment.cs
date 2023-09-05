using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagment : MonoBehaviour
{
	private static readonly object lockObject = new object();
	private static SceneManagment _instance;

	private Animator _animator;
	private CanvasGroup _fadeGroup;
	
	public static SceneManagment Instance
	{
		get
		{
			if (_instance == null)
			{
				lock (lockObject)
				{
					if (_instance == null)
					{
						GameObject singletonObject = new GameObject(nameof(SceneManagment));
						_instance = singletonObject.AddComponent<SceneManagment>();
					}
				}
			}
			return _instance;
		}
	}

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		
		_animator = GetComponentInChildren<Animator>();
		_fadeGroup = GetComponentInChildren<CanvasGroup>();
		
		_fadeGroup.alpha = 1f;
	}

	public void LoadScene(string sceneName, float delay = 0f)
	{
		StartCoroutine(DelayedSceneLoad(sceneName, delay));
	}

	IEnumerator DelayedSceneLoad(string sceneName, float delay)
	{
		yield return new WaitForSeconds(delay);
		SceneManager.LoadScene(sceneName);
	}
	
	public void FadeAndLoadScene(string sceneName)
	{
		_animator.SetTrigger("FadeIn");
		StartCoroutine(FadeAndLoad(sceneName));
	}

	IEnumerator FadeAndLoad(string sceneName)
	{
		yield return new WaitForSeconds(2);
		SceneManager.LoadScene(sceneName);
	}
}
