using UnityEngine;

public class UIButtons : MonoBehaviour
{
    [SerializeField] private Animator am;
    
    public void Play()
    {
        am.SetTrigger("Play");
    }

    public void exitgame()
    {
        Application.Quit();
    }
}
