using System;
using System.Collections.Generic;
using System.IO;
using ApacheTech.VintageMods.Core.Services.FileSystem.Abstractions;
using ApacheTech.VintageMods.Core.Services.FileSystem.Enums;
using ApacheTech.VintageMods.Core.Services.FileSystem.FileAdaptors;

namespace ApacheTech.VintageMods.Core.Services.FileSystem.Extensions
{
    public static class FileTypeExtensions
    {
        private static readonly Dictionary<string, FileType> Types = new()
        {
            { ".json", FileType.Json },
            { ".data", FileType.Json },
            { ".dat", FileType.Binary },
            { ".bin", FileType.Binary },
            { ".dll", FileType.Binary },
            { ".txt", FileType.Text },
            { ".md", FileType.Text },
        };
        
        public static FileType ParseFileType(this FileSystemInfo file)
        {
            var extension = file.Extension;
            return Types.ContainsKey(extension) 
                ? Types[extension] 
                : FileType.Binary;
        }

        public static ModFileBase CreateModFileWrapper(this FileInfo file)
        {
            var fileType = ParseFileType(file);
            return fileType switch
            {
                FileType.Json => new JsonModFile(file),
                FileType.Binary => new BinaryModFile(file),
                FileType.Text => new TextModFile(file),
                _ => throw new ArgumentOutOfRangeException(nameof(fileType))
            };
        }
    }
}
