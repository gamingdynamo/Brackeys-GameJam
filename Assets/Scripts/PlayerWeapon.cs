using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerWeapon : MonoBehaviour
{
    public GameObject weapon_projectile;
    public GameObject player_weapon_barrel;

    public GameObject player_rotation;
    Plane plane = new Plane(Vector3.up, 0);

    // weapon delay
    private float next_shot_time;
    public float shots_interval;
    void Start()
    {
        // ADD PLAYER POWERUPS LIKE A GRENADE HE CAN THROW TO DAMAGE GROUPS

    }

    void Update()
    {
        // rotate player to look at the curser in world space
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(plane.Raycast(ray, out distance))
        {
            Vector3 worldpos = ray.GetPoint(distance - 1.6f);

            Vector3 aimposition = new Vector3(worldpos.x, worldpos.y , worldpos.z); // + 1.4f
            player_rotation.transform.LookAt(aimposition);
        }

        if (Time.time > next_shot_time)
        {
            // shoot
            if (Input.GetMouseButtonDown(0))
            {
                GameObject projectile = Instantiate(weapon_projectile, player_weapon_barrel.transform.position, player_rotation.transform.rotation);
                projectile.GetComponent<Projectile>().friendly_projectile = true;
                // modify damage
                projectile.GetComponent<Projectile>().friendly_projectile = true;
                projectile.GetComponent<Rigidbody>().AddForce(player_rotation.transform.transform.forward * 1000f);
                next_shot_time = Time.time + shots_interval;
            }
            
        }
    }
}
