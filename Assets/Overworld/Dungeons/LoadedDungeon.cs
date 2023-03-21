using UnityEngine;

namespace Assets.Dungeons
{
    [CreateAssetMenu(fileName = nameof(LoadedDungeon), menuName = "ScriptableObjects/LoadedDungeon")]
    public class LoadedDungeon : ScriptableObject
    {
        public int currentLevel;
        public DungeonData loadedDungeonData;
    }
}