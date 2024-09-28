using Erwine.Leonard.T.GDIPlus.Palette.Helpers;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Erwine.Leonard.T.GDIPlus.Palette.Ditherers.Ordered
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    public abstract class BaseOrderedDitherer : BaseColorDitherer
    {
        #region | Properties |

        /// <summary>
        /// Gets the width of the matrix.
        /// </summary>
        protected abstract byte MatrixWidth { get; }

        /// <summary>
        /// Gets the height of the matrix.
        /// </summary>
        protected abstract byte MatrixHeight { get; }

        #endregion

        #region << BaseColorDitherer >>

        /// <summary>
        /// See <see cref="BaseColorDitherer.OnProcessPixel"/> for more details.
        /// </summary>
        protected override bool OnProcessPixel(Pixel sourcePixel, Pixel targetPixel)
        {
            // reads the source pixel
            Color oldColor = ImageBuffer.GetColorFromPixel(sourcePixel);

            // converts alpha to solid color
            oldColor = QuantizationHelper.ConvertAlpha(oldColor);

            // retrieves matrix coordinates
            int x = targetPixel.X % MatrixWidth;
            int y = targetPixel.Y % MatrixHeight;

            // determines the threshold
            int threshold = Convert.ToInt32(CachedMatrix[x, y]);

            // only process dithering if threshold is substantial
            if (threshold > 0)
            {
                int red = GetClampedColorElement(oldColor.R + threshold);
                int green = GetClampedColorElement(oldColor.G + threshold);
                int blue = GetClampedColorElement(oldColor.B + threshold);

                Color newColor = Color.FromArgb(255, red, green, blue);

                if (TargetBuffer.IsIndexed)
                {
                    byte newPixelIndex = (byte) Quantizer.GetPaletteIndex(newColor, targetPixel.X, targetPixel.Y);
                    targetPixel.Index = newPixelIndex;
                }
                else
                {
                    targetPixel.Color = newColor;
                }
            }

            // writes the process pixel
            return true;
        }

        #endregion

        #region << IColorDitherer >>

        /// <summary>
        /// See <see cref="IColorDitherer.IsInplace"/> for more details.
        /// </summary>
        public override bool IsInplace => true;

        #endregion
    }
}
