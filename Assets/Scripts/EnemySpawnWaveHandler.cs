using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnWaveHandler : MonoBehaviour
{
    public List<GameObject> Enemies = new List<GameObject>();
    public GameObject spawnEffect;
    public bool startWave;
    [SerializeField] bool waveClear = false;
    public List<Vector3> initialPosition = new List<Vector3>();
    public List<Quaternion> initialRotation = new List<Quaternion>();

    // Start is called before the first frame update
    void Start()
    {
        Rewind();
        for (int i = 0; i < transform.childCount; i++)
        {
            Enemies.Add(transform.GetChild(i).gameObject);
            initialPosition.Add(transform.GetChild(i).position);
            initialRotation.Add(transform.GetChild(i).rotation);

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

    public bool isWaveClear()
    {
        if (!startWave)
        {
            return false;
        }
        waveClear = true;
        foreach (GameObject g in Enemies)
        {
            if (g.activeSelf)
            {
                waveClear = false;

            }
        }
        return waveClear;
    }

    public void Rewind()
    {

        startWave = false;
        waveClear = false;
        foreach (GameObject g in Enemies)
        {
            g.SetActive(true);
            g.GetComponent<EnemyScript>().Rewind();
            g.SetActive(false);
        }
    }
}
