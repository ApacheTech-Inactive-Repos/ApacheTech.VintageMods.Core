using ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions;
using ApacheTech.VintageMods.Core.Services.FileSystem.Enums;
using System.IO;

namespace ApacheTech.VintageMods.Core.Services.FileSystem.Registration
{
    internal interface IModFileRegistrar
    {
        void CopyFileToOutputDirectory(string fileName, FileInfo file, FileType fileType);
        string GetScopedPath(string fileName, FileScope scope);
        ModFileBase InstantiateModFile(FileType fileType, FileInfo file);
        FileType ParseFileType(FileSystemInfo file);
    }
}