namespace ApacheTech.VintageMods.Core.Abstractions
{
    public abstract class ScopedOperation : IScopedOperation
    {
        public abstract void OnFinally();

        public void Dispose()
        {
            OnFinally();
        }
    }
}
