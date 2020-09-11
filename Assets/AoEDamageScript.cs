using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class AoEDamageScript : MonoBehaviour
{
    public float damagePerSecond;
    public CapsuleCollider2D capsuleCollider2D;
    //public ParticleSystem particleSystem;
    public Light2D light;
    public float radius;
    public float duration = 5;
    public float damageRate = 0.5f;
    [SerializeField] List<GameObject> damageList;
    [SerializeField] float cooldown = 1;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector2(radius * 2f, radius * 2f);
        setEffect();

    }

    // Update is called once per frame
    void Update()
    {
        doDamageToList();
        checkDestroy();
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
        if (cooldown > damageRate)
        {

            EnemyScript e;
            try
            {

                foreach (GameObject g in damageList)
                {
                    if (g == null)
                    {
                        break;
                    }
                    if (g.activeSelf)
                    {
                        if (g.TryGetComponent<EnemyScript>(out e))
                        {
                            e.takeDamage(damagePerSecond);
                        }
                    }
                }
            } catch (System.InvalidOperationException)
            {

            }
            cooldown = 0;
        }
        cooldown += Time.deltaTime;
    }

    void setEffect()
    {
        //ParticleSystem.ShapeModule t = particleSystem.shape;
        //t.radius = radius;
        //ParticleSystem.EmissionModule e = particleSystem.emission;
        //e.rateOverTimeMultiplier = e.rateOverTime.constant * radius * radius * 4;
        light.pointLightOuterRadius = radius *1.2f;
    }

    void checkDestroy()
    {
        duration -= Time.deltaTime;
        if (duration < 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }


}
