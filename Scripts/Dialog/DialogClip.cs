using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogClip : MonoBehaviour
{
    public List<Sprite> HeadImg;
    public List<string> TextContent;
    public List<int> fontSize;
    public List<float> Duration;
    public List<string> Names;
    public List<int> specialEffect;
    public List<AudioClip> audios;
    public GameObject eventTrigger;
}
