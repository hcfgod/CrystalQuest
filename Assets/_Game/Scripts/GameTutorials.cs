using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveSystem;

public class GameTutorials : MonoBehaviour
{
	[SerializeField] private bool hasShownStartTutorial = false;

	private void Start()
	{
		hasShownStartTutorial = SaveGame.Load<bool>("GameTutorial");

		HandleStartingTutorial();
	}

	private void HandleStartingTutorial()
	{
		StartCoroutine(ShowStep1());
		
		#region steps
		
		IEnumerator ShowStep1()
		{
			if (hasShownStartTutorial)
			{
				yield break;
			}

			ITutorialElement textElement = new TextElement("Crystal Quest Tutorial", Color.white, 48, new Vector2(0, 500));
			TutorialStep step1 = new TutorialStep(new List<ITutorialElement> { textElement }, 5);
        
			TutorialManager.Instance.EnqueueAndDisplayStep("step1", step1, 2);
		
			yield return new WaitForSeconds(2);

			StartCoroutine(ShowStep2());
		}

		IEnumerator ShowStep2()
		{
			yield return new WaitForSeconds(5);

			ITutorialElement textElement = new TextElement("Press W,A,S,D to move.", Color.white, 24, new Vector2(0, 500));
			TutorialStep step2 = new TutorialStep(new List<ITutorialElement> { textElement }, 7);

			TutorialManager.Instance.EnqueueAndDisplayStep("step2", step2);
		
			yield return new WaitForSeconds(2);

			StartCoroutine(ShowStep3());
		}
	
		IEnumerator ShowStep3()
		{
			yield return new WaitForSeconds(7);

			ITutorialElement textElement = new TextElement("Press Left-shift to sprint", Color.white, 24, new Vector2(0, 500));
			ITutorialElement textElement2 = new TextElement("And Left-Control or C to crouch", Color.white, 24, new Vector2(0, 470));
			TutorialStep step3 = new TutorialStep(new List<ITutorialElement> { textElement, textElement2 }, 7);

			TutorialManager.Instance.EnqueueAndDisplayStep("step3", step3);
		
			hasShownStartTutorial = true;
			SaveGame.Save("GameTutorial", hasShownStartTutorial);
		}
		
		#endregion
	}
}
