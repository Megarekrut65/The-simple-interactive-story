using System;
using JetBrains.Annotations;
using UnityEngine;

namespace SimpleStory
{
    public class FontManager: MonoBehaviour
    {
        [SerializeField] private Font[] fonts;
        [SerializeField] private Font defaultFont;

        [CanBeNull]
        public Font GetFont(string fontName)
        {
            Font font = Array.Find(fonts, font => font.name == fontName);
            return font != null ? font : defaultFont;
        }
    }
}