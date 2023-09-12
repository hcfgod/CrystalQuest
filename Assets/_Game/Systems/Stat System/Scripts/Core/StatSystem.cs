using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace StatSystem
{
	// StatSystem manages a collection of Stat objects.
	public class StatSystem
	{
		// Dictionary to hold StatData and corresponding Stat objects.
		private Dictionary<StatData, Stat> stats = new Dictionary<StatData, Stat>();

		// Adds a new Stat to the system.
		public void AddStat(StatData statData, Stat stat)
		{
			if (stats.ContainsKey(statData))
			{
				throw new InvalidOperationException("A stat with the same StatData already exists.");
			}
			
			stat.CheckConditions();
			
			stats[statData] = stat;
		}

		public Stat GetStat(StatData statData)
		{
			if (!stats.TryGetValue(statData, out Stat stat))
			{
				throw new KeyNotFoundException("Stat not found.");
			}
		
			return stat;
		}
	
		public void IncreaseStatValue(StatData statData, float amount)
		{
			if (amount < 0)
			{
				throw new ArgumentException("Amount must be non-negative.");
			}
		
			Stat stat = GetStat(statData);
		
			if (stat != null)
			{
				stat.IncreaseStatValue(amount);
			}
		}

		public void DecreaseStatValue(StatData statData, float amount)
		{
			if (amount < 0)
			{
				throw new ArgumentException("Amount must be non-negative.");
			}
		
			Stat stat = GetStat(statData);
		
			if (stat != null)
			{
				stat.DecreaseStatValue(amount);
			}
		}

		public IEnumerator RegenerateOverTime(MonoBehaviour host, Stat stat, float delay, float regenAmount)
		{
			if (stat == null) yield break;

			while (stat.Value < stat.MaxValue)
			{
				stat.IncreaseStatValue(regenAmount);
				yield return new WaitForSeconds(delay);
			}
		}

		public IEnumerator DepleteOverTime(MonoBehaviour host, Stat stat, float delay, float decrementAmount)
		{
			if (stat == null) yield break;

			while (stat.Value > 0)
			{
				stat.DecreaseStatValue(decrementAmount);
				yield return new WaitForSeconds(delay);
			}
		}
	
		public  Dictionary<StatData, Stat> GetStatDictionary()
		{
			return stats;
		}
	}
}
