using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    [SerializeField] PickupTypes pickupType;
    public float amout;
    public bool used = false;
    public Sound sound_Pickup;
    // Start is called before the first frame update
    public void Rewind()
    {
        used = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!used)
            {
                collision.gameObject.GetComponentInChildren<WeaponTypeScript>().addAmmo(amout);
                playSound_Use1();
                gameObject.SetActive(false);
                used = true;
            }
        }
    }
    void playSound_Use1()
    {
         FindObjectOfType<SoundManager>().Play(sound_Pickup.name);

    }
}
