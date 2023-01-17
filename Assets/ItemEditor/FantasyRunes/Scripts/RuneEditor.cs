using System.Collections.Generic;
using Assets.ItemEditor.Common.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.ItemEditor.FantasyRunes.Scripts
{
    public class RuneEditor : EditorBase
    {
        public List<Sprite> StoneList;
        public List<Sprite> RuneList;

        public Image Stone;
        public Image Rune;

        public Slider StoneHue;
        public Slider StoneSaturation;
        public Slider StoneBrightness;

        public Slider RuneHue;
        public Slider RuneSaturation;
        public Slider RuneBrightness;

        private int _stoneIndex;
        private int _runeIndex;

        public void SwitchStone(int direction)
        {
            _stoneIndex = LoopIndex(_stoneIndex + direction, StoneList.Count);
            Stone.sprite = StoneList[_stoneIndex];
            AdjustStone();
        }

        public void SwitchRune(int direction)
        {
            _runeIndex = LoopIndex(_runeIndex + direction, RuneList.Count);
            Rune.sprite = RuneList[_runeIndex];
            AdjustRune();
        }

        #if UNITY_EDITOR

        public void PaintStone()
        {
            ColorPicker.Open(Stone.color);
            ColorPicker.OnColorPicked = color => Stone.color = color;
        }

        public void PaintRune()
        {
            ColorPicker.Open(Rune.color);
            ColorPicker.OnColorPicked = color => Rune.color = color;
        }

        public void Save()
        {
            var stone = Stone.sprite.texture.GetPixels();
            var rune = Rune.sprite.texture.GetPixels();

            for (var i = 0; i < stone.Length; i++)
            {
                stone[i] *= Stone.color;
                rune[i] *= Rune.color;
            }

            Save("Rune", Stone.sprite.texture.width, Stone.sprite.texture.height, stone, rune);
        }

        #endif

        public void AdjustStone()
        {
            ApplyHSV(StoneHue.value, StoneSaturation.value, StoneBrightness.value, StoneList[_stoneIndex].texture, Stone);
        }

        public void AdjustRune()
        {
            ApplyHSV(RuneHue.value, RuneSaturation.value, RuneBrightness.value, RuneList[_runeIndex].texture, Rune);
        }
    }
}