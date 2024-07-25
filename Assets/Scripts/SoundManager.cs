using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private AudioClip motetaClip;
    [SerializeField] private AudioClip motenakattaClip;
    [SerializeField] private AudioClip buttonClip;
    [SerializeField] private AudioClip enterClip;
    [SerializeField] private AudioClip leaveClip;

    private AudioSource bgmAudioSource;
    private AudioSource sfxAudioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            bgmAudioSource = gameObject.AddComponent<AudioSource>();
            bgmAudioSource.loop = true;
            bgmAudioSource.playOnAwake = false;
            bgmAudioSource.clip = bgmClip;
            bgmAudioSource.volume = 0.5f;

            sfxAudioSource = gameObject.AddComponent<AudioSource>();
            sfxAudioSource.loop = false;
            sfxAudioSource.playOnAwake = false;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Title" || scene.name == "Lobby" || scene.name == "Main")
        {
            if (!bgmAudioSource.isPlaying)
            {
                bgmAudioSource.Play();
            }
        }
        else if (scene.name == "Result")
        {
            if (bgmAudioSource.isPlaying)
            {
                bgmAudioSource.Stop();
            }
        }
    }

    public void PlayResultSound(string result)
    {
        if (result == "モテる！")
        {
            sfxAudioSource.clip = motetaClip;
        }
        else if (result == "モテない...")
        {
            sfxAudioSource.clip = motenakattaClip;
        }
        sfxAudioSource.Play();
    }

    public void PlayButtonSound()
    {
        sfxAudioSource.PlayOneShot(buttonClip);
    }

    public void PlayEnterSound()
    {
        sfxAudioSource.PlayOneShot(enterClip);
    }

    public void PlayLeaveSound()
    {
        sfxAudioSource.PlayOneShot(leaveClip);
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
