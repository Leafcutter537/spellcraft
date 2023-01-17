using System;
using UnityEngine;

namespace Assets.ItemEditor.Common.Scripts
{
    /// <summary>
    /// Used to prepare textures for saving.
    /// </summary>
    public static class TextureHelper
    {
        public static Texture2D MergeLayers(int width, int height, params Color[][] layers)
        {
            if (layers.Length == 0) throw new Exception("No layers to merge.");
            
            var result = new Color[width * height];

            foreach (var layer in layers)
            {
                for (var i = 0; i < result.Length; i++)
                {
                    var color = layer[i];

                    if (color.a < 1)
                    {
                        ToPremultipliedAlpha(ref color);
                        result[i] = result[i] * (1 - color.a) + color;
                    }
                    else
                    {
                        result[i] = color;
                    }
                }
            }

            for (var i = 0; i < result.Length; i++)
            {
                ToStraightAlpha(ref result[i]);
            }

            var texture = new Texture2D(width, height) { filterMode = FilterMode.Point };

            texture.SetPixels(result);
            texture.Apply();

            return texture;
        }

        public static Rect GetContentRect(Texture2D texture)
        {
            var pixels = texture.GetPixels();
            var minX = texture.width - 1;
            var minY = texture.height - 1;
            var maxX = 0;
            var maxY = 0;

            for (var x = 0; x < texture.width; x++)
            {
                for (var y = 0; y < texture.height; y++)
                {
                    if (pixels[x + y * texture.width].a > 0)
                    {
                        minX = Mathf.Min(x, minX);
                        minY = Mathf.Min(y, minY);
                    }
                }
            }

            for (var x = texture.width - 1; x >= 0; x--)
            {
                for (var y = texture.height - 1; y >= 0; y--)
                {
                    if (pixels[x + y * texture.width].a > 0)
                    {
                        maxX = Mathf.Max(x, maxX);
                        maxY = Mathf.Max(y, maxY);
                    }
                }
            }

            pixels = texture.GetPixels(minX, minY, maxX - minX, maxY - minY);
            texture = new Texture2D(maxX - minX, maxY - minY);
            texture.SetPixels(pixels);
            texture.Apply();

            return new Rect(minX, minY, maxX - minX + 1, maxY - minY + 1);
        }

        public static Color AdjustColor(Color color, float hue, float saturation, float value)
        {
            hue /= 180f;
            saturation /= 100f;
            value /= 100f;

            var a = color.a;

            Color.RGBToHSV(color, out var h, out var s, out var v);

            h += hue / 2f;

            if (h > 1) h -= 1;
            else if (h < 0) h += 1;

            color = Color.HSVToRGB(h, s, v);

            var grey = 0.3f * color.r + 0.59f * color.g + 0.11f * color.b;

            color.r = grey + (color.r - grey) * (saturation + 1);
            color.g = grey + (color.g - grey) * (saturation + 1);
            color.b = grey + (color.b - grey) * (saturation + 1);

            if (color.r < 0) color.r = 0;
            if (color.g < 0) color.g = 0;
            if (color.b < 0) color.b = 0;

            color.r += value * color.r;
            color.g += value * color.g;
            color.b += value * color.b;
            color.a = a;

            return color;
        }

        private static void ToPremultipliedAlpha(ref Color color)
        {
            color.r *= color.a;
            color.g *= color.a;
            color.b *= color.a;
        }

        private static void ToStraightAlpha(ref Color color)
        {
            color.r /= color.a;
            color.g /= color.a;
            color.b /= color.a;
        }
    }
}