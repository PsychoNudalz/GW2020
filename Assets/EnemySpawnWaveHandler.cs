using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnWaveHandler : MonoBehaviour
{
    public List<GameObject> Enemies = new List<GameObject>();
    public GameObject spawnEffect;
    public bool startWave;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (GameObject g in Enemies)
        {
            g.SetActive(false);
            //Instantiate()
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool setStartWave(bool t)
    {
        startWave = t;
        if (t)
        {
            GameObject effect;
            foreach (GameObject g in Enemies)
            {
                g.SetActive(true);
                effect = Instantiate(spawnEffect, g.transform.position, Quaternion.identity, transform);
                Destroy(effect, 5f);
            }
        }
        return startWave;
    }

    public void cleanList()
    {
        
    }
}
