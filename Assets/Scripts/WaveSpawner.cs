using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Profiling;
using System.Linq;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class WaveSpawner : MonoBehaviour
{
    private GameObject tower;
    // should become a resource load instead of placing in array
    public GameObject[] zombies_prefabs;

    public GameObject spawnpos_parent;
    public List<Transform> spawnpositions;

    public int wave;
    public float wavecountdown;

    public bool nextroundstarting;
    //UI
    [Header("UI references")]
    public TMP_Text countdownfield;
    public TMP_Text wavefield;

    // wave settings
    [Header("Current Wave Settings")]
    [SerializeField] private int spawncount;
    [SerializeField] private float zombie_damage_mp;
    [SerializeField] private float zombie_health_mp;
    [SerializeField] private float zombie_count_mp;

    void Start()
    {
        tower = GameObject.FindGameObjectWithTag("tower");

        // start preperation
        wavecountdown = 10;
        nextroundstarting = true;
        // get the spawnpos
        foreach (Transform pos in spawnpos_parent.GetComponentsInChildren<Transform>())
        {
            //spawnpositions.Append(pos.transform);
            //Debug.Log(pos.gameObject.transform);
        }
    }

    void Update()
    {
        // subtract the countdown if needed
        if (wavecountdown > 0) 
        { 
            wavecountdown -= Time.deltaTime;
            if(wavecountdown <= 0 ) 
            {
                countdownfield.text = "";
                wavecountdown = 0;

                // start wave
                NewWave();
                nextroundstarting = false;
            }
            else
            {
                countdownfield.text = Mathf.RoundToInt(wavecountdown).ToString();
            }
        }
        
        // if all zombies are dead set new countdown so the next round starts
        if(GameObject.FindGameObjectsWithTag("zombie").Length == 0 && !nextroundstarting) { wavecountdown = 20; nextroundstarting = true; }
    }

    void NewWave()
    {
        // set wave in UI
        wave += 1;
        wavefield.text = "Wave: " + wave.ToString();

        // spawn zombies
        for (int i = 0;i < spawncount; i++) 
        {
            // get one of the random spawn positions
            // add some kind of delay between spawn?
            Transform spawnpos = spawnpositions[Random.Range(0, spawnpositions.Count())];
            GameObject zombie = Instantiate(zombies_prefabs[Random.Range(0, zombies_prefabs.Count())], spawnpos);

            // set wave based specs
            zombie.GetComponent<Zombie>().damage = Mathf.RoundToInt(zombie.GetComponent<Zombie>().damage * zombie_damage_mp);

            zombie.GetComponent<Zombie>().hp = Mathf.RoundToInt(zombie.GetComponent<Zombie>().hp * zombie_health_mp);
            zombie.GetComponent<Zombie>().maxhp = Mathf.RoundToInt(zombie.GetComponent<Zombie>().maxhp * zombie_health_mp);

            zombie.GetComponent<Zombie>().tower = tower;
            //zombie.transform.position = spawnpos.position;
        }
        spawncount = Mathf.RoundToInt(spawncount * zombie_count_mp);

        Debug.Log("Wave: "+ wave.ToString() + " STARTED");
    }

}
