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

        Debug.Log($"ComplexBlobData.A.A = {blob.Value.A.A}");
        Debug.Log($"ComplexBlobData.A.C = {blob.Value.A.C}");
        Debug.Log($"ComplexBlobData.A.String = {blob.Value.A.String.ToString()}");
        Debug.Log($"ComplexBlobData.A.IntArray = {string.Join(",", blob.Value.A.IntArray.ToArray())}");
        Debug.Log($"ComplexBlobData.A.LongPtr = {blob.Value.A.LongPtr.Value}");

        Debug.Log($"ComplexBlobData.B.A = {blob.Value.B.A}");
        Debug.Log($"ComplexBlobData.B.B = {string.Join(",", blob.Value.B.B.ToArray())}");
        Debug.Log($"ComplexBlobData.B.C = {blob.Value.B.C.ToString()}");
        Debug.Log($"ComplexBlobData.B.D = {blob.Value.B.D.Value}");

        Debug.Log($"ComplexBlobData.C.A = {blob.Value.C.A}");
        Debug.Log($"ComplexBlobData.C.C = {blob.Value.C.C}");
        Debug.Log($"ComplexBlobData.C.String = {blob.Value.C.String.ToString()}");
        Debug.Log($"ComplexBlobData.C.IntArray = {string.Join(",", blob.Value.C.IntArray.ToArray())}");
        Debug.Log($"ComplexBlobData.C.Float2Ptr = {blob.Value.C.Float2Ptr.Value}");
    }
}

public struct BlobData2
{
    public int A;
    public BlobArray<int2> B;
    public BlobString C;
    public BlobPtr<int3> D;
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
