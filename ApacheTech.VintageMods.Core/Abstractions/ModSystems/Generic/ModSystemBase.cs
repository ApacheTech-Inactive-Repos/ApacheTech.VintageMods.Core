using JetBrains.Annotations;
using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.Abstractions.ModSystems.Generic
{
    /// <summary>
    ///     Base representation of a ModSystem used to extend Vintage Story.
    /// </summary>
    /// <seealso cref="ModSystem" />
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers | ImplicitUseTargetFlags.WithInheritors)]
    public abstract class ModSystemBase : ModSystem
    {
        /// <summary>
        ///     Common API Components that are available on the server and the client. Cast to ICoreServerAPI, or ICoreClientAPI, to access side specific features.
        /// </summary>
        protected ICoreAPI UApi { get; private set; }

        /// <summary>
        ///     Called during initial mod loading, called before any mod receives the call to Start().
        /// </summary>
        /// <param name="api">
        ///     Common API Components that are available on the server and the client. Cast to ICoreServerAPI or
        ///     ICoreClientAPI to access side specific features.
        /// </param>
        public override void StartPre(ICoreAPI api)
        {
            UApi = api;
        }

        /// <summary>
        ///     If you need mods to be executed in a certain order, adjust this methods return value.
        ///     The server will call each Mods Start() method the ascending order of each mods execute order value.
        ///     And thus, as long as every mod registers it's event handlers in the Start() method, all event handlers
        ///     will be called in the same execution order.
        ///     Default execute order of some survival mod parts.
        ///     World Gen:
        ///     - GenTerra: 0
        ///     - RockStrata: 0.1
        ///     - Deposits: 0.2
        ///     - Caves: 0.3
        ///     - BlockLayers: 0.4
        ///     Asset Loading:
        ///     - Json Overrides loader: 0.05
        ///     - Load hardcoded mantle block: 0.1
        ///     - Block and Item Loader: 0.2
        ///     - Recipes (Smithing, Knapping, ClayForming, Grid recipes, Alloys) Loader: 1
        /// </summary>
        public override double ExecuteOrder()
        {
            return 0.05;
        }
    }
}