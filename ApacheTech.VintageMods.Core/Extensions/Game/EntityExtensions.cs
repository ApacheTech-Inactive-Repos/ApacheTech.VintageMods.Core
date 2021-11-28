using System;
using JetBrains.Annotations;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace ApacheTech.VintageMods.Core.Extensions.Game
{
    /// <summary>
    ///     Extension methods for when working with entities.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class EntityExtensions
    {
        /// <summary>
        ///     Applies a force to an entity, for a given direction vector.
        /// </summary>
        /// <param name="entity">The entity to apply the force to.</param>
        /// <param name="forwardVec">The forward direction vector, in which to apply the force.</param>
        /// <param name="force">The amount of force to apply.</param>
        public static void ApplyForce(this Entity entity, Vec3d forwardVec, double force)
        {
            entity.Pos.Motion.X *= forwardVec.X * force;
            entity.Pos.Motion.Y *= forwardVec.Y * force;
            entity.Pos.Motion.Z *= forwardVec.Z * force;
        }

        /// <summary>
        ///     Changes the facing of a given agent, to face directly away from a target gameworld location.
        /// </summary>
        /// <param name="agentPos">The agent's position.</param>
        /// <param name="targetPos">The target position.</param>
        /// <returns>An <see cref="EntityPos"/>, containing the agent's current XYZ position, and the new YPR rotations.</returns>
        public static EntityPos LookAwayFrom(this EntityPos agentPos, Vec3d targetPos)
        {
            var cartesianCoordinates = targetPos.SubCopy(agentPos.XYZ).Normalize();
            var yaw = GameMath.TWOPI - (float) Math.Atan2(cartesianCoordinates.Z, cartesianCoordinates.X);
            var pitch = (float) Math.Asin(-cartesianCoordinates.Y);
            var entityPos = agentPos.Copy();
            entityPos.Yaw = (yaw + GameMath.PI) % GameMath.TWOPI;
            entityPos.Pitch = pitch;
            return entityPos;
        }

        /// <summary>
        ///     Changes the facing of a given agent, to face directly towards a target gameworld location.
        /// </summary>
        /// <param name="agentPos">The agent's position.</param>
        /// <param name="targetPos">The target position.</param>
        /// <returns>An <see cref="EntityPos"/>, containing the agent's current XYZ position, and the new YPR rotations.</returns>
        public static EntityPos LookAt(this EntityPos agentPos, Vec3d targetPos)
        {
            var cartesianCoordinates = targetPos.SubCopy(agentPos.XYZ).Normalize();
            var yaw = GameMath.TWOPI - (float) Math.Atan2(cartesianCoordinates.Z, cartesianCoordinates.X);
            var pitch = (float) Math.Asin(cartesianCoordinates.Y);
            var entityPos = agentPos.Copy();
            entityPos.Yaw = yaw % GameMath.TWOPI;
            entityPos.Pitch = GameMath.PI - pitch;
            return entityPos;
        }
    }
}