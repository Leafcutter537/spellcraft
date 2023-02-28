using TMPro;
using Assets.Combat;
using UnityEngine;

public class PlayerStatTotal : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerStatText;
    [SerializeField] private PlayerStats playerStats;

    public void UpdatePlayerStats()
    {
        string statText = "";
        statText += "HP: " + playerStats.GetCombatStat(CombatStat.HP) + "\n";
        statText += "MP: " + playerStats.GetCombatStat(CombatStat.MP) + "\n";
        statText += "Resilience: " + playerStats.GetCombatStat(CombatStat.Resilience) + "\n";
        statText += "Projectile Power: " + playerStats.GetCombatStat(CombatStat.ProjectilePower) + "\n";
        statText += "Shield Power: " + playerStats.GetCombatStat(CombatStat.ShieldPower) + "\n";
        statText += "Healing Power: " + playerStats.GetCombatStat(CombatStat.HealPower) + "\n";
        playerStatText.text = statText;
    }
}
