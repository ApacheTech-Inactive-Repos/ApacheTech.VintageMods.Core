namespace ApacheTech.VintageMods.Core.Libraries.Maths.Interpolation
{
    public interface IInterpolator
    {
        double ValueAt(double mu, int pointIndex, int pointIndexNext);
    }
}