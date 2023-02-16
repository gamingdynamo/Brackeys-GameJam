using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float hpregenmultiplier;
    private float time_since_damage;

    public float hp;
    public int maxhp;

    public Image playerhpfill;

    public GameManager gamemanager;
    void Start()
    {
        
    }


    void Update()
    {
        playerhpfill.fillAmount = hp / maxhp;

        if (time_since_damage <= 0)
        {
            // start regen
            if (hp < maxhp)
            {
                hp += Time.deltaTime * hpregenmultiplier;
                if (hp > maxhp) { hp = maxhp; }
            }
        }
        else { time_since_damage -= Time.deltaTime; }
    }

    void IDamageable.damage(int damage, bool friendly)
    {
        if (friendly) { return; }

        hp -= damage;
        time_since_damage = 5f;
        if (hp <= 0)
        {
            Debug.Log("The player was killed");
            gamemanager.hasplayerdied = true;
            Destroy(gameObject);
        }
    }
}
