using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatSystem
{
	public class StatInteractionManager
	{
		private Dictionary<string, IStatInteraction> interactions = new Dictionary<string, IStatInteraction>();

		public void RegisterInteraction(string interactionName, IStatInteraction interaction)
		{
			interactions[interactionName] = interaction;
		}

		public void TriggerInteraction(string interactionName, Stat baseStat, Stat targetStat)
		{
			if (interactions.ContainsKey(interactionName))
			{
				interactions[interactionName].ApplyInteraction(baseStat, targetStat);
			}
		}
	}
}
