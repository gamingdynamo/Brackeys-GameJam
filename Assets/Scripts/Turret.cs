using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class Turret : MonoBehaviour, IInteractable
{
    public GameObject head;
    public GameObject barrel_end;

    public GameObject projectile;

    public GameObject current_target;

    private float next_shot_time;

    private AudioSource audio;

    [Header("Damage")]
    public int cannondamage;
    public int cannondamagelevel;
    private int damage_upgr_cost_wood = 2;
    private int damage_upgr_cost_iron = 1;
    public TMP_Text damage;

    [Header("Interval")]
    public float cannonshotinterval;
    public float cannonshotintervallevel;
    private int interval_upgr_cost_wood = 2;
    private int interval_upgr_cost_iron = 1;
    public TMP_Text interval;

    [Header("Range")]
    public float cannonrange;
    public float cannonrangelevel;
    private int range_upgr_cost_wood = 2;
    private int range_upgr_cost_iron = 1;
    public TMP_Text canrange;

    private PlayerResources playerResources;

    public GameObject RelatedUpgradeUI;
    private void Start()
    {
        audio = GetComponent<AudioSource>();
        playerResources = FindObjectOfType<PlayerResources>();
    }

    // Update is called once per frame
    void Update()
    {
        // set new target if old target dissapeared
        if (current_target == null)
        {
            foreach (GameObject zomb in GameObject.FindGameObjectsWithTag("zombie"))
            {
                try
                {
                    GameObject zombie_target_point = zomb.transform.Find("AttackHeight").gameObject;

                    float dist = Vector3.Distance(gameObject.transform.position, zomb.transform.position);
                    if (dist < cannonrange)
                    {
                        current_target = zombie_target_point;
                        break;
                    }
                }
                catch { }
            }
        }
        else
        {
            // look at target because we have a target
            head.transform.LookAt(current_target.transform.position);

            // shoot at target with delay
            if (Time.time > next_shot_time)
            {
                GameObject project = Instantiate(projectile, barrel_end.transform.position, head.transform.rotation);
                project.GetComponent<Projectile>().friendly_projectile = true;
                project.GetComponent<Projectile>().damage = cannondamage;
                project.GetComponent<Rigidbody>().AddForce(head.transform.forward * 1000f);
                next_shot_time = Time.time + cannonshotinterval;
                audio.Play();
            }
        }
        // ui update
        damage.text = "level: " + cannondamagelevel.ToString() + "<br>" + "wood: " + damage_upgr_cost_wood.ToString() + " iron: " + damage_upgr_cost_iron.ToString();
        interval.text = "level: " + cannonshotintervallevel.ToString() + "<br>" + "wood: " + interval_upgr_cost_wood.ToString() + " iron: " + interval_upgr_cost_iron.ToString();
        canrange.text = "level: " + cannonrangelevel.ToString() + "<br>" + "wood: " + range_upgr_cost_wood.ToString() + " iron: " + range_upgr_cost_iron.ToString();

    }

    public void increasespeed()
    {
        if (playerResources.wood - interval_upgr_cost_wood >= 0 && playerResources.iron - interval_upgr_cost_iron >= 0)
        {
            playerResources.wood -= interval_upgr_cost_wood;
            playerResources.iron -= interval_upgr_cost_iron;

            cannonshotinterval = cannonshotinterval - 0.05f;
            interval_upgr_cost_wood += 1;
            interval_upgr_cost_iron += 1;
            cannonshotintervallevel += 1;
        }
    }

    public void increasedmg()
    {
        if (playerResources.wood - damage_upgr_cost_wood >= 0 && playerResources.iron - damage_upgr_cost_iron >= 0)
        {
            playerResources.wood -= damage_upgr_cost_wood;
            playerResources.iron -= damage_upgr_cost_iron;

            cannondamage = Mathf.RoundToInt(cannondamage * 1.1f);
            damage_upgr_cost_wood += 1;
            damage_upgr_cost_iron += 1;
            cannondamagelevel += 1;
        }
    }

    public void increaserange()
    {
        if (playerResources.wood - range_upgr_cost_wood >= 0 && playerResources.iron - range_upgr_cost_iron >= 0)
        {
            playerResources.wood -= range_upgr_cost_wood;
            playerResources.iron -= range_upgr_cost_iron;

            cannonrange = cannonrange + 0.5f;
            range_upgr_cost_wood += 1;
            range_upgr_cost_iron += 1;
            cannonrangelevel += 1;
        }
    }

    public void setuiactive()
    {
        RelatedUpgradeUI.SetActive(true);
    }

    public void deactivateui()
    {
        RelatedUpgradeUI.SetActive(false);
    }
}
