using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject playerHealth;
    [SerializeField] private GameObject towerHealth;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private GameObject waveCounter;
    [SerializeField] private GameObject countDown;
    
    public void EnableUi(string name)
    {
        if (name.Equals("PlayerHealth"))
        {
            playerHealth.active = true;
        }
        else if (name.Equals("TowerHealth"))
        {
            towerHealth.active = true;
        }
        else if (name.Equals("DeathScreen"))
        {
            deathScreen.active = true;
        }
        else if (name.Equals("MainMenu"))
        {
            mainMenu.active = true;
        }
        else if (name.Equals("UpgradeMenu"))
        {
            upgradeMenu.active = true;
        }
        else if (name.Equals("waves"))
        {
            waveCounter.active = true;
            countDown.active = true;
        }
    }

    public void DisableUi(string name)
    {
        if (name.Equals("PlayerHealth"))
        {
            playerHealth.active = false;
        }
        else if (name.Equals("TowerHealth"))
        {
            towerHealth.active = false;
        }
        else if (name.Equals("DeathScreen"))
        {
            deathScreen.active = false;
        }
        else if (name.Equals("MainMenu"))
        {
            mainMenu.active = false;
        }
        else if (name.Equals("UpgradeMenu"))
        {
            upgradeMenu.active = false;
        }
        else if (name.Equals("waves"))
        {
            waveCounter.active = false;
            countDown.active = false;
        }
    }
}
