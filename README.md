# BlobEditor

Edit `BlobAsset` in Inspector and then easily create from code.

![image](https://user-images.githubusercontent.com/683655/130668171-ea822e22-c49d-438c-b6b4-6211253df859.png)

## Upgrade Note
⚠️ v1.2: upgrade from version 1.1 will lose data, you should either recreate data manually or use `BlobAssetV1`

## Installation
- (Recommend) [OpenUPM](https://openupm.com/packages/com.quabug.blob-editor/): `openupm add com.quabug.blob-editor`
- or UPM: edit *Packages/manifest.json*
```
{
  "dependencies": {
    "com.quabug.blob-editor": "1.1.0",
    ...
  },
  "scopedRegistries": [
    {
      "name": "package.openupm.com",
      "url": "https://package.openupm.com",
      "scopes": [
        "com.quabug.blob-editor"
      ]
    }
  ]
}
```

## Usage

### `BlobAsset<>`

``` c#
public class SingleBlobComponent : MonoBehaviour
{
    public BlobAsset<BlobArray<int>> IntArray;

    private void Awake()
    {
        // get `BlobAssetReference` from `BlobAsset`
        BlobAssetReference<BlobArray<int>> blob = IntArray.Reference;
        // or use blob value directly
        ref BlobArray<int> _ = ref IntArray.Value;
    }
}
```

``` c#
struct Blob
{
    public int Int;
    public BlobString String;
}

public class BlobStructureComponent : MonoBehaviour
{
    public BlobAsset<Blob> BlobValue;

    private void Awake()
    {
        // get `BlobAssetReference` from `BlobAsset`
        BlobAssetReference<BlobData> blob = BlobValue.Reference;
        // or use blob value directly
        string _ = BlobValue.Value.String.ToString();
    }
}
```

### Extend builder for new type

Builders of *primitive* types and *com.unity.mathematics* types are provided by default.

A specific builder must be provided for any new type.

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

- `PlainDataBuilder<>`: Builder of POD. Show the POD as whole in inspector. Not support data with its own builder, e.g. `BlobPtr`, `BlobArray` or `BlobString`.
- `BlobDataBuilder<>`: Builder of structure with **Blob-Specific** data inside. Split each data inside structure into its own builder to show and edit. Support data with `DefaultBuilder` or `CustomBuilder`, e.g. `BlobPtr`, `BlobArray` or `BlobString`.
- `ArrayBuilder<>`: Builder for `BlobArray<>`.
- `PtrBuilder<>`: Builder for `BlobPtr<>`.

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
