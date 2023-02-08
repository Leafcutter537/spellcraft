using UnityEngine;

[CreateAssetMenu(fileName = nameof(PlayerStats), menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public string playerName;
    [SerializeField] private int baseHP;
    [SerializeField] private int baseMP;

    public int GetHP()
    {
        return baseHP;
    }

    public int GetMP()
    {
        return baseMP;
    }
}
