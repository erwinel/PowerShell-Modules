#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Erwine.Leonard.T.GDIPlus.Palette.Ditherers.ErrorDiffusion
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    public class TwoRowSierraDitherer : BaseErrorDistributionDitherer
    {
        /// <summary>
        /// See <see cref="BaseColorDitherer.CreateCoeficientMatrix"/> for more details.
        /// </summary>
        protected override byte[,] CreateCoeficientMatrix() => new byte[,]
            {
                { 0, 0, 0, 0, 0 },
                { 0, 0, 0, 4, 3 },
                { 1, 2, 3, 2, 1 }
            };

        /// <summary>
        /// See <see cref="BaseErrorDistributionDitherer.MatrixSideWidth"/> for more details.
        /// </summary>
        protected override int MatrixSideWidth => 2;

        /// <summary>
        /// See <see cref="BaseErrorDistributionDitherer.MatrixSideHeight"/> for more details.
        /// </summary>
        protected override int MatrixSideHeight => 1;
    }
}
