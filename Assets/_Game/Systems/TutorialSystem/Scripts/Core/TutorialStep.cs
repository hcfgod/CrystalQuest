using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStep
{
	private List<ITutorialElement> elements;
	private float duration;

	public TutorialStep(List<ITutorialElement> elements, float duration)
	{
		this.elements = elements;
		this.duration = duration;
	}

	public void Display(GameObject parent)
	{
		foreach (var element in elements)
		{
			element.Display(parent);
		}
	}

	public void Hide(GameObject parent)
	{
		if (parent != null)
		{
			foreach (Transform child in parent.transform)
			{
				child.gameObject.SetActive(false);
			}
		}
	}
	
	public float GetDuration()
	{
		return duration;
	}
}
