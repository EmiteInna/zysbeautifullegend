using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance;
    public Cinemachine.CinemachineImpulseSource impulse;
    [Header("对话内容")]
    public List<Sprite> HeadImg;
    public List<string> TextContent;
    public List<int> fontSize;
    public List<float> Duration;
    public List<string> Names;
    public List<int> specialEffect;
    public List<AudioClip> audios;
    [Header("值")]
    public bool is_open;
    public bool is_finished_in_one_clip;
    public bool has_clicked;
    public float transparency;
    public float text_percent;
    public float text_percent_now;
    public Sprite blank;
    int index;
    int clipSize;
    [Header("映射")]
    public GameObject dialogPanel;
    public Text Dialog_Text;
    public Text name_Text;
    public Image Head_Image;
    public GameObject source;
    public CanvasGroup canvas;
    public GameObject eventtrigger;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Initialize();
    }
    private void Start()
    {
        impulse = GetComponent<Cinemachine.CinemachineImpulseSource>();
    }
    public void Initialize()
    {
        dialogPanel.SetActive(false);
        canvas.alpha = 0.0f;
        canvas.blocksRaycasts = false;
    }
    IEnumerator OnOpen()
    {
        is_open = !is_open;
        if(is_open)dialogPanel.SetActive(is_open);
        if(is_open)yield return Fade(1);
        else yield return Fade(0);
        if (!is_open) dialogPanel.SetActive(!is_open);
        yield return null;
    }
    IEnumerator Fade(float targetAlpha)
    {
        float time = 1.5f;
        canvas.blocksRaycasts = true;
        float speed = Mathf.Abs(canvas.alpha - targetAlpha) / time;
        while (!Mathf.Approximately(canvas.alpha, targetAlpha))
        {
            canvas.alpha = Mathf.MoveTowards(canvas.alpha, targetAlpha, speed * TimeManager.Instance.realdeltatime);
            yield return null;
        }
        canvas.blocksRaycasts = false;
    }
    
    IEnumerator PlayClips()
    {
        GManager.Instance.ShowDialog();
        yield return OnOpen();
        index = 0;
        clipSize = HeadImg.Count;
        while (index < clipSize)
        {
            is_finished_in_one_clip = false;
            has_clicked = false;
            Head_Image.sprite = HeadImg[index];
            Dialog_Text.text = string.Empty;
            Dialog_Text.fontSize = fontSize[index];
            text_percent = Duration[index];
            name_Text.text = Names[index];
            Dialog_Text.text = string.Empty;
            //SPECIAL
            if (specialEffect[index] == 1)
            {
            }
            //
            text_percent_now = text_percent/TextContent[index].Length;
            GameObject pl = GameObject.FindGameObjectWithTag("Player");
            if (pl != null && audios[index]!=null)SoundManager.Instance.Playsound(pl.transform.position, audios[index], 5);
            int cnt = 0;
            while (text_percent - 0>0.0001f && !has_clicked)
            {
                Dialog_Text.text += TextContent[index][cnt];
                cnt++;
                text_percent -= text_percent_now;
                yield return new WaitForSecondsRealtime(text_percent_now);
            }
            Dialog_Text.text = TextContent[index];
            has_clicked = false;
            while (!has_clicked)
            {
                yield return null;
            }
            is_finished_in_one_clip = true;
            index++;
        }
        if (eventtrigger != null)
        {
            StartCoroutine(eventtrigger.GetComponent<EvTrigger>().triggered());
        }
        Dialog_Text.text=string.Empty;
        name_Text.text=string.Empty;
        Head_Image.sprite=blank;
        yield return OnOpen();
        GManager.Instance.intoNormal();
    }
    IEnumerator LoadClips()
    {
        HeadImg.Clear();
        TextContent.Clear();
        fontSize.Clear();
        Duration.Clear();
        Names.Clear();
        specialEffect.Clear();
        audios.Clear();
        eventtrigger = null;
        foreach (var i in source.GetComponent<DialogClip>().HeadImg)HeadImg.Add(i);
        foreach (var i in source.GetComponent<DialogClip>().TextContent) TextContent.Add(i);
        foreach (var i in source.GetComponent<DialogClip>().fontSize) fontSize.Add(i);
        foreach (var i in source.GetComponent<DialogClip>().Duration) Duration.Add(i);
        foreach (var i in source.GetComponent<DialogClip>().Names) Names.Add(i);
        foreach (var i in source.GetComponent<DialogClip>().specialEffect) specialEffect.Add(i);
        foreach (var i in source.GetComponent<DialogClip>().audios) audios.Add(i);
        eventtrigger = source.GetComponent<DialogClip>().eventTrigger;
        yield return null;
    }
    public IEnumerator PlayDialog()
    {
        yield return LoadClips();
        yield return PlayClips();
    }
   
    void Update()
    {
        if (!has_clicked)
        {
            if(Input.GetMouseButtonDown(0))has_clicked = true;
        }
        
    }
    
}
