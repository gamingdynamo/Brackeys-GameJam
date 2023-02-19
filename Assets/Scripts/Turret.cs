using UnityEngine;
using UnityEngine.Rendering;

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
                    if (dist < range)
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
                project.GetComponent<Rigidbody>().AddForce(head.transform.forward * 1000f);
                next_shot_time = Time.time + shot_interval;
                audio.Play();
            }
        }
    }
}
