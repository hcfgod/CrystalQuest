using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System;

namespace UIToolkitHelper
{
	using System.Collections.Generic;
	/// <summary>
	/// A base Menu class for initializing and styling UI Toolkit interfaces.
	/// </summary>
	public class MenuBase : MonoBehaviour
	{
		private Dictionary<string, VisualElement> elementDictionary = new Dictionary<string, VisualElement>();
		
		[Tooltip("The UIDocument that provides the root VisualElement.")]
		public UIDocument _document;

		[Tooltip("The StyleSheet used for styling the interface.")]
		public StyleSheet _styleSheet;

		[SerializeField] private bool showOnStart = false;
		[SerializeField] private bool showOnlyInPlayMode = false;
		
		/// <summary>
		/// The root VisualElement.
		/// </summary>
		protected VisualElement root;

		private void Start()
		{
			if(!showOnStart)
				return;
				
			StartInitializingGUI();
		}
    
		private void OnValidate()
		{
			if(Application.isPlaying || showOnlyInPlayMode)
				return;
            
			StartInitializingGUI();
		}
		
		public void StartInitializingGUI()
		{
			if(!gameObject.activeInHierarchy)
				return;
				
			StartCoroutine(InitializingGUIRoutine());
		}
		
		/// <summary>
		/// Initializes the GUI by setting the root and applying the StyleSheet.
		/// </summary>
		/// 
		private IEnumerator InitializingGUIRoutine()
		{
			yield return null;
			
			if (_document == null || _styleSheet == null)
			{
				Debug.LogWarning("UIDocument and StyleSheet should not be null.");
				yield break;
			}

			root = _document.rootVisualElement;
        
			root.Clear();
        
			root.styleSheets.Add(_styleSheet);
            
			StartCoroutine(GenerateGUI());
		}
        
		/// <summary>
		/// A method to be overridden for generating the GUI.
		/// </summary>
		public virtual IEnumerator GenerateGUI()
		{
			yield return null;
		}
    
		public void StartHidingGUI()
		{
			StartCoroutine(HideGUIRoutine());
		}
		
		public virtual IEnumerator HideGUIRoutine()
		{
			yield return null;
		}
		
		public virtual void FinisHidingGUI()
		{
			root.Clear();
		}
		
		/// <summary>
		/// Creates a new VisualElement with the specified class names.
		/// </summary>
		protected VisualElement Create(string id, params string[] classNames)
		{
			return Create<VisualElement>(id, classNames);
		}
    
		/// <summary>
		/// Creates a new VisualElement of a specified type with the given class names.
		/// </summary>
		protected T Create<T>(string id, params string[] classNames) where T : VisualElement, new()
		{
			var element = new T();
			
			if(classNames.Length > 0)
			{
				foreach (var className in classNames)
				{
					element.AddToClassList(className);
				}
			}

			// Add element to dictionary
			if (!elementDictionary.ContainsKey(id))
			{
				elementDictionary.Add(id, element);
			}
			else
			{
				elementDictionary[id] = element;
			}

			return element;
		}
		
		public T GetElementById<T>(string id) where T : VisualElement
		{
			if (elementDictionary.TryGetValue(id, out var element))
			{
				if (element is T typedElement)
				{
					return typedElement;
				}
				else
				{
					Debug.LogWarning($"Element with ID {id} is not of type {typeof(T)}.");
				}
			}
			else
			{
				Debug.LogWarning($"Element with ID {id} not found.");
			}
			return null;
		}
		
		public void AnimateElement(VisualElement element, Vector3 startPosition, Vector3 targetPosition, int duration, bool shouldFade = false)
		{
			if (Application.isPlaying)
			{
				element.experimental.animation.Position(targetPosition, duration).from = startPosition;
				
				if (shouldFade)
				{
					element.experimental.animation.Start(0, 1, duration, (e, v) => e.style.opacity = new StyleFloat(v));
				}
			}
		}
		
		/// <summary>
		/// Gets a default start position for animating an element into view.
		/// </summary>
		/// <param name="container">The VisualElement container for the element.</param>
		/// <returns>The default start position for the animation.</returns>
		public Vector3 GetDefaultStartPosition(VisualElement container, float startPositionOffset, StartPositionDirection startPositionDirection = StartPositionDirection.UP)
		{
			switch(startPositionDirection)
			{
				case StartPositionDirection.UP:
					return container.worldTransform.GetPosition() + Vector3.up * startPositionOffset;
				case StartPositionDirection.RIGHT:
					return container.worldTransform.GetPosition() + Vector3.right * startPositionOffset;
				default:
					return container.worldTransform.GetPosition() + Vector3.up * startPositionOffset;
			}
		}

		public Vector3 GetDefaultTargetPosition(VisualElement container)
		{
			return container.worldTransform.GetPosition();
		}
	}
}

public enum StartPositionDirection
{
	UP, RIGHT
}
