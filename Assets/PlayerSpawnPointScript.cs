using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnPointScript : MonoBehaviour
{
    public GameObject currentCharacter;
    public List<GameObject> characterPool = new List<GameObject>();
    public List<Transform> spawnPool = new List<Transform>();
    [Header("Camera")]
    public CinemachineVirtualCamera cinemachine;
    [Header("DUUMguy")]
    public GameObject DUUMguy;
    public Transform DUUMguy_spawn;
    [Header("VRguy")]
    public GameObject VRguy;
    public Transform VRguy_spawn;

    Keyboard kb;

    // Start is called before the first frame update
    void Awake()
    {
        characterPool.Add(DUUMguy);
        characterPool.Add(VRguy);
        spawnPool.Add(DUUMguy_spawn);
        spawnPool.Add(VRguy_spawn);
        //pickChracterVRguy();
        //pickCharacterDUUMguy();


    }

    // Update is called once per frame
    void Update()
    {
        kb = InputSystem.GetDevice<Keyboard>();
        if (kb.iKey.isPressed)
        {
            //currentCharacter.GetComponent<PlayerInputHandlerScript>().replayEvents();
            Rewind();

        }
        if (kb.pKey.isPressed)
        {
            //currentCharacter.GetComponent<PlayerInputHandlerScript>().activeAI(false);

            //Rewind();
            pickChracterVRguy();

        }
        else if (kb.oKey.isPressed)
        {
            //currentCharacter.GetComponent<PlayerInputHandlerScript>().activeAI(true);

            //Rewind();
            pickCharacterDUUMguy();
        }
    }


    public void Rewind()
    {
        for (int i = 0; i< spawnPool.Count && i <characterPool.Count;i++)
        {
            characterPool[i].GetComponent<PlayerInputHandlerScript>().Rewind();
            characterPool[i].transform.position = spawnPool[i].transform.position;

        }
    }

    public void pickChracterVRguy()
    {
        pickCharacter(VRguy);
    }

    public void pickCharacterDUUMguy()
    {
        pickCharacter(DUUMguy);
    }

    void pickCharacter(GameObject g)
    {
        currentCharacter = g;

        foreach (GameObject character in characterPool)
        {
            if (!character.Equals(currentCharacter))
            {
                character.GetComponent<PlayerInputHandlerScript>().activeAI(true);
            }
            else
            {
                character.GetComponent<PlayerInputHandlerScript>().activeAI(false);

            }
        }
        setCNFocus(currentCharacter);
        Rewind();
    }


    public void setCNFocus(GameObject g)
    {
        Transform t = g.GetComponent<PlayerMovementScript>().midpoint.transform;
        cinemachine.LookAt = t;
        cinemachine.Follow = t;
    }
}
