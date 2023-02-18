using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkingZombie : MonoBehaviour, IDamageable
{
    public GameObject tower;
    public GameObject player;

    private GameObject target;
    private NavMeshAgent agent;

    [Header("Sounds")]
    public AudioSource footsteps;
    public AudioSource zombiesfx;
    public List<AudioClip> zombiesfxclips;

    public int hp;
    public int maxhp;

    public int damage;
    public float damage_interval;
    private float next_damage_time;

    // zombie type
    public bool small_zombie;
    public bool large_zombie;

    // range of the arms
    public float damage_range = 1.5f;
    public GameObject damage_ray_origin;

    [SerializeField] private Animator m_animator = null;

    private bool isdead;

    private bool ismoving;

    public bool Ismoving
    {
        get { return ismoving; }
        set
        {
            if (value == ismoving) { return; }
            else
            {
                ismoving = value;
                if (ismoving)
                {
                    footsteps.Play();
                }
                else
                {
                    footsteps.Stop();
                }
            }
        }
    }

    void Start()
    {
        //overlap_collider = GetComponent<SphereCollider>();
        agent = GetComponent<NavMeshAgent>();

        if (!small_zombie)
        {
            agent.SetDestination(tower.transform.position);
        }
        else
        {
            agent.SetDestination(player.transform.position);
        }
        
        target = tower;
        StartCoroutine(PlayZombieSounds());
        if (!m_animator) { gameObject.GetComponent<Animator>(); }
    }

    void FixedUpdate()
    {
        // If moving set walking anim
        if (isdead) { Ismoving = false;  return; }
        float velocity = agent.velocity.magnitude / agent.speed;
        m_animator.SetFloat("MoveSpeed", velocity);

        if(velocity > 0.05f) { Ismoving = true ; } else { Ismoving = false; }

        // if small zombie we need to constanly get the latest position of the player
        if (small_zombie)
        {
            agent.SetDestination(player.transform.position);
        }

        if (Time.time > next_damage_time)
        {
            // shoot raycast with damage
            RaycastHit hit;
            if (Physics.Raycast(damage_ray_origin.transform.position, transform.forward, out hit, damage_range))
            {
                if (hit.transform.gameObject.GetComponent<IDamageable>() != null)
                {
                    if (hit.transform.gameObject.tag != "zombie")
                    {
                        hit.transform.gameObject.GetComponent<IDamageable>().damage(damage, false);
                        m_animator.SetTrigger("Attack");
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
        if(small_zombie || large_zombie) { return; }
        if (other.gameObject.tag == "Player")
        {
            agent.SetDestination(other.transform.position);
            target = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (small_zombie || large_zombie) { return; }
        if (other.gameObject.tag == "Player")
        {
            agent.SetDestination(tower.transform.position);
            target = tower;
            gameObject.transform.LookAt(tower.transform.position); //new Vector3(tower.transform.position.x,  , tower.transform.position.z)
        }
    }

    void IDamageable.damage(int damage, bool friendly)
    {
        hp -= damage;
        if (hp <= 0)
        {
            // add random drop with chances?
            isdead = true;
            //gameObject.GetComponent<NavMeshAgent>().enabled = false;
            agent.speed= 0;
            agent.angularSpeed= 0;
            Quaternion quaternion = gameObject.transform.rotation;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            gameObject.transform.rotation = new Quaternion(0, quaternion.y, 0, quaternion.w);
            m_animator.SetTrigger("Dead");
            gameObject.transform.rotation = new Quaternion(0, quaternion.y, 0, quaternion.w);
            StartCoroutine(despawndelay());
        }
    }

    IEnumerator PlayZombieSounds()
    {
        yield return new WaitForSeconds(Random.Range(1, 3));
        AudioClip toplay = zombiesfxclips[Random.Range(0,zombiesfxclips.Count)];
        zombiesfx.clip = toplay;
        zombiesfx.Play();

        // delay between sounds
        yield return new WaitForSeconds(Random.Range(7,9));
        StartCoroutine(PlayZombieSounds());
    }
    IEnumerator despawndelay()
    {
        Destroy(gameObject.transform.Find("AttackHeight").gameObject);
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
