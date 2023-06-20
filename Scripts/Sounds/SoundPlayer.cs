using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioClip adc;
    public AudioSource ads;
    public float last;
    public void playclip()
    {
        ads.clip = adc;
        ads.Play();
        Invoke("die", last);
    }
    public void die()
    {
        Destroy(gameObject);
    }
}
