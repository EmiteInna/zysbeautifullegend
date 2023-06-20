using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public GameObject AudioPlayer;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void Playsound(Vector3 position,AudioClip audio,float last)
    {
        GameObject player=Instantiate(AudioPlayer, position, Quaternion.identity);
        player.GetComponent<SoundPlayer>().adc = audio;
        player.GetComponent<SoundPlayer>().last = last;
        player.transform.position = position;
        player.GetComponent<SoundPlayer>().playclip();

    }
}
