using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatSystem
{
	public class StatCondition
	{
		public string StatName { get; private set; }
		public float Threshold { get; private set; }
		public StatConditionType Type { get; private set; }

		public StatCondition(string statName, float threshold, StatConditionType type)
		{
			StatName = statName;
			Threshold = threshold;
			Type = type;
		}

		public bool CheckCondition(Stat stat)
		{
			switch (Type)
			{
			case StatConditionType.LessThan:
				return stat.Value < Threshold;
			case StatConditionType.GreaterThan:
				return stat.Value > Threshold;
			case StatConditionType.EqualTo:
				return stat.Value == Threshold;
			default:
				return false;
			}
		}
	}
}