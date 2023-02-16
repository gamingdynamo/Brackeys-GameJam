using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour, IDamageable
{
    private SphereCollider overlap_collider;
    public GameObject tower;

    private GameObject target;
    private NavMeshAgent agent;

    public int hp;
    public int maxhp;

    public int damage;
    public float damage_interval;
    private float next_damage_time;

    // range of the arms
    public float damage_range = 1.5f;

    void Start()
    {
        overlap_collider = GetComponent<SphereCollider>();
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(tower.transform.position);
        target = tower;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > next_damage_time)
        {
            // shoot raycast with damage
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, damage_range))
            {
                if (hit.transform.gameObject.GetComponent<IDamageable>() != null)
                {
                    if (hit.transform.gameObject.tag != "zombie")
                    {
                        hit.transform.gameObject.GetComponent<IDamageable>().damage(damage, false);
                        next_damage_time = Time.time + damage_interval;
                    }
                }
            }
        }
        if (agent.remainingDistance < 4)
        {
            gameObject.transform.LookAt(target.transform.position);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            agent.SetDestination(other.transform.position);
            target = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            agent.SetDestination(tower.transform.position);
            target= tower;
            gameObject.transform.LookAt(tower.transform.position); //new Vector3(tower.transform.position.x,  , tower.transform.position.z)
        }
    }

    void IDamageable.damage(int damage, bool friendly)
    {
        hp -= damage;
        if(hp <=0) 
        {
            // add random drop with chances?
            Destroy(gameObject);
        }
    }
}
