using System.Drawing.Imaging;

namespace Erwine.Leonard.T.GDIPlus.Palette.Extensions
{
    /// <summary>
    /// The utility extender class.
    /// </summary>
    public static partial class Extend
    {
        /// <summary>
        /// Gets the bit count for a given pixel format.
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <returns>The bit count.</returns>
        public static byte GetBitDepth(this PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format1bppIndexed: 
                    return 1;

                case PixelFormat.Format4bppIndexed: 
                    return 4;

                case PixelFormat.Format8bppIndexed: 
                    return 8;

                case PixelFormat.Format16bppArgb1555:
                case PixelFormat.Format16bppGrayScale:
                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                    return 16;

                case PixelFormat.Format24bppRgb: 
                    return 24;

                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    return 32;

                case PixelFormat.Format48bppRgb: 
                    return 48;

                case PixelFormat.Format64bppArgb:
                case PixelFormat.Format64bppPArgb:
                    return 64;

                default:
                    string message = string.Format("A pixel format '{0}' not supported!", pixelFormat);
                    throw new NotSupportedException(message);
            }
        }

        /// <summary>
        /// Gets the available color count for a given pixel format.
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <returns>The available color count.</returns>
        public static ushort GetColorCount(this PixelFormat pixelFormat)
        {
            // checks whether a pixel format is indexed, otherwise throw an exception
            if (!pixelFormat.IsIndexed())
            {
                string message = string.Format("Cannot retrieve color count for a non-indexed format '{0}'.", pixelFormat);
                throw new NotSupportedException(message);
            }

            switch (pixelFormat)
            {
                case PixelFormat.Format1bppIndexed:
                    return 2;

                case PixelFormat.Format4bppIndexed:
                    return 16;

                case PixelFormat.Format8bppIndexed:
                    return 256;

                default:
                    string message = string.Format("A pixel format '{0}' not supported!", pixelFormat);
                    throw new NotSupportedException(message);
            }
        }

        /// <summary>
        /// Gets the friendly name of the pixel format.
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <returns></returns>
        public static string GetFriendlyName(this PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format1bppIndexed:
                    return "Indexed (2 colors)";

                case PixelFormat.Format4bppIndexed:
                    return "Indexed (16 colors)";

                case PixelFormat.Format8bppIndexed:
                    return "Indexed (256 colors)";

                case PixelFormat.Format16bppGrayScale:
                    return "Grayscale (65536 shades)";

                case PixelFormat.Format16bppArgb1555:
                    return "Highcolor + Alpha mask (32768 colors)";

                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                    return "Highcolor (65536 colors)";

                case PixelFormat.Format24bppRgb:
                    return "Truecolor (24-bit)";

                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                    return "Truecolor + Alpha (32-bit)";

                case PixelFormat.Format32bppRgb:
                    return "Truecolor (32-bit)";

                case PixelFormat.Format48bppRgb:
                    return "Truecolor (48-bit)";

                case PixelFormat.Format64bppArgb:
                case PixelFormat.Format64bppPArgb:
                    return "Truecolor + Alpha (64-bit)";

                default:
                    string message = string.Format("A pixel format '{0}' not supported!", pixelFormat);
                    throw new NotSupportedException(message);
            }
        }

        /// <summary>
        /// Determines whether the specified pixel format is indexed.
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <returns>
        /// 	<c>true</c> if the specified pixel format is indexed; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsIndexed(this PixelFormat pixelFormat) => (pixelFormat & PixelFormat.Indexed) == PixelFormat.Indexed;

        /// <summary>
        /// Determines whether the specified pixel format is supported.
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <returns>
        /// 	<c>true</c> if the specified pixel format is supported; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSupported(this PixelFormat pixelFormat) => pixelFormat switch
        {
            PixelFormat.Format1bppIndexed or PixelFormat.Format4bppIndexed or PixelFormat.Format8bppIndexed or PixelFormat.Format16bppArgb1555 or PixelFormat.Format16bppRgb555 or PixelFormat.Format16bppRgb565 or PixelFormat.Format24bppRgb or PixelFormat.Format32bppArgb or PixelFormat.Format32bppPArgb or PixelFormat.Format32bppRgb or PixelFormat.Format48bppRgb or PixelFormat.Format64bppArgb or PixelFormat.Format64bppPArgb => true,
            _ => false,
        };

        /// <summary>
        /// Gets the format by color count.
        /// </summary>
        public static PixelFormat GetFormatByColorCount(int colorCount)
        {
            if (colorCount <= 0 || colorCount > 256)
            {
                string message = string.Format("A color count '{0}' not supported!", colorCount);
                throw new NotSupportedException(message);
            }

            PixelFormat result = PixelFormat.Format1bppIndexed;

            if (colorCount > 16)
            {
                result = PixelFormat.Format8bppIndexed;
            }
            else if (colorCount > 2)
            {
                result = PixelFormat.Format4bppIndexed;
            }

            return result;
        }

        /// <summary>
        /// Determines whether the specified pixel format has an alpha channel.
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <returns>
        /// 	<c>true</c> if the specified pixel format has an alpha channel; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasAlpha(this PixelFormat pixelFormat) => (pixelFormat & PixelFormat.Alpha) == PixelFormat.Alpha ||
                   (pixelFormat & PixelFormat.PAlpha) == PixelFormat.PAlpha;

        /// <summary>
        /// Determines whether [is deep color] [the specified pixel format].
        /// </summary>
        /// <param name="pixelFormat">The pixel format.</param>
        /// <returns>
        /// 	<c>true</c> if [is deep color] [the specified pixel format]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDeepColor(this PixelFormat pixelFormat) => pixelFormat switch
        {
            PixelFormat.Format16bppGrayScale or PixelFormat.Format48bppRgb or PixelFormat.Format64bppArgb or PixelFormat.Format64bppPArgb => true,
            _ => false,
        };
    }
}


