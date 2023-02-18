using System;
using UnityEditor.Presets;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light directionalLight;
    [SerializeField] private LightingPreset preset;
    [SerializeField, Range(0,24)] private float timeOfDay;

    private void Update()
    {
        if (preset == null)
            return;

        if (Application.isPlaying)
        {
            timeOfDay += Time.deltaTime;
            timeOfDay %= 24;
            UpdateLighting(timeOfDay / 24f);
        }
        else
        {
            UpdateLighting(timeOfDay / 24f);
        }
    }

    private void UpdateLighting(float timePrecent)
    {
        RenderSettings.ambientLight = preset.ambientColor.Evaluate(timePrecent);
        RenderSettings.fogColor = preset.fogColor.Evaluate(timePrecent);

        if (directionalLight != null)
        {
            directionalLight.color = preset.directionalColor.Evaluate(timePrecent);
            directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePrecent * 360f) - 90f, 170, 0));
        }
    }

    private void OnValidate()
    {
        if (directionalLight != null)
            return;

        if (RenderSettings.sun != null)
        {
            directionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();

            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    directionalLight = light;
                    return;
                }
            }
        }
    }
}
