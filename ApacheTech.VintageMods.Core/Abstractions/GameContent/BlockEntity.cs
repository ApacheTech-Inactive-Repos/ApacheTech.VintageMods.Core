using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.Abstractions.GameContent
{
    public class BlockEntity<TBlock> : BlockEntity where TBlock : Block
    {
        protected TBlock OwnerBlock => Block as TBlock;
    }
}