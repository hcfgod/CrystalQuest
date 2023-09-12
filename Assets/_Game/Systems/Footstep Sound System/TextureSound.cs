using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TextureSound
{
	public Texture Albedo;
	public AudioClip[] footstepAudio;
	public AudioClip[] landingAudio;
	
	public float sprintingDelay;
	public float walkingDelay;
	public float crouchingDelay;
	
	public float sprintingVolume;
	public float walkingVolume;
	public float crouchingVolume;
	public float landingVolume;
}
