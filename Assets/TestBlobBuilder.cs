using Blob;
using Unity.Entities;
using UnityEngine;


public class TestBlobBuilder : MonoBehaviour
{
    public SerializedBuilder<BlobData> Blob;

    public void Awake()
    {
        var blob = Blob.Create();
        Debug.Log($"{nameof(BlobData)}.{nameof(BlobData.A)} = {blob.Value.A}");
        Debug.Log($"{nameof(BlobData)}.{nameof(BlobData.C)} = {blob.Value.C}");
        Debug.Log($"{nameof(BlobData)}.{nameof(BlobData.String)} = {blob.Value.String.ToString()}");
        Debug.Log($"{nameof(BlobData)}.{nameof(BlobData.IntArray)} = {string.Join(",", blob.Value.IntArray.ToArray())}");
    }
}

public struct BlobData
{
    public int A;
    public BlobString String;
    private float B;
    public BlobArray<int> IntArray;
    public long C;
}
