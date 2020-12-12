using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;


public class RepairManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject repairUI;
    public Text displayNameText;
    public Text bodyText;
    public Button exitButton;
    public Button repairButton;

    // Controls
    [HideInInspector] public string exitKey = "tab";
    [HideInInspector] public string repairKey = "r";
    string exitKeySecondary = "x";
    KeyCode repairKeySecondary = KeyCode.RightArrow;

    // Repair
    GameObject repairableObject;
    bool inRepair = false;

    // Disable during repair
    FirstPersonController playerController;
    Image crosshair;

    
    void Awake()
    {
        exitButton.GetComponentInChildren<Text>().text = "Exit [" + exitKey + "]";
        repairButton.GetComponentInChildren<Text>().text = "Repair [" + repairKey + "] >";

        exitButton.onClick.AddListener(delegate { ExitRepair(); });
        repairButton.onClick.AddListener(delegate { Proceed(); });
        
        repairUI.SetActive(false);
    }

    void Start()
    {
        crosshair = GameObject.Find("Crosshair").GetComponent<Image>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
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
        } else if (Input.GetKeyDown(repairKey) || Input.GetKeyDown(repairKeySecondary)) {
            repairButton.onClick.Invoke();
        }
    }

    public void AddNewRepair(string name, string body, GameObject obj)
    {
        displayNameText.text = name;
        bodyText.text = body;
        repairableObject = obj;

        if (repairableObject.GetComponent<Repairable>().PlayerCanRepair()) {
            repairButton.gameObject.SetActive(true);
        } else {
            repairButton.gameObject.SetActive(false);
        }

        StartRepair();
    }

    void StartRepair()
    {
        inRepair = true;
        repairUI.SetActive(true);

        // Disable components during repair
        crosshair.enabled = false;
        playerController.enabled = false;
    }

    void ExitRepair()
    {
        inRepair = false;
        repairUI.SetActive(false);

        // Re-enable components
        crosshair.enabled = true;
        playerController.enabled = true;
    }

    void Proceed()
    {
        if (inRepair) {
            if (repairableObject.GetComponent<Repairable>().PlayerCanRepair()) {
                ExitRepair();
                repairableObject.GetComponent<Repairable>().TryRepair();
            } else {
                // Player pressed the repairKey without meeting requirements
                ExitRepair();
            }
        }
    }
}
