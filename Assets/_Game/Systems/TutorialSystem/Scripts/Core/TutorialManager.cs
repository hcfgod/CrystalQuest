using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
	private static TutorialManager _instance;
	public static TutorialManager Instance => _instance;

	[SerializeField] private Canvas tutorialUICanvas;
	
	[SerializeField] private float fadeInDuration;
	[SerializeField] private float fadeOutDuration;
	
	private Dictionary<string, TutorialStep> tutorialSteps = new Dictionary<string, TutorialStep>();
	
	private CanvasGroup canvasGroup;

	public Sprite someSprite;
	
	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			_instance = this;
		}
		
		if(!SearchForCanvasGroup())
		{
			Debug.LogWarning("No Canvas Group Found.");
		}
	}
	
	private bool SearchForCanvasGroup()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		
		if(canvasGroup == null)
		{
			canvasGroup = GetComponentInChildren<CanvasGroup>();
			
			if(canvasGroup == null)
			{
				canvasGroup = GetComponentInParent<CanvasGroup>();
				
				if(canvasGroup == null)
					return false;
			}
		}
		
		return true;
	}
	
	public void EnqueueStep(string id, TutorialStep step)
	{
		tutorialSteps[id] = step;
	}

	public void DisplayStep(string id, float delay = 0)
	{
		if (tutorialSteps.ContainsKey(id))
		{
			TutorialStep step = tutorialSteps[id];
			step.Display(canvasGroup.gameObject);
			
			// Start coroutines to handle the fade-in and fade-out
			StartCoroutine(DelayBeforeFadeIn(delay));
			StartCoroutine(RemoveTutorialAfterDuration(step.GetDuration(), canvasGroup.gameObject));
		}
	}
	
	public void EnqueueAndDisplayStep(string id, TutorialStep step, float delay = 0)
	{
		tutorialSteps[id] = step;
		
		if (tutorialSteps.ContainsKey(id))
		{
			step.Display(canvasGroup.gameObject);
			
			// Start coroutines to handle the fade-in and fade-out
			StartCoroutine(DelayBeforeFadeIn(delay));
			StartCoroutine(RemoveTutorialAfterDuration(step.GetDuration(), canvasGroup.gameObject));
		}
	}
	
	public void HideStep(string id)
	{
		if (tutorialSteps.ContainsKey(id))
		{
			TutorialStep step = tutorialSteps[id];
			step.Hide(canvasGroup.gameObject);
		}
	}

	private IEnumerator DelayBeforeFadeIn(float delay)
	{
		yield return new WaitForSeconds(delay);
		StartCoroutine(FadeIn());
	}
	
	private IEnumerator FadeIn()
	{
		float currentTime = 0.0f;

		while (currentTime < fadeInDuration)
		{
			float alpha = Mathf.Lerp(0, 1, currentTime / fadeInDuration);
			canvasGroup.alpha = alpha;
			currentTime += Time.deltaTime;
			yield return null;
		}

		canvasGroup.alpha = 1;
	}

	private IEnumerator FadeOut()
	{
		float currentTime = 0.0f;

		while (currentTime < fadeOutDuration)
		{
			float alpha = Mathf.Lerp(1, 0, currentTime / fadeOutDuration);
			canvasGroup.alpha = alpha;
			currentTime += Time.deltaTime;
			yield return null;
		}

		canvasGroup.alpha = 0;
	}

	private IEnumerator RemoveTutorialAfterDuration(float duration, GameObject parent)
	{
		yield return new WaitForSeconds(duration);

		// Start the fade-out coroutine
		StartCoroutine(FadeOut());

		// Wait for fade-out to complete before removing elements
		yield return new WaitForSeconds(1.0f);

		// Remove all child elements from the parent GameObject
		foreach (Transform child in parent.transform)
		{
			Destroy(child.gameObject);
		}
	}
}
