using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToWorldInteraction : MonoBehaviour
{
    public GameObject SelectedBuilding;
    public GameManager gamemanager;

    public Camera cam;

    Plane plane = new Plane(Vector3.up, 0);
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.GetComponent<IInteractable>() != null)
                {
                    if(SelectedBuilding != hit.collider.gameObject)
                    {
                        SelectedBuilding.GetComponent<IInteractable>().deactivateui();
                    }
                    hit.collider.GetComponent<IInteractable>().setuiactive();
                    SelectedBuilding = hit.collider.gameObject;
                }
                print(hit.collider.name);
            }


        }
        if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Tab))
        {
            if(SelectedBuilding!= null)
            {
                SelectedBuilding.GetComponent<IInteractable>().deactivateui();
                SelectedBuilding= null;
            }
        }
    }
}
