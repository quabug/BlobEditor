using System;
using Unity.Assertions;

namespace Blob
{
    [AttributeUsage(AttributeTargets.Field)]
    public class CustomBuilderAttribute : Attribute
    {
        public Type BuilderType;
        public CustomBuilderAttribute(Type builderType)
        {
            Assert.IsTrue(typeof(IBuilder).IsAssignableFrom(builderType));
            BuilderType = builderType;
        }
    }
}
