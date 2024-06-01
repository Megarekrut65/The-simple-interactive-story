using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Global
{
    public class FontManager : MonoBehaviour
    {
        [SerializeField] private Font[] fonts;
        [SerializeField] private Font defaultFont;

        public static FontManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        [CanBeNull]
        public Font GetFont(string fontName)
        {
            Font font = Array.Find(fonts, font => font.name == fontName);
            return font != null ? font : defaultFont;
        }

        [CanBeNull]
        public static Font GetTextFont(string fontName)
        {
            if (Instance == null) return null;
            
            return Instance.GetFont(fontName);
        }
    }
}