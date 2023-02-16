using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToWorldInteraction : MonoBehaviour
{
    public GameObject SelectedBuilding;

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
                }
                    print(hit.collider.name);
            }
        }
    }

    void SelectBuilding()
    {
        
    }
}
