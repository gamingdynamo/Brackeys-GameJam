using System;
using System.Collections;
using UnityEditor.Presets;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light directionalLight;
    [SerializeField] private LightingPreset preset;
    [SerializeField, Range(0,24)] private float timeOfDay;

    public float daystart_time;
    public float nightstart_time;

    public float suntarget;

    private WaveSpawner waveSpawner;
    private int previouswave;

    private void Start()
    {
        waveSpawner = FindObjectOfType<WaveSpawner>();
        StartCoroutine(slowupdate());
        suntarget = daystart_time + 3;
    }

    // once per second tick
    IEnumerator slowupdate()
    {
        yield return new WaitForSeconds(1);

        if (previouswave != waveSpawner.wave) 
        {
            // new wave/ night started
            suntarget = nightstart_time + 3 ;
            previouswave = waveSpawner.wave ;
            // add delay so zombies can spawn
            yield return new WaitForSeconds(2);
        }
        else
        {
            int totalspawnedzombies = waveSpawner.spawncount;
            int totalalivezombies = GameObject.FindGameObjectsWithTag("zombie").Length;

            float total_nightime = 24 - (nightstart_time + 3) + daystart_time;
            if (totalalivezombies == 0)
            {
                // all zombies killed enter daytime
                suntarget = daystart_time + 3 ;
            }

            
        }
        StartCoroutine(slowupdate());
    }

    private void FixedUpdate()
    {
        if (preset == null)
            return;

        // we have to force the game to switch to one
        if ( timeOfDay > 23.5) { timeOfDay = 0.5f; suntarget = 3; }
       
        if (Application.isPlaying)
        {
            //timeOfDay += Time.deltaTime;
            timeOfDay = Mathf.Lerp(timeOfDay, suntarget, 0.01f);

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
