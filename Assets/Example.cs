using System;
using Blob;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class Example : MonoBehaviour
{
    public BlobAsset<ExampleBlob> Blob;

    private void Awake()
    {
        // get `BlobAssetReference` from `BlobAsset`
        BlobAssetReference<ExampleBlob> blob = Blob.Reference;
        // or use blob value directly
        var _ = Blob.Value.Float3;
    }
}

public struct ExampleBlob
{
    public int Int;
    public float3 Float3;
    public BlobString String;
    public BlobArray<double2> Double2Array;
    public BlobArray<BlobString> StringArray;
    public BlobPtr<long> LongPtr;
    public Guid Guid;
    [CustomBuilder(typeof(ObjectName))] public BlobString GameObjectName;
}