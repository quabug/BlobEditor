> ‼CAUTION: DO NOT use this on IL2CPP backend until [this bug](https://issuetracker.unity3d.com/issues/marshal-dot-offsetof-returns-incorrect-offset-when-building-the-project-with-il2cpp-scripting-backend) of `Marshal.OffsetOf` is fixed.
# BlobEditor

Edit `BlobAsset` in Inspector and then easily create from code.

![image](https://user-images.githubusercontent.com/683655/129061436-32d815a5-3fc6-43e5-afcf-79ed0080d4a6.png)

## Usage

### `SerializedBuilder<>`

``` c#
public struct BlobData
{
    public int Value;
}

public class BlobComponent : MonoBehaviour
{
    public SerializedBuilder<BlobData> Blob;

    private void Awake()
    {
        BlobAssetReference<BlobData> blob = Blob.Create();
        // use blob
        Debug.Log($"{blob.Value.Value}");
    }
}
```

### Extend builder for new type

Builders of *primitive* types and *com.unity.mathematics* types are provided by default.

A specific type of builder must be provided for any new type.

Take `Guid` as example:
``` c#
public struct GuidBlob
{
    public Guid Value;
}

[Serializable]
public class GuidBuilder : Builder<Guid>
{
    public string Guid = System.Guid.NewGuid().ToString();

    public override void Build(BlobBuilder builder, ref Guid data)
    {
        data = System.Guid.Parse(Guid);
    }
}
```

### `DefaultBuilderAttribute` and `CustomBuilderAttribute`
There's may have multiple builders for a single type, one of them must be defined as `DefaultBuilder`, or use `CustomBuilder` to assign a specific builder for a value of blob.
``` c#
[Serializable, DefaultBuilder]
public class StringBuilder : Builder<BlobString>
{
    public string Value = "";

    public override void Build(BlobBuilder builder, ref BlobString data)
    {
        builder.AllocateString(ref data, Value);
    }
}

// a builder of `BlobString` which save name of *GameObject* into `BlobString`
[Serializable]
public class ObjectName : Builder<BlobString>
{
    public GameObject GameObject;

    public override void Build(BlobBuilder builder, ref BlobString data)
    {
        builder.AllocateString(ref data, GameObject == null ? "" : GameObject.name);
    }
}

public struct Blob
{
    public BlobString String; // will use `StringBuilder` by default
    [CustomBuilder(typeof(ObjectName))] public BlobString ObjectName;
}
```
