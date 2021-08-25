using System;
using Blob;
using Unity.Animation;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class Example : MonoBehaviour
{
    public BlobAsset<ExampleBlob> Blob;
    public BlobViewer Viewer;

    private void Awake()
    {
        // get `BlobAssetReference` from `BlobAsset`
        BlobAssetReference<ExampleBlob> blob = Blob.Reference;
        // or use blob value directly
        var _ = Blob.Value.Float3;

        Viewer.View(Blob.Reference);
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
    public AnimationCurveBlob AnimationCurve;
    public ComponentType.AccessMode Enum;
    public BlobArray<BlobArray<BlobArray<BlobPtr<BlobString>>>> Arrays;
}