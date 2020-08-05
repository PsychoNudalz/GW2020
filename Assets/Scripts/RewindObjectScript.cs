using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindObjectScript : MonoBehaviour
{
    //public GameObject objectPrefab;
    public List<GameObject> rewindObjects = new List<GameObject>();
    public List<Vector3> initialPosition = new List<Vector3>();
    public List<Quaternion> initialRotation = new List<Quaternion>();
    public GameObject currentObject;
    // Start is called before the first frame update

    public void Start()
    {
        /*
        if (currentObject == null)
        {
            currentObject = Instantiate(objectPrefab, transform);
        }
        */
        for (int i = 0; i < transform.childCount; i++)
        {
            rewindObjects.Add(transform.GetChild(i).gameObject);
            initialPosition.Add(transform.GetChild(i).position);
            initialRotation.Add(transform.GetChild(i).rotation);

        }
    }


    public void Rewind()
    {
        for (int j = 0; j < rewindObjects.Count; j++)
        {
            currentObject = rewindObjects[j];

            currentObject.SetActive(true);
            currentObject.transform.position = initialPosition[j];
            currentObject.transform.rotation = initialRotation[j];
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

            }
            else if (currentObject.CompareTag("Pickup"))
            {
                PickupScript i;
                if (currentObject.TryGetComponent<PickupScript>(out i))
                {
                    i.Rewind();
                }
            }
        }

    }
}
