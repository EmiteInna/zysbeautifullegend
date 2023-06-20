using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    Debug.Log("Saved");
        //    SavePlayerData();
        //}
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    Debug.Log("Load");
        //    LoadPlayerData();
        //}
    }
    public void SavePlayerData()
    {
        Save(GManager.Instance.playerStats.characterData, GManager.Instance.playerStats.characterData.name);
    }
    public void LoadPlayerData()
    {
        Load(GManager.Instance.playerStats.characterData, GManager.Instance.playerStats.characterData.name);
    }
    public void Save(Object data,string key)
    {
        var jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.Save();
    }
    public void Load(Object data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key),data);
        }
    }
}
