using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceedureManager : MonoBehaviour
{
    public static ProceedureManager Instance;
    [Header("进程参量")]
    public bool BloodArtifactCollected;
    public GameObject GodBastrigger;
    public GameObject GodBastDialog;
    public GameObject tombDialog;
    public List<AudioClip> BGMs;
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

    public void SetGodBas()
    {
        if (BloodArtifactCollected) return;
        if (tombDialog != null) tombDialog.SetActive(false);
        GodBastrigger.SetActive(true);
        GodBastDialog.SetActive(true);
        GameObject pl = GameObject.FindGameObjectWithTag("Player");
        pl.GetComponent<PlayerController>().agent.Warp(GodBastDialog.transform.position);
        Camera.main.GetComponent<AudioSource>().clip = BGMs[1];
        Camera.main.GetComponent<AudioSource>().Play();
        //pl.transform.SetPositionAndRotation(new Vector3(0, 40, 0), Quaternion.identity);
        //        pl.transform.SetPositionAndRotation(GodBastDialog.transform.position+new Vector3(0,40,0),Quaternion.identity);
        Camera.main.transform.position = new Vector3(pl.transform.position.x, 9.9f + 4.5f, pl.transform.position.z);
        BloodArtifactCollected = true;
    }
    public void turnBGMBack()
    {
        Camera.main.GetComponent<AudioSource>().clip = BGMs[0];
        Camera.main.GetComponent<AudioSource>().Play();
    }
}
