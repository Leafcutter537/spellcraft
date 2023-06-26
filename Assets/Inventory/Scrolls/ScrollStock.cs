
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Inventory.Scrolls
{
    [CreateAssetMenu(fileName = nameof(ScrollStock), menuName = "ScriptableObjects/Scrolls/ScrollStock")]
    public class ScrollStock : ScriptableObject
    {
        [SerializeField] private List<ScrollData> baseScrollStock;
        public List<ScrollData> scrollStock;
        public int currentIndex;
        public void CopyBase()
        {
            scrollStock = new List<ScrollData>();
            foreach (ScrollData scroll in baseScrollStock)
            {
                scrollStock.Add(scroll);
            }
        }

        public void AddScroll(ScrollData scrollData)
        {
            scrollStock.Add(scrollData);
        }

        public string AddScrolls(ScrollData[] scrollDataArray, string rewardString)
        {
            foreach (ScrollData scroll in scrollDataArray)
            {
                rewardString += "\nThe " + scroll.scrollName + " Scroll has been added in the Spellforge!";
                scrollStock.Add(scroll);
            }
            return rewardString;
        }
    }
}
