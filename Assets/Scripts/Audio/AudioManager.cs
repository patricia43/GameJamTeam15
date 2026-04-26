using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] private AudioSource musicSourcePrefab;

    private AudioSource currentMusicSource;
    [SerializeField] private AudioClip debugClip;

    [SerializeField] private AudioClip backgroundMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (backgroundMusic != null)
        {
            PlayMusic(backgroundMusic, 0.6f, true);
        }
    }

    public AudioSource PlaySound(AudioClip clip, Transform spawnPoint, float volume)
    {
        if (clip == null) return null;

        AudioSource source = Instantiate(soundFXObject, spawnPoint.position, Quaternion.identity);

        source.clip = clip;
        source.volume = volume;
        source.Play();

        Destroy(source.gameObject, clip.length);

        return source;
    }

    public void PlayMusic(AudioClip clip, float volume = 1f, bool loop = true)
    {
        if (clip == null)
            return;

        // If music already playing, stop it first
        if (currentMusicSource != null)
        {
            Destroy(currentMusicSource.gameObject);
        }

        currentMusicSource = Instantiate(musicSourcePrefab, transform.position, Quaternion.identity);
        DontDestroyOnLoad(currentMusicSource.gameObject);

        currentMusicSource.clip = clip;
        currentMusicSource.volume = volume;
        currentMusicSource.loop = loop;
        currentMusicSource.Play();
    }

    public void StopMusic()
    {
        if (currentMusicSource == null)
            return;

        Destroy(currentMusicSource.gameObject);
        currentMusicSource = null;
    }

    // FOR TESTING - DELETE LATER

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            DebugPlaySound();
        }
    }

    private void DebugPlaySound()
    {
        if (debugClip == null)
        {
            Debug.LogWarning("No debug clip assigned in AudioManager.");
            return;
        }

        Debug.Log("Playing debug sound (P pressed)");

        PlaySound(debugClip, transform, 1f);
    }
}