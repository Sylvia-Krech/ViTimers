namespace VTimer.Helpers;

// This is dumb, but I dont know how to store a reference to a basic type, whereas classes are always stored as reference
public class KeyVal<K, V>
{
    public K Key { get; set; }
    public V Value { get; set; }
    public KeyVal(K key, V val)
    {
        this.Key = key;
        this.Value = val;
    }
}