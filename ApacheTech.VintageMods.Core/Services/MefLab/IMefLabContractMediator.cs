using System.IO;

// ReSharper disable UnusedMember.Global

namespace ApacheTech.VintageMods.Core.Services.MefLab
{
    public interface IMefLabContractMediator
    {
        void SendContractToServer(string contractName, FileInfo contractFile);
        void SendContractToClient(string contractName, FileInfo contractFile);
    }
}
