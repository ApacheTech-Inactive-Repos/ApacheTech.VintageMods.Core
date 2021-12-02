using Vintagestory.API.Common;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace ApacheTech.VintageMods.Core.Extensions.Game
{ 
    public static class ClassRegistryExtensions
    {
        /// <summary>
        ///     Registers a new Item class. <br/>
        ///     Must happen before any blocks are loaded. <br/>
        ///     Must be registered on both the client, and server.
        /// </summary>
        /// <typeparam name="T">The type of the Item to register.</typeparam>
        /// <param name="api">The game's internal API.</param>
        /// <param name="name">The name of the Item to add to the registry.</param>
        public static void RegisterItem<T>(this ICoreAPICommon api, string name = nameof(T))
        {
            api.RegisterItemClass(name, typeof(T));
        }

        /// <summary>
        ///     Registers a new Block class. <br/>
        ///     Must happen before any blocks are loaded. <br/>
        ///     Must be registered on both the client, and server.
        /// </summary>
        /// <typeparam name="T">The type of the Block to register.</typeparam>
        /// <param name="api">The game's internal API.</param>
        /// <param name="name">The name of the Block to add to the registry.</param>
        public static void RegisterBlock<T>(this ICoreAPICommon api, string name = nameof(T))
        {
            api.RegisterBlockClass(name, typeof(T));
        }

        /// <summary>
        ///     Registers a new Block class. <br/>
        ///     Must happen before any blocks are loaded. <br/>
        ///     Must be registered on both the client, and server.
        /// </summary>
        /// <typeparam name="T">The type of the Block to register.</typeparam>
        /// <param name="api">The game's internal API.</param>
        /// <param name="name">The name of the Block to add to the registry.</param>
        public static void RegisterBlockEntity<T>(this ICoreAPICommon api, string name = nameof(T))
        {
            api.RegisterBlockEntityClass(name, typeof(T));
        }

        /// <summary>
        ///     Registers a new Block Behaviour. <br/>
        ///     Must happen before any blocks are loaded. <br/>
        ///     Must be registered on both the client, and server.
        /// </summary>
        /// <typeparam name="T">The type of the Block Behaviour to register.</typeparam>
        /// <param name="api">The game's internal API.</param>
        /// <param name="name">The name of the Block Behaviour to add to the registry.</param>
        public static void RegisterBlockBehaviour<T>(this ICoreAPICommon api, string name = nameof(T))
        {
            api.RegisterBlockBehaviorClass(name, typeof(T));
        }

        /// <summary>
        ///     Registers a new BlockEntity Behaviour. <br/>
        ///     Must happen before any blocks are loaded. <br/>
        ///     Must be registered on both the client, and server.
        /// </summary>
        /// <typeparam name="T">The type of the BlockEntity Behaviour to register.</typeparam>
        /// <param name="api">The game's internal API.</param>
        /// <param name="name">The name of the BlockEntity Behaviour to add to the registry.</param>
        public static void RegisterBlockEntityBehaviour<T>(this ICoreAPICommon api, string name = nameof(T))
        {
            api.RegisterBlockEntityBehaviorClass(name, typeof(T));
        }

        /// <summary>
        ///     Registers a new Block Behaviour. <br/>
        ///     Must happen before any blocks are loaded. <br/>
        ///     Must be registered on both the client, and server.
        /// </summary>
        /// <typeparam name="T">The type of the Block Behaviour to register.</typeparam>
        /// <param name="api">The game's internal API.</param>
        /// <param name="name">The name of the Block Behaviour to add to the registry.</param>
        public static void RegisterEntity<T>(this ICoreAPICommon api, string name = nameof(T))
        {
            api.RegisterEntity(name, typeof(T));
        }

        /// <summary>
        ///     Registers a new Entity Behaviour. <br/>
        ///     Must be registered on both the client, and server.
        /// </summary>
        /// <typeparam name="T">The type of the Entity Behaviour to register.</typeparam>
        /// <param name="api">The game's internal API.</param>
        /// <param name="name">The name of the Entity Behaviour to add to the registry.</param>
        public static void RegisterEntityBehaviour<T>(this ICoreAPICommon api, string name = nameof(T))
        {
            api.RegisterEntityBehaviorClass(name, typeof(T));
        }

        /// <summary>
        ///     Registers a new Crop Behaviour. <br/>
        ///     Must be registered on both the client, and server.
        /// </summary>
        /// <typeparam name="T">The type of the Crop Behaviour to register.</typeparam>
        /// <param name="api">The game's internal API.</param>
        /// <param name="name">The name of the Crop Behaviour to add to the registry.</param>
        public static void RegisterCropBehaviour<T>(this ICoreAPICommon api, string name = nameof(T))
        {
            api.RegisterCropBehavior(name, typeof(T));
        }

        /// <summary>
        ///     Registers a new Collectible Behaviour. <br/>
        ///     Must be registered on both the client, and server.
        /// </summary>
        /// <typeparam name="T">The type of the Collectible Behaviour to register.</typeparam>
        /// <param name="api">The game's internal API.</param>
        /// <param name="name">The name of the Collectible Behaviour to add to the registry.</param>
        public static void RegisterCollectibleBehaviour<T>(this ICoreAPICommon api, string name = nameof(T))
        {
            api.RegisterCollectibleBehaviorClass(name, typeof(T));
        }
    }
}