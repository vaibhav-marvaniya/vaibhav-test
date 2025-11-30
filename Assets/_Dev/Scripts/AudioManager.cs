using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource CardFlipAudioSource;
    public AudioSource MatchAudioSource;
    public AudioSource MismatchAudioSource;
    public AudioSource GameOverAudioSource;

    public AudioClip CardFlipClip;
    public AudioClip MatchClip;
    public AudioClip MismatchClip;
    public AudioClip GameOverClip;

    public static AudioManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AudioManager>();
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }
    private static AudioManager _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    public void PlayCardFlip()
    {
        if (CardFlipAudioSource != null && CardFlipClip != null)
        {
            CardFlipAudioSource.PlayOneShot(CardFlipClip);
        }
    }

    public void PlayMatchFlip()
    {
        if (MatchAudioSource != null && MatchClip != null)
        {
            MatchAudioSource.PlayOneShot(MatchClip);
        }
    }

    public void PlayMismatchFlip()
    {
        if (MismatchAudioSource != null && MismatchClip != null)
        {
            MismatchAudioSource.PlayOneShot(MismatchClip);
        }
    }

    public void PlayGameoverClip()
    {
        if (GameOverAudioSource != null && GameOverClip != null)
        {
            GameOverAudioSource.PlayOneShot(GameOverClip);
        }
    }
}
