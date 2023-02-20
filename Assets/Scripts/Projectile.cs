using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public bool friendly_projectile;
    public float destroy_delay = 1; // Time to wait before destroying the projectile

    private float destroy_timer; // Timer to count down to destruction

    private void Start()
    {
        // Start the timer when the projectile is instantiated
        destroy_timer = destroy_delay;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            // Damage the object if it is damageable
            damageable.damage(damage, friendly_projectile);
    	    // Reset the timer when the projectile collides with anything
            destroy_timer = destroy_delay;
        }

    
    }

    private void Update()
    {
        // Update the timer and destroy the projectile when it reaches zero
        destroy_timer -= Time.deltaTime;

        if (destroy_timer <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
