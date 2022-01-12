using System;
using Vintagestory.API.Common;

namespace ApacheTech.VintageMods.Core.Abstractions.Behaviours
{
    public abstract class BlockEntityBehaviour<TBlockEntity> : BlockEntityBehavior where TBlockEntity: BlockEntity
    {
        protected TBlockEntity Entity { get; }

        protected BlockEntityBehaviour(BlockEntity blockEntity) : base(blockEntity)
        {
            if (blockEntity is not TBlockEntity entity)
            {
                throw new InvalidCastException("This behaviour cannot be applied to the specified block entity.");
            }
            Entity = entity;
        }
    }
}