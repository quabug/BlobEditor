using System;
using Blob;
using Unity.Entities;
using UnityEngine;

public class TestCustomBuilder : MonoBehaviour
{
    public BlobAsset<CustomBuilderData> Blob;

    private void Awake()
    {
        Debug.Log($"CustomBuilderData.Guid = {Blob.Value.Guid}");
        Debug.Log($"CustomBuilderData.StringGuid = {Blob.Value.StringGuid.ToString()}");
        Debug.Log($"CustomBuilderData.ObjectName = {Blob.Value.ObjectName.ToString()}");
        Debug.Log($"CustomBuilderData.Objects = {TestBlobBuilder.StringArrayToString(ref Blob.Value.Objects)}");
    }
}

public struct CustomBuilderData
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
public class GuidViewer : Viewer<Guid>
{
    public string Guid = System.Guid.NewGuid().ToString();

    public override void View(ref Guid data)
    {
        Guid = data.ToString();
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