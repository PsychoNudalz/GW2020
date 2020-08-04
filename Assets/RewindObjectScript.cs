using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindObjectScript : MonoBehaviour
{
    public GameObject objectPrefab;
    public GameObject currentObject;
    // Start is called before the first frame update

    public void Start()
    {
        if (currentObject == null)
        {
            currentObject = Instantiate(objectPrefab, transform);
        }
    }


    public void Rewind()
    {
        currentObject.SetActive(true);
        currentObject.transform.position = transform.position;
        currentObject.transform.rotation = transform.rotation;
        if (currentObject.CompareTag("Enemy"))
        {

        }
        else if (currentObject.CompareTag("Object"))
        {
            InteractableObjectScript i;
            if (currentObject.TryGetComponent<InteractableObjectScript>(out i))
            {
                i.Rewind();
            }

        } else if (currentObject.CompareTag("PickUp"))
        {
            PickupScript i;
            if (currentObject.TryGetComponent<PickupScript>(out i))
            {
                i.Rewind();
            }
        }
    }
}
