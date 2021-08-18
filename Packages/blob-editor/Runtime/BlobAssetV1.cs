using System;
using Unity.Collections;
using Unity.Entities;

namespace Blob
{
    [Serializable, Obsolete("for backward compatibility of version 1 only")]
    public class BlobAssetV1<T> where T : unmanaged
    {
        public BlobDataBuilder<T> Builder;

        BlobAssetReference<T> _blobAssetReference;

        public BlobAssetReference<T> Reference
        {
            get
            {
                if (!_blobAssetReference.IsCreated) _blobAssetReference = Create();
                return _blobAssetReference;
            }
        }

        public ref T Value => ref Reference.Value;

        private BlobAssetReference<T> Create()
        {
            using var builder = new BlobBuilder(Allocator.Temp);
            ref var root = ref builder.ConstructRoot<T>();
            Builder.Build(builder, ref root);
            return builder.CreateBlobAssetReference<T>(Allocator.Persistent);
        }
    }
}