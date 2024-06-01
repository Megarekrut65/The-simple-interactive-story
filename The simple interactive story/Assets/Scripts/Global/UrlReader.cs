using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Global
{
    public static class UrlReader
    {
        public static IEnumerator ReadSprite(string url, Action<Sprite> answer, int width=2, int height=2)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                answer(null);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
            
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), 
                    new Vector2(texture.width / 2f, texture.height / 2f));

                answer(sprite);
            }
        }

        public static IEnumerator ReadAudio(string name, string url, Action<AudioClip> answer)
        {
            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG);
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                answer(null);
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                clip.name = name;
                answer(clip);
            }
        }
    }
}