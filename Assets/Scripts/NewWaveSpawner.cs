using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Profiling;
using System.Linq;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class NewWaveSpawner : MonoBehaviour
{
    private GameObject tower;
    // should become a resource load instead of placing in array
    public GameObject[] zombies_prefabs;

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

    [SerializeField] private Transform spawnPos;

    private Vector3 spawnPosVect;

    void Start()
    {
        tower = GameObject.FindGameObjectWithTag("tower");
        
        wavecountdown = 10;
        nextroundstarting = true;
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
            
            spawnPosVect = new Vector3(Random.Range(-70f, 70f), 10f, Random.Range(100f, 20f));

            GameObject zombie = Instantiate(zombies_prefabs[Random.Range(0, zombies_prefabs.Count())],spawnPosVect, transform.rotation );
            
            Debug.Log(spawnPosVect.ToString());

            // set wave based specs
            zombie.GetComponent<Zombie>().damage = Mathf.RoundToInt(zombie.GetComponent<Zombie>().damage * zombie_damage_mp);

            zombie.GetComponent<Zombie>().hp = Mathf.RoundToInt(zombie.GetComponent<Zombie>().hp * zombie_health_mp);
            zombie.GetComponent<Zombie>().maxhp = Mathf.RoundToInt(zombie.GetComponent<Zombie>().maxhp * zombie_health_mp);

            zombie.GetComponent<Zombie>().tower = tower;

            if (Physics.Raycast(zombie.transform.position, zombie.transform.TransformDirection(Vector3.down),
                    out RaycastHit hit, 20f))
            {
                if (hit.collider.gameObject.tag.Equals("ground"))
                {
                    zombie.gameObject.transform.position = new Vector3(zombie.transform.position.x,
                        hit.point.y, zombie.gameObject.transform.position.z);
                }
            }
            //zombie.transform.position = spawnpos.position;
        }
        spawncount = Mathf.RoundToInt(spawncount * zombie_count_mp);

        Debug.Log("Wave: "+ wave.ToString() + " STARTED");
    }

}
