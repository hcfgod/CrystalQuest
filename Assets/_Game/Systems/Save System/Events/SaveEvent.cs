using System;
using UnityEngine.Events;

namespace SaveSystem.Events
{

	/// <summary>
	/// Save event.
	/// </summary>
	[Serializable]
	public class SaveEvent : UnityEvent<string, object, SaveGameSettings>
	{

	}

}