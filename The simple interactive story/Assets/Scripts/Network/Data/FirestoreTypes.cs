using System;

namespace Network.Data
{
    [Serializable]
    public class FirestoreString
    {
        public string stringValue;
    }

    [Serializable]
    public class FirestoreTimestamp
    {
        public string timestampValue;
    }
    [Serializable]
    public class FirestoreDouble
    {
        public double doubleValue;
    }

    [Serializable]
    public class FirestoreBoolean
    {
        public bool booleanValue;
    }
    [Serializable]
    public class FirestoreMapValue<TFields>
    {
        public TFields fields;
    }

    [Serializable]
    public class FirestoreMap<TFields>
    {
        public FirestoreMapValue<TFields> mapValue;
    }

    [Serializable]
    public class FirestoreArrayValue<TFields>
    {
        public FirestoreMap<TFields>[] values;
    }

    [Serializable]
    public class FirestoreArray<TFields>
    {
        public FirestoreArrayValue<TFields> arrayValue;
    }
    
    [Serializable]
    public class FirestoreDocument<TFields>
    {
        public string name;
        public TFields fields;
        public string createTime;
        public string updateTime;
    }

    [Serializable]
    public class FirestoreCollection<TFields>
    {
        public FirestoreDocument<TFields>[] documents;
    }
}