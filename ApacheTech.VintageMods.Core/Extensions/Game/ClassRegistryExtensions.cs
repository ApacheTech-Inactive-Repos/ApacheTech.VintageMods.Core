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
        /// <typeparam name="TItemClass">The type of the Item to register.</typeparam>
        /// <param name="api">The game's internal API.</param>
        /// <param name="name">The name of the Item to add to the registry.</param>
        public static void RegisterItem<TItemClass>(this ICoreAPICommon api, string name = nameof(TItemClass))
        {
            api.RegisterItemClass(name, typeof(TItemClass));
        }

        /// <summary>
        ///     Registers a new Block class. <br/>
        ///     Must happen before any blocks are loaded. <br/>
        ///     Must be registered on both the client, and server.
        /// </summary>
        /// <typeparam name="TItemClass">The type of the Block to register.</typeparam>
        /// <param name="api">The game's internal API.</param>
        /// <param name="name">The name of the Block to add to the registry.</param>
        public static void RegisterBlock<TItemClass>(this ICoreAPICommon api, string name = nameof(TItemClass))
        {
            api.RegisterBlockClass(name, typeof(TItemClass));
        }

        /// <summary>
        ///     Registers a new Block class. <br/>
        ///     Must happen before any blocks are loaded. <br/>
        ///     Must be registered on both the client, and server.
        /// </summary>
        /// <typeparam name="TItemClass">The type of the Block to register.</typeparam>
        /// <param name="api">The game's internal API.</param>
        /// <param name="name">The name of the Block to add to the registry.</param>
        public static void RegisterBlockEntity<TItemClass>(this ICoreAPICommon api, string name = nameof(TItemClass))
        {
            api.RegisterBlockEntityClass(name, typeof(TItemClass));
        }

        /// <summary>
        ///     Registers a new Block Behaviour. <br/>
        ///     Must happen before any blocks are loaded. <br/>
        ///     Must be registered on both the client, and server.
        /// </summary>
        /// <typeparam name="TItemClass">The type of the Block Behaviour to register.</typeparam>
        /// <param name="api">The game's internal API.</param>
        /// <param name="name">The name of the Block Behaviour to add to the registry.</param>
        public static void RegisterBlockBehaviour<TItemClass>(this ICoreAPICommon api, string name = nameof(TItemClass))
        {
            api.RegisterBlockBehaviorClass(name, typeof(TItemClass));
        }

        /// <summary>
        ///     Registers a new BlockEntity Behaviour. <br/>
        ///     Must happen before any blocks are loaded. <br/>
        ///     Must be registered on both the client, and server.
        /// </summary>
        /// <typeparam name="TItemClass">The type of the BlockEntity Behaviour to register.</typeparam>
        /// <param name="api">The game's internal API.</param>
        /// <param name="name">The name of the BlockEntity Behaviour to add to the registry.</param>
        public static void RegisterBlockEntityBehaviour<TItemClass>(this ICoreAPICommon api, string name = nameof(TItemClass))
        {
            api.RegisterBlockEntityBehaviorClass(name, typeof(TItemClass));
        }

        /// <summary>
        ///     Registers a new Block Behaviour. <br/>
        ///     Must happen before any blocks are loaded. <br/>
        ///     Must be registered on both the client, and server.
        /// </summary>
        /// <typeparam name="TItemClass">The type of the Block Behaviour to register.</typeparam>
        /// <param name="api">The game's internal API.</param>
        /// <param name="name">The name of the Block Behaviour to add to the registry.</param>
        public static void RegisterEntity<TItemClass>(this ICoreAPICommon api, string name = nameof(TItemClass))
        {
            api.RegisterEntity(name, typeof(TItemClass));
        }

        /// <summary>
        ///     Registers a new Entity Behaviour. <br/>
        ///     Must be registered on both the client, and server.
        /// </summary>
        /// <typeparam name="TItemClass">The type of the Entity Behaviour to register.</typeparam>
        /// <param name="api">The game's internal API.</param>
        /// <param name="name">The name of the Entity Behaviour to add to the registry.</param>
        public static void RegisterEntityBehaviour<TItemClass>(this ICoreAPICommon api, string name = nameof(TItemClass))
        {
            api.RegisterEntityBehaviorClass(name, typeof(TItemClass));
        }

        /// <summary>
        ///     Registers a new Crop Behaviour. <br/>
        ///     Must be registered on both the client, and server.
        /// </summary>
        /// <typeparam name="TItemClass">The type of the Crop Behaviour to register.</typeparam>
        /// <param name="api">The game's internal API.</param>
        /// <param name="name">The name of the Crop Behaviour to add to the registry.</param>
        public static void RegisterCropBehaviour<TItemClass>(this ICoreAPICommon api, string name = nameof(TItemClass))
        {
            api.RegisterCropBehavior(name, typeof(TItemClass));
        }

        /// <summary>
        ///     Registers a new Collectible Behaviour. <br/>
        ///     Must be registered on both the client, and server.
        /// </summary>
        /// <typeparam name="TItemClass">The type of the Collectible Behaviour to register.</typeparam>
        /// <param name="api">The game's internal API.</param>
        /// <param name="name">The name of the Collectible Behaviour to add to the registry.</param>
        public static void RegisterCollectibleBehaviour<TItemClass>(this ICoreAPICommon api, string name = nameof(TItemClass))
        {
            api.RegisterCollectibleBehaviorClass(name, typeof(TItemClass));
        }
    }
}