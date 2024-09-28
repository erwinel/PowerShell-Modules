#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Erwine.Leonard.T.GDIPlus.Palette.Ditherers.Ordered
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    public class BayerDitherer8 : BaseOrderedDitherer
    {
        /// <summary>
        /// See <see cref="BaseColorDitherer.CreateCoeficientMatrix"/> for more details.
        /// </summary>
        protected override byte[,] CreateCoeficientMatrix() => new byte[,]
            {
                {  1, 49, 13, 61,  4, 52, 16, 64 },
                { 33, 17, 45, 29, 36, 20, 48, 32 },
                {  9, 57,  5, 53, 12, 60,  8, 56 },
                { 41, 25, 37, 21, 44, 28, 40, 24 },
                {  3, 51, 15, 63,  2, 50, 14, 62 },
                { 35, 19, 47, 31, 34, 18, 46, 30 },
                { 11, 59,  7, 55, 10, 58,  6, 54 },
                { 43, 27, 39, 23, 42, 26, 38, 22 }
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
