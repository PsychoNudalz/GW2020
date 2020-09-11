using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopUpScript : MonoBehaviour
{
    public TextMeshPro damageText;
    public float ttl = 2f;
    [SerializeField] float ttl_Current = 0;
    public Vector3 moveDir;
    public Color textColour;


    private void Start()
    {
        //textColour = damageText.color;
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            animate();
            checkTTL();
        }
    }



    public void SetUp(float damageValue)
    {
        gameObject.SetActive(true);
        ttl_Current = ttl;
        textColour.a = 1;
        damageText.color = textColour;
        if (damageText == null)
        {
            damageText = GetComponent<TextMeshPro>();
        }
        damageText.text = damageValue.ToString();
    }

    void checkTTL()
    {
        ttl_Current -= Time.deltaTime;
        if (ttl_Current < 0)
        {
            gameObject.SetActive(false);
        }
    }

    void animate()
    {
        transform.position += moveDir * Time.deltaTime;
        textColour.a = ttl_Current / ttl;
        damageText.color = textColour;
    }
}
