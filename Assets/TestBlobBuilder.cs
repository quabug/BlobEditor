using System.Linq;
using Blob;
using Unity.Entities;
using UnityEngine;


public class TestBlobBuilder : MonoBehaviour
{
    public SerializedBuilder<BlobData> Blob;

    public void Awake()
    {
        var blob = Blob.Create();
        Debug.Log($"{nameof(BlobData)}.{nameof(BlobData.Int)} = {blob.Value.Int}");
        Debug.Log($"{nameof(BlobData)}.{nameof(BlobData.Long)} = {blob.Value.Long}");
        Debug.Log($"{nameof(BlobData)}.{nameof(BlobData.String)} = {blob.Value.String.ToString()}");
        Debug.Log($"{nameof(BlobData)}.{nameof(BlobData.IntArray)} = {string.Join(",", blob.Value.IntArray.ToArray())}");
        Debug.Log($"{nameof(BlobData)}.{nameof(BlobData.ULongPtr)} = {blob.Value.ULongPtr.Value}");
        Debug.Log($"{nameof(BlobData)}.{nameof(BlobData.StringArray)} = {StringArrayToString(ref blob.Value.StringArray)}");
        Debug.Log($"{nameof(BlobData)}.{nameof(BlobData.UInt)} = {blob.Value.UInt}");
        Debug.Log($"{nameof(BlobData)}.{nameof(BlobData.DoublePtr)} = {blob.Value.DoublePtr.Value}");
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

public struct BlobData
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
}
