using System;
using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.Abstractions.GameContent
{
    public abstract class BlockBehaviour<TBlock> : BlockBehavior where TBlock : Block
    {
        protected TBlock BlockInstance { get; }

        protected BlockBehaviour(Block block) : base(block)
        {
            if (block is not TBlock blockInstance)
            {
                throw new InvalidCastException("This behaviour cannot be applied to the specified block.");
            }
            BlockInstance = blockInstance;
        }
    }
}