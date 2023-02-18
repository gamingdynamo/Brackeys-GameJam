using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToWorldInteraction : MonoBehaviour
{
    public GameObject SelectedBuilding;
    public GameManager gamemanager;
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider.GetComponent<IInteractable>() != null) 
                {
                    hit.collider.GetComponent<IInteractable>().returnupgradables();
                    // enable ui upgrades gameobject
                    gamemanager.UpgradeGrid.SetActive(true);

                }
                    print(hit.collider.name);
            }
        }
        if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Tab))
        {
            gamemanager.UpgradeGrid.SetActive(false);
        }
    }

    void SelectBuilding()
    {
        
    }
}
