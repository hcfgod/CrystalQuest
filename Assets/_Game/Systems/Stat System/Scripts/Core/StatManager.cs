using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace StatSystem
{
	// StatManager is responsible for managing multiple Stat objects.
	public class StatManager : MonoBehaviour, IDisposable
	{
		// List of StatData objects.
		public List<StatData> statDataList; // Assign in Unity Editor

		private StatSystem statSystem;

		private StatInteractionManager _statInteractionManager = new StatInteractionManager();
		
		void Awake()
		{
			statSystem = new StatSystem();
			InitializeStats();
		}

		private void InitializeStats()
		{
			foreach (var data in statDataList)
			{
				AddStat(data);
			}
		}

		// Centralized way of adding a new Stat to the system.
		public void AddStat(StatData statData)
		{
			if (statData == null)
			{
				throw new ArgumentNullException("StatData cannot be null.");
			}

			Stat newStat = new Stat(statData);
			statSystem.AddStat(statData, newStat);
		}

		// Retrieves a Stat based on StatData.
		public Stat GetStat(StatData statData)
		{
			if (statData == null)
			{
				throw new ArgumentNullException("StatData cannot be null.");
			}

			return statSystem.GetStat(statData);
		}

		public void IncreaseStatValue(StatData statData, float amount)
		{
			if (amount < 0)
			{
				throw new ArgumentException("Amount must be non-negative.");
			}
		
			statSystem.IncreaseStatValue(statData, amount);
		}

		public void DecreaseStatValue(StatData statData, float amount)
		{
			if (amount < 0)
			{
				throw new ArgumentException("Amount must be non-negative.");
			}
		
			statSystem.DecreaseStatValue(statData, amount);
		}
	
		public void RegenerateStatValueOverTime(StatData statData, float delay, float regenAmount)
		{
			Stat stat = GetStat(statData);
		
			if (stat == null)
			{
				throw new InvalidOperationException("Stat not found.");
			}
			if (stat.CurrentCoroutine != null)
			{
				throw new InvalidOperationException("A coroutine is already running for this stat.");
			}

			stat.CurrentCoroutine = StartCoroutine(statSystem.RegenerateOverTime(this, stat, delay, regenAmount));
		}

		public void DepleteStatValueOverTime(StatData statData, float delay, float decrementAmount)
		{
			Stat stat = GetStat(statData);
		
			if (stat == null)
			{
				throw new InvalidOperationException("Stat not found.");
			}
			if (stat.CurrentCoroutine != null)
			{
				throw new InvalidOperationException("A coroutine is already running for this stat.");
			}
    
			stat.CurrentCoroutine = StartCoroutine(statSystem.DepleteOverTime(this, stat, delay, decrementAmount));
		}
	
		public void AddStatCondition(Stat stat, StatCondition condition)
		{
			stat.GetStatCondidtions().Add(condition);
		}

		public void CheckStatConditions(Stat stat)
		{
			stat.CheckConditions();
		}
	
		public void AddPermantantStatModifier(Stat stat, StatModifier modifier)
		{
			stat.AddModifier(modifier);
		}
	
		public void RemovePermantantStatModifier(Stat stat, StatModifier modifier)
		{
			stat.RemoveModifier(modifier);
		}
	
		public void AddTemporaryStatModifier(Stat stat, StatModifier modifier)
		{
			stat.AddModifier(modifier);
			StartCoroutine(StatModifierDurationRoutine(stat, modifier));
		}
	
		public IEnumerator StatModifierDurationRoutine(Stat stat, StatModifier modifier)
		{
			yield return new WaitForSeconds(modifier.Duration);
			stat.RemoveModifier(modifier);
		}
	
		public void AddStatDependency(Stat baseStat, Stat dependentStat)
		{
			baseStat.AddDependentStat(dependentStat);
		}
	
		public void RemoveStatDependency(Stat baseStat, Stat dependentStat)
		{
			baseStat.RemoveDependentStat(dependentStat);
		}
	
		public void StopStatCoroutine(StatData statData)
		{
			Stat stat = GetStat(statData);
			if (stat == null) return;
		
			if(stat.CurrentCoroutine == null) return;
			
			StopCoroutine(stat.CurrentCoroutine);
			stat.CurrentCoroutine = null;
		}
		
		public StatInteractionManager GetStatInteractionManager()
		{
			return _statInteractionManager;
		}
		
		public void Dispose(){}
	}
}
