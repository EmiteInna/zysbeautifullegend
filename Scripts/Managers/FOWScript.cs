using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class FOWScript : MonoBehaviour
{
    public GameObject FOWPlane;
    public Transform playerTransform;
    public LayerMask forLayer;
    public float radius = 3f;
    public float radiusSqr { get { return radius*radius; } }
    private Mesh mesh;
    private Vector3[] vertices;
    private Color[] colors;
    private Vector3 hitpoint;

    bool is_ready;
    public int width, height;
    public Vector3 horizontal_dif, vertical_dif;
    private void Start()
    {
        is_ready = false;
        FOWPlane.SetActive(true);
        Invoke("Initialize", 0.04f);
    }
    private void Update()
    {
        if (is_ready == false) return;
        //UpdateColor();
        StartCoroutine(UpdateColorOptimized());
    }
    void Initialize()
    {
        is_ready = true;

        GameObject pl = GameObject.FindGameObjectWithTag("Player");
        if (pl == null)
        {
            Debug.Log("MISSING PLAYER!");
        }
        playerTransform = pl.transform;
        mesh = FOWPlane.GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        colors = new Color[vertices.Length];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.black;
        }
        horizontal_dif = FOWPlane.transform.TransformPoint(vertices[1])- FOWPlane.transform.TransformPoint(vertices[0]);
        vertical_dif = FOWPlane.transform.TransformPoint(vertices[width])- FOWPlane.transform.TransformPoint(vertices[0]);
        mesh.colors = colors;
       // StartCoroutine(debugger());
    }
    IEnumerator debugger()
    {
        for(int i = 0; i < vertices.Length; i++)
        {
            colors[i].a = 0;
        }
        for(int i = 0; i < vertices.Length; i++)
        {
            colors[i].a = 1;
            if (i%604==0) {
                Debug.Log(i/604);
                yield return new WaitForSecondsRealtime(0.5f);
            }
            mesh.colors = colors;
            yield return null;
        }
    }
    
    IEnumerator UpdateColorOptimized()
    {
        Ray r= new Ray(transform.position, playerTransform.position - transform.position);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, 1000, forLayer, QueryTriggerInteraction.Collide))
        {
            hitpoint = hit.point;
            yield return ThreadingCountMeshColors();
            
        }
        yield return null;
    }
    IEnumerator ThreadingCountMeshColors()
    {
        
        float vertical = (hitpoint.x - FOWPlane.transform.TransformPoint(vertices[0]).x) / vertical_dif.x;
        int ver = (int)(vertical + 0.5f);
        for (int i = ver - (int)radius; i <= ver + (int)radius; i++)
        {
            if (i < 0 || i >= width) continue;
            for (int j = 0; j < width; j++)
            {
                int index = i * width + j;
                float dist = Vector3.SqrMagnitude(FOWPlane.transform.TransformPoint(vertices[index]) - hitpoint);
                if (dist < radiusSqr)
                {
                    float alpha = Mathf.Min(colors[index].a, dist / radiusSqr);
                    colors[index].a = alpha;
                }
            }
        }
        mesh.colors = colors;
        yield return null;
    }
    
    void UpdateColor()
    {
        Ray r = new Ray(transform.position, playerTransform.position - transform.position);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, 1000, forLayer, QueryTriggerInteraction.Collide))
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 v = FOWPlane.transform.TransformPoint(vertices[i]);
                float dist = Vector3.SqrMagnitude(v - hit.point);
                if (dist < radiusSqr)
                {
                    float alpha = Mathf.Min(colors[i].a, dist / radiusSqr);
                    colors[i].a = alpha;
                }
            }
            mesh.colors = colors;
        }
        
    }
}
