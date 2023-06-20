using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    public float realtime, realdeltatime;
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
    void Update()
    {
        realdeltatime = Time.realtimeSinceStartup - realtime;
        realtime = Time.realtimeSinceStartup;
        if (!BackpackManager.Instance.is_opened)
        {
            foreach(var item in BackpackManager.Instance.items)
            {
                if (item.GetComponent<ItemController>().cooldown_now > 0)
                {
                    item.GetComponent<ItemController>().cooldown_now = Mathf.Max(0, item.GetComponent<ItemController>().cooldown_now - realdeltatime);
                }
            }
        }
    }
}
