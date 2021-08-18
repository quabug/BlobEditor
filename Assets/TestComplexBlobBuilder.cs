using Blob;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


public class TestComplexBlobBuilder : MonoBehaviour
{
    public BlobAsset<ComplexData> Blob;

    public void Awake()
    {
        Debug.Log($"ComplexData.A.Int = {Blob.Value.A.Int}");
        Debug.Log($"ComplexData.A.Long = {Blob.Value.A.Long}");
        Debug.Log($"ComplexData.A.String = {Blob.Value.A.String.ToString()}");
        Debug.Log($"ComplexData.A.IntArray = {string.Join(",", Blob.Value.A.IntArray.ToArray())}");
        Debug.Log($"ComplexData.A.LongPtr = {Blob.Value.A.ULongPtr.Value}");

        Debug.Log($"ComplexData.B.Int = {Blob.Value.B.Int}");
        Debug.Log($"ComplexData.B.Int2Array = {string.Join(",", Blob.Value.B.Int2Array.ToArray())}");
        Debug.Log($"ComplexData.B.String = {Blob.Value.B.String.ToString()}");
        Debug.Log($"ComplexData.B.Int3Ptr = {Blob.Value.B.Int3Ptr.Value}");

        Debug.Log($"ComplexData.C.Float3 = {Blob.Value.C.Float3}");
        Debug.Log($"ComplexData.C.Double2x2 = {Blob.Value.C.Double2x2}");
        Debug.Log($"ComplexData.C.String = {Blob.Value.C.String.ToString()}");
        Debug.Log($"ComplexData.C.IntArray = {string.Join(",", Blob.Value.C.Int3Array.ToArray())}");
        Debug.Log($"ComplexData.C.Float2Ptr = {Blob.Value.C.Float2Ptr.Value}");
    }
}

public struct SimpleData2
{
    public int Int;
    public BlobArray<int2> Int2Array;
    public BlobString String;
    public BlobPtr<int3> Int3Ptr;
}

public class SimpleData2Builder : BlobDataBuilder<SimpleData2> {}

public struct ComplexData
{
    public SimpleData A;
    public SimpleData2 B;
    public MathData C;
}

public class ComplexDataBuilder : BlobDataBuilder<ComplexData> {}
