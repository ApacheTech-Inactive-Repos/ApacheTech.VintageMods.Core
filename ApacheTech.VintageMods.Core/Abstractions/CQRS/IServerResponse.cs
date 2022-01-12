namespace ApacheTech.VintageMods.Core.Abstractions.CQRS
{
    /// <summary>
    ///     Serialisable wrapper for server responses.
    /// </summary>
    /// <typeparam name="T">The type of response to wrap.</typeparam>
    public interface IServerResponse<out T>
    {
        T Response { get; }
    }
}