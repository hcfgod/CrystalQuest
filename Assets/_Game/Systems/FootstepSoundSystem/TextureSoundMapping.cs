using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class TextureSoundMapping
{
	public Texture groundTexture;  // The texture associated with the ground type
	public List<AudioClip> footstepSounds;  // List of sounds for this texture
}
