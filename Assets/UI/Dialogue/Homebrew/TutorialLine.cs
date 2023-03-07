
using System;
using UnityEngine;

namespace Assets.Tutorial
{
    [Serializable]
    public class TutorialLine
    {
        [TextAreaAttribute]
        public string text;
        public int panelIndex;
    }
}
