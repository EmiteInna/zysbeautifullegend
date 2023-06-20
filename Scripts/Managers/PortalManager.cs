using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public bool readyTo;
    public Vector3 destPos;
    public string destScene;
    private void Start()
    {
        if (KeyManager.Instance == null) return;
        KeyManager.Instance.OnPortalPushed += Teleport;
    }
    private void OnDisable()
    {
        if (KeyManager.Instance == null) return;
        KeyManager.Instance.OnPortalPushed -= Teleport;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Player")
            readyTo = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            readyTo = false;
    }
    void Teleport()
    {
        if(readyTo) SceneController.Instance.TransitionToDestination(destPos, destScene);
    }
   
}
