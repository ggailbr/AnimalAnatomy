using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AnatomyOrganizer : MonoBehaviour
{
    public GameObject SystemPanelPosition;
    public GameObject PartPanelPosition;

    public GameObject UIPrefab;
    public GameObject ButtonPrefab;


    private List<GameObject> Systems = new List<GameObject>();
    private List<GameObject> PartPanels = new List<GameObject>();
    private GameObject SystemPanelinstance;

    // Start is called before the first frame update
    void Start()
    {
        SystemPanelinstance = Instantiate(UIPrefab, SystemPanelPosition.transform);
        GameObject ButtonInstance;

        //bool one_showing = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            int j = i;
            Systems.Add(transform.GetChild(i).gameObject);
            /*
            if (Systems[i].GetComponent<BodySystem>().Showing)
            {
                if (one_showing)
                {
                    Systems[i].GetComponent<BodySystem>().ToggleSystem();
                }
                else
                {
                    one_showing = true;
                }
            }
            */
            // Create A button and rename it to the System Name
            ButtonInstance = Instantiate(ButtonPrefab, SystemPanelinstance.transform);
            ButtonInstance.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = Systems[i].name;
            // On click, toggle showing the system and show the correct part panel
            ButtonInstance.GetComponent<Button>().onClick.AddListener(Systems[j].GetComponent<BodySystem>().ToggleSystem);
            ButtonInstance.GetComponent<Button>().onClick.AddListener(delegate { ShowPartPanel(j); });
            ButtonInstance.GetComponent<Button>().onClick.AddListener(delegate { DisableOtherParts(j); });
            // Create a Part Panel for the System
            PartPanels.Add(Instantiate(UIPrefab, PartPanelPosition.transform));
            PartPanels[i].SetActive(false);
            // For each part in the system, make a button
            for (int idx = 0; idx < Systems[i].GetComponent<BodySystem>().Parts.Length; idx++)
            {
                int jdx = idx;
                // Create a button for each part
                ButtonInstance = Instantiate(ButtonPrefab, PartPanels[i].transform);
                ButtonInstance.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = Systems[i].GetComponent<BodySystem>().Parts[idx].name;
                // On press, toggle the part based on the system logic (should disable all other parts and system)
                ButtonInstance.GetComponent<Button>().onClick.AddListener(delegate{ Systems[j].GetComponent<BodySystem>().TogglePart(jdx); });
                // On press, Disable all other systems present for each part
                ButtonInstance.GetComponent<Button>().onClick.AddListener(delegate { DisableOtherSystems(j); });
            }
        }
    }

    void ShowPartPanel(int idx)
    {
        // Disable all other part panels
        for (int i = 0; i < transform.childCount; i++)
        {
            if (PartPanels[i].activeInHierarchy && i != idx)
            {
                PartPanels[i].SetActive(false);
            }
        }
        // Show the current part panel
        PartPanels[idx].SetActive(true);
    }

    void DisableOtherSystems(int idx)
    {
        // Disable each system that is showing except for the chosen one
        for (int i = 0; i < transform.childCount; i++)
        {
            if(i == idx)
            {
                continue;
            }
            if (Systems[i].GetComponent<BodySystem>().Showing)
            {
                Systems[i].GetComponent<BodySystem>().ToggleSystem();
            }
            else
            { 
                Systems[i].GetComponent<BodySystem>().DisableAllParts();
            }
        }
    }

    void DisableOtherParts(int idx)
    {
        // Disable each system that is showing except for the chosen one
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == idx)
            {
                continue;
            }
            Systems[i].GetComponent<BodySystem>().DisableAllParts();
        }
    }
}
