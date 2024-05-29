using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Network
{
    public static class Fetcher
    {
        private static IEnumerator GetNumerator(string url, string token, Action<string, string> callback)
        {
            using UnityWebRequest request = UnityWebRequest.Get(url);
            
            request.SetRequestHeader("Content-Type", "application/json; charset=UTF-8");
            //request.SetRequestHeader("Authorization", $"Token {token}");

            yield return request.SendWebRequest();
            while (!request.isDone)
            {
                yield return null;
            }

            if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                callback(request.downloadHandler.text, null);
            }
            else
            {
                string res = request.downloadHandler.text;
                callback(null, res); 
            }
        }
        
        public static void Get(MonoBehaviour behaviour, string url, string token, Action<string, string> callback)
        {
            behaviour.StartCoroutine(GetNumerator(url, token, callback));
        }
        
        private static IEnumerator PostNumerator(string url, string token, WWWForm form, Action<string, string> callback)
        {
            using (UnityWebRequest request = UnityWebRequest.Post(url, form))
            {
                request.SetRequestHeader("Authorization", $"Token {token}");

                yield return request.SendWebRequest();
                while (!request.isDone)
                {
                    yield return null;
                }

                if (request.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
                {
                    callback(request.downloadHandler.text, null);
                }
                else
                {
                    string res = request.downloadHandler.text;
                    callback(null, res); 
                }
            }
        }
        
        public static void Post(MonoBehaviour behaviour, string url, string token, WWWForm form, 
            Action<string, string> callback)
        {
            behaviour.StartCoroutine(PostNumerator(url, token, form, callback));
        }
    }
}