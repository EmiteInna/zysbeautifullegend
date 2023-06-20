using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    public bool death_after_spawn;
    public GameObject spawn;
    public string detectTag;
    public float range;
    public float cooldown;
    bool is_dead;
    private void Start()
    {
        is_dead = false;
        StartCoroutine(SpawnEntity());
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);

    }
    bool DetectRange()
    {
        var colliders = Physics.OverlapSphere(transform.position, range);
        foreach (var tar in colliders)
        {
            if (tar.CompareTag(detectTag))
            {
                return true;
            }
        }
        return false;
    }
    IEnumerator SpawnEntity()
    {
        while (!is_dead)
        {
            if (!DetectRange())
            {
                Instantiate(spawn, transform.position, Quaternion.identity);
                if (death_after_spawn) is_dead = true;
            }
            yield return new WaitForSecondsRealtime(cooldown);
        }
        Destroy(gameObject);
        yield return null;
    }
}
