using UnityEngine;

public class SpawnLightManager : MonoBehaviour
{
    [SerializeField] private GameObject lightManager;

    private GameObject lightManagerClone;
    private void Awake()
    {
        lightManagerClone = Instantiate(lightManager);

        lightManagerClone.GetComponent<LightingManager>().directionalLight = GameObject.FindObjectOfType<Light>();
    }
}
