using UnityEngine;
using System.Collections.Generic;

namespace HollowNight.World
{
    /// <summary>
    /// Manages all audio effects and music in the game
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }
        
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private float masterVolume = 1f;
        [SerializeField] private float musicVolume = 0.7f;
        [SerializeField] private float sfxVolume = 0.8f;
        
        private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            if (musicSource == null)
                musicSource = gameObject.AddComponent<AudioSource>();
            
            if (sfxSource == null)
                sfxSource = gameObject.AddComponent<AudioSource>();
        }
        
        public void PlayMusic(string musicName, bool loop = true)
        {
            if (audioClips.TryGetValue(musicName, out AudioClip clip))
            {
                musicSource.clip = clip;
                musicSource.loop = loop;
                musicSource.volume = musicVolume * masterVolume;
                musicSource.Play();
            }
            else
            {
                Debug.LogWarning($"Music {musicName} not found");
            }
        }
        
        public void PlaySFX(string sfxName)
        {
            if (audioClips.TryGetValue(sfxName, out AudioClip clip))
            {
                sfxSource.volume = sfxVolume * masterVolume;
                sfxSource.PlayOneShot(clip);
            }
            else
            {
                Debug.LogWarning($"SFX {sfxName} not found");
            }
        }
        
        public void StopMusic()
        {
            musicSource.Stop();
        }
        
        public void SetMasterVolume(float volume)
        {
            masterVolume = Mathf.Clamp01(volume);
            UpdateVolumes();
        }
        
        private void UpdateVolumes()
        {
            if (musicSource != null)
                musicSource.volume = musicVolume * masterVolume;
            
            if (sfxSource != null)
                sfxSource.volume = sfxVolume * masterVolume;
        }
    }
}
