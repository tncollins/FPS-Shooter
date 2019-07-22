using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{

    private AudioSource source;
    [SerializeField]
    private AudioClip scream, die;

    [SerializeField]
    private AudioClip[] attack;
    
    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayScream()
    {
        source.clip = scream;
        source.Play();
    }

    public void PlayAttack()
    {
        source.clip = attack[Random.Range(0, attack.Length)];
        source.Play();
    }

    public void PlayDead()
    {
        source.clip = die;
        source.Play();
    }
}
