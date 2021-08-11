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
        Debug.Log($"{nameof(MathBlobData)}.{nameof(MathBlobData.Float3)} = {blob.Value.Float3}");
        Debug.Log($"{nameof(MathBlobData)}.{nameof(MathBlobData.Bool2)} = {blob.Value.Bool2}");
        Debug.Log($"{nameof(MathBlobData)}.{nameof(MathBlobData.Double2x2)} = {blob.Value.Double2x2}");
        Debug.Log($"{nameof(MathBlobData)}.{nameof(MathBlobData.String)} = {blob.Value.String.ToString()}");
        Debug.Log($"{nameof(MathBlobData)}.{nameof(MathBlobData.Int3Array)} = {string.Join(",", blob.Value.Int3Array.ToArray())}");
        Debug.Log($"{nameof(MathBlobData)}.{nameof(MathBlobData.Float2Ptr)} = {blob.Value.Float2Ptr.Value}");
    }
}

public struct MathBlobData
{
    public float3 Float3;
    public BlobString String;
    public bool2 Bool2;
    public BlobArray<int3> Int3Array;
    public double2x2 Double2x2;
    public BlobPtr<float2> Float2Ptr;
}