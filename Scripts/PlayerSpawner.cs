using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPre;
    public Vector3 Pos;
    private void Start()
    {
        ProceedureManager.Instance.turnBGMBack();
        if (GameObject.FindGameObjectWithTag("Player") == null)
            Instantiate(playerPre, Pos, Quaternion.identity);
        else Debug.Log("There's already one");
    }
}
