namespace ApacheTech.VintageMods.Core.Annotation.Attributes
{
    public interface IVintageModInfo
    {
        string LatestVersion { get; set; }
        string ModId { get; set; }
        string ModName { get; set; }
        string RootDirectoryName { get; set; }
    }
}