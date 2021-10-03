using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundRepeater : MonoBehaviour
{
    [SerializeField] private Vector2 _repeatDelayRandom;
    
    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        Play();
    }

    public void Play()
    {
        _source.Play();
        Invoke(nameof(Play), Random.Range(_repeatDelayRandom.x, _repeatDelayRandom.y));
    }
}
