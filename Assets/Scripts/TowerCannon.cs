using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerCannon : MonoBehaviour
{
    public GameObject head;
    public GameObject barrel_end;

    public GameObject projectile;

    public List<GameObject> zombies_in_range;
    public GameObject current_target;

    public float shot_interval;
    private float next_shot_time;

    public float range;
    public float attackAngle;

    void Update()
    {
        // check if previous target is not out of range (if not we can search for a new enemy the same tick)
        if (current_target != null)
        {
            float dist = Vector3.Distance(gameObject.transform.position, current_target.transform.position);
            if (dist < range)
            {
                Vector3 targetsRange = current_target.transform.position - head.transform.position;

                float angle = Vector3.Angle(targetsRange, head.transform.forward);

                // if the angle is larger than attack angle current target is out of range
                if (angle > attackAngle)
                {
                    current_target = null;
                }
            }
        }

        // set new target if old target dissapeared
        if (current_target == null)
        {
            foreach (GameObject zomb in GameObject.FindGameObjectsWithTag("zombie"))
            {
                try
                {
                    GameObject zombie_target_point = zomb.transform.Find("AttackHeight").gameObject;

                    float dist = Vector3.Distance(gameObject.transform.position, zombie_target_point.transform.position);
                    if (dist < range)
                    {
                        Vector3 targetsRange = zombie_target_point.transform.position - head.transform.position;

                        float angle = Vector3.Angle(targetsRange, gameObject.transform.forward);

                        //within angle?
                        if (angle <= attackAngle)
                        {
                            // set new target
                            current_target = zombie_target_point;
                            break;
                        }
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
            }
        }
    }
}
