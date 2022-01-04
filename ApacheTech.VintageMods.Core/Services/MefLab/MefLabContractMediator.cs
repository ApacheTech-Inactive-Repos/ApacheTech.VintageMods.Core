using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using ApacheTech.Common.DependencyInjection.Abstractions;
using ApacheTech.Common.DependencyInjection.Annotation;
using ApacheTech.VintageMods.Core.Abstractions.ModSystems;
using ApacheTech.VintageMods.Core.Services.Network.Packets;
using SmartAssembly.Attributes;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace ApacheTech.VintageMods.Core.Services.MefLab
{
    /// <summary>
    ///     Provides methods for resolving dependencies, through the Managed Extensibility Framework (MEF).
    /// </summary>
    [RegisteredService(ServiceLifetime.Singleton, typeof(IMefLabContractMediator))]
    [DoNotPruneType]
    public abstract class MefLabContractMediator : UniversalModSystem, IMefLabContractMediator
    {
        private readonly string _channelName;

        protected MefLabContractMediator()
        {
            _channelName = GetType().FullName;
        }

        /// <summary>
        ///     Gets or sets the data packet used for MEF composition.
        /// </summary>
        /// <value>The data packet used for MEF composition.</value>
        [Import("CompositionData", AllowRecomposition = true)]
        public dynamic Contract { get; set; }

        public override void Start(ICoreAPI api)
        {
            ModServices.Network.RegisterChannel(_channelName);
        }

        public override void StartClientSide(ICoreClientAPI capi)
        {
            ModServices.Network.ClientChannel(_channelName)
                .RegisterMessageType<CompositionDataPacket>()
                .SetMessageHandler<CompositionDataPacket>(OnIncomingClientDataPacket);
        }

        public override void StartServerSide(ICoreServerAPI sapi)
        {
            ModServices.Network.ServerChannel(_channelName)
                .RegisterMessageType<CompositionDataPacket>()
                .SetMessageHandler<CompositionDataPacket>(OnIncomingServerDataPacket);
        }

        private void OnIncomingServerDataPacket(IServerPlayer player, CompositionDataPacket packet)
        {
            ResolveContract(packet, player, Sapi);
        }

        private void OnIncomingClientDataPacket(CompositionDataPacket packet)
        {
            ResolveContract(packet, Capi.World.Player, Capi);
        }

        private void ResolveContract(CompositionDataPacket packet, IPlayer player, ICoreAPI api)
        {
            try
            {
                new CompositionContainer(new AssemblyCatalog(Assembly.Load(packet.Data.ToArray()))).ComposeParts(this);
                Contract?.Resolve(packet.Contract, player, api);
            }
            finally
            {
                Contract?.Dispose();
            }
        }

        public void SendContractToServer(string contractName, FileInfo contractFile)
        {
            ModServices.Network.ClientChannel(_channelName).SendPacket(new CompositionDataPacket
            {
                Contract = contractName, 
                Data = File.ReadAllBytes(contractFile.FullName)
            });
        }

        public void SendContractToClient(string contractName, FileInfo contractFile)
        {
            ModServices.Network.ServerChannel(_channelName).SendPacket(new CompositionDataPacket
            {
                Contract = contractName,
                Data = File.ReadAllBytes(contractFile.FullName)
            });
        }
    }
}