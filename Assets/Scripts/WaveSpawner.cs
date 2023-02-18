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
    [Header("SpawnSettings")]
    public int maxspawndistance = 70;
    public int minspawndistance = 50;

    private GameObject tower;
    private GameObject player;
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
        player = GameObject.FindGameObjectWithTag("Player");

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

        int alreadyspawnedcount = 0;

        // spawn zombies
        while (alreadyspawnedcount < spawncount) 
        {
            Vector3 spawnPosVect = new Vector3(Random.Range(-maxspawndistance, maxspawndistance), 10f, Random.Range(-maxspawndistance, maxspawndistance));

            // check if the position is not to close to the tower
            if (Vector3.Distance(spawnPosVect, tower.transform.position) > minspawndistance)
            {
                // raycast to get the spawn height and if its a valid ground
                if (Physics.Raycast(spawnPosVect, Vector3.down, out RaycastHit hit))
                {
                    if (hit.collider.gameObject.tag == "ground")
                    {
                        // Now we know this is a save place to spawn the zombie
                        Vector3 actualspawnpos = new Vector3(spawnPosVect.x, hit.point.y + 0.5f, spawnPosVect.z);

                        GameObject zombie = Instantiate(zombies_prefabs[Random.Range(0, zombies_prefabs.Count())], actualspawnpos, transform.rotation);

                        WalkingZombie settings = zombie.GetComponent<WalkingZombie>();


                        // set wave based specs
                        settings.damage = Mathf.RoundToInt(settings.damage * zombie_damage_mp);

                        settings.hp = Mathf.RoundToInt(settings.hp * zombie_health_mp);
                        settings.maxhp = Mathf.RoundToInt(settings.maxhp * zombie_health_mp);

                        settings.tower = tower;
                        settings.player = player;

                        // we succesfully spawned an zombie
                        alreadyspawnedcount++;
                    }
                }
            }
        }
        spawncount = Mathf.RoundToInt(spawncount * zombie_count_mp);
        zombie_health_mp = zombie_health_mp * zombie_health_mp;
        zombie_damage_mp = zombie_damage_mp * zombie_health_mp;

        Debug.Log("Wave: "+ wave.ToString() + " STARTED");
    }

}
