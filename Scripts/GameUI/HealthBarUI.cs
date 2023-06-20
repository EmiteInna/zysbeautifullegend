using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public GameObject healthUIPre;
    public GameObject textnamePre;
    public Transform headPoint;
    public bool alwaysVisible;
    public float visibleTime;
    Image healthSlider;
    Transform UIbar;
    Transform cam;
    GameObject textname;
    CharacterStats currentStats;
    private void Awake()
    {
        currentStats=GetComponent<CharacterStats>();
        currentStats.UpdateHealthBarOnAttack += UpdateHealthBar;
    }
    private void Start()
    {
        cam = Camera.main.transform;
        foreach(Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                UIbar=Instantiate(healthUIPre, canvas.transform).transform;
                textname = Instantiate(textnamePre, canvas.transform);
                healthSlider = UIbar.GetChild(0).GetComponent<Image>();
                UIbar.gameObject.SetActive(alwaysVisible);
                textname.transform.GetChild(0).GetComponent<Text>().text = currentStats.EntityName;
            }
        }
    }
    private void UpdateHealthBar(int currenthealth,int maxhealth)
    {

        if (currenthealth <= 0)
        {
            Destroy(UIbar.gameObject);
            Destroy(textname);
            currentStats.UpdateHealthBarOnAttack -= UpdateHealthBar;
            return;
        }
        UIbar.gameObject.SetActive(true);
        float slid = (float)currenthealth / (float)maxhealth;
        healthSlider.fillAmount = slid;
    }
    private void LateUpdate()
    {
        if (UIbar != null)
        {
            UIbar.position = headPoint.position;
            UIbar.forward = -cam.forward;
        }
        if (textname != null)
        {
            textname.transform.position = headPoint.position;
            textname.transform.forward = -cam.forward;
        }
    }

}
