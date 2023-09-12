using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatSystem
{
	public interface IStatFormula
	{
		float Calculate(List<Stat> baseStats, List<StatCondition> statConditions = null);
	}
}
