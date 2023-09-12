using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatSystem
{
	public class DefaultFormula : IStatFormula
	{		
		public float Calculate(List<Stat> baseStats, List<StatCondition> statConditions = null)
		{
			float totalValue = 0;
			
			foreach (var stat in baseStats)
			{
				totalValue += stat.Value;
			}
			
			return totalValue;
		}
	}
}