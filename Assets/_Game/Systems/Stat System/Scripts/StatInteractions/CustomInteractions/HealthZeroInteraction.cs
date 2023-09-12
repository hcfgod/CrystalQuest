using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatSystem;

public class HealthZeroInteraction : IStatInteraction
{
	public void ApplyInteraction(Stat baseStat, Stat targetStat)
	{
		if (baseStat.Name == "Health" && baseStat.Value <= 0)
		{
			Debug.Log("Player is Dead");
		}
	}
}
