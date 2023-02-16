using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject head;
    public GameObject barrel;
    public GameObject barrel_end;

    public GameObject projectile;

    public List<GameObject> zombies_in_range;
    public GameObject current_target;

    public float shot_interval;
    private float next_shot_time;

    public float range;

    // Update is called once per frame
    void Update()
    {
        // set new target if old target dissapeared
        if (current_target == null)
        {
            if (zombies_in_range.Count() > 0)
            {
                current_target = zombies_in_range[0];
            }
            else
            {
                foreach(GameObject zomb in GameObject.FindGameObjectsWithTag("zombie"))
                {
                    float dist = Vector3.Distance(gameObject.transform.position, zomb.transform.position);
                    if(dist < range)
                    {
                        current_target = zomb; 
                        break;
                    }
                }
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


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "zombie")
        {
            zombies_in_range.Append(other.gameObject);
        }
        Debug.Log(other);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "zombie")
        {
            if (current_target == other.gameObject) { current_target= null; }

            zombies_in_range.Remove(other.gameObject);
        }
    }
}
