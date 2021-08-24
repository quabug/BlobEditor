using Blob;
using Unity.Entities;
using UnityEngine;


public class TestBlobBuilder : MonoBehaviour
{
    public BlobAsset<SimpleData> Blob;

    public void Awake()
    {
        Debug.Log($"{nameof(SimpleData)}.{nameof(SimpleData.Int)} = {Blob.Value.Int}");
        Debug.Log($"{nameof(SimpleData)}.{nameof(SimpleData.Long)} = {Blob.Value.Long}");
        Debug.Log($"{nameof(SimpleData)}.{nameof(SimpleData.String)} = {Blob.Value.String.ToString()}");
        Debug.Log($"{nameof(SimpleData)}.{nameof(SimpleData.IntArray)} = {string.Join(",", Blob.Value.IntArray.ToArray())}");
        Debug.Log($"{nameof(SimpleData)}.{nameof(SimpleData.ULongPtr)} = {Blob.Value.ULongPtr.Value}");
        Debug.Log($"{nameof(SimpleData)}.{nameof(SimpleData.StringArray)} = {StringArrayToString(ref Blob.Value.StringArray)}");
        Debug.Log($"{nameof(SimpleData)}.{nameof(SimpleData.UInt)} = {Blob.Value.UInt}");
        Debug.Log($"{nameof(SimpleData)}.{nameof(SimpleData.DoublePtr)} = {Blob.Value.DoublePtr.Value}");
        Debug.Log($"{nameof(SimpleData)}.{nameof(SimpleData.TestEnum)} = {Blob.Value.TestEnum}");
    }

    public static string StringArrayToString(ref BlobArray<BlobString> array)
    {
        if (array.Length == 0) return "";

        var msg = "";
        for (var i = 0; i < array.Length - 1; i++)
        {
            msg += array[i].ToString();
            msg += ",";
        }
        msg += array[array.Length - 1].ToString();
        return msg;
    }
}

public struct SimpleData
{
    public int Int;
    public BlobString String;
    public float Float;
    public BlobArray<int> IntArray;
    public long Long;
    public BlobPtr<ulong> ULongPtr;
    public BlobArray<BlobString> StringArray;
    public uint UInt;
    public BlobPtr<double> DoublePtr;
    public Test TestEnum;
}

public enum Test
{
    Foo, Bar
}