using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerFootsteps : MonoBehaviour
{
	[SerializeField] private PlayerData _playerData;
	
	[SerializeField] private FootstepMap footstepMap;
	[SerializeField] private LayerMask groundMask;
	[SerializeField] private List<GameObject> objectsToIgnore = new List<GameObject>();
	
	[SerializeField] private float crouchWalkingStepInterval;
	[SerializeField] private float walkingStepInterval;
	[SerializeField] private float runningStepInterval;
	
	[SerializeField] private float crouchWalkingStepVolume;
	[SerializeField] private float walkingStepVolume;
	[SerializeField] private float runningStepVolume;

	
	private float stepVolume = 1;
	private float stepInterval = 0.5f;
	private Coroutine footstepCoroutine;

	protected void Update()
	{
		if(_playerData.isIdle)
		{
			if(footstepCoroutine == null)
				return;
				
			StopCoroutine(footstepCoroutine);
			footstepCoroutine = null;
		}
		else
		{
			if(footstepCoroutine == null)
			{
				footstepCoroutine = StartCoroutine(FootstepCoroutine());
			}
		}
	}

	private IEnumerator FootstepCoroutine()
	{
		while (true)
		{
			if(_playerData.isWalking && !_playerData.isCrouching)
			{
				stepInterval = walkingStepInterval;
				stepVolume = walkingStepVolume;
			}
			else if(_playerData.isRunning)
			{
				stepInterval = runningStepInterval;
				stepVolume = runningStepVolume;
			}
			else if(_playerData.isCrouching && _playerData.isWalking)
			{
				stepInterval = crouchWalkingStepInterval;
				stepVolume = crouchWalkingStepVolume;
			}
				
			yield return new WaitForSeconds(stepInterval);


			bool canPlayFootstepSound = false;
			
			RaycastHit hit;
			
			if (Physics.Raycast(transform.position, Vector3.down, out hit, groundMask))
			{
				for(int objectsToIgnoreIndex = 0; objectsToIgnoreIndex < objectsToIgnore.Count; objectsToIgnoreIndex++)
				{
					GameObject currentObjectInIgnoreList = objectsToIgnore[objectsToIgnoreIndex];
					
					if(hit.collider.gameObject == currentObjectInIgnoreList)
					{
						canPlayFootstepSound = false;
					}
					else
					{
						canPlayFootstepSound = true;
					}
				}
				
				if(canPlayFootstepSound)
				{
					// Get the texture from the material under the player
					Texture groundTexture = hit.collider.GetComponent<Renderer>().material.mainTexture;

					// Find the corresponding sound list
					List<AudioClip> clipList = footstepMap.mappings.FirstOrDefault(mapping => mapping.groundTexture.Equals(groundTexture))?.footstepSounds;

					if (clipList != null && clipList.Count > 0)
					{
						// Choose a random sound from the list
						AudioClip clip = clipList[Random.Range(0, clipList.Count)];

						// Play sound
						AudioManager.instance.PlaySFX(clip, stepVolume, true);
					}
				}
			}
		}
	}
	
	public void SetStepInterval(float interval)
	{
		stepInterval = interval;
	}
}
