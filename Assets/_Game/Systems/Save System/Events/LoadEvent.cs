using System;
using UnityEngine.Events;

namespace SaveSystem.Events
{

	/// <summary>
	/// Load event.
	/// </summary>
	[Serializable]
	public class LoadEvent : UnityEvent<string, object, SaveGameSettings>
	{
		
	}

}