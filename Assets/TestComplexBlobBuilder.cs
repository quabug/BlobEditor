using Blob;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


public class TestComplexBlobBuilder : MonoBehaviour
{
    public SerializedBuilder<ComplexBlobData> Blob;

    public void Awake()
    {
        var blob = Blob.Create();

        Debug.Log($"ComplexBlobData.A.Int = {blob.Value.A.Int}");
        Debug.Log($"ComplexBlobData.A.Long = {blob.Value.A.Long}");
        Debug.Log($"ComplexBlobData.A.String = {blob.Value.A.String.ToString()}");
        Debug.Log($"ComplexBlobData.A.IntArray = {string.Join(",", blob.Value.A.IntArray.ToArray())}");
        Debug.Log($"ComplexBlobData.A.LongPtr = {blob.Value.A.ULongPtr.Value}");

        Debug.Log($"ComplexBlobData.B.Int = {blob.Value.B.Int}");
        Debug.Log($"ComplexBlobData.B.Int2Array = {string.Join(",", blob.Value.B.Int2Array.ToArray())}");
        Debug.Log($"ComplexBlobData.B.String = {blob.Value.B.String.ToString()}");
        Debug.Log($"ComplexBlobData.B.Int3Ptr = {blob.Value.B.Int3Ptr.Value}");

        Debug.Log($"ComplexBlobData.C.Float3 = {blob.Value.C.Float3}");
        Debug.Log($"ComplexBlobData.C.Double2x2 = {blob.Value.C.Double2x2}");
        Debug.Log($"ComplexBlobData.C.String = {blob.Value.C.String.ToString()}");
        Debug.Log($"ComplexBlobData.C.IntArray = {string.Join(",", blob.Value.C.Int3Array.ToArray())}");
        Debug.Log($"ComplexBlobData.C.Float2Ptr = {blob.Value.C.Float2Ptr.Value}");
    }
}

public struct BlobData2
{
    public int Int;
    public BlobArray<int2> Int2Array;
    public BlobString String;
    public BlobPtr<int3> Int3Ptr;
}

public struct ComplexBlobData
{
    public BlobData A;
    public BlobData2 B;
    public MathBlobData C;
}

public class BlobDataBuilder : SerializedBuilder<BlobData> {}
public class BlobData2Builder : SerializedBuilder<BlobData2> {}
public class MathBlobDataBuilder : SerializedBuilder<MathBlobData> {}
