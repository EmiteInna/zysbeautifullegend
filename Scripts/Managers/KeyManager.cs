using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class KeyManager : MonoBehaviour
{
    public static KeyManager Instance;
    RaycastHit hitInfo;
    public event Action OnPortalPushed;
    public event Action OnBackpackPushed;
    public event Action OnPausePushed;
    public GameObject MapPanel;
    bool map_visible;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        map_visible = false;
    }
    private void Update()
    {
        checkKeys();
    }
    public void checkKeys()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            map_visible = !map_visible;
            MapPanel.SetActive(map_visible);   
        }
        if (GManager.Instance.gamestate != GameStates.PAUSED && GManager.Instance.gamestate != GameStates.DIALOG)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                OnPortalPushed?.Invoke();
            }
        }
            if (Input.GetKeyDown(KeyCode.B))
            {
                OnBackpackPushed?.Invoke();
            }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPausePushed?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            GameObject pl = GameObject.FindGameObjectWithTag("Player");
            Camera.main.transform.position = new Vector3(pl.transform.position.x, 9.9f+4.5f, pl.transform.position.z);
        }
    }
}
