using Blob;
using Unity.Animation;
using Unity.Entities;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class TestSingleBlobBuilder : MonoBehaviour
{
    public BlobAsset<int> Int;
    public BlobAsset<BlobArray<BlobString>> StringArray;
    public BlobAsset<AnimationCurveBlob> AnimationCurve;

    public void Awake()
    {
        Debug.Log($"{nameof(TestSingleBlobBuilder)}.{nameof(Int)} = {Int.Value}");
        Debug.Log($"{nameof(TestSingleBlobBuilder)}.{nameof(StringArray)} = {TestBlobBuilder.StringArrayToString(ref StringArray.Value)}");
        Debug.Log($"{nameof(TestSingleBlobBuilder)}.{nameof(AnimationCurve)}(0.5f) = {AnimationCurveEvaluator.Evaluate(0.5f, AnimationCurve.Reference)}");
    }
}