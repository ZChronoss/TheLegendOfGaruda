using System.Collections.Generic;
using UnityEngine;

public class RestButtonController : MonoBehaviour
{
    public GameObject restButton;
    [SerializeField] private List<BonfireController> bonfires;

    void Start()
    {
        // Automatically find all bonfires in the scene if not assigned manually
        if (bonfires == null || bonfires.Count == 0)
        {
            BonfireController[] foundBonfires = Object.FindObjectsByType<BonfireController>(FindObjectsSortMode.None);
            bonfires = new List<BonfireController>(foundBonfires);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if any bonfire allows resting
        bool canRestAtAnyBonfire = false;

        foreach (BonfireController bonfire in bonfires)
        {
            if (bonfire.canRest)
            {
                canRestAtAnyBonfire = true;
                break;
            }
        }

        // Enable or disable the rest button based on the status of all bonfires
        restButton.SetActive(canRestAtAnyBonfire);
    }
}
