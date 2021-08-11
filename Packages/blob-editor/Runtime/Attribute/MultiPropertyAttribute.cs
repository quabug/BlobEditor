using System;
using System.Diagnostics;
using UnityEngine;

namespace Blob
{
    [AttributeUsage(AttributeTargets.Field)]
    [Conditional("UNITY_EDITOR")]
    public abstract class MultiPropertyAttribute : PropertyAttribute { }
}