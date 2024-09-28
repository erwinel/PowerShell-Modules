using System.Globalization;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Erwine.Leonard.T.GDIPlus
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    /// <summary>
    /// Model representing a color in terms of hue, saturation and brightness as floating-point values.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct HsbColorF : IEquatable<HsbColorF>, IEquatable<IHsbColorModel<byte>>, IEquatable<IRgbColorModel<byte>>, IHsbColorModel<float>
    {
        private readonly float _alpha, _hue, _saturation, _brightness;

        #region Properties

        /// <summary>
        /// The opaqueness of the color.
        /// </summary>
        public readonly float Alpha => _alpha;

        /// <summary>
        /// The hue of the color.
        /// </summary>
        public readonly float Hue => _hue;

        /// <summary>
        /// The color saturation.
        /// </summary>
        public readonly float Saturation => _saturation;

        /// <summary>
        /// The brightness of the color.
        /// </summary>
        public readonly float Brightness => _brightness;

        readonly bool IColorModel.IsNormalized => false;

        readonly ColorStringFormat IColorModel.DefaultStringFormat => ColorStringFormat.HSLAPercent;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hue"></param>
        /// <param name="saturation"></param>
        /// <param name="brightness"></param>
        /// <param name="alpha"></param>
        public HsbColorF(float hue, float saturation, float brightness, float alpha)
        {
            if (hue < 0f || hue > ColorExtensions.HUE_MAXVALUE)
                throw new ArgumentOutOfRangeException(nameof(hue));
            if (saturation < 0f || saturation > 1f)
                throw new ArgumentOutOfRangeException(nameof(saturation));
            if (brightness < 0f || brightness > 1f)
                throw new ArgumentOutOfRangeException(nameof(brightness));
            if (alpha < 0f || alpha > 1f)
                throw new ArgumentOutOfRangeException(nameof(alpha));
            _hue = (hue == ColorExtensions.HUE_MAXVALUE) ? 0f : hue;
            _saturation = saturation;
            _brightness = brightness;
            _alpha = alpha;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hue"></param>
        /// <param name="saturation"></param>
        /// <param name="brightness"></param>
        public HsbColorF(float hue, float saturation, float brightness) : this(hue, saturation, brightness, 1f) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public HsbColorF(HsbColor32 value)
        {
            _hue = value.Hue.ToDegrees();
            _saturation = value.Saturation.ToPercentage();
            _brightness = value.Brightness.ToPercentage();
            _alpha = value.Alpha.ToPercentage();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ahsb"></param>
        public HsbColorF(int ahsb)
        {
            byte[] values = BitConverter.GetBytes(ahsb);
            _brightness = values[0].ToPercentage();
            _saturation = values[1].ToPercentage();
            _hue = values[2].ToDegrees();
            _alpha = values[3].ToPercentage();
        }

        #endregion
        
        #region As* Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public readonly HsbColor32 AsHsb32() { return new HsbColor32(this); }

        readonly IHsbColorModel<byte> IColorModel.AsHsb32() { return AsHsb32(); }

        readonly IHsbColorModel<float> IColorModel.AsHsbF() { return this; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public readonly RgbColor32 AsRgb32() { return new RgbColor32(this); }

        readonly IRgbColorModel<byte> IColorModel.AsRgb32() { return AsRgb32(); }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public readonly RgbColorF AsRgbF() { return new RgbColorF(this); }

        readonly IRgbColorModel<float> IColorModel.AsRgbF() { return AsRgbF(); }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public readonly HsbColorFNormalized AsNormalized() { return new HsbColorFNormalized(this); }

        readonly IHsbColorModel<float> IHsbColorModel<float>.AsNormalized() { return AsNormalized(); }

        readonly IColorModel<float> IColorModel<float>.AsNormalized() { return AsNormalized(); }

        readonly IColorModel IColorModel.AsNormalized() { return AsNormalized(); }

        #endregion
        
        #region Equals Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <param name="exact"></param>
        /// <returns></returns>
        public readonly bool Equals(IRgbColorModel<float> other, bool exact)
        {
            if (other == null || _alpha != other.Alpha)
                return false;

            float b;
            if (exact)
            {
                ColorExtensions.RGBtoHSB(other.Red, other.Green, other.Blue, out float h, out float s, out b);
                return _hue == h && _saturation == s && _brightness == b;
            }

            ColorExtensions.HSBtoRGB(_hue, _saturation, _brightness, out float r, out float g, out b);
            return other.Red == r && other.Green == g && other.Blue == b;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <param name="exact"></param>
        /// <returns></returns>
        public readonly bool Equals(IRgbColorModel<byte> other, bool exact)
        {
            if (other == null)
                return false;

            if (exact)
            {
                if (other.Alpha.ToPercentage() != _alpha)
                    return false;
                ColorExtensions.RGBtoHSB(other.Red.ToPercentage(), other.Green.ToPercentage(), other.Blue.ToPercentage(), out float h, out float s, out float b);
                return _hue == h && _saturation == s && _brightness == b;
            }

            return AsNormalized().Equals(other, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <param name="exact"></param>
        /// <returns></returns>
        public readonly bool Equals(IHsbColorModel<byte> other, bool exact)
        {
            if (other == null)
                return false;
            if (exact)
                return _alpha == other.Alpha.ToPercentage() && _hue == other.Hue.ToDegrees() && _saturation == other.Saturation.ToPercentage() && _brightness == other.Brightness.ToPercentage();
            if (!other.IsNormalized)
                other = other.AsNormalized();
            return _alpha.FromPercentage() == other.Alpha && _hue.FromDegrees() == other.Hue && _saturation.FromPercentage() == other.Saturation && _brightness.FromPercentage() == other.Brightness;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <param name="exact"></param>
        /// <returns></returns>
        public readonly bool Equals(IColorModel other, bool exact)
        {
            if (other == null)
                return false;
            if (other is HsbColorF)
                return Equals((HsbColorF)other);
            if (other is IHsbColorModel<float>)
                return Equals((IHsbColorModel<float>)other);
            if (other is IHsbColorModel<byte>)
                return Equals((IHsbColorModel<byte>)other, exact);
            if (other is IRgbColorModel<float>)
                return Equals((IRgbColorModel<float>)other, exact);
            return other is IRgbColorModel<byte> && Equals((IRgbColorModel<byte>)other, exact);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly bool Equals(HsbColorF other) { return other._alpha == _alpha && other._hue == _hue && other._saturation   == _saturation && other._brightness == _brightness; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly bool Equals(IHsbColorModel<byte> other) { return Equals(other, false); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly bool Equals(IRgbColorModel<byte> other) { return Equals(other, false); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly bool Equals(IHsbColorModel<float> other) { return other.Alpha == _alpha && other.Hue == _hue && other.Saturation == _saturation && other.Brightness == _brightness; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly bool Equals(IRgbColorModel<float> other) { return Equals(other, false); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly bool Equals(IColorModel other) { return Equals(other, false); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly bool Equals(System.Drawing.Color other)
        {
            if (other.A != _alpha.FromPercentage())
                return false;
            ColorExtensions.HSBtoRGB(_hue, _saturation, _brightness, out float r, out float g, out float b);
            return r.FromPercentage() == other.R && g.FromPercentage() == other.G && b.FromPercentage() == other.B;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly bool Equals(System.Windows.Media.Color other)
        {
            if (other.A != _alpha.FromPercentage())
                return false;
            ColorExtensions.HSBtoRGB(_hue, _saturation, _brightness, out float r, out float g, out float b);
            return r.FromPercentage() == other.R && g.FromPercentage() == other.G && b.FromPercentage() == other.B;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override readonly bool Equals(object obj)
        {
            if (obj == null)
                return false;
            object value = (obj is PSObject) ? ((PSObject)obj).BaseObject : obj;
            if (value is HsbColorF)
                return Equals((HsbColorF)value);
            if (value is IHsbColorModel<byte>)
                return Equals((IHsbColorModel<byte>)value);
            if (value is IHsbColorModel<float>)
                return Equals((IHsbColorModel<float>)value, false);
            if (value is IRgbColorModel<byte>)
                return Equals((IRgbColorModel<byte>)value, false);
            if (value is IRgbColorModel<float>)
                return Equals((IRgbColorModel<float>)value, false);
            if (value is int)
                return Equals((int)value);
            value = ColorExtensions.AsSimplestType(value);

            if (value is string)
                return (string)value == ToString();

            if (value is int)
                return ToAHSB() == (int)value;

            if (value is float)
                return ToAHSB() == (float)value;

            if (obj is PSObject && ColorExtensions.TryGetColor((PSObject)obj, out IColorModel color))
                return Equals(color.AsHsbF());
            return false;
        }

        #endregion

        /// <summary>
        /// Returns the hash code for this value.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override readonly int GetHashCode() { return ToAHSB(); }

        #region MergeAverage Method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public readonly IHsbColorModel<float> MergeAverage(IEnumerable<IHsbColorModel<float>> other)
        {
            if (other == null)
                return this;

            ColorExtensions.HSBtoRGB(_hue, _saturation, _brightness, out float rF, out float gF, out float bF);
            double r = rF, g = gF, b = bF, a = _alpha;
            double brightness = _brightness;
            int count = 0;
            foreach (IHsbColorModel<float> item in other)
            {
                if (item != null)
                {
                    count++;
                    ColorExtensions.HSBtoRGB(item.Hue, item.Saturation, item.Brightness, out rF, out gF, out bF);
                    r += (double)rF;
                    g += (double)gF;
                    b += (double)bF;
                    a += (double)item.Alpha;
                    brightness += (double)item.Brightness;
                }
            }
            if (count == 0)
                return this;

            ColorExtensions.RGBtoHSB((float)(r / (double)count), (float)(r / (double)count), (float)(r / (double)count), out float h, out float s, out bF);
            return new HsbColorFNormalized(h, s, bF, (float)(a / (double)count));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public IHsbColorModel<float> MergeAverage(IEnumerable<IColorModel> other)
        {
            throw new NotImplementedException();
        }

        IColorModel IColorModel.MergeAverage(IEnumerable<IColorModel> other) { return MergeAverage(other); }

        #endregion
        
        #region ShiftHue Method

        /// <summary>
        /// Returns a <see cref="HsbColorF" /> value with the color hue adjusted.
        /// </summary>
        /// <param name="degrees">The number of degrees to shift the hue value, ranging from -360.0 to 360.0. A positive value shifts the hue in the red-to-cyan direction, and a negative value shifts the hue in the cyan-to-red direction.</param>
        /// <returns>A <see cref="HsbColorF" /> value with the color hue adjusted.</returns>
        /// <remarks>The values 0.0, -360.0 and 360.0 have no effect since they would result in no hue change.</remarks>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="degrees" /> is less than -360.0 or <paramref name="degrees" /> is greater than 360.0.</exception>
        public readonly HsbColorF ShiftHue(float degrees)
        {
            if (degrees < -360f || degrees > 360f)
                throw new ArgumentOutOfRangeException(nameof(degrees));
            if (degrees == 0f || degrees == 360f || degrees == -360f)
                return this;
            float hue = _hue + degrees;
            if (hue < 0f)
                hue += 360f;
            else if (hue >= 360f)
                hue -= 360f;
            return new HsbColorF(hue, _saturation, _brightness, _alpha);
        }

        readonly IHsbColorModel<float> IHsbColorModel<float>.ShiftHue(float degrees) { return ShiftHue(degrees); }

        readonly IColorModel<float> IColorModel<float>.ShiftHue(float degrees) { return ShiftHue(degrees); }

        readonly IColorModel IColorModel.ShiftHue(float degrees) { return ShiftHue(degrees); }

        #endregion
        
        #region ShiftSaturation Method

        /// <summary>
        /// Returns a <see cref="HsbColorF" /> value with the color saturation adjusted.
        /// </summary>
        /// <param name="percentage">The percentage to saturate the color, ranging from -1.0 to 1.0. A positive value increases saturation, a negative value decreases saturation and a zero vale has no effect.</param>
        /// <returns>A <see cref="HsbColorF" /> value with the color saturation adjusted.</returns>
        /// <remarks>For positive values, the target saturation value is determined using the following formula: <c>saturation + (1.0 - saturation) * percentage</c>
        /// <para>For negative values, the target saturation value is determined using the following formula: <c>saturation + saturation * percentage</c></para></remarks>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="percentage" /> is less than -1.0 or <paramref name="percentage" /> is greater than 1.0.</exception>
        public readonly HsbColorF ShiftSaturation(float percentage)
        {
            if (percentage < -1f || percentage > 1f)
                throw new ArgumentOutOfRangeException(nameof(percentage));
            if (percentage == 0f || (percentage == 1f) ? _saturation == 1f : percentage == -1f && _saturation == 0f)
                return this;
            return new HsbColorF(_hue, _saturation + ((percentage > 0f) ? (1f - _saturation) : _saturation) * percentage, _brightness, _alpha);
        }

        readonly IHsbColorModel<float> IHsbColorModel<float>.ShiftSaturation(float percentage) { return ShiftSaturation(percentage); }

        readonly IColorModel<float> IColorModel<float>.ShiftSaturation(float percentage) { return ShiftSaturation(percentage); }

        readonly IColorModel IColorModel.ShiftSaturation(float percentage) { return ShiftSaturation(percentage); }

        #endregion
        
        #region ShiftBrightness Method

        /// <summary>
        /// Returns a <see cref="HsbColorF" /> value with the color brightness adjusted.
        /// </summary>
        /// <param name="percentage">The percentage to saturate the color, ranging from -1.0 to 1.0. A positive value increases brightness, a negative value decreases brightness and a zero vale has no effect.</param>
        /// <returns>A <see cref="HsbColorF" /> value with the color brightness adjusted.</returns>
        /// <remarks>For positive values, the target brightness value is determined using the following formula: <c>brightness + (1.0 - brightness) * percentage</c>
        /// <para>For negative values, the target brightness value is determined using the following formula: <c>brightness + brightness * percentage</c></para></remarks>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="percentage" /> is less than -1.0 or <paramref name="percentage" /> is greater than 1.0.</exception>
        public readonly HsbColorF ShiftBrightness(float percentage)
        {
            if (percentage < -1f || percentage > 1f)
                throw new ArgumentOutOfRangeException(nameof(percentage));
            if (percentage == 0f || (percentage == 1f) ? _brightness == 1f : percentage == -1f && _brightness == 0f)
                return this;
            return new HsbColorF(_hue, _saturation, _brightness + ((percentage > 0f) ? (1f - _brightness) : _brightness) * percentage, _alpha);
        }

        readonly IHsbColorModel<float> IHsbColorModel<float>.ShiftBrightness(float percentage) { return ShiftBrightness(percentage); }

        readonly IColorModel<float> IColorModel<float>.ShiftBrightness(float percentage) { return ShiftBrightness(percentage); }

        readonly IColorModel IColorModel.ShiftBrightness(float percentage) { return ShiftBrightness(percentage); }

        #endregion

        /// <summary>
        /// Gets the AHSB integer value for the current <see cref="HsbColorF" /> value.
        /// </summary>
        /// <returns>The AHSB integer value for the current <see cref="HsbColorF" /> value.</returns>
        public readonly int ToAHSB() { return BitConverter.ToInt32([_brightness.FromPercentage(), _saturation.FromPercentage(), _hue.FromPercentage(), _alpha.FromPercentage()], 0); }

        #region ToString Methods

        /// <summary>
        /// Gets formatted string representing the current color value.
        /// </summary>
        /// <param name="format">The color string format to use.</param>
        /// <returns>The formatted string representing the current color value.</returns>
        public readonly string ToString(ColorStringFormat format)
        {
            float r, g, b;
            switch (format)
            {
                case ColorStringFormat.HSLAHex:
                    return HsbColor32.ToHexidecimalString(_hue.FromDegrees(), _saturation.FromPercentage(), _brightness.FromPercentage(), _alpha.FromPercentage(), false);
                case ColorStringFormat.HSLAHexOpt:
                    return HsbColor32.ToHexidecimalString(_hue.FromDegrees(), _saturation.FromPercentage(), _brightness.FromPercentage(), _alpha.FromPercentage(), true);
                case ColorStringFormat.HSLAValues:
                    return HsbColor32.ToValueParameterString(_hue.FromDegrees(), _saturation.FromPercentage(), _brightness.FromPercentage(), _alpha);
                case ColorStringFormat.HSLHex:
                    return HsbColor32.ToHexidecimalString(_hue.FromDegrees(), _saturation.FromPercentage(), _brightness.FromPercentage(), false);
                case ColorStringFormat.HSLHexOpt:
                    return HsbColor32.ToHexidecimalString(_hue.FromDegrees(), _saturation.FromPercentage(), _brightness.FromPercentage(), true);
                case ColorStringFormat.HSLPercent:
                    return HsbColorF.ToPercentParameterString(_hue, _saturation, _brightness);
                case ColorStringFormat.HSLValues:
                    return HsbColor32.ToValueParameterString(_hue.FromDegrees(), _saturation.FromPercentage(), _brightness.FromPercentage());
                case ColorStringFormat.RGBAHex:
                    ColorExtensions.HSBtoRGB(_hue, _saturation, _brightness, out r, out g, out b);
                    return RgbColor32.ToHexidecimalString(r.FromPercentage(), g.FromPercentage(), b.FromPercentage(), _alpha.FromPercentage(), false);
                case ColorStringFormat.RGBAHexOpt:
                    ColorExtensions.HSBtoRGB(_hue, _saturation, _brightness, out r, out g, out b);
                    return RgbColor32.ToHexidecimalString(r.FromPercentage(), g.FromPercentage(), b.FromPercentage(), _alpha.FromPercentage(), true);
                case ColorStringFormat.RGBAPercent:
                    ColorExtensions.HSBtoRGB(_hue, _saturation, _brightness, out r, out g, out b);
                    return RgbColorF.ToPercentParameterString(r, g, b, _alpha);
                case ColorStringFormat.RGBAValues:
                    ColorExtensions.HSBtoRGB(_hue, _saturation, _brightness, out r, out g, out b);
                    return RgbColor32.ToValueParameterString(r.FromPercentage(), g.FromPercentage(), b.FromPercentage(), _alpha.FromPercentage());
                case ColorStringFormat.RGBHex:
                    ColorExtensions.HSBtoRGB(_hue, _saturation, _brightness, out r, out g, out b);
                    return RgbColor32.ToHexidecimalString(r.FromPercentage(), g.FromPercentage(), b.FromPercentage(), false);
                case ColorStringFormat.RGBHexOpt:
                    ColorExtensions.HSBtoRGB(_hue, _saturation, _brightness, out r, out g, out b);
                    return RgbColor32.ToHexidecimalString(r.FromPercentage(), g.FromPercentage(), b.FromPercentage(), true);
                case ColorStringFormat.RGBPercent:
                    ColorExtensions.HSBtoRGB(_hue, _saturation, _brightness, out r, out g, out b);
                    return RgbColorF.ToPercentParameterString(r, g, b);
                case ColorStringFormat.RGBValues:
                    ColorExtensions.HSBtoRGB(_hue, _saturation, _brightness, out r, out g, out b);
                    return RgbColor32.ToValueParameterString(r.FromPercentage(), g.FromPercentage(), b.FromPercentage());
            }
            return HsbColorF.ToPercentParameterString(_hue, _saturation, _brightness, _alpha);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override readonly string ToString(){ return HsbColorF.ToPercentParameterString(_hue, _saturation, _brightness, _alpha); }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="h"></param>
        /// <param name="s"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public static string ToPercentParameterString(float h, float s, float b, float a)
        {
            return "hsla(" + Math.Round(h, 0).ToString() + ", " + Math.Round(Convert.ToDouble(s * 100f), 0).ToString() + "%, " +
                Math.Round(Convert.ToDouble(b * 100f), 0).ToString() + "%, " + Math.Round(Convert.ToDouble(a * 100f), 0).ToString() + "%)";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="h"></param>
        /// <param name="s"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ToPercentParameterString(float h, float s, float b)
        {
            return "hsl(" + Math.Round(h, 0).ToString() + ", " + Math.Round(Convert.ToDouble(s * 100f), 0).ToString() + "%, " +
                Math.Round(Convert.ToDouble(b * 100f), 0).ToString() + "%)";
        }

        #endregion
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse(string text, out HsbColorF result) { return TryParse(text, false, out result); }

        internal static bool TryParse(string text, bool strict, out HsbColorF result)
        {
            if (text != null && (text = text.Trim()).Length > 0)
            {
                Match match = HsbColor32.ParseRegex.Match(text);
                if (match.Success)
                {
                    try
                    {
                        if (match.Groups["h"].Success)
                        {
                            result = text.Length switch
                            {
                                3 => new HsbColorF(int.Parse(new string(new char[] { text[0], text[0], text[1], text[1], text[2], text[2] }), NumberStyles.HexNumber) << 8),
                                4 => new HsbColorF(int.Parse(new string(new char[] { text[0], text[0], text[1], text[1], text[2], text[2] }), NumberStyles.HexNumber) << 8 | int.Parse(new string(new char[] { text[3], text[3] }))),
                                8 => new HsbColorF(int.Parse(text.Substring(0, 6), NumberStyles.HexNumber) << 8 | int.Parse(text.Substring(6), NumberStyles.HexNumber)),
                                _ => new HsbColorF(int.Parse(text, NumberStyles.HexNumber) << 8),
                            };
                            return true;
                        }
                        
                        float alpha = 100f;
                        if (!match.Groups["a"].Success || ((match.Groups["a"].Value.EndsWith("%") ? (float.TryParse(match.Groups["a"].Value.Substring(0, match.Groups["a"].Length - 1), out alpha) && (alpha = alpha / 100f) <= 1f) : (float.TryParse(match.Groups["a"].Value, out alpha) && alpha <= 1f)) && alpha >= 0f))
                        {
                            if (match.Groups["b"].Success)
                            {
                                if (int.TryParse(match.Groups["h"].Value, out int h) && h > -1 && h < 256 && int.TryParse(match.Groups["s"].Value, out int s) && s > -1 && s < 256 &&
                                    int.TryParse(match.Groups["l"].Value, out int l) && l > -1 && l < 256)
                                {
                                    result = new HsbColorF(((byte)h).ToDegrees(), ((byte)s).ToPercentage(), ((byte)l).ToPercentage(), alpha);
                                    return true;
                                }
                            }
                            else if (float.TryParse(match.Groups["h"].Value, out float hF) && hF >= 0f && hF <= 360f && float.TryParse(match.Groups["s"].Value, out float sF) && sF >= 0f && sF <= 100f && float.TryParse(match.Groups["l"].Value, out float lF) && lF >= 0f && lF <= 100f)
                            {
                                result = new HsbColorF(hF, sF / 100f, lF / 100f, alpha);
                                return true;
                            }
                        }
                    }
                    catch { }
                }
                else if (!strict && RgbColorF.TryParse(text, true, out RgbColorF rgb))
                {
                    ColorExtensions.RGBtoHSB(rgb.Red, rgb.Blue, rgb.Green, out float h, out float s, out float b);
                    result = new HsbColorF(h, s, b, rgb.Alpha);
                    return true;
                }
            }
            
            result = default(HsbColorF);
            return false;
        }
    }
}
