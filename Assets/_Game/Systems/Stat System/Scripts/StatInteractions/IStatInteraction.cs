using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatSystem
{
	public interface IStatInteraction
	{
		void ApplyInteraction(Stat baseStat, Stat targetStat);
	}
}
