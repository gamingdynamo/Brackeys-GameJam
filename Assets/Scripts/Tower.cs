using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tower : MonoBehaviour, IDamageable, IInteractable, IUpgradeable
{
    [Header("Health")]
    public float hp;
    public int maxhp;
    public int hplevel;
    private int hp_upgr_cost_wood = 2;
    private int hp_upgr_cost_iron = 1;
    public TMP_Text hptext;

    [Header("Regen")]
    public float hpregenmultiplier;
    public int hpregenmultiplierlevel;
    private int regen_upgr_cost_wood = 2;
    private int regen_upgr_cost_iron = 1;
    public TMP_Text regen;


    private float time_since_damage;

    [Header("Damage")]
    public int towercannondamage;
    public int towercannondamagelevel;
    private int damage_upgr_cost_wood = 2;
    private int damage_upgr_cost_iron = 1;
    public TMP_Text damage;

    [Header("Interval")]
    public float towercannonshotinterval;
    public float towercannonshotintervallevel;
    private int interval_upgr_cost_wood = 2;
    private int interval_upgr_cost_iron = 1;
    public TMP_Text interval;

    [Header("Range")]
    public float towercannonrange;
    public float towercannonrangelevel;
    private int range_upgr_cost_wood = 2;
    private int range_upgr_cost_iron = 1;
    public TMP_Text range;

    public Image towerhpfill;

    public GameObject RelatedUpgradeUI;

    private GameManager gamemanager;
    private PlayerResources playerResources;

    void Start()
    {
        playerResources = FindObjectOfType<PlayerResources>();
        gamemanager = FindObjectOfType<GameManager>();
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

        // update ui values RIP
        hptext.text = "level: " + hplevel.ToString() + "<br>" + "wood: " + hp_upgr_cost_wood.ToString() +"iron: " + hp_upgr_cost_iron.ToString();
        regen.text = "level: " + hpregenmultiplierlevel.ToString() + "<br>" + "wood: " + regen_upgr_cost_wood.ToString() +" iron: " + regen_upgr_cost_iron.ToString();
        damage.text = "level: " + towercannondamagelevel.ToString() + "<br>" + "wood: " + damage_upgr_cost_wood.ToString() + " iron: " + damage_upgr_cost_iron.ToString();
        interval.text = "level: " + towercannonshotintervallevel.ToString() + "<br>" + "wood: " + interval_upgr_cost_wood.ToString() + " iron: " + interval_upgr_cost_iron.ToString();
        range.text = "level: " + towercannonrangelevel.ToString() + "<br>" + "wood: " + range_upgr_cost_wood.ToString() + " iron: " + range_upgr_cost_iron.ToString();

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
        if (playerResources.wood - interval_upgr_cost_wood > 0 && playerResources.iron - interval_upgr_cost_iron > 0)
        {
            towercannonshotinterval = towercannonshotinterval - 0.05f;
            interval_upgr_cost_wood += 1;
            interval_upgr_cost_iron += 1;
            towercannonshotintervallevel += 1;
        }
    }

    public void increasedmg()
    {
        if(playerResources.wood - damage_upgr_cost_wood > 0 && playerResources.iron - damage_upgr_cost_iron > 0)
        {
            towercannondamage = Mathf.RoundToInt(towercannondamage * 1.1f);
            damage_upgr_cost_wood += 1;
            damage_upgr_cost_iron += 1;
            towercannondamagelevel += 1;
        }
    }

    public void increasehp()
    {
        if (playerResources.wood - hp_upgr_cost_wood > 0 && playerResources.iron - hp_upgr_cost_iron > 0)
        {
            maxhp = Mathf.RoundToInt(maxhp * 1.1f);
            hp_upgr_cost_wood += 1;
            hp_upgr_cost_iron += 1;
            hplevel += 1;
        }
    }

    public void increaserange()
    {
        if (playerResources.wood - range_upgr_cost_wood > 0 && playerResources.iron - range_upgr_cost_iron > 0)
        {
            towercannonrange = towercannonrange + 0.5f;
            range_upgr_cost_wood += 1;
            range_upgr_cost_iron += 1;
            towercannonrangelevel += 1;
        }
    }

    public void increaserecoveryspeed()
    {
        if (playerResources.wood - regen_upgr_cost_wood > 0 && playerResources.iron - regen_upgr_cost_iron > 0)
        {
            hpregenmultiplier = hpregenmultiplier * 1.1f;
            regen_upgr_cost_wood += 1;
            regen_upgr_cost_iron += 1;
            hpregenmultiplierlevel += 1;
        }
    }

    public void setuiactive()
    {
        RelatedUpgradeUI.SetActive(true);
    }

    public void deactivateui()
    {
        RelatedUpgradeUI.SetActive(false);
    }
}
