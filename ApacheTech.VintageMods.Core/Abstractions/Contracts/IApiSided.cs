using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.Abstractions.Contracts
{
    /// <summary>
    ///     Represents a class that is determinate, based on the app-side it is instantiated on. 
    /// </summary>
    public interface IApiSided
    {
        /// <summary>
        ///     The app-side that this instance was instantiated on.
        /// </summary>
        public EnumAppSide Side { get; }
    }
}