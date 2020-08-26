using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIHandlerScript : MonoBehaviour
{
    public PlayerSpawnPointScript playerSpawn;
    [SerializeField] GameObject currentChaaracter;
    public GameObject characterPicker;

    [Header("Crosshair")]
    public Transform Crosshair;
    public Camera cam;

    [Header("Health")]
    public PlayerStates playerStates;
    public TextMeshProUGUI healthTextBox;
    [Header("Ammo")]
    public WeaponTypeScript weaponTypeScript;
    public TextMeshProUGUI ammoTextBox;
    public Animator ammoIconAnimator;
    [Header("Secondary")]
    public Image image;
    public TextMeshProUGUI secondaryCooldown;
    public UseSecondaryScript useSecondaryScript;
    public Sprite emptySprite;
    [Header("Current Time")]
    public TimeManagerScript timeManagerScript;
    public TextMeshProUGUI currentTimeTextBox;
    //public float currentTime;
    [Header("Logger")]
    public bool isLogger;
    public bool updateLogger = true;
    public PlayerInputHandlerScript playerInput;
    public TextMeshProUGUI eventTextBox;
    [Header("Rewind Effect")]
    public GameObject rewindEffect;
    public float rewindEffectDuration = .5f;
    [Header("Sounds")]
    public SoundManager soundManager;
    public Sound sound_Rewinding;


    // Start is called before the first frame update
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        timeManagerScript = FindObjectOfType<TimeManagerScript>();
        if (playerSpawn != null)
        {
            updateCurrentCharacter();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        updateCurrentCharacter();
        if (currentChaaracter != null)
        {

            updateCrosshair();

            displayHealth();
            displayAmmo();
            displayGrab();
            displaySecondaryCooldown();
            updateAmmoIcon();
            if (isLogger)
            {

                displayLog();
            }
            updateCurrentTime();
        }
        else
        {
            if (!characterPicker.activeSelf)
            {
                if (!Cursor.visible)
                {
                    Cursor.visible = true;
                }
                if (Crosshair.gameObject.activeSelf)
                {
                    Crosshair.gameObject.SetActive(false);

                }
                characterPicker.SetActive(true);
            }
            else
            {
                //characterPicker.SetActive(false);

            }
        }
    }

    void displayHealth()
    {
        healthTextBox.text = playerStates.getHeathUI();
    }

    void displayAmmo()
    {
        ammoTextBox.text = weaponTypeScript.currentMag + "/" + weaponTypeScript.Ammo + "\n";

    }

    void displayGrab()
    {
        try
        {
            if (useSecondaryScript.storedObject != null)
            {
                image.sprite = useSecondaryScript.storedObject.GetComponentInChildren<SpriteRenderer>().sprite;
            }

        }
        catch (System.Exception e)
        {
            print("Error");
        }
    }

    void displaySecondaryCooldown()
    {
        if (useSecondaryScript.timeNow_useCooldown > 0.1f)
        {
            if (!secondaryCooldown.gameObject.activeSelf)
            {
                secondaryCooldown.gameObject.SetActive(true);

            }
            secondaryCooldown.text = Mathf.Round(useSecondaryScript.timeNow_useCooldown).ToString();
        } else
        {
            if (secondaryCooldown.gameObject.activeSelf)
            {
                secondaryCooldown.gameObject.SetActive(false);
                if (useSecondaryScript.extraGameObject != null)
                {
                    image.sprite = useSecondaryScript.extraGameObject.GetComponentInChildren<SpriteRenderer>().sprite;
                }
                else
                {
                    image.sprite = emptySprite;
                }

            }
        }
    }

    void displayLog()
    {
        string text = "";
        foreach (EventType e in playerInput.savedEvents)
        {
            text += e.ToString();
        }
        eventTextBox.text = text;
    }

    public void updateCurrentCharacter()
    {
        if (playerSpawn.currentCharacter == null)
        {
            currentChaaracter = null;
            return;
        }

        else
        {
            currentChaaracter = playerSpawn.currentCharacter;
            playerStates = currentChaaracter.GetComponent<PlayerStates>();
            PlayerInputHandlerScript pi = currentChaaracter.GetComponent<PlayerInputHandlerScript>();
            weaponTypeScript = pi.weaponTypeScript;
            setReloadAnimationSpeed();
            useSecondaryScript = pi.useSecondaryScript;
            if (updateLogger)
            {
                playerInput = pi;
            }


        }
    }
    public void pickChracterVRguy()
    {
        characterPicker.SetActive(false);
        StartCoroutine(playRewindEffect());
        playerSpawn.pickChracterVRguy();


    }

    public void pickCharacterDUUMguy()
    {
        characterPicker.SetActive(false);
        StartCoroutine(playRewindEffect());
        playerSpawn.pickCharacterDUUMguy();


    }
    public void pickCharacterDN()
    {
        characterPicker.SetActive(false);
        StartCoroutine(playRewindEffect());
        playerSpawn.pickCharacterDN();

    }
    public void pickCharacterSkullguy()
    {
        characterPicker.SetActive(false);
        StartCoroutine(playRewindEffect());
        playerSpawn.pickCharacterSkullguy();

    }

    IEnumerator playRewindEffect()
    {
        rewindEffect.SetActive(true);
        //playSound_Rewinding();
        Material effect = rewindEffect.GetComponentInChildren<RawImage>().material;
        effect.SetFloat("_TimeNow", Time.time);
        yield return new WaitForSeconds(rewindEffectDuration);
        rewindEffect.SetActive(false);
        //effect.SetFloat("_Speed", 1f);
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

    void updateCrosshair()
    {
        if (Cursor.visible)
        {
            Cursor.visible = false;
        }
        if (!Crosshair.gameObject.activeSelf)
        {
            Crosshair.gameObject.SetActive(true);

        }

        Crosshair.position = Mouse.current.position.ReadValue();
    }

    void setReloadAnimationSpeed()
    {
        ammoIconAnimator.speed = 1 / weaponTypeScript.reloadTime;
    }

    void updateAmmoIcon()
    {
        ammoIconAnimator.SetBool("isReloading", weaponTypeScript.isReloading);
    }
    void updateCurrentTime()
    {
        currentTimeTextBox.text = timeManagerScript.getCurrentTimeDisplay();
    }

}
