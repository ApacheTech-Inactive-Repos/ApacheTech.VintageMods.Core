namespace ApacheTech.VintageMods.Core.Abstractions.CQRS
{
    /// <summary>
    ///     Serialisable wrapper for client responses.
    /// </summary>
    /// <typeparam name="T">The type of response to wrap.</typeparam>
    public interface IClientResponse<out T>
    {
        T Response { get; }
    }
}