using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerResources : MonoBehaviour
{ 
    public int wood;  
    public int iron;

    public TMP_Text woodcount;
    public TMP_Text ironcount;

    private void Update()
    {
        woodcount.text = "Wood: " + wood.ToString();
        ironcount.text = "Iron: " + iron.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "wood")
        {
            wood += 1;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "iron")
        {
            iron += 1;
            Destroy(collision.gameObject);
        }
    }
} 
