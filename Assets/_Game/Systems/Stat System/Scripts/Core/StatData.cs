using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// StatData is a ScriptableObject that holds the initial and maximum values for a stat.
[CreateAssetMenu(fileName = "StatData", menuName = "Stats/New Stat")]
public class StatData : ScriptableObject
{
	public string statName;
	
	public float initialValue;
	public float minValue;
	public float maxValue;
}
