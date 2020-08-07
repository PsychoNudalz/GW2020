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
    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!Enemies.Contains(transform.GetChild(i).gameObject))
            {
                print(name + " Adding " + transform.GetChild(i).gameObject);
                Enemies.Add(transform.GetChild(i).gameObject);
                initialPosition.Add(transform.GetChild(i).position);
                initialRotation.Add(transform.GetChild(i).rotation);


            }
        }
        Rewind();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!startWave && collision.CompareTag("Player"))
        {
            setStartWave(true);
        }
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
        else
        {
            //Rewind();
        }
        return startWave;
    }

    public bool isWaveClear()
    {
        if (!startWave)
        {
            return false;
        }
        waveClear = true && startWave;
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

        setStartWave(false);
        waveClear = false;
        for (int j = 0; j < Enemies.Count; j++)
        {
            GameObject currentObject = Enemies[j];
            currentObject.SetActive(true);

            currentObject.transform.position = initialPosition[j];
            currentObject.transform.rotation = initialRotation[j];
        }
        foreach (GameObject g in Enemies)
        {
            g.SetActive(true);
            g.GetComponent<EnemyScript>().Rewind();
            g.SetActive(false);
        }
    }
}
