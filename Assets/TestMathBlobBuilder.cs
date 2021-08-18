using Blob;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class TestMathBlobBuilder : MonoBehaviour
{
    public BlobAsset<MathData> Blob;

    public void Awake()
    {
        Debug.Log($"{nameof(MathData)}.{nameof(MathData.Float3)} = {Blob.Value.Float3}");
        Debug.Log($"{nameof(MathData)}.{nameof(MathData.Bool2)} = {Blob.Value.Bool2}");
        Debug.Log($"{nameof(MathData)}.{nameof(MathData.Double2x2)} = {Blob.Value.Double2x2}");
        Debug.Log($"{nameof(MathData)}.{nameof(MathData.String)} = {Blob.Value.String.ToString()}");
        Debug.Log($"{nameof(MathData)}.{nameof(MathData.Int3Array)} = {string.Join(",", Blob.Value.Int3Array.ToArray())}");
        Debug.Log($"{nameof(MathData)}.{nameof(MathData.Float2Ptr)} = {Blob.Value.Float2Ptr.Value}");
    }
}

public struct MathData
{
    public float3 Float3;
    public BlobString String;
    public bool2 Bool2;
    public BlobArray<int3> Int3Array;
    public double2x2 Double2x2;
    public BlobPtr<float2> Float2Ptr;
}

public class MathBlobDataBuilder : BlobDataBuilder<MathData> {}
