using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIToolkitHelper
{
	using UnityEngine.UIElements;
	public class MenuManager : MonoBehaviour
	{
		private static readonly object lockObject = new object();
		private static MenuManager _instance;

		public static MenuManager Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (lockObject)
					{
						if (_instance == null)
						{
							GameObject singletonObject = new GameObject(nameof(MenuManager));
							_instance = singletonObject.AddComponent<MenuManager>();
							DontDestroyOnLoad(singletonObject);
						}
					}
				}
				return _instance;
			}
		}

		private Dictionary<string, MenuBase> menuDictionary = new Dictionary<string, MenuBase>();

		private void Awake()
		{
			if (_instance == null)
			{
				_instance = this;
				DontDestroyOnLoad(gameObject);
			}
			else if (_instance != this)
			{
				Destroy(gameObject);
			}
		}

		public void RegisterMenu(string menuID, MenuBase menu)
		{
			if (!menuDictionary.ContainsKey(menuID))
			{
				menuDictionary.Add(menuID, menu);
			}
			else
			{
				menuDictionary[menuID] = menu;
			}
		}

		public void UnregisterMenu(string menuID)
		{
			if (menuDictionary.ContainsKey(menuID))
			{
				menuDictionary.Remove(menuID);
			}
			else
			{
				Debug.LogWarning($"Menu with ID {menuID} does not exist.");
			}
		}

		public MenuBase GetMenuById(string menuID)
		{
			if (menuDictionary.TryGetValue(menuID, out var menu))
			{
				return menu;
			}
			Debug.LogWarning($"Menu with ID {menuID} not found.");
			return null;
		}
        
		public T GetElementById<T>(string menuID, string elementID) where T : VisualElement
		{
			MenuBase menu = GetMenuById(menuID);
			
			if(menu != null)
			{
				return menu.GetElementById<T>(elementID);
			}
			return null;
		}
	}
}
