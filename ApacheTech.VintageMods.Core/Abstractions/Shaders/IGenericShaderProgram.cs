﻿using Vintagestory.API.Client;

namespace ApacheTech.VintageMods.Core.Abstractions.Shaders
{
    /// <summary>
    ///     Represents a generic shader program, that allows values to be passed to the shader, every frame.
    /// </summary>
    /// <seealso cref="IShaderProgram" />
    public interface IGenericShaderProgram : IShaderProgram
    {
        /// <summary>
        ///     Runs before every frame, to set the values for the shader pass.
        /// </summary>
        void UpdateUniforms();
    }
}