using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    #region Fields

    public static AudioManager instance;

    [Tooltip("Must have a folder path of 'Resources' -> 'Audio' with all your audio clips in the 'Audio' Folder inside 'Resources' Folder.")]
    [SerializeField] bool autoLoadAudio;

    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private AudioSource sfxSource;

    [SerializeField]
    private GameObject audioSourcePrefab;

    [SerializeField]
    private AudioMixer audioMixer;

    private List<AudioSource> audioSourcePool = new List<AudioSource>();
    private int poolSize = 10;

    [SerializeField]
    private List<Audio> audioClips = new List<Audio>();
    #endregion

    #region Monobehavior

    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        if(autoLoadAudio)
        {
            LoadAudio();
        }

        // Initialize the AudioSource pool
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource audioSource = Instantiate(audioSourcePrefab, transform).GetComponent<AudioSource>();

            AudioMixerGroup[] audioMixerGroups = audioMixer.FindMatchingGroups("Master");

            if (audioMixerGroups != null)
            {
                if (audioMixerGroups.Length > 0)
                {
                    audioSource.outputAudioMixerGroup = audioMixerGroups[0];
                }
            }

            audioSource.gameObject.SetActive(false);
            audioSourcePool.Add(audioSource);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Play3DSound("Test", transform.position, 0.5f);
            ChangeMasterVolume(-80);
        }
    }

    #endregion

    #region Methods

    private void LoadAudio()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");

        foreach (AudioClip clip in clips)
        {
            audioClips.Add(new Audio(clip.name, clip));
        }
    }

    public void PlayMusic(string name, bool loop = true)
    {
        Audio audio = this.audioClips.Find(a => a.audioName == name);

        if (audio != null)
        {
            musicSource.clip = audio.audioClip;
            musicSource.loop = loop;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Audio not found: " + name);
        }
    }

	public void PlaySFX(string name, float volume, bool fromPool)
	{
		AudioSource audioSource = GetPooledAudioSource();
		Audio audio = this.audioClips.Find(a => a.audioName == name);
		
		if (audio == null)
		{
			Debug.LogWarning("Audio not found: " + name);
			return;
		}
		
		if(fromPool)
		{
			audioSource.gameObject.SetActive(true);
			audioSource.clip = audio.audioClip;
			audioSource.volume = volume;
			audioSource.PlayOneShot(audio.audioClip);
			StartCoroutine(DeactivateAfterFinished(audioSource));
		}
		else
		{	
			sfxSource.volume = volume;
			sfxSource.PlayOneShot(audio.audioClip);
		} 
    }
    
	public void PlaySFX(AudioClip audioClip, float volume, bool fromPool)
	{
		AudioSource audioSource = GetPooledAudioSource();
		
		if(fromPool)
		{
			audioSource.gameObject.SetActive(true);
			audioSource.clip = audioClip;
			audioSource.volume = volume;
			audioSource.PlayOneShot(audioClip);
			StartCoroutine(DeactivateAfterFinished(audioSource));
		}
		else
		{
			sfxSource.volume = volume;
			sfxSource.PlayOneShot(audioClip);
		}
	}

    public void Play3DSound(string name, Vector3 position, float volume = 1, float minHearingDistance = 1f, float maxHearingDistance = 30, float dopplerLevel = 0, AudioRolloffMode rolloffMode = AudioRolloffMode.Linear)
    {
        Audio audio = audioClips.Find(a => a.audioName == name);

        if (audio != null)
        {
            // Get an AudioSource from the pool
            AudioSource audioSource = GetPooledAudioSource();

            if (volume < 0)
                volume = 0;

            if (volume > 1)
                volume = 1;

            if (dopplerLevel > 5)
                dopplerLevel = 5;

            if(dopplerLevel < 0)
                dopplerLevel = 0;

            if(minHearingDistance < 1)
                minHearingDistance = 1;
            
            if (audioSource != null)
            {
                audioSource.volume = volume;
                audioSource.spatialBlend = 1;
                audioSource.minDistance = minHearingDistance;
                audioSource.maxDistance = maxHearingDistance;
                audioSource.dopplerLevel = dopplerLevel;
                audioSource.rolloffMode = rolloffMode;
                audioSource.transform.position = position;
                audioSource.clip = audio.audioClip;
                audioSource.gameObject.SetActive(true);
                audioSource.Play();

                StartCoroutine(DeactivateAfterFinished(audioSource));
            }
        }
        else
        {
            Debug.LogWarning("Audio not found: " + name);
        }
    }

	public void Play3DSound(AudioClip clip, Vector3 position, float volume = 1, float minHearingDistance = 1f, float maxHearingDistance = 30, float dopplerLevel = 0, AudioRolloffMode rolloffMode = AudioRolloffMode.Linear)
	{
		// Get an AudioSource from the pool
		AudioSource audioSource = GetPooledAudioSource();

		if (volume < 0)
			volume = 0;

		if (volume > 1)
			volume = 1;

		if (dopplerLevel > 5)
			dopplerLevel = 5;

		if(dopplerLevel < 0)
			dopplerLevel = 0;

		if(minHearingDistance < 1)
			minHearingDistance = 1;
            
		if (audioSource != null)
		{
			audioSource.volume = volume;
			audioSource.spatialBlend = 1;
			audioSource.minDistance = minHearingDistance;
			audioSource.maxDistance = maxHearingDistance;
			audioSource.dopplerLevel = dopplerLevel;
			audioSource.rolloffMode = rolloffMode;
			audioSource.transform.position = position;
			audioSource.clip = clip;

			audioSource.gameObject.SetActive(true);
			audioSource.Play();
			StartCoroutine(DeactivateAfterFinished(audioSource));
		}
	}

    //Volume is using decibel's so -80 is mute, 0 is normal, and 20 is maxed out
    public void ChangeMasterVolume(float volume)
    {
        if (volume < -80)
            volume = -80;

        if(volume > 20)
            volume = 20;

        audioMixer.SetFloat("MasterVolume", volume);
    }

    //Volume is using decibel's so -80 is mute, 0 is normal, and 20 is maxed out
    public void ChangeMusicVolume(float volume)
    {
        if (volume < -80)
            volume = -80;

        if (volume > 20)
            volume = 20;

        audioMixer.SetFloat("MusicVolume", volume);
    }

    //Volume is using decibel's so -80 is mute, 0 is normal, and 20 is maxed out
    public void ChangeSfxVolume(float volume)
    {
        if (volume < -80)
            volume = -80;

        if (volume > 20)
            volume = 20;

        audioMixer.SetFloat("SfxVolume", volume);
    }

    private IEnumerator DeactivateAfterFinished(AudioSource audioSource)
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        audioSource.gameObject.SetActive(false);
    }

    private AudioSource GetPooledAudioSource()
    {
        foreach (AudioSource audioSource in audioSourcePool)
        {
            if (!audioSource.gameObject.activeInHierarchy)
            {
                return audioSource;
            }
        }

        // If no inactive AudioSource is found, null is returned
        return null;
    }

    #endregion
}
