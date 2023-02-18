using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public bool friendly_projectile;
    private void OnCollisionEnter(Collision collision)
    {
        try 
        {
            collision.gameObject.GetComponent<IDamageable>().damage(damage, friendly_projectile);
        } 
        catch{ } // projectile didn't hit a damagable
        
        Destroy(gameObject);
    }
}
