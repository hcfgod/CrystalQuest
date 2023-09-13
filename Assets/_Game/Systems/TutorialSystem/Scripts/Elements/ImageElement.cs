using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageElement : ITutorialElement
{
	private Sprite image;
	private Vector2 size;
	private Vector2 position;

	public ImageElement(Sprite image, Vector2 size, Vector2 position)
	{
		this.image = image;
		this.size = size;
		this.position = position;
	}

	public void Display(GameObject parent)
	{
		GameObject imageObject = new GameObject("TutorialImage");
		imageObject.transform.SetParent(parent.transform, false);
        
		Image imageComponent = imageObject.AddComponent<Image>();
		imageComponent.sprite = image;
        
		RectTransform rectTransform = imageObject.GetComponent<RectTransform>();
		rectTransform.sizeDelta = size;
		rectTransform.anchoredPosition = position;
	}
}
