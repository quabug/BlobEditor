using Blob;
using Unity.Animation;
using Unity.Entities;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class TestSingleBlobBuilder : MonoBehaviour
{
    public BlobAsset<int> Int;
    public BlobAsset<BlobArray<BlobString>> StringArray;
    public BlobAsset<BlobArray<BlobArray<BlobPtr<BlobString>>>> StringArrayArray;
    public BlobAsset<AnimationCurveBlob> AnimationCurve;

    public void Awake()
    {
        Debug.Log($"{nameof(TestSingleBlobBuilder)}.{nameof(Int)} = {Int.Value}");
        Debug.Log($"{nameof(TestSingleBlobBuilder)}.{nameof(StringArray)} = {TestBlobBuilder.StringArrayToString(ref StringArray.Value)}");
        Debug.Log($"{nameof(TestSingleBlobBuilder)}.{nameof(AnimationCurve)}(0.5f) = {AnimationCurveEvaluator.Evaluate(0.5f, AnimationCurve.Reference)}");
        Debug.Log($"{nameof(TestSingleBlobBuilder)}.{nameof(StringArrayArray)} = {StringArrayArrayToString(ref StringArrayArray.Value)}");
    }

    private static string StringArrayArrayToString(ref BlobArray<BlobArray<BlobPtr<BlobString>>> array)
    {
        if (array.Length == 0) return "";

        var msg = "";
        for (var i = 0; i < array.Length; i++)
        {
            for (var j = 0; j < array[i].Length; j++)
            {
                msg += array[i][j].Value.ToString();
                msg += ",";
            }
            msg += "|";
        }
        return msg;
    }
}