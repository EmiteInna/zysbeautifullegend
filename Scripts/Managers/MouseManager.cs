using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class MouseManager : MonoBehaviour
{
    public Texture2D mouseTextIdle;
    public Texture2D mouseTextAttack;
    public Texture2D mouseTextPick;
    public static MouseManager Instance;
    RaycastHit hitInfo;
    public event Action<Vector3> OnMouseClicked;
    public event Action<GameObject> OnEnemyClicked;
    public event Action<GameObject> OnItemClicked;
    public float upperX, lowerX, upperZ, lowerZ;
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
        SetCursorTexture();
        MouseControl();
        CameraMove();
    }
    void SetCursorTexture()
    {
        LayerMask canbeRaycast = 1 << 7;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,out hitInfo,500,canbeRaycast))
        {
            if (hitInfo.collider.gameObject.tag == "ground")
            {
                Cursor.SetCursor(mouseTextIdle, new Vector2(16, 16), CursorMode.Auto);
            }
            if (hitInfo.collider.gameObject.tag == "enemy" || hitInfo.collider.gameObject.tag == "boss")
            {
                Cursor.SetCursor(mouseTextAttack, new Vector2(16, 16), CursorMode.Auto);
            }
            if (hitInfo.collider.gameObject.tag == "item" || hitInfo.collider.gameObject.tag == "friend")
            {
                Cursor.SetCursor(mouseTextPick, new Vector2(16, 16), CursorMode.Auto);
            }
        }
    }
    void MouseControl()
    {
        
        if (Input.GetMouseButtonDown(1)&&hitInfo.collider!=null)
        {
            if (hitInfo.collider.gameObject.tag == "ground" || hitInfo.collider.gameObject.tag == "friend")
            {
                OnMouseClicked?.Invoke(hitInfo.point);
            }
            if (hitInfo.collider.gameObject.tag == "enemy" || hitInfo.collider.gameObject.tag == "boss")
            {
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
            }
            if (hitInfo.collider.gameObject.tag == "item")
            {
                OnItemClicked?.Invoke(hitInfo.collider.gameObject);
            }
        }
    }
    void CameraMove()
    {
        if (!(GManager.Instance.gamestate == GameStates.NORMAL)) return;
        //Debug.Log(string.Format("x={0},z={1}",Camera.main.transform.position.x,Camera.main.transform.position.z));
        float thres = 0.1f;
        float speed = 0.2f;
      
        if (Input.mousePosition.x < Screen.width * thres)
        {
            if (Camera.main.transform.position.x - speed <lowerX) return;
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x - speed, Camera.main.transform.position.y, Camera.main.transform.position.z);
        }
        if (Input.mousePosition.x > Screen.width * (1-thres))
        {
            if (Camera.main.transform.position.x + speed > upperX) return;
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + speed, Camera.main.transform.position.y, Camera.main.transform.position.z);
        }
        if (Input.mousePosition.y < Screen.height * thres)
        {
            if (Camera.main.transform.position.z - speed < lowerZ) return;
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x , Camera.main.transform.position.y, Camera.main.transform.position.z-speed);
        }
        if (Input.mousePosition.y > Screen.height * (1 - thres))
        {
            if (Camera.main.transform.position.z + speed >upperZ) return;
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x , Camera.main.transform.position.y, Camera.main.transform.position.z+speed);
        }
    }
}
