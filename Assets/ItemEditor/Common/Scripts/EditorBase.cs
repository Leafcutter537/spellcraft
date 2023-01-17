using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.ItemEditor.Common.Scripts
{
    public class EditorBase : MonoBehaviour
    {
        protected int LoopIndex(int index, int listCount)
        {
            if (index < 0) index = listCount - 1;
            if (index == listCount) index = 0;

            return index;
        }

        protected void ApplyHSV(float hue, float saturation, float value, Texture2D source, Image image)
        {
            var pixels = source.GetPixels();

            for (var i = 0; i < pixels.Length; i++)
            {
                if (pixels[i].a <= 0) continue;

                pixels[i] = TextureHelper.AdjustColor(pixels[i], hue, saturation, value);
            }

            var texture = new Texture2D(source.width, source.height);

            texture.SetPixels(pixels);
            texture.Apply();
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
        }

        #if UNITY_EDITOR

        protected void Save(string type, int width, int height, params Color[][] layers)
        {
            var texture = TextureHelper.MergeLayers(width, height, layers);
            var rect = TextureHelper.GetContentRect(texture);
            var pixels = texture.GetPixels((int)rect.min.x, (int)rect.min.y, (int)rect.width, (int)rect.height);
            var offsetX = (texture.width - rect.width) / 2;
            var offsetY = (texture.height - rect.height) / 2;

            texture = new Texture2D(texture.width, texture.height);
            texture.SetPixels(new Color[texture.width * texture.height]);
            texture.SetPixels((int)offsetX, (int)offsetY, (int)rect.width, (int)rect.height, pixels);
            texture.Apply();

            var path = UnityEditor.EditorUtility.SaveFilePanel($"Save {type} as PNG", "", $"{type}.png", "png");

            File.WriteAllBytes(path, texture.EncodeToPNG());
            Debug.Log($"Screenshot saved as {path}.");
        }

        #endif
    }
}