﻿using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.Abstractions.ModSystems.Generic
{
    /// <summary>
    ///     Base representation of a ModSystem used to extend Vintage Story.
    /// </summary>
    /// <seealso cref="ModSystem" />
    public abstract class ModSystemBase : ModSystem
    {
        /// <summary>
        ///     Common API Components that are available on the server and the client.<br/>
        ///     Cast to ICoreServerAPI, or ICoreClientAPI, to access side specific features.
        /// </summary>
        protected ICoreAPI UApi { get; private set; }

        /// <summary>
        ///     Called during initial mod loading, called before any mod receives the call to Start().
        /// </summary>
        /// <param name="api">
        ///     Common API Components that are available on the server and the client.<br/>
        ///     Cast to ICoreServerAPI or ICoreClientAPI to access side specific features.
        /// </param>
        public override void StartPre(ICoreAPI api)
        {
            UApi = api;
        }

        /// <summary>
        ///     If you need mods to be executed in a certain order, adjust this methods return value.<br/>
        ///     The server will call each Mods Start() method the ascending order of each mods execute order value.<br/>
        ///     And thus, as long as every mod registers it's event handlers in the Start() method, all event handlers<br/>
        ///     will be called in the same execution order.<br/>
        ///     Default execute order of some survival mod parts.<br/><br/>
        /// 
        ///     World Gen:<br/>
        ///     - GenTerra: 0<br/>
        ///     - RockStrata: 0.1<br/>
        ///     - Deposits: 0.2<br/>
        ///     - Caves: 0.3<br/>
        ///     - BlockLayers: 0.4<br/><br/>
        /// 
        ///     Asset Loading:<br/>
        ///     - Json Overrides loader: 0.05<br/>
        ///     - Load hardcoded mantle block: 0.1<br/>
        ///     - Block and Item Loader: 0.2<br/>
        ///     - Recipes (Smithing, Knapping, ClayForming, Grid recipes, Alloys) Loader: 1
        /// </summary>
        public override double ExecuteOrder()
        {
            return 0.05;
        }
    }
}