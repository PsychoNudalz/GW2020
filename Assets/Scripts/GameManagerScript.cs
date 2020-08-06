using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManagerScript : MonoBehaviour
{
    public PlayerSpawnPointScript playerSpawnPointScript;

    public List<EnemySpawnWaveHandler> enemySpawnWaveHandlers = new List<EnemySpawnWaveHandler>();
    [SerializeField] int wave;
    public List<RewindObjectScript> rewindList = new List<RewindObjectScript>();
    Keyboard kb;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] gl = GameObject.FindGameObjectsWithTag("RewindObject");
        foreach(GameObject g in gl)
        {
            RewindObjectScript r;
            if (g.TryGetComponent<RewindObjectScript>(out r))
            {
                rewindList.Add(r);
            }
        }
        playerSpawnPointScript = FindObjectOfType<PlayerSpawnPointScript>();
    }

    // Update is called once per frame
    void Update()
    {
        enemySpawnWave();
        checkRewind();
    }

    void enemySpawnWave()
    {
        if (wave < enemySpawnWaveHandlers.Count)
        {

            if (enemySpawnWaveHandlers[wave].isWaveClear())
            {
                wave += 1;
            }
            else
            {
                if (!enemySpawnWaveHandlers[wave].startWave)
                {
                    enemySpawnWaveHandlers[wave].setStartWave(true);
                }
            }
        }
    }

    void checkRewind()
    {
        kb = InputSystem.GetDevice<Keyboard>();
        if (kb.iKey.isPressed)
        {
            //print("rewinding world");
            Rewind();

        }

    }

    public void Rewind()
    {
        wave = 0;
        playerSpawnPointScript.Rewind();
        foreach(RewindObjectScript r in rewindList)
        {
            r.Rewind();
        }
        foreach(EnemySpawnWaveHandler e in enemySpawnWaveHandlers)
        {
            e.Rewind();
        }
    }
    
}
