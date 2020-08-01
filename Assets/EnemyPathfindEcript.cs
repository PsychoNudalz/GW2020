using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfindEcript : MonoBehaviour
{
    public AIDestinationSetter des;
    // Start is called before the first frame update
    void Awake()
    {
        des.target = GameObject.FindGameObjectWithTag("Player").transform;
    }

}
