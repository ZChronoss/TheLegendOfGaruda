using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] List<TKey> keys = new List<TKey>();
    [SerializeField] List<TValue> values = new List<TValue>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        foreach(KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        if(keys.Count != values.Count)
        {
            Debug.Log("The number of keys (" +  keys.Count +") does not match the number of values (" +  values.Count + ").");
        }

        for(int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }
}
