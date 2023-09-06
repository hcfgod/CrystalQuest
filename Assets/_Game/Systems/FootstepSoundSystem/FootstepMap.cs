using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FootstepMap", menuName = "Footstep Sound Map")]
public class FootstepMap : ScriptableObject
{
	public List<TextureSoundMapping> mappings;
}
