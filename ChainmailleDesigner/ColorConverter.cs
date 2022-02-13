// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: ColorConverter.cs


// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License, version 3, as
// published by the Free Software Foundation.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.


using System;

// Tuples for colors in specific color spaces.
using HslColor = System.Tuple<int, int, int>;
using NHslColor = System.Tuple<double, double, double>;
using LabColor = System.Tuple<double, double, double>;
using RgbColor = System.Tuple<int, int, int>;
using NRgbColor = System.Tuple<double, double, double>;
using SRgbColor = System.Tuple<double, double, double>;
using XyzColor = System.Tuple<double, double, double>;
using FXyzColor = System.Tuple<double, double, double>;
using NXyzColor = System.Tuple<double, double, double>;

namespace ChainmailleDesigner
{
  /// <summary>
  /// Converts colors from one color space to another.
  /// </summary>
  public static class ColorConverter
  {
    // D65 white in CIE X,Y,Z color space.
    public static readonly XyzColor D65XYZ =
      new Tuple<double, double, double>(0.950489, 1.0, 1.088840);
    // D50 white in CIE X,Y,Z color space.
    public static readonly XyzColor D50XYZ =
      new Tuple<double, double, double>(0.964212, 1.0, 0.825188);
    // Constant for normalized RGB.
    public const double maxRGB = 255.0;
    // Constants for sRGB conversions.
    public const double srgbAlpha = 0.055;
    public const double srgbAlpha1 = 1.0 + srgbAlpha;
    public const double srgbAlpha1I = 1.0 / srgbAlpha1;
    public const double srgbBeta = 12.92;
    public const double srgbBetaI = 1.0 / srgbBeta;
    public const double srgbGamma = 2.4;
    public const double srgbDelta = 0.04045;
    public const double srgbEpsilon = 0.0031308;
    public const double srgbZeta = 1.0 / 2.4;
    // Constants for fXYZ convertions.
    public const double fxyzAlpha = 500.0;
    public const double fxyzBeta = 200.0;
    public const double fxyzGamma = 116.0;
    public const double fxyzDelta = 6.0 / 29.0;
    public const double fxyzEpsilon = 16.0;
    public const double fxyzZeta = 4.0 / 29.0;
    public const double fxyzEta = fxyzDelta * fxyzDelta * fxyzDelta;
    public const double fxyzTheta = 1.0 / 3.0;
    public const double fxyzIota = 1.0 / fxyzKappa;
    public const double fxyzKappa = 3.0 * fxyzDelta * fxyzDelta;

    /// <summary>
    /// Determines the distance between two colors (CIE delta-E 2000).
    /// Input colors must each be a CIE L*a*b* triple.
    /// cf. Wikipedia "Color difference".
    /// </summary>
    /// <param name="lab1"></param>
    /// <param name="lab2"></param>
    /// <returns></returns>
    public static double ColorDifference(LabColor lab1, LabColor lab2)
    {
      // kL is unity for graphic arts; use 2.0 for textiles.
      // Higher values of kL de-emphasize luminance differences.
      return ColorDifference(lab1, lab2, 1.0);
    }

    /// <summary>
    /// Determines the distance between two colors (CIE delta-E 2000).
    /// Input colors must each be a CIE L*a*b* triple.
    /// cf. Wikipedia "Color difference".
    /// </summary>
    /// <param name="lab1"></param>
    /// <param name="lab2"></param>
    /// <param name="kL">Unity for graphic arts; use 2.0 for textiles.
    /// Higher values of kL de-emphasize luminance differences.</param>
    /// <returns></returns>
    public static double ColorDifference(LabColor lab1, LabColor lab2, double kL)
    {
      double result = 0.0;

      double deltaLPrime = lab2.Item1 - lab1.Item1;
      double barL = 0.5 * (lab1.Item1 + lab2.Item1);
      double c1 = MathUtils.RootSumSquares(lab1.Item2, lab1.Item3);
      double c2 = MathUtils.RootSumSquares(lab2.Item2, lab2.Item3);
      double barC = 0.5 * (c1 + c2);
      double barC7 = Math.Pow(barC, 7.0);
      double rootBarC7 = Math.Sqrt(barC7 / (barC7 + Math.Pow(25.0, 7.0)));
      double aPrime1 = lab1.Item2 * (1.0 + 0.5 * (1.0 - rootBarC7));
      double aPrime2 = lab2.Item2 * (1.0 + 0.5 * (1.0 - rootBarC7));
      double cPrime1 = MathUtils.RootSumSquares(aPrime1, lab1.Item3);
      double cPrime2 = MathUtils.RootSumSquares(aPrime2, lab2.Item3);
      double deltaCPrime = cPrime2 - cPrime1;
      double barCPrime = 0.5 * (cPrime1 + cPrime2);
      double hPrime1 = MathUtils.NormalizeDegrees(cPrime1 != 0.0 ?
        UnitConverter.ConvertValue(Math.Atan2(lab1.Item3, aPrime1),
        Units.Radians, Units.DegreesOfArc) : 0.0);
      double hPrime2 = MathUtils.NormalizeDegrees(cPrime2 != 0.0 ?
        UnitConverter.ConvertValue(Math.Atan2(lab2.Item3, aPrime2),
        Units.Radians, Units.DegreesOfArc) : 0.0);
      double diffHPrime = Math.Abs(hPrime1 - hPrime2);
      double deltaHPrime = hPrime2 - hPrime1;
      if (diffHPrime > 180.0)
      {
        deltaHPrime += hPrime2 <= hPrime1 ? 360.0 : -360.0;
      }
      if (cPrime1 == 0.0 || cPrime2 == 0.0)
      {
        deltaHPrime = 0.0;
      }
      double deltaBigHPrime = 2.0 * Math.Sqrt(cPrime1 * cPrime2) *
        Math.Sin(0.5 * UnitConverter.ConvertValue(
          deltaHPrime, Units.DegreesOfArc, Units.Radians));
      double sumHPrime = hPrime1 + hPrime2;
      double barBigHPrime = sumHPrime;
      if (cPrime1 != 0.0 && cPrime2 != 0.0)
      {
        barBigHPrime *= 0.5;
        if (diffHPrime > 180.0)
        {
          barBigHPrime += sumHPrime < 360.0 ? 180.0 : -180;
        }
      }
      double bigT = 1.0 -
        0.17 * Math.Cos(UnitConverter.ConvertValue(
          barBigHPrime - 30.0, Units.DegreesOfArc, Units.Radians)) +
        0.24 * Math.Cos(UnitConverter.ConvertValue(
          2.0 * barBigHPrime, Units.DegreesOfArc, Units.Radians)) +
        0.32 * Math.Cos(UnitConverter.ConvertValue(
          3.0 * barBigHPrime + 6.0, Units.DegreesOfArc, Units.Radians)) -
        0.20 * Math.Cos(UnitConverter.ConvertValue(
          4.0 * barBigHPrime - 63.0, Units.DegreesOfArc, Units.Radians));
      double midL2 = MathUtils.Square(barL - 50.0);
      double sL = 1.0 + 0.015 * midL2 / Math.Sqrt(20.0 + midL2);
      double sC = 1.0 + 0.045 * barCPrime;
      double sH = 1.0 + 0.015 * barCPrime * bigT;
      double barCPrime7 = Math.Pow(barCPrime, 7.0);
      double rootBarCPrime7 = Math.Sqrt(barCPrime7 /
        (barCPrime7 + Math.Pow(25.0, 7.0)));
      double rT = -2.0 * rootBarCPrime7 * Math.Sin(UnitConverter.ConvertValue(
        60.0 * Math.Exp(-MathUtils.Square((barBigHPrime - 275.0) / 25.0)),
        Units.DegreesOfArc, Units.Radians));
      double kC = 1.0; // "usually unity"
      double kH = 1.0; // "usually unity"
      result = Math.Sqrt(
        MathUtils.Square(deltaLPrime / (kL * sL)) +
        MathUtils.Square(deltaCPrime / (kC * sC)) +
        MathUtils.Square(deltaBigHPrime / (kH * sH)) +
        rT * deltaCPrime * deltaBigHPrime / (kC * sC * kH * sH));

      return result;
    }

    /// <summary>
    /// Converts HSL to RGB.
    /// Code from Guillaume Leparmentier, "Manipulating colors in .NET".
    /// Slightly adapted for integer arguments.
    /// </summary>
    /// <param name="hsl">HSL color triad. H full scale is 360. S & L full
    /// scales are 240.</param>
    /// <returns>RGB color triad. Each element full scale is 255.</returns>
    public static RgbColor HslToRgb(HslColor hsl)
    {
      double h = hsl.Item1;
      double s = hsl.Item2 / 240.0;
      double l = hsl.Item3 / 240.0;
      if (s == 0)
      {
        // Achromatic color (gray)
        return DenormalizeRgb(new NRgbColor(l, l, l));
      }
      else
      {
        double q = (l < 0.5) ? (l * (1.0 + s)) : (l + s - (l * s));
        double p = (2.0 * l) - q;

        double Hk = h / 360.0;
        double[] T = new double[3];
        T[0] = Hk + (1.0 / 3.0);    // Tr
        T[1] = Hk;                // Tb
        T[2] = Hk - (1.0 / 3.0);    // Tg

        for (int i = 0; i < 3; i++)
        {
          if (T[i] < 0) T[i] += 1.0;
          if (T[i] > 1) T[i] -= 1.0;

          if ((T[i] * 6) < 1)
          {
            T[i] = p + ((q - p) * 6.0 * T[i]);
          }
          else if ((T[i] * 2.0) < 1) //(1.0/6.0)<=T[i] && T[i]<0.5
          {
            T[i] = q;
          }
          else if ((T[i] * 3.0) < 2) // 0.5<=T[i] && T[i]<(2.0/3.0)
          {
            T[i] = p + (q - p) * ((2.0 / 3.0) - T[i]) * 6.0;
          }
          else T[i] = p;
        }

        return DenormalizeRgb(new NRgbColor(T[0], T[1], T[2]));
      }
    }

    /// <summary>
    /// Converts S, L components from the range [0, 1] to the range [0, 240].
    /// H is assumed to remain in degrees [0, 360).
    /// </summary>
    /// <param name="nHSL"></param>
    /// <returns></returns>
    public static HslColor DenormalizeHsl(NHslColor nHSL)
    {
      return new HslColor(
        Convert.ToInt32(Double.Parse(String.Format("{0:0.00}", nHSL.Item1))),
        Convert.ToInt32(Double.Parse(String.Format("{0:0.00}", nHSL.Item2 * 240.0))),
        Convert.ToInt32(Double.Parse(String.Format("{0:0.00}", nHSL.Item3 * 240.0))));
    }

    /// <summary>
    /// Converts R, G, B components from the range [0, 1] to the range [0, 255].
    /// </summary>
    /// <param name="nHSL"></param>
    /// <returns></returns>
    public static RgbColor DenormalizeRgb(NRgbColor nRGB)
    {
      return new RgbColor(
        Convert.ToInt32(Double.Parse(String.Format("{0:0.00}", nRGB.Item1 * maxRGB))),
        Convert.ToInt32(Double.Parse(String.Format("{0:0.00}", nRGB.Item2 * maxRGB))),
        Convert.ToInt32(Double.Parse(String.Format("{0:0.00}", nRGB.Item3 * maxRGB))));
    }

    /// <summary>
    /// Returns XYZ values in the range [0, 1] to their accepted values based on D50 white.
    /// (Y is already in the correct range, of course.)
    /// </summary>
    /// <param name="nXYZ"></param>
    /// <returns></returns>
    public static XyzColor DenormalizeXyzD50(NXyzColor nXYZ)
    {
      return new XyzColor(
        nXYZ.Item1 * D50XYZ.Item1, nXYZ.Item2 * D50XYZ.Item2, nXYZ.Item3 * D50XYZ.Item3);
    }

    /// <summary>
    /// Returns XYZ values in the range [0, 1] to their accepted values based on D65 white.
    /// (Y is already in the correct range, of course.)
    /// </summary>
    /// <param name="nXYZ"></param>
    /// <returns></returns>
    public static XyzColor DenormalizeXyzD65(NXyzColor nXYZ)
    {
      return new XyzColor(
        nXYZ.Item1 * D65XYZ.Item1, nXYZ.Item2 * D65XYZ.Item2, nXYZ.Item3 * D65XYZ.Item3);
    }

    /// <summary>
    /// Converts values of f(XYZ) into normalized XYZ.
    /// </summary>
    /// <param name="fXYZ"></param>
    /// <returns></returns>
    public static NXyzColor FXyzToNXyz(FXyzColor fXYZ)
    {
      return new NXyzColor(
        (fXYZ.Item1 > fxyzDelta ? MathUtils.Cube(fXYZ.Item1) : (fXYZ.Item1 - fxyzZeta) * fxyzKappa),
        (fXYZ.Item2 > fxyzDelta ? MathUtils.Cube(fXYZ.Item2) : (fXYZ.Item2 - fxyzZeta) * fxyzKappa),
        (fXYZ.Item3 > fxyzDelta ? MathUtils.Cube(fXYZ.Item3) : (fXYZ.Item3 - fxyzZeta) * fxyzKappa));
    }

    public static double GammaCorrectedToLinear(double gammaed)
    {
      return gammaed <= srgbDelta ? gammaed * srgbBetaI :
        Math.Pow((gammaed + srgbAlpha) * srgbAlpha1I, srgbGamma);
    }

    public static RgbColor LabToRgb(LabColor lab)
    {
      return XyzToRgb(LabToXyz(lab));
    }

    /// <summary>
    /// Converts values from CIE L*a*b* into CIE XYZ.
    /// </summary>
    /// <param name="lab"></param>
    /// <returns></returns>
    public static XyzColor LabToXyz(LabColor lab)
    {
      // Convet to fXYZ.
      double fy = (lab.Item1 + fxyzEpsilon) / fxyzGamma;
      FXyzColor fXYZ = new Tuple<double, double, double>(
        fy + lab.Item2 / fxyzAlpha, fy, fy - lab.Item3 / fxyzBeta);
      // Convert to normalized XYZ.
      NXyzColor nXYZ = FXyzToNXyz(fXYZ);
      // Denormalize XYZ.
      return DenormalizeXyzD65(nXYZ);
    }

    public static double LinearToGammaCorrected(double linear)
    {
      return linear <= srgbEpsilon ? srgbBeta * linear :
        srgbAlpha1 * Math.Pow(linear, srgbZeta) - srgbAlpha;
    }

    /// <summary>
    /// Converts 8-bit RGB values to values in the range [0, 1].
    /// </summary>
    /// <param name="rgb"></param>
    public static NRgbColor NormalizeRgb(RgbColor rgb)
    {
      return new NRgbColor(
        rgb.Item1 / maxRGB, rgb.Item2 / maxRGB, rgb.Item3 / maxRGB);
    }

    /// <summary>
    /// Converts XYZ to values in the range [0, 1] based on D50 white.
    /// (Y is already in the correct range, of course.)
    /// </summary>
    /// <param name="xyz"></param>
    public static NXyzColor NormalizeXyzD50(XyzColor xyz)
    {
      return new NXyzColor(
        xyz.Item1 / D50XYZ.Item1, xyz.Item2 / D50XYZ.Item2, xyz.Item3 / D50XYZ.Item3);
    }

    /// <summary>
    /// Converts XYZ to values in the range [0, 1] based on D65 white.
    /// (Y is already in the correct range, of course.)
    /// </summary>
    /// <param name="xyz"></param>
    public static NXyzColor NormalizeXyzD65(XyzColor xyz)
    {
      return new NXyzColor(
        xyz.Item1 / D65XYZ.Item1, xyz.Item2 / D65XYZ.Item2, xyz.Item3 / D65XYZ.Item3);
    }

    /// <summary>
    /// Convert normalized RGB with gamma correction to linear sRGB.
    /// </summary>
    /// <param name="nRGB"></param>
    /// <returns></returns>
    public static SRgbColor NRgbToSRgb(NRgbColor nRGB)
    {
      return new SRgbColor(
        GammaCorrectedToLinear(nRGB.Item1),
        GammaCorrectedToLinear(nRGB.Item2),
        GammaCorrectedToLinear(nRGB.Item3));
    }

    /// <summary>
    /// Converts normalized XYZ to f(XYZ).
    /// </summary>
    /// <param name="nXYZ"></param>
    /// <returns></returns>
    public static FXyzColor NXyzToFXyz(NXyzColor nXYZ)
    {
      return new FXyzColor(
        (nXYZ.Item1 > fxyzEta ? Math.Pow(nXYZ.Item1, fxyzTheta) : fxyzIota * nXYZ.Item1 + fxyzZeta),
        (nXYZ.Item2 > fxyzEta ? Math.Pow(nXYZ.Item2, fxyzTheta) : fxyzIota * nXYZ.Item2 + fxyzZeta),
        (nXYZ.Item3 > fxyzEta ? Math.Pow(nXYZ.Item3, fxyzTheta) : fxyzIota * nXYZ.Item3 + fxyzZeta));
    }

    /// <summary>
    /// Converts RGB to HSL.
    /// Code from Guillaume Leparmentier, "Manipulating colors in .NET".
    /// Adapted for integer arguments, tuples, etc., etc.
    /// </summary>
    /// <param name="rgb">RGB color triad, each element full scale is 255.
    /// </param>
    /// <returns>HSL color triad, each element full scale is 240.</returns>
    public static HslColor RgbToHsl(RgbColor rgb)
    {
      double h = 0, s = 0, l = 0;

      // Normalize red, green, blue values.
      NRgbColor nRGB = NormalizeRgb(rgb);
      double r = nRGB.Item1;
      double g = nRGB.Item2;
      double b = nRGB.Item3;

      double max = Math.Max(r, Math.Max(g, b));
      double min = Math.Min(r, Math.Min(g, b));

      // hue
      if (max == min)
      {
        h = 0; // undefined
      }
      else if (max == r && g >= b)
      {
        h = 60.0 * (g - b) / (max - min);
      }
      else if (max == r && g < b)
      {
        h = 60.0 * (g - b) / (max - min) + 360.0;
      }
      else if (max == g)
      {
        h = 60.0 * (b - r) / (max - min) + 120.0;
      }
      else if (max == b)
      {
        h = 60.0 * (r - g) / (max - min) + 240.0;
      }

      // luminance
      l = (max + min) / 2.0;

      // saturation
      if (l == 0 || max == min)
      {
        s = 0;
      }
      else if (0 < l && l <= 0.5)
      {
        s = (max - min) / (max + min);
      }
      else if (l > 0.5)
      {
        s = (max - min) / (2 - (max + min)); //(max-min > 0)?
      }

      return DenormalizeHsl(new NHslColor(h, s, l));
    }

    public static LabColor RgbToLab(RgbColor rgb)
    {
      return XyzToLab(RgbToXyz(rgb));
    }

    /// <summary>
    /// Convert RGB to CIE XYZ
    /// </summary>
    /// <param name="rgb"></param>
    /// <returns></returns>
    public static XyzColor RgbToXyz(RgbColor rgb)
    {
      // Normalize red, green, blue values.
      NRgbColor nRGB = NormalizeRgb(rgb);
      // Convert to sRGB.
      SRgbColor sRGB = NRgbToSRgb(nRGB);
      // Convert to XYZ.
      return SRgbToXyz(sRGB);
    }

    /// <summary>
    /// Converts linear sRGB into normalized RGB with gamma correction.
    /// </summary>
    /// <param name="sRGB"></param>
    /// <returns></returns>
    public static NRgbColor SRgbToNRgb(SRgbColor sRGB)
    {
      return new NRgbColor(
        LinearToGammaCorrected(sRGB.Item1),
        LinearToGammaCorrected(sRGB.Item2),
        LinearToGammaCorrected(sRGB.Item3));
    }

    /// <summary>
    /// Converts linear sRGB to CIE XYZ.
    /// </summary>
    /// <param name="sRGB"></param>
    /// <returns></returns>
    public static XyzColor SRgbToXyz(SRgbColor sRGB)
    {
      return new XyzColor(
        sRGB.Item1 * 0.412390799 + sRGB.Item2 * 0.357584339 + sRGB.Item3 * 0.180480788,
        sRGB.Item1 * 0.212639006 + sRGB.Item2 * 0.715168679 + sRGB.Item3 * 0.072192315,
        sRGB.Item1 * 0.019330819 + sRGB.Item2 * 0.119194780 + sRGB.Item3 * 0.950532152);
    }

    /// <summary>
    /// Converts CIE XYZ to CIE L*a*b*.
    /// </summary>
    /// <param name="xyz"></param>
    /// <returns></returns>
    public static LabColor XyzToLab(XyzColor xyz)
    {
      // Normalize XYZ per D65 white.
      NXyzColor nXYZ = NormalizeXyzD65(xyz);
      // Convert to fXYZ.
      FXyzColor fXYZ = NXyzToFXyz(nXYZ);
      // Convert to L*a*b*.
      return new LabColor(
        fxyzGamma * fXYZ.Item2 - fxyzEpsilon,
        fxyzAlpha * (fXYZ.Item1 - fXYZ.Item2),
        fxyzBeta * (fXYZ.Item2 - fXYZ.Item3));
    }

    /// <summary>
    /// Converts CIE XYZ to RGB.
    /// </summary>
    /// <param name="xyz"></param>
    /// <returns></returns>
    public static RgbColor XyzToRgb(XyzColor xyz)
    {
      // Convert to sRGB.
      SRgbColor sRGB = XyzToSRgb(xyz);
      // Convert to nRGB.
      NRgbColor nRGB = SRgbToNRgb(sRGB);
      // Denormalize red, green, blue.
      return DenormalizeRgb(nRGB);
    }

    /// <summary>
    /// Converts CIE XYZ to linear sRGB.
    /// </summary>
    /// <param name="xyz"></param>
    /// <returns></returns>
    public static SRgbColor XyzToSRgb(XyzColor xyz)
    {
      return new SRgbColor(
        xyz.Item1 * 3.240969942 - xyz.Item2 * 1.537383178 - xyz.Item3 * 0.498610760,
       -xyz.Item1 * 0.969243636 + xyz.Item2 * 1.875967502 + xyz.Item3 * 0.041555057,
        xyz.Item1 * 0.055630080 - xyz.Item2 * 0.203976959 + xyz.Item3 * 1.056971514);
    }

  }
}
