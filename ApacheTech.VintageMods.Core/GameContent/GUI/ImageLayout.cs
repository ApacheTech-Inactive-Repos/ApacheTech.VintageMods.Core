namespace ApacheTech.VintageMods.Core.GameContent.GUI
{
    public enum ImageLayout
    {
        None, // Original Size (Bound to Box, with Overflow Hidden [needs a clip])
        Tile, // Tile is default, nothing needs be done. (Cairo Default)
        Centre, // Original size, auto determine XY positions to draw from.
        Stretch, // Scaled based on size of bounding box. (Default)
        Zoom, // Generic scaling.
    }
}