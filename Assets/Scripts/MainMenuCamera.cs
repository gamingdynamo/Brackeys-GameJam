using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCamera;
    [SerializeField] private GameObject player;
    
    void DisableOtherUi()
    {
        GameObject.FindObjectOfType<UIManager>().DisableUi("waves");
    }

    void EnableOtherUi()
    {
        GameObject.FindObjectOfType<UIManager>().EnableUi("waves");
        
        mainMenuCamera.active = false;
        player.active = true;
        
    }

    void DisableMainMenuUi()
    {
        GameObject.FindObjectOfType<UIManager>().DisableUi("MainMenu");
    }
}
