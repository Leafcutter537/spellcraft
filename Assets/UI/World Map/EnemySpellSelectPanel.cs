using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Combat.Enemy;
using Assets.Inventory.Spells;
using UnityEngine;

public class EnemySpellSelectPanel : SelectPanel
{
    [SerializeField] private EnemySpellGenerator enemySpellGenerator;
    protected override void GetInventory()
    {
        if (itemList == null)
        {
            itemList = new List<SelectChoice>();
        }
    }

    public void UpdateSpellList(List<EnemySpellData> enemySpells)
    {
        List<Spell> listEnemySpells = enemySpellGenerator.CreateSpellList(enemySpells);
        itemList = listEnemySpells.Cast<SelectChoice>().ToList();
        RefreshInventory();
    }
}
