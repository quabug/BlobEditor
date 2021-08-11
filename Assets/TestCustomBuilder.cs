using System;
using Blob;
using Unity.Entities;
using UnityEngine;

public class TestCustomBuilder : MonoBehaviour
{
    public SerializedBuilder<CustomBuilderBlob> Blob;

    private void Awake()
    {
        var blob = Blob.Create();
        Debug.Log($"CustomBuilderBlob.Guid = {blob.Value.Guid}");
        Debug.Log($"CustomBuilderBlob.StringGuid = {blob.Value.StringGuid.ToString()}");
        Debug.Log($"CustomBuilderBlob.ObjectName = {blob.Value.ObjectName.ToString()}");
        Debug.Log($"CustomBuilderBlob.Objects = {TestBlobBuilder.StringArrayToString(ref blob.Value.Objects)}");
    }
}

public struct CustomBuilderBlob
{
    public Guid Guid;
    [CustomBuilder(typeof(StringGuid))] public BlobString StringGuid;
    [CustomBuilder(typeof(ObjectName))] public BlobString ObjectName;
    [CustomBuilder(typeof(ObjectNameArray))] public BlobArray<BlobString> Objects;
}

[Serializable, DefaultBuilder]
public class GuidBuilder : Builder<Guid>
{
    public string Guid = System.Guid.NewGuid().ToString();

    public override void Build(BlobBuilder builder, ref Guid data)
    {
        data = System.Guid.Parse(Guid);
    }
}

[Serializable]
public class StringGuid : Builder<BlobString>
{
    public string Guid = System.Guid.NewGuid().ToString();

    public override void Build(BlobBuilder builder, ref BlobString data)
    {
        builder.AllocateString(ref data, Guid);
    }
}

[Serializable]
public class ObjectName : Builder<BlobString>
{
    public GameObject GameObject;

    public override void Build(BlobBuilder builder, ref BlobString data)
    {
        builder.AllocateString(ref data, GameObject == null ? "" : GameObject.name);
    }
}

[Serializable]
public class ObjectNameArray : Builder<BlobArray<BlobString>>
{
    public GameObject[] GameObjects = Array.Empty<GameObject>();

    public override void Build(BlobBuilder builder, ref BlobArray<BlobString> data)
    {
        var array = builder.Allocate(ref data, GameObjects.Length);
        for (var i = 0; i < GameObjects.Length; i++) builder.AllocateString(ref array[i], GameObjects[i].name);
    }
}