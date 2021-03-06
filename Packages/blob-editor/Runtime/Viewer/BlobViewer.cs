using System;
using Unity.Entities;
using UnityEngine;

namespace Blob
{
    [Serializable]
    public class BlobViewer
    {
        [SerializeField] public bool Writable = false;
        [SerializeField] internal string TypeName = "(Empty)";
        [SerializeReference, UnboxSingleProperty, UnityDrawProperty] internal IViewer Viewer;

        public unsafe void View<T>(T* dataPtr) where T : unmanaged => UnsafeView(new IntPtr(dataPtr), typeof(T));

        public void UnsafeView(IntPtr dataPtr, Type type)
        {
#if UNITY_EDITOR
            TypeName = type.ToReadableName();
            var viewerType = type.FindViewerType();
            if (Viewer == null || Viewer.GetType() != viewerType)
                Viewer = (IViewer) Activator.CreateInstance(viewerType);
            Viewer.View(dataPtr, type);
#endif
        }
    }

    public static class BlobViewerExtension
    {
        public static unsafe void View<T>(this BlobViewer viewer, BlobAssetReference<T> blobAssetReference) where T : unmanaged
        {
            fixed (T* ptr = &blobAssetReference.Value)
            {
                viewer.View(ptr);
            }
        }
    }
}