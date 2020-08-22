using Cinemachine;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSpawnPointScript : MonoBehaviour
{
    //public InputMaster controls;

    public GameObject gameManager;
    public GameObject currentCharacter;
    public List<GameObject> characterPool = new List<GameObject>();
    public List<Transform> spawnPool = new List<Transform>();
    public GameObject grave;
    public bool playerIsDead;
    public float rewindTime;
    public float rewindTimeScale;
    [SerializeField] bool isRewinding = true;
    [Header("Rewind Effect")]
    public GameObject rewindEffect;
    public float rewindEffectDuration = 1f;
    [Header("Camera")]
    public CinemachineVirtualCamera cinemachine;
    [Header("DUUMguy")]
    public GameObject DUUMguy;
    public Transform DUUMguy_spawn;
    [Header("VRguy")]
    public GameObject VRguy;
    public Transform VRguy_spawn;
    [Header("DomNuk")]
    public GameObject DomNuk;
    public Transform DomNuk_spawn;
    [Header("Skullguy")]
    public GameObject Skullguy;
    public Transform Skullguy_spawn;
    [Header("Input")]
    public PlayerInput playerInputComponent;
    Keyboard kb;

    [Header("Sounds")]
    public SoundManager soundManager;
    public Sound sound_Rewinding;
    public Sound sound_RewindFinish;
    public Sound sound_Death;
    public Sound sound_Theme;


    //public Sound sound_RewindFinish;

    // Start is called before the first frame update
    void Awake()
    {
        //controls = new InputMaster();
        gameManager = GameObject.FindGameObjectWithTag("Manager");
        soundManager = gameManager.GetComponent<SoundManager>();


        characterPool.Add(DUUMguy);
        characterPool.Add(VRguy);
        characterPool.Add(DomNuk);
        characterPool.Add(Skullguy);
        spawnPool.Add(DUUMguy_spawn);
        spawnPool.Add(VRguy_spawn);
        spawnPool.Add(DomNuk_spawn);
        spawnPool.Add(Skullguy_spawn);

        //pickChracterVRguy();
        //pickCharacterDUUMguy();
        for (int i = 0; i < spawnPool.Count && i < characterPool.Count; i++)
        {
            characterPool[i].transform.position = spawnPool[i].transform.position;
            characterPool[i].GetComponent<PlayerInputHandlerScript>().activeAI(true);
            characterPool[i].GetComponent<PlayerInputHandlerScript>().Rewind();

        }

    }


    // Update is called once per frame
    void Update()
    {

        kb = InputSystem.GetDevice<Keyboard>();
        if (kb.iKey.isPressed)
        {
            //currentCharacter.GetComponent<PlayerInputHandlerScript>().replayEvents();
            //Rewind();
            //startRewind();
            if (currentCharacter != null)
            {
                currentCharacter.GetComponent<PlayerStates>().takeDamage(100);
                playerDeathRoutine();
            }

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
            pickCharacterDN();
        }


        //Player Death Rewind Routine

        if (currentCharacter != null)
        {
            playerDeathRoutine();

        }
        if (currentCharacter != null && isRewinding)
        {
            finishRewind();
        }
        if (isRewinding)
        {
            gameManager.GetComponent<TimeManagerScript>().setStartTime();

        }
    }


    public void Rewind()
    {

        for (int i = 0; i < spawnPool.Count && i < characterPool.Count; i++)
        {
            characterPool[i].transform.position = spawnPool[i].transform.position;
            characterPool[i].GetComponent<PlayerInputHandlerScript>().Rewind();

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
    public void pickCharacterDN()
    {
        pickCharacter(DomNuk);
    }

    public void pickCharacterSkullguy()
    {
        pickCharacter(Skullguy);
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
        if (currentCharacter == null)
        {
            return;
        }
        currentCharacter.GetComponent<PlayerInputHandlerScript>().movePlayer(context);
    }

    public void shoot(InputAction.CallbackContext context)
    {
        if (currentCharacter == null)
        {
            return;
        }
        currentCharacter.GetComponent<PlayerInputHandlerScript>().shoot(context);


    }
    public void useWeapon(InputAction.CallbackContext context)
    {
        if (currentCharacter == null)
        {
            return;
        }

        currentCharacter.GetComponent<PlayerInputHandlerScript>().useWeapon(context);

    }
    public void reload(InputAction.CallbackContext context)
    {
        if (currentCharacter == null)
        {
            return;
        }
        currentCharacter.GetComponent<PlayerInputHandlerScript>().reload(context);

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
        //soundManager.stopAllSound();

        Instantiate(grave, currentCharacter.transform.position, Quaternion.identity);
    }

    IEnumerator startRewind()
    {
        print("Start Rewinding");
        gameManager.GetComponent<TimeManagerScript>().slowDown(rewindTimeScale, rewindTime);
        yield return new WaitForSeconds(rewindTime * rewindTimeScale);
        soundManager.stopAllSound();
        playSound_Theme();
        isRewinding = true;
        print("Finish Slowmotion");

    }
    void finishRewind()
    {
        print("finish rewind");
        pickCharacter(currentCharacter);
        //playRewindEffect();
        soundManager.stopAllSound();

        gameManager.GetComponent<GameManagerScript>().Rewind();
        isRewinding = false;
        gameManager.GetComponent<TimeManagerScript>().setStartTime();
        playSound_RewindFinish();
        playSound_Theme();
    }

    void playSound_Theme()
    {
        if (currentCharacter == null)
        {
            soundManager.Play(sound_Theme.name);
        }
        else
        {
            soundManager.Stop(sound_Theme.name);
            soundManager.Play(currentCharacter.GetComponent<PlayerStates>().sound_Theme.name);
        }
    }

    IEnumerator playRewindEffect()
    {
        rewindEffect.SetActive(true);
        playSound_Rewinding();
        Material effect = rewindEffect.GetComponentInChildren<RawImage>().material;
        effect.SetFloat("_TimeNow", Time.time);
        yield return new WaitForSeconds(rewindEffectDuration);
        rewindEffect.SetActive(false);
        //effect.SetFloat("_Speed", 1f);
    }

    void playerDeathRoutine()
    {
        if (isPlayerDead() && !isRewinding)
        {
            //playSound_Death();
            currentCharacter = null;

            StartCoroutine(startRewind());

            //Rewind();

        }
    }


    //Sounds
    void playSound_Death()
    {
        print("play death sound");

        soundManager.Play(sound_Death.name);

    }


    void playSound_Rewinding()
    {
        try
        {
            soundManager.Play(sound_Rewinding.name);

        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Failed to play");
        }
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
    /*
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    */


}
