using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialogueUI;
    public Text displayNameText;
    public Text bodyText;
    public Button exitButton;
    public Button backButton;
    public Button nextButton;

    // Controls
    [HideInInspector] public string exitKey = "tab";
    [HideInInspector] public string backKey = "q";
    [HideInInspector] public string nextKey = "r";
    string exitKeySecondary = "x";
    KeyCode backKeySecondary = KeyCode.LeftArrow;
    KeyCode nextKeySecondary = KeyCode.RightArrow;

    // Dialogue
    [HideInInspector] public List<string> dialogueLines;
    [HideInInspector] public string displayName;
    [HideInInspector] public int dialoguePosition;
    [HideInInspector] public string nextButtonFinalString;
    [HideInInspector] public bool inDialogue = false;

    // Disable during dialogue
    MouseLook playerMouseLook;
    PlayerMovement playerMovement;
    Image crosshair;


    void Awake()
    {
        exitButton.GetComponentInChildren<Text>().text = "Exit [" + exitKey + "]";
        backButton.GetComponentInChildren<Text>().text = "< Back [" + backKey + "]";
        nextButton.GetComponentInChildren<Text>().text = "Next [" + nextKey + "] >";

        exitButton.onClick.AddListener(delegate { ExitDialogue(); });
        backButton.onClick.AddListener(delegate { BackDialogue(); });
        nextButton.onClick.AddListener(delegate { NextDialogue(); });
        
        backButton.gameObject.SetActive(false);
        dialogueUI.SetActive(false);
    }

    void Start()
    {
        crosshair = GameObject.Find("Crosshair").GetComponent<Image>();
        playerMouseLook = Camera.main.GetComponent<MouseLook>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void Update()
    {
        TryButtonClick();
    }

    // UI button clicks on key press
    void TryButtonClick()
    {
        if (Input.GetKeyDown(exitKey) || Input.GetKeyDown(exitKeySecondary)) {
            exitButton.onClick.Invoke();
        } else if (Input.GetKeyDown(backKey) || Input.GetKeyDown(backKeySecondary)) {
            backButton.onClick.Invoke();
        } else if (Input.GetKeyDown(nextKey) || Input.GetKeyDown(nextKeySecondary)) {
            nextButton.onClick.Invoke();
        }
    }

    public void AddNewDialogue(string name, string[] lines, string final)
    {
        displayName = name;
        dialoguePosition = 0;
        dialogueLines = new List<string>(lines.Length);
        dialogueLines.AddRange(lines);

        // Change "Next" button text on last line of dialogue
        nextButtonFinalString = final;

        SetUIElements();
    }

    public void SetUIElements()
    {
        // Dialogue will only open when lines to display are provided
        if (dialogueLines.Count > 0) {
            displayNameText.text = displayName;
            bodyText.text = dialogueLines[dialoguePosition];
            UpdateUIButtons();
            StartDialogue();
        }
    }
    
    // Change UI elements based on position in dialogue
    public virtual void UpdateUIButtons()
    {
        // Back button visibility
        if (dialoguePosition == 0) {
            // Start of dialogue
            backButton.gameObject.SetActive(false);
        } else {
            backButton.gameObject.SetActive(true);
        }

        // Next button text
        if (dialoguePosition < dialogueLines.Count-1) {
            // Not at the end of dialogue
            // Default string and colour
            nextButton.GetComponentInChildren<Text>().text = "Next [" + nextKey + "] >";
            nextButton.GetComponentInChildren<Text>().color = new Color32(0, 180, 255, 255);
        } else {
            // Final string and lighter blue colour
            nextButton.GetComponentInChildren<Text>().text = nextButtonFinalString + " [" + nextKey + "] >";
            nextButton.GetComponentInChildren<Text>().color = new Color32(0, 240, 255, 255);
        }
    }

    void StartDialogue()
    {
        inDialogue = true;
        dialogueUI.SetActive(true);

        // Disable components during dialogue
        crosshair.enabled = false;
        playerMouseLook.enabled = false;
        playerMovement.enabled = false;
    }

    public virtual void ExitDialogue()
    {
        inDialogue = false;
        dialoguePosition = 0;
        dialogueUI.SetActive(false);

        // Re-enable components
        crosshair.enabled = true;
        playerMouseLook.enabled = true;
        playerMovement.enabled = true;
    }

    public virtual void NextDialogue()
    {
        if (inDialogue) {
            if (dialoguePosition == dialogueLines.Count-1) {
                // At the end of dialogue
                ExitDialogue();
            }
            else {
                dialoguePosition++;
                bodyText.text = dialogueLines[dialoguePosition];
            }
            UpdateUIButtons();
        }
    }

    void BackDialogue()
    {
        if (inDialogue) {
            if (dialoguePosition != 0) {
                // Not at the start of dialogue
                dialoguePosition--;
                bodyText.text = dialogueLines[dialoguePosition];
            }
            UpdateUIButtons();
        }
    }
}
