using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatSystem
{
	public class StatModifier
	{
		public float Value;
		public StatModifierType Type;
		public float Duration;  // set to -1 for permanent modifiers

		public StatModifier(float value, StatModifierType type, float duration = -1)
		{
			Value = value;
			Type = type;
			Duration = duration;
		}
	}
}