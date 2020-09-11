using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AICurrentActivityDisplayScript : MonoBehaviour
{

    public PlayerInputHandlerScript playerInputHandlerScript;
    public Image characterImage;
    public TextMeshProUGUI currentActivity;
    // Start is called before the first frame update
    void Start()
    {
        characterImage.sprite = playerInputHandlerScript.getCharacterSprite();

    }

    // Update is called once per frame
    void Update()
    {
        currentActivity.text = playerInputHandlerScript.ToString();
    }

}
