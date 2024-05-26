using JetBrains.Annotations;
using UnityEngine;

namespace Network
{
    public abstract class MessageContent
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }

        public override string ToString()
        {
            return $"Code: {Code}, Message: {Message}, Status: {Status}";
        }
    }
    public class FirebaseError
    {
        [CanBeNull] private readonly MessageContent _error;
        private readonly string _message;

        public FirebaseError(string message)
        {
            _message = message;
            
            FirebaseError err = JsonUtility.FromJson<FirebaseError>(message);
            if (err?._error != null) _error = err._error;
        }

        public override string ToString()
        {
            return _error!=null? _error.ToString(): _message;
        }
    }
}