using Vintagestory.API.Client;

namespace ApacheTech.VintageMods.Core.Abstractions.Shaders
{
    public interface IGenericRenderer<TShaderProgram> : IRenderer where TShaderProgram : IGenericShaderProgram
    {
        TShaderProgram Shader { get; set; }
    }
}