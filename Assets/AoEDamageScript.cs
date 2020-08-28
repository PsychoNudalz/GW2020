using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class AoEDamageScript : MonoBehaviour
{
    public float damagePerSecond;
    public CapsuleCollider2D capsuleCollider2D;
    public ParticleSystem particleSystem;
    public Light2D light;
    public float radius;
    [SerializeField] List<GameObject> damageList;
    [SerializeField] float cooldown;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector2(radius/2f, radius / 2f);
        setEffect();

    }

    // Update is called once per frame
    void Update()
    {
        doDamageToList();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            damageList.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        try
        {
            damageList.Remove(collision.gameObject);
        }
        catch (System.Exception e)
        {
            Debug.LogError(name + " tried to remove " + collision.name);
        }
    }

    void doDamageToList()
    {
        if (cooldown > 1f)
        {
            EnemyScript e;
            foreach (GameObject g in damageList)
            {
                if (g.TryGetComponent<EnemyScript>(out e))
                {
                    e.takeDamage(damagePerSecond);
                }
            }
            cooldown = 0;
        }
        cooldown += Time.deltaTime;
    }

    void setEffect()
    {
        ParticleSystem.ShapeModule t = particleSystem.shape;
        t.radius = radius;
        ParticleSystem.EmissionModule e = particleSystem.emission;
        e.rateOverTimeMultiplier =  e.rateOverTime.constant* radius * radius*4;
        light.pointLightOuterRadius = radius;
    }
}
