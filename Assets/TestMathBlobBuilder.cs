using Blob;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class TestMathBlobBuilder : MonoBehaviour
{
    public SerializedBuilder<MathBlobData> Blob;

    public void Awake()
    {
        var blob = Blob.Create();
        Debug.Log($"{nameof(MathBlobData)}.{nameof(MathBlobData.A)} = {blob.Value.A}");
        Debug.Log($"{nameof(MathBlobData)}.{nameof(MathBlobData.C)} = {blob.Value.C}");
        Debug.Log($"{nameof(MathBlobData)}.{nameof(MathBlobData.String)} = {blob.Value.String.ToString()}");
        Debug.Log($"{nameof(MathBlobData)}.{nameof(MathBlobData.IntArray)} = {string.Join(",", blob.Value.IntArray.ToArray())}");

    }
}

public struct MathBlobData
{
    public float3 A;
    public BlobString String;
    private bool2 B;
    public BlobArray<int3> IntArray;
    public double2x2 C;
}