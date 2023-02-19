using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuCamera;
    [SerializeField] private GameObject[] otherUi;
    
    
    void DisableOtherUi()
    {
        GameObject.FindObjectOfType<UIManager>().DisableUi("waves");
    }

    void EnableOtherUi()
    {
        GameObject.FindObjectOfType<UIManager>().EnableUi("waves");
        
        mainMenuCamera.active = false;
        
        foreach(GameObject ui in otherUi)
        {
            ui.SetActive(true);
        }
        
    }

    void DisableMainMenuUi()
    {
        GameObject.FindObjectOfType<UIManager>().DisableUi("MainMenu");

        foreach (GameObject ui in otherUi)
        {
            ui.SetActive(false);
        }

    }
}
