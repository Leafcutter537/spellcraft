using UnityEngine;

public class StatBundle
{
    public int maxHP;
    public int maxMP;
    public int resilience;
    public int projectilePower;
    public int shieldPower;
    public int healPower;

    public StatBundle(int maxHP, int maxMP, int resilience, int projectilePower, int shieldPower, int healPower)
    {
        this.maxHP = maxHP;
        this.maxMP = maxMP;
        this.resilience = resilience;
        this.projectilePower = projectilePower;
        this.shieldPower = shieldPower;
        this.healPower = healPower;
    }

    public StatBundle(StatBundle statBundle)
    {
        this.maxHP = statBundle.maxHP;
        this.maxMP = statBundle.maxMP;
        this.resilience = statBundle.resilience;
        this.projectilePower = statBundle.projectilePower;
        this.shieldPower = statBundle.shieldPower;
        this.healPower = statBundle.healPower;
    }
}
