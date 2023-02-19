using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class Turret : MonoBehaviour
{
    public GameObject head;
    public GameObject barrel_end;

    public GameObject projectile;

    public GameObject current_target;

    public float shot_interval;
    private float next_shot_time;

    public float range;

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

    private void Start()
    {
        audio = GetComponent<AudioSource>();
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
    }

    public void increasespeed()
    {
        if (cannonshotinterval < 0.4f) { }// deny upgrade

        cannonshotinterval = cannonshotinterval - 0.05f;
        interval_upgr_cost_wood += 1;
        interval_upgr_cost_iron += 1;
        cannonshotintervallevel += 1;

    }

    public void increasedmg()
    {

        cannondamage = Mathf.RoundToInt(cannondamage * 1.1f);
        damage_upgr_cost_wood += 1;
        damage_upgr_cost_iron += 1;
        cannondamagelevel += 1;
    }

    public void increaserange()
    {
        cannonrange = cannonrange + 0.5f;
        range_upgr_cost_wood += 1;
        range_upgr_cost_iron += 1;
        cannonrangelevel += 1;
    }
}
