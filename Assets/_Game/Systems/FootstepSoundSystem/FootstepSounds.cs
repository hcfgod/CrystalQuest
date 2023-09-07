using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lightbug.CharacterControllerPro.Implementation;

public class FootstepSounds : MonoBehaviour
{
    #region VariablesAndMethods

	[SerializeField] private NormalMovement _playerMovment;
	
	[SerializeField] private LayerMask FloorLayer;
	[SerializeField] private TextureSound[] TextureSounds;
	[SerializeField] private bool BlendTerrainSounds;
	
	[SerializeField] private float sprintingDelay;
	[SerializeField] private float walkingDelay;
	[SerializeField] private float crouchingDelay;
	
	[SerializeField] private float sprintingVolume;
	[SerializeField] private float walkingVolume;
	[SerializeField] private float crouchingVolume;
	[SerializeField] private float landingVolume;

	[SerializeField] private PlayerData _playerData;
	private bool _canPlayLandSound = false;

	private Coroutine checkGroundRoutine;
	private Coroutine footstepRoutine;
	
    #endregion

    #region MonoBehaviour
	
	private void Start()
	{
		_playerMovment.CharacterActor.OnLanded += Landed;
		
		if(checkGroundRoutine == null && footstepRoutine == null)
			checkGroundRoutine = StartCoroutine(CheckGround());
	}

	private void OnEnable()
	{	
		checkGroundRoutine = StartCoroutine(CheckGround());		
	}
	
	private void OnDisable()
	{
		if(checkGroundRoutine != null && footstepRoutine != null)
		{
			checkGroundRoutine = null;
			footstepRoutine = null;
		}
	}
	
	private void OnDestroy()
	{
		_playerMovment.CharacterActor.OnLanded -= Landed;
	}
	
    #endregion

    #region Methods

	private void Landed()
	{
		_canPlayLandSound = true;
	}

	private void PlayLandedSound(Terrain Terrain, Vector3 HitPoint)
	{
		Vector3 terrainPosition = HitPoint - Terrain.transform.position;
		Vector3 splatMapPosition = new Vector3(terrainPosition.x / Terrain.terrainData.size.x, 0, terrainPosition.z / Terrain.terrainData.size.z);

		int x = Mathf.FloorToInt(splatMapPosition.x * Terrain.terrainData.alphamapWidth);
		int z = Mathf.FloorToInt(splatMapPosition.z * Terrain.terrainData.alphamapHeight);

		float[,,] alphaMap = Terrain.terrainData.GetAlphamaps(x, z, 1, 1);

		if (!BlendTerrainSounds)
		{
			int primaryIndex = 0;

			for (int i = 1; i < alphaMap.Length; i++)
			{
				if (alphaMap[0, 0, i] > alphaMap[0, 0, primaryIndex])
				{
					primaryIndex = i;
				}
			}

			foreach (TextureSound textureSound in TextureSounds)
			{
				if (textureSound.Albedo == Terrain.terrainData.terrainLayers[primaryIndex].diffuseTexture)
				{
					AudioClip clip = GetLandedAudioFromTextureSound(textureSound);
					AudioManager.instance.PlaySFX(clip, landingVolume, true);
				}
			}
		}
		else
		{
			List<AudioClip> clips = new List<AudioClip>();

			int clipIndex = 0;

			for (int i = 0; i < alphaMap.Length; i++)
			{
				if (alphaMap[0, 0, i] > 0)
				{
					foreach (TextureSound textureSound in TextureSounds)
					{
						if (textureSound.Albedo == Terrain.terrainData.terrainLayers[i].diffuseTexture)
						{
							AudioClip clip = GetLandedAudioFromTextureSound(textureSound);
							AudioManager.instance.PlaySFX(clip, landingVolume, true);

							clips.Add(clip);
							clipIndex++;
						}
					}
				}
			}
		}
	}

	private void PlayLandedSound(Renderer Renderer)
	{
		foreach (TextureSound textureSound in TextureSounds)
		{
			if (textureSound.Albedo == Renderer.material.GetTexture("_MainTex"))
			{
				AudioClip clip = GetLandedAudioFromTextureSound(textureSound);
				AudioManager.instance.PlaySFX(clip, sprintingVolume, true);
			}
		}
	}

	private IEnumerator CheckGround()
	{
		while (true)
		{
			if (_playerData.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f, FloorLayer))
			{
				bool componentFound = false;

				if (hit.collider.TryGetComponent<Terrain>(out Terrain terrain))
				{
					if (_canPlayLandSound)
					{
						PlayLandedSound(terrain, hit.point);
						_canPlayLandSound = false;
					}

					yield return StartCoroutine(PlayFootstepSoundFromTerrain(terrain, hit.point));
					componentFound = true;
				}
				else if (hit.collider.TryGetComponent<Renderer>(out Renderer renderer))
				{
					if (_canPlayLandSound)
					{
						PlayLandedSound(renderer);
						_canPlayLandSound = false;
					}

					yield return StartCoroutine(PlayFootstepSoundFromRenderer(renderer));
					componentFound = true;
				}

				if (!componentFound)
				{
					// Try to get the component in children
					if (hit.collider.GetComponentInChildren<Terrain>())
					{
						Terrain childTerrain = hit.collider.GetComponentInChildren<Terrain>();

						if (_canPlayLandSound)
						{
							PlayLandedSound(childTerrain, hit.point);
							_canPlayLandSound = false;
						}

						yield return footstepRoutine = StartCoroutine(PlayFootstepSoundFromTerrain(childTerrain, hit.point));
					}
					else if (hit.collider.GetComponentInChildren<Renderer>())
					{
						Renderer childRenderer = hit.collider.GetComponentInChildren<Renderer>();

						if (_canPlayLandSound)
						{
							PlayLandedSound(childRenderer);
							_canPlayLandSound = false;
						}

						yield return footstepRoutine = StartCoroutine(PlayFootstepSoundFromRenderer(childRenderer));
					}
				}
			}

			yield return null;
		}
	}

	private IEnumerator PlayFootstepSoundFromTerrain(Terrain Terrain, Vector3 HitPoint)
	{
		Vector3 terrainPosition = HitPoint - Terrain.transform.position;
		Vector3 splatMapPosition = new Vector3(terrainPosition.x / Terrain.terrainData.size.x,0,terrainPosition.z / Terrain.terrainData.size.z);

		int x = Mathf.FloorToInt(splatMapPosition.x * Terrain.terrainData.alphamapWidth);
		int z = Mathf.FloorToInt(splatMapPosition.z * Terrain.terrainData.alphamapHeight);

		float[,,] alphaMap = Terrain.terrainData.GetAlphamaps(x, z, 1, 1);

		if (!BlendTerrainSounds)
		{
			int primaryIndex = 0;

			for (int i = 1; i < alphaMap.Length; i++)
			{
				if (alphaMap[0, 0, i] > alphaMap[0, 0, primaryIndex])
				{
					primaryIndex = i;
				}
			}

			foreach (TextureSound textureSound in TextureSounds)
			{
				if (textureSound.Albedo == Terrain.terrainData.terrainLayers[primaryIndex].diffuseTexture)
				{
					AudioClip clip = GetFootstepAudioFromTextureSound(textureSound);

					if (_playerData.isRunning)
					{
						AudioManager.instance.PlaySFX(clip, sprintingVolume, true);

						yield return new WaitForSeconds(sprintingDelay);
					}
					else if (_playerData.isWalking && !_playerData.isCrouching)
					{
						AudioManager.instance.PlaySFX(clip, walkingVolume, true);

						yield return new WaitForSeconds(walkingDelay);
					}
					else if (_playerData.isCrouching && _playerData.isWalking)
					{
						AudioManager.instance.PlaySFX(clip, crouchingVolume, true);

						yield return new WaitForSeconds(crouchingDelay);
					}

					break;
				}
			}
		}
		else
		{
			List<AudioClip> clips = new List<AudioClip>();

			int clipIndex = 0;

			for (int i = 0; i < alphaMap.Length; i++)
			{
				if (alphaMap[0, 0, i] > 0)
				{
					foreach(TextureSound textureSound in TextureSounds)
					{
						if (textureSound.Albedo == Terrain.terrainData.terrainLayers[i].diffuseTexture)
						{
							AudioClip clip = GetFootstepAudioFromTextureSound(textureSound);
                            
							if (_playerData.isRunning)
							{
								AudioManager.instance.PlaySFX(clip, sprintingVolume, true);
							}
							else if (_playerData.isWalking && !_playerData.isCrouching)
							{
								AudioManager.instance.PlaySFX(clip, walkingVolume, true);
							}
							else if (_playerData.isCrouching && _playerData.isWalking)
							{
								AudioManager.instance.PlaySFX(clip, crouchingVolume, true);
							}

							clips.Add(clip);
							clipIndex++;
							break;
						}
					}
				}
			}

			if (_playerData.isRunning)
			{
				yield return new WaitForSeconds(sprintingDelay);
			}
			else if (_playerData.isWalking && !_playerData.isCrouching)
			{
				yield return new WaitForSeconds(walkingDelay);
			}
			else if (_playerData.isCrouching && _playerData.isWalking)
			{
				yield return new WaitForSeconds(crouchingDelay);
			}
		}
	}

	private IEnumerator PlayFootstepSoundFromRenderer(Renderer Renderer)
	{
		foreach (TextureSound textureSound in TextureSounds)
		{
			if (textureSound.Albedo == Renderer.material.GetTexture("_MainTex"))
			{
				AudioClip clip = GetFootstepAudioFromTextureSound(textureSound);

				if (_playerData.isRunning)
				{
					AudioManager.instance.PlaySFX(clip, sprintingVolume, true);

					yield return new WaitForSeconds(sprintingDelay);
				}
				else if (_playerData.isWalking && !_playerData.isCrouching)
				{
					AudioManager.instance.PlaySFX(clip, walkingVolume, true);

					yield return new WaitForSeconds(walkingDelay);
				}
				else if (_playerData.isCrouching && _playerData.isWalking)
				{
					AudioManager.instance.PlaySFX(clip, crouchingVolume, true);

					yield return new WaitForSeconds(crouchingDelay);
				}

				break;
			}
		}
	}

	private AudioClip GetFootstepAudioFromTextureSound(TextureSound TextureSound)
	{
		int clipIndex = Random.Range(0, TextureSound.footstepAudio.Length);
		return TextureSound.footstepAudio[clipIndex];
	}

	private AudioClip GetLandedAudioFromTextureSound(TextureSound TextureSound)
	{
		int clipIndex = Random.Range(0, TextureSound.landingAudio.Length);
		return TextureSound.landingAudio[clipIndex];
	}
	
    #endregion
}
