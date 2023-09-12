using UnityEngine;

[System.Serializable]
public class Audio
{
    public string audioName;
    public AudioClip audioClip;

    public Audio(string name, AudioClip clip)
    {
        this.audioName = name;
        this.audioClip = clip;
    }
}
