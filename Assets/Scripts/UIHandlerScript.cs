using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandlerScript : MonoBehaviour
{
    public PlayerSpawnPointScript playerSpawn;
    [SerializeField] GameObject currentChaaracter;

    [Header("Health")]
    public PlayerStates playerStates;
    public TextMeshProUGUI healthTextBox;
    [Header("Ammo")]
    public WeaponTypeScript weaponTypeScript;
    public TextMeshProUGUI ammoTextBox;
    [Header("Stored Object")]
    public Image image;
    public UseSecondaryScript useSecondaryScript;
    public Sprite emptySprite;
    [Header("Logger")]
    public bool isLogger;
    public PlayerInputHandlerScript playerInput;
    public TextMeshProUGUI eventTextBox;


    // Start is called before the first frame update
    void Start()
    {
        if (playerSpawn != null)
        {
            updateCurrentCharacter();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        displayHealth();
        displayAmmo();
        displayGrab();
        if (isLogger)
        {

        displayLog();
        }
    }

    void displayHealth()
    {
        healthTextBox.text = playerStates.getHeathUI();
    }

    void displayAmmo()
    {
        ammoTextBox.text = weaponTypeScript.currentMag + "/" + weaponTypeScript.Ammo+"\n";

    }

    void displayGrab()
    {
        try
        {
            if (useSecondaryScript.storedObject != null)
            {
                image.sprite = useSecondaryScript.storedObject.GetComponentInChildren<SpriteRenderer>().sprite;
            }
            else
            {
                image.sprite = emptySprite;
            }
        }
        catch (System.Exception e)
        {
            print("Error");
        }
    }

    void displayLog()
    {
        string text = "";
        foreach(EventType e in playerInput.savedEvents)
        {
            text += e.ToString();
        }
        eventTextBox.text = text;
    }

    public void updateCurrentCharacter()
    {
        if (!currentChaaracter.Equals(playerSpawn.currentCharacter))
        {
            currentChaaracter = playerSpawn.currentCharacter;
            playerStates = currentChaaracter.GetComponent<PlayerStates>();
            playerInput = currentChaaracter.GetComponent<PlayerInputHandlerScript>();
            weaponTypeScript = playerInput.weaponTypeScript;
            useSecondaryScript = playerInput.useSecondaryScript;


        }
    }



}
