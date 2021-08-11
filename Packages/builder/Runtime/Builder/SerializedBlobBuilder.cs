using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using UnityEngine;

namespace Blob
{

    public interface IBuilder
    {
        void Build(BlobBuilder builder, IntPtr dataPtr);
    }

    public abstract class Builder<T> : IBuilder where T : unmanaged
    {
        public unsafe void Build(BlobBuilder builder, IntPtr dataPtr)
        {
            Build(builder, ref UnsafeUtility.AsRef<T>(dataPtr.ToPointer()));
        }

        public abstract void Build(BlobBuilder builder, ref T data);
    }

    [Serializable]
    public class DefaultBuilder<T> : Builder<T> where T : unmanaged
    {
        public T Value;

        public override void Build(BlobBuilder builder, ref T data)
        {
            data = Value;
        }
    }

    [Serializable]
    public class ArrayBuilder<T> : Builder<BlobArray<T>> where T : unmanaged
    {
        [SerializeReference, UnboxSinglePropertyBuilder, UnityDrawProperty] public IBuilder[] Value;

        public override void Build(BlobBuilder builder, ref BlobArray<T> data)
        {
            var arrayBuilder = builder.Allocate(ref data, Value.Length);
            for (var i = 0; i < Value.Length; i++) ((Builder<T>)Value[i]).Build(builder, ref arrayBuilder[i]);
        }
    }

    [Serializable]
    public class StringBuilder : Builder<BlobString>
    {
        public string Value;

        public override void Build(BlobBuilder builder, ref BlobString data)
        {
            builder.AllocateString(ref data, Value);
        }
    }
    //
    // [Serializable]
    // public class PtrBuilder<T> : Builder<BlobPtr<T>> where T : unmanaged
    // {
    //     [SerializeReference] public IBuilder PtrValue;
    //
    //     public override void Build(BlobBuilder builder, ref BlobPtr<T> data)
    //     {
    //     }
    // }

    [Serializable]
    public class SerializedBuilder<T> : Builder<T> where T : unmanaged
    {
        [HideInInspector] public string[] FieldNames;
        [SerializeReference, UnboxSinglePropertyBuilder, UnityDrawProperty] public IBuilder[] Builders;

        public override unsafe void Build(BlobBuilder builder, ref T data)
        {
            var dataPtr = new IntPtr(UnsafeUtility.AddressOf(ref data));
            var fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            for (var i = 0; i < Builders.Length; i++)
            {
                var offset = Marshal.OffsetOf<T>(fields[i].Name).ToInt32();
                Builders[i].Build(builder, dataPtr + offset);
            }
        }

        public BlobAssetReference<T> Create()
        {
            using var builder = new BlobBuilder(Allocator.Temp);
            ref var root = ref builder.ConstructRoot<T>();
            Build(builder, ref root);
            return builder.CreateBlobAssetReference<T>(Allocator.Persistent);
        }
    }
}