using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance;
    public GameObject atkBuffer;
    public GameObject dfsBuffer;
    public GameObject critChanceBuffer;
    public GameObject critDamageBuffer;
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
    public void BuffAttack(float amount,float time)
    {
        GameObject g=Instantiate(atkBuffer);
        g.GetComponent<atkBuffer>().amount = amount;
        g.GetComponent<atkBuffer>().time = time;
    }
    public void BuffDefense(float amount, float time)
    {
        GameObject g = Instantiate(dfsBuffer);
        g.GetComponent<dfsBuffer>().amount = amount;
        g.GetComponent<dfsBuffer>().time = time;
    }
    public void BuffCritChanceDefense(float amount, float time)
    {
        GameObject g = Instantiate(critChanceBuffer);
        g.GetComponent<ccBuffer>().amount = amount;
        g.GetComponent<ccBuffer>().time = time;
    }
    public void BuffCritDamageDefense(float amount, float time)
    {
        GameObject g = Instantiate(critDamageBuffer);
        g.GetComponent<cdBuffer>().amount = amount;
        g.GetComponent<cdBuffer>().time = time;
    }
}
