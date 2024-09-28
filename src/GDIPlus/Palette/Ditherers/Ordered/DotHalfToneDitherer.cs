#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Erwine.Leonard.T.GDIPlus.Palette.Ditherers.Ordered
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    public class DotHalfToneDitherer : BaseOrderedDitherer
    {
        /// <summary>
        /// See <see cref="BaseColorDitherer.CreateCoeficientMatrix"/> for more details.
        /// </summary>
        protected override byte[,] CreateCoeficientMatrix() => new byte[,]
            {
                { 25,  9, 23, 31, 35, 45, 43, 33 },
                { 11,  1,  7, 21, 47, 59, 57, 41 },
                { 13,  3,  5, 19, 49, 61, 63, 55 },
                { 27, 15, 17, 29, 37, 51, 53, 39 },
                { 36, 46, 44, 34, 26, 10, 24, 32 },
                { 48, 60, 58, 42, 12,  2,  8, 22 },
                { 50, 62, 64, 56, 14,  4,  6, 20 },
                { 38, 52, 54, 40, 28, 16, 18, 30 }
            };

        /// <summary>
        /// See <see cref="BaseOrderedDitherer.MatrixWidth"/> for more details.
        /// </summary>
        protected override byte MatrixWidth => 8;

        /// <summary>
        /// See <see cref="BaseOrderedDitherer.MatrixHeight"/> for more details.
        /// </summary>
        protected override byte MatrixHeight => 8;
    }
}
