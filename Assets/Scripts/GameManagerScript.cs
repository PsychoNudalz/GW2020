using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public List<EnemySpawnWaveHandler> enemySpawnWaveHandlers = new List<EnemySpawnWaveHandler>();
    [SerializeField] int wave;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemySpawnWaveHandlers[wave].Enemies.Count == 0)
        {
            wave += 1;
        } else
        {
            if (!enemySpawnWaveHandlers[wave].startWave)
            {
                enemySpawnWaveHandlers[wave].setStartWave(true);
            }
        }
    }
}
