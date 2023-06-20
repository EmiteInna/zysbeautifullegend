using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseManager : MonoBehaviour
{
    public GameObject PausePanel;
    public Slider bgmSlider;
    public Slider seSlider;
    public List<string> tips;
    public AudioMixer bgmMixer;
    public AudioMixer seMixer;
    public Text tipstext;
    public static PauseManager Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        PausePanel.SetActive(false);
    }
    public void SetBGMVolumn()
    {
        bgmMixer.SetFloat("bgm", bgmSlider.value);
    }
    public void SetSoundEffectVolumn()
    {
        seMixer.SetFloat("se", seSlider.value);
    }
    public void show()
    {
        if (BackpackManager.Instance.is_opened) return;
        PausePanel.SetActive(true);
        int r = Random.Range(0, tips.Count);
        tipstext.text = tips[r];
    }
    public void unshow()
    {
        PausePanel.SetActive(false);
    }
}
