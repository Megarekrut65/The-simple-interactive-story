using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace Main
{
    public static class JsManager
    {
        [DllImport("__Internal")]
        [CanBeNull]
        public static extern string Token();
        
        [DllImport("__Internal")]
        [CanBeNull]
        public static extern string Locale();

        [DllImport("__Internal")]
        [CanBeNull]
        public static extern string UidFromUrl();
    }
}