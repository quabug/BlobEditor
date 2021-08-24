using System;
using System.Reflection;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;

namespace Blob
{
    public static class Utility
    {
        public static IntPtr ArrayElementAsPtr(IntPtr arrayDataPtr, long index, long size) => new IntPtr(arrayDataPtr.ToInt64() + index * size);

        // TODO: hack into `BlobBuilder` to make some dynamic (non-generic) methods to avoid using reflection.
        public static BlobBuilderDynamicArray AllocateDynamicArray(this BlobBuilder builder, Type elementType, IntPtr arrayPtr, int length)
        {
            var func = typeof(Utility).GetMethod(nameof(AllocateDynamicArray), BindingFlags.Static | BindingFlags.NonPublic);
            return (BlobBuilderDynamicArray) func.MakeGenericMethod(elementType).Invoke(null, new object[] {builder, arrayPtr, length});
        }

        private static unsafe BlobBuilderDynamicArray AllocateDynamicArray<T>(BlobBuilder builder, IntPtr arrayPtr, int length) where T : unmanaged
        {
            ref var data = ref UnsafeUtility.AsRef<BlobArray<T>>(arrayPtr.ToPointer());
            var arrayBuilder = builder.Allocate(ref data, length);
            return new BlobBuilderDynamicArray(new IntPtr(arrayBuilder.GetUnsafePtr()), length, typeof(T));
        }

        public static IntPtr AllocateDynamicPtr(this BlobBuilder builder, Type dataType, IntPtr dataPtr)
        {
            var func = typeof(Utility).GetMethod(nameof(AllocateDynamicPtr), BindingFlags.Static | BindingFlags.NonPublic);
            return (IntPtr) func.MakeGenericMethod(dataType).Invoke(null, new object[] {builder, dataPtr});
        }

        private static unsafe IntPtr AllocateDynamicPtr<T>(BlobBuilder builder, IntPtr dataPtr) where T : unmanaged
        {
            ref var data = ref UnsafeUtility.AsRef<BlobPtr<T>>(dataPtr.ToPointer());
            ref var blobPtr = ref builder.Allocate(ref data);
            return new IntPtr(UnsafeUtility.AddressOf(ref blobPtr));
        }
    }
}