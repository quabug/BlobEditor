namespace Blob
{
    public class UnityDrawPropertyAttribute : MultiPropertyAttribute
    {
        public UnityDrawPropertyAttribute() => order = int.MaxValue;
    }
}