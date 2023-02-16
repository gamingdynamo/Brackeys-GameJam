using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool hasplayerdied;
    public bool istowerdestroyed;

    public GameObject DeathScreen;
    public GameObject UpgradeGrid;
    public GameObject UpgradeContainer;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hasplayerdied || istowerdestroyed)
        {
            // show died screen with stats?
            DeathScreen.SetActive(true);
        }
    }
}
