using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopUpPoolScript : MonoBehaviour
{
    [SerializeField] GameObject damagePopUpPF;
    [SerializeField] List<DamagePopUpScript> pool;
    public int poolSize = 10;
    [SerializeField] int poolPointer=-1;
     // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            pool.Add(Instantiate(damagePopUpPF,transform).GetComponent<DamagePopUpScript>());
        }
    }



    public void newDamageValue(float damageValue)
    {
        DamagePopUpScript currentPopUp = getNextPopUp();
        currentPopUp.transform.position = transform.position;
        currentPopUp.SetUp(damageValue);
    }

    DamagePopUpScript getNextPopUp()
    {
        poolPointer++;
        poolPointer = poolPointer % poolSize;
        return pool[poolPointer];
    }
}
