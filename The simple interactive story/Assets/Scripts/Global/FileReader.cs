using System;
using UnityEngine;

namespace Global
{
    public class FileReader
    {
        public static Sprite ReadSprite(string fullPath, int width=2, int height=2)
        {
            byte[] bytes = AllFileReader.ReadBytes(fullPath);
            Texture2D texture = new Texture2D(width, height);
            texture.LoadImage(bytes);
            
            Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), 
                new Vector2(0.5f, 0.5f), 100.0f);
 
            return sprite;
        }

        public static AudioClip ReadAudio(string name, string fullPath, int channels=1, int sampleRate=44100)
        {
            byte[] bytes = AllFileReader.ReadBytes(fullPath);
            float[] samples = new float[bytes.Length / 4];
            Buffer.BlockCopy(bytes, 0, samples, 0, bytes.Length);

            AudioClip clip = AudioClip.Create(name, samples.Length, channels, sampleRate, false);
            clip.SetData(samples, 0);

            return clip;
        }
    }
}