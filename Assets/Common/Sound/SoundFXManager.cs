using Unity.Mathematics;
using UnityEngine;
using Random=UnityEngine.Random;

public class SoundFXManager : MonoBehaviour
{
    private static SoundFXManager _instance;
    public static SoundFXManager Instance => _instance;

    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume){
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
        Destroy(audioSource.gameObject, audioSource.clip.length);
    }
    public void PlayRandomSoundFXClip(AudioClip[] audioClips, Transform spawnTransform, float volume){
        int rand = Random.Range(0, audioClips.Length);
        PlaySoundFXClip(audioClips[rand], spawnTransform, volume);
    }

}
