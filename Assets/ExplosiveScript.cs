using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveScript : MonoBehaviour
{
    public Rigidbody2D rb;
    public CapsuleCollider2D capsuleCollider;
    public GameObject spriteRenderer;
    [Header("Explosive")]
    public bool isExplosive = false;
    public GameObject explosiveClusterObject;
    [SerializeField] List<GameObject> explosiveClusters;
    public int clusterAmount = 1;
    [SerializeField] bool isArmed = false;
    [SerializeField] bool isExploded = false;

    public Vector3 throwPosition;
    [Header("TripMine")]
    public float detectRange;
    [SerializeField] Vector2 armAngle;
    [SerializeField] LayerMask layerMask;

    [Header("Tags")]
    [SerializeField] List<string> explodeList;
    [SerializeField] List<string> armList;

    [Header("Sound")]
    public SoundManager soundManager;
    public Sound sound_Explosion;

    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();

        explosiveClusters = new List<GameObject>(clusterAmount);
        GameObject temp;
        for (int i = 0; i < clusterAmount; i++)
        {
            temp = Instantiate(explosiveClusterObject, transform);
            temp.SetActive(false);
            explosiveClusters.Add(temp);
        }
    }


    private void Update()
    {
        if (isArmed && !isExploded)
        {
            setRays();
        }
        else
        {
            if (Mathf.Abs((throwPosition - transform.position).magnitude) < 0.7f)
            {
                detonate();
            }
        }
    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (armList.Contains(collision.collider.tag))
        {
            armTrap(collision);

        }
        else if (explodeList.Contains(collision.collider.tag))
        {
            detonate();

        }
    }


    public void detonate()
    {
        if (!isExploded)
        {

            playSound_Explosion();
            //print(name + " kaboom");
            spriteRenderer.SetActive(false);
            activeClusters();
            holdPosition();
            isExploded = true;
        }

    }


    public void armTrap(Collision2D collision)
    {
        //print(name + " armed");
        holdPosition();
        armAngle = collision.GetContact(0).normal;
        transform.right = collision.GetContact(0).normal;
        transform.position = collision.GetContact(0).point;
        transform.position += transform.right * .1f;
        holdPosition();
        isArmed = true;

    }

    void activeClusters()
    {
        for (int i = 0; i < clusterAmount; i++)
        {
            explosiveClusters[i].SetActive(true);

        }
    }

    void holdPosition()
    {
        rb.bodyType = RigidbodyType2D.Static;
        capsuleCollider.enabled = false;
    }

    void setRays()
    {
        //print(name + " setting rays");
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, armAngle, detectRange, layerMask);
        if (hit)
        {
            //print(name + " detect" + hit.collider.gameObject.tag);
            if (explodeList.Contains(hit.collider.tag))
            {
                detonate();
            }
        }
        Debug.DrawRay(transform.position, armAngle * detectRange, Color.red);
    }
    void playSound_Explosion()
    {
        print("play explosion");
        soundManager.Play(sound_Explosion.name);

    }
}
