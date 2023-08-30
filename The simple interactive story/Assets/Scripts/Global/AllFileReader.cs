using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Global
{
    public static class AllFileReader {

        private static bool NeedWeb()
        {
            return Application.platform == RuntimePlatform.Android
                   || Application.platform == RuntimePlatform.IPhonePlayer
                   || Application.platform == RuntimePlatform.WebGLPlayer;
        }
        public static string Read(string path) {
            string res;
            if (path.Contains("://") || path.Contains(":///") || NeedWeb()) {
                UnityWebRequest www = UnityWebRequest.Get(path);
                www.SendWebRequest();
                while (!www.isDone) {
                }

                res = www.downloadHandler.text;
            } else {
                res = File.ReadAllText(path);
            }

            return res;
        }
        public static byte[] ReadBytes(string path) {
            byte[] res;
            
            if (path.Contains("://") || path.Contains(":///") || NeedWeb()) {
                UnityWebRequest www = UnityWebRequest.Get(path);
                www.SendWebRequest();
                while (!www.isDone) {
                }

                res = www.downloadHandler.data;
            } else
            {
                res = File.ReadAllBytes(path);
            }

            return res;
        }
    }
}