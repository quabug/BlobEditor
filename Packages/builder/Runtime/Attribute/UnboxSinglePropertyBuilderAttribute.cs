using JetBrains.Annotations;

namespace Blob
{
    [BaseTypeRequired(typeof(IBuilder))]
    public class UnboxSinglePropertyBuilderAttribute : MultiPropertyAttribute {}
}