using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySystem : MonoBehaviour
{

    public GameObject SystemInteractable;

    public GameObject[] Parts;

    public bool Showing = false;

    private GameObject SystemModel_Instance;

    private List<bool> PartsShowing = new List<bool>();
    private List<GameObject> Parts_Instance = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        GameObject part_instance;
        SystemModel_Instance = Instantiate(SystemInteractable, transform);
        SystemModel_Instance.SetActive(Showing);
        for(int i = 0; i < Parts.Length; i++)
        {
            part_instance = Instantiate(Parts[i], transform);
            Parts_Instance.Add(part_instance);
            Parts_Instance[i].SetActive(false);
            PartsShowing.Add(false);
        }
    }

    public void TogglePart(int index)
    {
        // Disable System Before showing part
        if (Showing)
        {
            ToggleSystem();
        }
        // Disable All other parts before showing current part
        for (int i = 0; i < Parts.Length; i++)
        {
            if(i == index)
            {
                continue;
            }
            Parts_Instance[i].SetActive(false);
            PartsShowing[i] = false;
        }
        // Toggle Part
        if (index < Parts.Length)
        {
            PartsShowing[index] = !PartsShowing[index];
            Parts_Instance[index].SetActive(PartsShowing[index]);
        }
    }

    public void ToggleSystem()
    {
        DisableAllParts();
        Showing = !Showing;
        SystemModel_Instance.SetActive(Showing);

    }

    public void DisableAllParts()
    {
        for (int i = 0; i < Parts.Length; i++)
        {
            if (PartsShowing[i])
            {
                Parts_Instance[i].SetActive(false);
                PartsShowing[i] = false;
            }
        }
    }
}
