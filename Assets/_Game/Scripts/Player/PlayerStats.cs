using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatSystem;

public class PlayerStats : MonoBehaviour
{
	[SerializeField] private StatData healthStatData;

	private StatManager _statManager;
	
	private Stat _healthStat;
	
	private void Awake()
	{
		_statManager = GetComponent<StatManager>();
	}
	
	private void Start()
	{
		_healthStat = _statManager.GetStat(healthStatData);
		
		StatCondition zeroHealthCondition = new StatCondition("Health", 0, StatConditionType.EqualTo);
		_healthStat.AddCondition(zeroHealthCondition);
		_healthStat.OnConditionMet += ZeroHealthConditionMet;
		
		_statManager.GetStatInteractionManager().RegisterInteraction("HealthZero", new HealthZeroInteraction());
		
		_statManager.DepleteStatValueOverTime(healthStatData, 1, 10);
	}
	
	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.K))
		{
			_statManager.StopStatCoroutine(healthStatData);
		}
	}
	
	private void ZeroHealthConditionMet(StatCondition condition)
	{
		if (condition.StatName == "Health" && condition.Type == StatConditionType.EqualTo)
		{
			_statManager.GetStatInteractionManager().TriggerInteraction("HealthZero", _healthStat, null);
		}
	}
}
