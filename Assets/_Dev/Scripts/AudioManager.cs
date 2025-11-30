using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource Cardflipaudiosource, Matchaudiosource, Mismatchaudiosource, Fameoveraudiosource;
    public AudioClip CardFlipClip, MatchClip, MismatchClip, GameoverClip;
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
        Cardflipaudiosource.PlayOneShot(CardFlipClip);
    }

    public void PlayMatchFlip()
    {
        Matchaudiosource.PlayOneShot(MatchClip);
    }

    public void PlayMismatchFlip()
    {
        Mismatchaudiosource.PlayOneShot(MismatchClip);
    }
    public void PlayGameoverClip()
    {
        Fameoveraudiosource.PlayOneShot(GameoverClip);
    }
}
