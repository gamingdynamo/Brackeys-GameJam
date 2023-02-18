using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR;

public class Tower : MonoBehaviour, IDamageable, IInteractable, IUpgradeable
{
    public float hp;
    public int maxhp;

    public float hpregenmultiplier;
    private float time_since_damage;

    public Image towerhpfill;
    public GameManager gamemanager;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        towerhpfill.fillAmount = hp/maxhp;

        if (time_since_damage <= 0)
        {
            // start regen
            if (hp < maxhp)
            {
                hp += Time.deltaTime * hpregenmultiplier;
                if (hp > maxhp) { hp = maxhp; }
            }
        }
        else
        {
            time_since_damage -= Time.deltaTime;
        }
    }

    void IDamageable.damage(int damage, bool friendly)
    {
        if (friendly) { return; }

        hp -= damage;
        time_since_damage = 10f;
        if (hp <= 0)
        {
            Debug.Log("The tower was destroyed");
            gamemanager.istowerdestroyed = true;
            Destroy(gameObject);
        }
    }

    public void returnupgradables()
    {
        // for each upgradable create a block
        GameObject prefab_container = gamemanager.UpgradeContainer;

        // block 1
        GameObject container = Instantiate(prefab_container, gamemanager.UpgradeGrid.transform);
        UpgradeContainer upgraderef = container.GetComponent<UpgradeContainer>();
        // set upgrade specific values
        upgraderef.upgrade_name.text = "";
        upgraderef.current_value.text = 1.1f.ToString();

    }

    public void increasespeed()
    {
        throw new NotImplementedException();
    }

    public void increasedmg()
    {
        throw new NotImplementedException();
    }

    public void increasehp()
    {
        throw new NotImplementedException();
    }

    public void increaserange()
    {
        throw new NotImplementedException();
    }
}
