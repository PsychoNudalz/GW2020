using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnPointScript : MonoBehaviour
{

    public GameObject gameManager;
    public GameObject currentCharacter;
    public List<GameObject> characterPool = new List<GameObject>();
    public List<Transform> spawnPool = new List<Transform>();
    public GameObject grave;
    public bool playerIsDead;
    public float rewindTime;
    public float rewindTimeScale;
    [SerializeField] bool isRewinding = false;
    [Header("Camera")]
    public CinemachineVirtualCamera cinemachine;
    [Header("DUUMguy")]
    public GameObject DUUMguy;
    public Transform DUUMguy_spawn;
    [Header("VRguy")]
    public GameObject VRguy;
    public Transform VRguy_spawn;
    [Header("VengfulGirl")]
    public GameObject VengfulGirl;
    public Transform VengfulGirl_spawn;

    [Header("Input")]
    public PlayerInput playerInputComponent;
    Keyboard kb;

    [Header("Sounds")]
    public SoundManager soundManager;
    public Sound sound_RewindFinish;
    public Sound sound_Death;

    //public Sound sound_RewindFinish;

    // Start is called before the first frame update
    void Awake()
    {

        gameManager = GameObject.FindGameObjectWithTag("Manager");
        soundManager = gameManager.GetComponent<SoundManager>();


        characterPool.Add(DUUMguy);
        characterPool.Add(VRguy);
        characterPool.Add(VengfulGirl);
        spawnPool.Add(DUUMguy_spawn);
        spawnPool.Add(VRguy_spawn);
        spawnPool.Add(VengfulGirl_spawn);
        pickChracterVRguy();
        //pickCharacterDUUMguy();


    }


    // Update is called once per frame
    void Update()
    {
        kb = InputSystem.GetDevice<Keyboard>();
        if (kb.iKey.isPressed)
        {
            //currentCharacter.GetComponent<PlayerInputHandlerScript>().replayEvents();
            //Rewind();

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
        else if (kb.lKey.isPressed)
        {
            pickCharacterVG();
        }
        playerDeathRoutine();
    }


    public void Rewind()
    {
        pickCharacterDUUMguy();
        //pickCharacter(currentCharacter);
        for (int i = 0; i < spawnPool.Count && i < characterPool.Count; i++)
        {
            characterPool[i].transform.position = spawnPool[i].transform.position;
            characterPool[i].GetComponent<PlayerInputHandlerScript>().Rewind();

        }

    }

    public void pickChracterVRguy()
    {
        pickCharacter(VRguy);
        //DUUMguy.GetComponent<PlayerInputHandlerScript>().activeAI(true);
        //VRguy.GetComponent<PlayerInputHandlerScript>().activeAI(false);

    }

    public void pickCharacterDUUMguy()
    {
        pickCharacter(DUUMguy);
        //VRguy.GetComponent<PlayerInputHandlerScript>().activeAI(true);
        //DUUMguy.GetComponent<PlayerInputHandlerScript>().activeAI(false);


    }
    public void pickCharacterVG()
    {
        pickCharacter(VengfulGirl);
        //VRguy.GetComponent<PlayerInputHandlerScript>().activeAI(true);
        //DUUMguy.GetComponent<PlayerInputHandlerScript>().activeAI(false);


    }


    void pickCharacter(GameObject g)
    {
        currentCharacter = g;
        //Rewind();

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
        //Rewind();
    }


    public void setCNFocus(GameObject g)
    {
        Transform t = g.GetComponent<PlayerMovementScript>().midpoint.transform;
        cinemachine.LookAt = t;
        cinemachine.Follow = t;
    }





    //Input

    public void movePlayer(InputAction.CallbackContext context)
    {
        currentCharacter.GetComponent<PlayerInputHandlerScript>().movePlayer(context);
    }

    public void shoot(InputAction.CallbackContext context)
    {
        currentCharacter.GetComponent<PlayerInputHandlerScript>().shoot(context);


    }
    public void useWeapon(InputAction.CallbackContext context)
    {

        currentCharacter.GetComponent<PlayerInputHandlerScript>().useWeapon(context);

    }
    public void reload(InputAction.CallbackContext context)
    {
        currentCharacter.GetComponent<PlayerInputHandlerScript>().reload(context);

    }

    void playSound_RewindFinish()
    {
        try
        {
            soundManager.Play(sound_RewindFinish.name);

        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Failed to play");
        }
    }


    public bool isPlayerDead()
    {
        bool temp = currentCharacter.GetComponent<PlayerStates>().checkDie();
        if (playerIsDead != temp & !isRewinding)
        {
            if (temp)
            {
                spawnGrave();

            }
        }
        playerIsDead = temp;
        return playerIsDead;
    }

    void spawnGrave()
    {
        Instantiate(grave, currentCharacter.transform.position, Quaternion.identity);
    }

    IEnumerator startRewind()
    {
        isRewinding = true;
        print("Start Rewinding");
        gameManager.GetComponent<TimeManagerScript>().slowDown(rewindTimeScale, rewindTime);
        yield return new WaitForSeconds(rewindTime*rewindTimeScale);
        //Rewind();
        //gameManager.GetComponent<TimeManagerScript>().resetTime();
        gameManager.GetComponent<GameManagerScript>().Rewind();
        isRewinding = false;
        playSound_RewindFinish();

        print("Finish Rewinding");

    }

    void playerDeathRoutine()
    {
        if (isPlayerDead() &&!isRewinding)
        {
            playSound_Death();
            StartCoroutine(startRewind());

            //Rewind();

        }
    }
    void playSound_Death()
    {
        print("play death sound");

        soundManager.Play(sound_Death.name);

    }
}
