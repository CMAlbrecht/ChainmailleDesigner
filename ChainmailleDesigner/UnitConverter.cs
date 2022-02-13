// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: UnitConverter.cs
// Description: Unified set of units, with utilities for performing
//              unit conversion.


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
using System.Collections.Generic;

namespace ChainmailleDesigner
{
  public enum Units
  {
    NoUnits = 0,
    // Time
    Years,
    Months,
    Fortnights,
    Days,
    Hours,
    MinutesOfTime,
    SecondsOfTime,
    Milliseconds,
    Microseconds,
    SecondsPerRotation,
    SecondsPerCycle,
    // Distance
    Feet,
    Inches,
    Yards,
    Ells,
    Fathoms,
    Hectofeet,
    Furlongs,
    Kiloyards,
    StatuteMiles,
    DataMiles,
    NauticalMiles,
    Meters,
    Centimeters,
    Hectometers,
    Kilometers,
    // Angle
    DegreesOfArc,
    MinutesOfArc,
    SecondsOfArc,
    Radians,
    Circles,
    SemiCircles,
    // Speed
    Knots, // nautical miles per hour
    MPH,   // statute miles per hour
    KPH,   // kilometers per hour
    DMPH,  // data miles per hour
    MPS,   // meters per second
    FTPS,  // feet per second
    RPH,   // radians per hour
    FFN,   // furlongs per fortnight
    FPM,   // feet per minute
    FPH,   // feet per hour
    NMPS,  // nautical miles per second
    Mach,  // Mach number (standard conditions)
    // Frequency
    Hz,    // Hertz
    KHz,   // kiloHertz
    MHz,   // megaHertz
    GHz,   // gigaHertz
    PPS,   // pulses per second
    KPPS,  // kilopulses per second
    MPPS,  // megapulses per second
    // Bearing
    DegreesTrue,     // degrees of bearing relative to north
    DegreesRelative, // degrees of bearing relative to course
    // Signal-to-Noise Ratio
    DB,    // decibels
    // elevation
    MSL,   // elevation relative to mean sea level
    HAE,   // height above the Earth's mean ellipsoid
    // wire gauge
    AWG
  };

  public enum UnitDomain
  {
    Invalid = 0,
    // Unit and conversion domains.
    Time               = 1 << 0,
    Distance           = 1 << 1,
    Angle              = 1 << 2,
    Speed              = 1 << 3,
    Frequency          = 1 << 4,
    Bearing            = 1 << 5,
    SignalToNoiseRatio = 1 << 6,
    Elevation          = 1 << 7,
    WireGauge          = 1 << 8,
    // Special conversion domains.
    TimeFrequency = Time | Frequency,
    AngleDistance = Angle | Distance,
    WireGaugeDistance = WireGauge | Distance
  };

  public static class UnitConverter
  {
    // =============================
    // Unit prefixes (powers of ten)
    // =============================

    public const double atto  = 1.0E-18;
    public const double femto = 1.0E-15;
    public const double pico  = 1.0E-12;
    public const double nano  = 1.0E-9;
    public const double micro = 1.0E-6;
    public const double milli = 1.0E-3;
    public const double centi = 1.0E-2;
    public const double deci  = 1.0E-1;
    public const double Deka = 1.0E1;
    public const double Hecto = 1.0E2;
    public const double Kilo = 1.0E3;
    public const double Mega = 1.0E6;
    public const double Giga = 1.0E9;
    public const double Tera = 1.0E12;

    // Shared between angular measurements and time measurements.
    public const double SecondsPerMinute = 60.0; // exact

    // ====================
    // Angular measurements
    // ====================

    public const double TwoPi = 2.0 * Math.PI;
    public const double DegreesPerCycle = 360.0; // exact
    public const double MinutesPerDegree = 60.0; // exact

    // =================
    // Time measurements
    // =================

    public const double DaysPerSiderialYear = 365.256363051; // approx.
    public const double MonthsPerYear = 12.0; // exact
    public const double DaysPerFortnight = 14.0; // exact
    public const double HoursPerDay = 24.0; // exact
    public const double MinutesPerHour = 60.0; // exact
    public const double SecondsPerDay = 86400;

    // =====================
    // Distance measurements
    // =====================

    // English distance

    public const double InchesPerFoot = 12.0; // exact
    public const double FeetPerYard = 3.0; // exact
    public const double FeetPerEll = 3.75; // exact
    public const double FeetPerFathom = 6.0; // exact
    public const double FeetPerFurlong = 660.0; // exact
    public const double FeetPerStatuteMile = 5280.0; // exact
    public const double FeetPerDataMile = 6000.0; // exact
    // Note: By international treaty, the nautical mile can no longer be
    // considered an English measure; see MetersPerNauticalMile, below.

    // Metric <-> English distance conversion

    public const double MetersPerFoot = 0.3048; // exact
    // Note: The above value is exact; per the international treaty of 1959 a
    // foot is exactly 30.48 centimeters, and the GPS uses this definition.
    // Most historical US Survey work, however, defines a meter as exactly
    // 39.37 inches (= 0.304800641... m/ft), a measure which is based upon
    // the Mendenhall Order of 1893.

    public const double MetersPerNauticalMile = 1852.0; // exact
    // Note: The above value is exact; per the international treaty of 1929 a
    // nautical mile is exactly 1852 meters. Prior to the international treaty,
    // however, the US nautical mile was 6080.2 US feet (about 1853.248 meters)
    // and the UK nautical mile was 6080 UK feet, or about 1853.184 meters.

    // ===============================================
    // Distance <-> Angle conversions (Earth measures)
    // ===============================================

    // WGS84 and GRS1980
    public const double EquatorialEarthRadiusMeters = 6378137.0;
    // WGS84
    public const double EarthFlattening = 1.0 / 298.257223563;
    // Note: The above parameters from from the World Geodetic System of 1984,
    // the most common reference system for the US military. The WGS84 value of
    // equatorial radius agrees with that of the Geodetic Reference System of
    // 1980. The flattening is the ratio used to relate the equatorial radius
    // to the polar radius. GRS1980 more-or-less agrees with this value, but
    // gives it as 1.0 / 298.257222.
    // The polar radius is computed as ( 1.0 - f ) times the equatorial radius.
    public const double QuadraticMeanEarthRadiusMeters = 6372797.6;
    // Supposed to be the "best" estimage of a single radius for the Earth,
    // this is computed as sqrt( ( 3 * R(equatorial)^2 + R(polar)^2 ) / 4 ).

    // ===============================
    // Speed conversions (Mach number)
    // ===============================

    public const double MachNmPerHour = 661.47;
    // This is for 60 deg. F at sea level

    // ============================
    // Unit abbreviations and names
    // ============================

    // This and the following names must correspond to the units enumeration.
    public static readonly string[] UnitAbbreviation = new string[]
    {
      "NA",
      "YR", "MO", "FN", "DY", "HR", "MIN", "SEC",
      "MS", "USEC", "SPR", "SPC",
      "FT", "IN", "YD", "ELL", "FH", "HF", "FL", "KY",
      "SM", "DM", "NM", "M", "CM", "HM", "KM",
      "DEG", "MINARC", "SECARC", "RAD", "CIRC", "SEMI",
      "K", "MPH", "KPH", "DMPH", "MPS", "FTPS",
      "RPH", "FFN", "FPM", "FPH", "NMPS", "MACH",
      "HZ", "KHZ", "MHZ", "GHZ", "PPS", "KPPS", "MPPS",
      "T", "R",
      "DB",
      "MSL", "HAE",
      "AWG"
    };

    public static readonly string[] UnitName = new string[]
    {
      "Not Applicable",
      "Years", "Months", "Fortnights", "Days", "Hours",
      "Minutes (time)", "Seconds (time)", "Milliseconds (time)",
      "Microseconds (time)", "Seconds per Rotation", "Seconds per Cycle",
      "Feet", "Inches", "Yards", "Ells", "Fathoms", "Hundreds of Feet",
      "Furlongs", "Thousand Yards", "Statute Miles", "Data Miles",
      "Nautical Miles", "Meters", "Centimeters", "Hectometers", "Kilometers",
      "Degrees of Arc", "Minutes of Arc", "Seconds of Arc",
      "Radians", "Full Circles", "Semicircles",
      "Knots", "Statute Miles per Hour", "Kilometers per Hour",
      "Data Miles per Hour", "Meters per Second", "Feet per Second",
      "Radians per Hour (on Earth surface)", "Furlongs per Fortnight",
      "Feet per Minute", "Feet per Hour", "Nautical Miles per Second",
      "Mach Number",
      "Hertz", "Kilohertz", "Megahertz", "Gigahertz",
      "Pulses per Second", "Kilopulses per Second", "Megapulses per Second",
      "True Bearing", "Relative Bearing",
      "Decibels",
      "Elevation (MSL)", "Height (HAE)",
      "Gauge"
    };

    // ============
    // Unit domains
    // ============

    public static readonly Units[] AllTimeUnits = new Units[]
    {
      Units.Years,
      Units.Months,
      Units.Fortnights,
      Units.Days,
      Units.Hours,
      Units.MinutesOfTime,
      Units.SecondsOfTime,
      Units.Milliseconds,
      Units.Microseconds,
      Units.SecondsPerRotation,
      Units.SecondsPerCycle
    };

    public static readonly Units[] AllDistanceUnits = new Units[]
    {
      Units.Feet,
      Units.Inches,
      Units.Yards,
      Units.Ells,
      Units.Fathoms,
      Units.Hectofeet,
      Units.Furlongs,
      Units.Kiloyards,
      Units.StatuteMiles,
      Units.DataMiles,
      Units.NauticalMiles,
      Units.Meters,
      Units.Centimeters,
      Units.Hectometers,
      Units.Kilometers
    };

    public static readonly Units[] AllAngleUnits = new Units[]
    {
      Units.DegreesOfArc,
      Units.MinutesOfArc,
      Units.SecondsOfArc,
      Units.Radians,
      Units.Circles,
      Units.SemiCircles
    };

    public static readonly Units[] AllSpeedUnits = new Units[]
    {
      Units.Knots,
      Units.MPH,
      Units.KPH,
      Units.DMPH,
      Units.MPS,
      Units.FTPS,
      Units.RPH,
      Units.FFN,
      Units.FPM,
      Units.FPH,
      Units.NMPS,
      Units.Mach
    };

    public static readonly Units[] AllFrequencyUnits = new Units[]
    {
      Units.Hz,
      Units.KHz,
      Units.MHz,
      Units.GHz,
      Units.PPS,
      Units.KPPS,
      Units.MPPS
    };

    public static readonly Units[] AllBearingUnits = new Units[]
    {
      Units.DegreesTrue,
      Units.DegreesRelative
    };

    public static readonly Units[] AllSignalToNoiseRatioUnits = new Units[]
    {
      Units.DB
    };

    public static readonly Units[] AllElevationUnits = new Units[]
    {
      Units.MSL,
      Units.HAE
    };

    public static readonly Units[] AllWireGaugeUnits = new Units[]
    {
      Units.AWG
    };

    private static List<UnitDomain> specialConversionDomains =
      new List<UnitDomain>();

    // ============
    // Dictionaries
    // ============

    // Conversion factor from the units to the standard units of their type.
    private static Dictionary<Units, double> conversionFactor =
      new Dictionary<Units, double>();

    // Domain from units.
    private static Dictionary<Units, UnitDomain> domainOfUnits =
      new Dictionary<Units, UnitDomain>();

    // Previously-requested conversion factors (for performance enhancement).
    // 
    private static Dictionary<Units, Dictionary<Units, double>>
      requestedConversionFactors =
      new Dictionary<Units, Dictionary<Units, double>>();

    // Previously requested conversion domains (for performance enhancement).
    private static Dictionary<Units, Dictionary<Units, UnitDomain>>
      requestedConversionDomains =
      new Dictionary<Units, Dictionary<Units, UnitDomain>>();

    // ======================
    // Methods and properties
    // ======================

    static UnitConverter()
    {
      specialConversionDomains.Add(UnitDomain.AngleDistance);
      specialConversionDomains.Add(UnitDomain.TimeFrequency);
      specialConversionDomains.Add(UnitDomain.WireGaugeDistance);

      // Units <-> Domains and conversion factors.
      domainOfUnits[Units.NoUnits] = UnitDomain.Invalid;
      conversionFactor[Units.NoUnits] = 0.0;

      double factor = 0.0;

      // Standard time units are seconds.
      foreach (Units units in AllTimeUnits)
      {
        domainOfUnits[units] = UnitDomain.Time;

        switch (units)
        {
          case Units.Years:
            factor = SecondsPerMinute * MinutesPerHour * HoursPerDay *
              DaysPerSiderialYear;
            break;
          case Units.Months:
            factor = SecondsPerMinute * MinutesPerHour * HoursPerDay *
              DaysPerSiderialYear / MonthsPerYear;
            break;
          case Units.Fortnights:
            factor = SecondsPerMinute * MinutesPerHour * HoursPerDay *
              DaysPerFortnight;
            break;
          case Units.Days:
            factor = SecondsPerMinute * MinutesPerHour * HoursPerDay;
            break;
          case Units.Hours:
            factor = SecondsPerMinute * MinutesPerHour;
            break;
          case Units.MinutesOfTime:
            factor = SecondsPerMinute;
            break;
          case Units.SecondsOfTime:
            factor = 1.0;
            break;
          case Units.Milliseconds:
            factor = milli;
            break;
          case Units.Microseconds:
            factor = micro;
            break;
          case Units.SecondsPerRotation:
          case Units.SecondsPerCycle:
            factor = 1.0;
            break;
          default:
            factor = 0.0;
            break;
        }
        conversionFactor[units] = factor;
      }

      foreach (Units units in AllDistanceUnits)
      {
        domainOfUnits[units] = UnitDomain.Distance;

        // Standard distance units are feet.
        switch (units)
        {
          case Units.Feet:
            factor = 1.0;
            break;
          case Units.Inches:
            factor = 1.0 / InchesPerFoot;
            break;
          case Units.Yards:
            factor = FeetPerYard;
            break;
          case Units.Ells:
            factor = FeetPerEll;
            break;
          case Units.Fathoms:
            factor = FeetPerFathom;
            break;
          case Units.Hectofeet:
            factor = Hecto;
            break;
          case Units.Furlongs:
            factor = FeetPerFurlong;
            break;
          case Units.Kiloyards:
            factor = Kilo * FeetPerYard;
            break;
          case Units.StatuteMiles:
            factor = FeetPerStatuteMile;
            break;
          case Units.DataMiles:
            factor = FeetPerDataMile;
            break;
          case Units.NauticalMiles:
            factor = MetersPerNauticalMile / MetersPerFoot;
            break;
          case Units.Meters:
            factor = 1.0 / MetersPerFoot;
            break;
          case Units.Centimeters:
            factor = centi / MetersPerFoot;
            break;
          case Units.Hectometers:
            factor = Hecto / MetersPerFoot;
            break;
          case Units.Kilometers:
            factor = Kilo / MetersPerFoot;
            break;
          default:
            factor = 0.0;
            break;
        }
        conversionFactor[units] = factor;
      }

      foreach (Units units in AllAngleUnits)
      {
        domainOfUnits[units] = UnitDomain.Angle;

        // Standard angle units are degrees.
        switch (units)
        {
          case Units.DegreesOfArc:
            factor = 1.0;
            break;
          case Units.MinutesOfArc:
            factor = 1.0 / MinutesPerDegree;
            break;
          case Units.SecondsOfArc:
            factor = 1.0 / (SecondsPerMinute * MinutesPerDegree);
            break;
          case Units.Radians:
            factor = DegreesPerCycle / TwoPi;
            break;
          case Units.Circles:
            factor = DegreesPerCycle;
            break;
          case Units.SemiCircles:
            factor = 0.5 * DegreesPerCycle;
            break;
        }
        conversionFactor[units] = factor;
      }

      foreach (Units units in AllSpeedUnits)
      {
        domainOfUnits[units] = UnitDomain.Speed;

        // Standard speed units are knots, i.e. nautical miles per hour.
        switch (units)
        {
          case Units.Knots:
            factor = 1.0;
            break;
          case Units.MPH:
            factor = FeetPerStatuteMile * MetersPerFoot /
              MetersPerNauticalMile;
            break;
          case Units.KPH:
            factor = Kilo / MetersPerNauticalMile;
            break;
          case Units.DMPH:
            factor = FeetPerDataMile * MetersPerFoot / MetersPerNauticalMile;
            break;
          case Units.MPS:
            factor = SecondsPerMinute * MinutesPerHour / MetersPerNauticalMile;
            break;
          case Units.FTPS:
            factor = MetersPerFoot * SecondsPerMinute * MinutesPerHour /
              MetersPerNauticalMile;
            break;
          case Units.RPH:
            factor = QuadraticMeanEarthRadiusMeters / MetersPerNauticalMile;
            break;
          case Units.FFN:
            factor = FeetPerFurlong * MetersPerFoot /
              (MetersPerNauticalMile * HoursPerDay * DaysPerFortnight);
            break;
          case Units.FPM:
            factor = MetersPerFoot * MinutesPerHour / MetersPerNauticalMile;
            break;
          case Units.FPH:
            factor = MetersPerFoot / MetersPerNauticalMile;
            break;
          case Units.NMPS:
            factor = SecondsPerMinute * MinutesPerHour;
            break;
          case Units.Mach:
            factor = MachNmPerHour;
            break;
          default:
            factor = 0.0;
            break;
        }
        conversionFactor[units] = factor;
      }

      foreach (Units units in AllFrequencyUnits)
      {
        domainOfUnits[units] = UnitDomain.Frequency;

        // Standard frequency units ae Hertz.
        switch (units)
        {
          case Units.Hz:
            factor = 1.0;
            break;
          case Units.KHz:
            factor = Kilo;
            break;
          case Units.MHz:
            factor = Mega;
            break;
          case Units.GHz:
            factor = Giga;
            break;
          case Units.PPS:
            factor = 1.0;
            break;
          case Units.KPPS:
            factor = Kilo;
            break;
          case Units.MPPS:
            factor = Mega;
            break;
          default:
            factor = 0.0;
            break;
        }
        conversionFactor[units] = factor;
      }

      foreach (Units units in AllBearingUnits)
      {
        domainOfUnits[units] = UnitDomain.Bearing;

        // Bearing units can not be converted by use of a factor.
        switch (units)
        {
          case Units.DegreesTrue:
          case Units.DegreesRelative:
            factor = 1.0;
            break;
          default:
            factor = 0.0;
            break;
        }
        conversionFactor[units] = factor;
      }

      foreach (Units units in AllSignalToNoiseRatioUnits)
      {
        domainOfUnits[units] = UnitDomain.SignalToNoiseRatio;

        // Standard signal-to-noise ratio units are dB.
        switch (units)
        {
          case Units.DB:
            factor = 1.0;
            break;
          default:
            factor = 0.0;
            break;
        }
        conversionFactor[units] = factor;
      }

      foreach (Units units in AllElevationUnits)
      {
        domainOfUnits[units] = UnitDomain.Elevation;

        // Elevation units can not be converted by use of a factor.
        switch (units)
        {
          case Units.MSL:
          case Units.HAE:
            factor = 1.0;
            break;
          default:
            factor = 0.0;
            break;
        }
        conversionFactor[units] = factor;
      }

      foreach (Units units in AllWireGaugeUnits)
      {
        domainOfUnits[units] = UnitDomain.WireGauge;

        // Standard wire gauge units are AWG.
        switch (units)
        {
          case Units.AWG:
            factor = 1.0;
            break;
          default:
            factor = 0.0;
            break;
        }
        conversionFactor[units] = factor;
      }
    }

    public static double ConvertValue(double value, Units fromUnits, Units toUnits)
    {
      double result = 0.0;

      // Determine unit conversion domain.
      UnitDomain domain = UnitDomain.Invalid;
      // Look on the "short list" first, in case we've already done this.
      lock (requestedConversionDomains)
      {
        if (requestedConversionDomains.ContainsKey(fromUnits))
        {
          if (requestedConversionDomains[fromUnits].ContainsKey(toUnits))
          {
            domain = requestedConversionDomains[fromUnits][toUnits];
          }
        }
      }
      if (domain == UnitDomain.Invalid)
      {
        // Wasn't on the short list, so figure it out.
        domain = DetermineApplicableConversionDomain(fromUnits, toUnits);
        if (domain != UnitDomain.Invalid)
        {
          // Add it to the "short list" for next time.
          lock (requestedConversionDomains)
          {
            if (!requestedConversionDomains.ContainsKey(fromUnits))
            {
              requestedConversionDomains.Add(fromUnits,
                new Dictionary<Units, UnitDomain>());
            }
            requestedConversionDomains[fromUnits].Add(toUnits, domain);
          }
        }
      }

      if (domain != UnitDomain.Invalid)
      {
        // Determine the conversion factor.
        double conversionFactor =
          UnitConversionFactor(fromUnits, toUnits, domain);

        // Convert the units of the value.
        if (domain == UnitDomain.TimeFrequency)
        {
          // This domain requires special handling.
          if (value != 0.0 && conversionFactor != 0.0)
          {
            result = 1.0 / (value * conversionFactor);
          }
        }
        else if (domain == UnitDomain.WireGaugeDistance)
        {
          // This domain is special. Wire gauge is geometric, not linear.
          if (fromUnits == Units.AWG)
          {
            result = conversionFactor * 0.005 * Math.Pow(92.0, (36.0 - value) / 39.0) /
              InchesPerFoot;
          }
          else if (toUnits == Units.AWG)
          {
            result = 36.0 - (39.0 * Math.Log(value * conversionFactor *
              InchesPerFoot / 0.005) / Math.Log(92.0));
          }
        }
        else
        {
          // Straightforward application of the conversion factor.
          result = value * conversionFactor;
        }
      }

      return result;
    }

    private static UnitDomain DetermineApplicableConversionDomain(
      Units fromUnits, Units toUnits)
    {
      // Applicability of simple conversion domain (units both in same domain).
      UnitDomain result = domainOfUnits[fromUnits] == domainOfUnits[toUnits] ?
        domainOfUnits[fromUnits] : UnitDomain.Invalid;

      if (result == UnitDomain.Invalid)
      {
        // Applicability of special conversion domain (change of domain).
        foreach (UnitDomain domain in specialConversionDomains)
        {
          if (((domain & domainOfUnits[fromUnits]) != 0) &&
              ((domain & domainOfUnits[toUnits]) != 0))
          {
            result = domain;
            break;
          }
        }
      }

      return result;
    }

    private static double UnitConversionFactor(Units fromUnits, Units toUnits,
      UnitDomain domain)
    {
      double result = 0.0;

      // Check for previously-requested conversion factor.
      lock (requestedConversionFactors)
      {
        if (requestedConversionFactors.ContainsKey(fromUnits))
        {
          if (requestedConversionFactors[fromUnits].ContainsKey(toUnits))
          {
            result = requestedConversionFactors[fromUnits][toUnits];
          }
        }
      }
      if (result == 0.0 && domain != UnitDomain.Invalid)
      {
        double fromFactor = conversionFactor[fromUnits];
        double toFactor = conversionFactor[toUnits];
        if (domain == UnitDomain.AngleDistance)
        {
          // This domain is special.
          if (domainOfUnits[fromUnits] == UnitDomain.Angle)
          {
            // Convert from earth-centered angle to distance on the earth's
            // surface.
            result = fromFactor * TwoPi / DegreesPerCycle *
              QuadraticMeanEarthRadiusMeters / (MetersPerFoot * toFactor);
          }
          else
          {
            // Convert from distance on the earth's surface to earth-centered
            // angle.
            result = fromFactor * MetersPerFoot /
              (QuadraticMeanEarthRadiusMeters * TwoPi / DegreesPerCycle *
              toFactor);
          }
        }
        else if (domain == UnitDomain.TimeFrequency)
        {
          // This domain is special AND requires special handling of the result,
          // since convertedValue = 1.0 / (originalValue * conversionFactor).
          result = fromFactor * toFactor;
        }
        else if (domain == UnitDomain.WireGaugeDistance)
        {
          // This domain is special, but the factor is straightforward.
          result = fromFactor / toFactor;
        }
        else if (toFactor != 0.0)
        {
          // This is one of the ordinary linear domains.
          result = fromFactor / toFactor;
        }
        if (result != 0.0)
        {
          // Record the conversion factor in the "short list" for next time.
          lock (requestedConversionFactors)
          {
            if (!requestedConversionFactors.ContainsKey(fromUnits))
            {
              requestedConversionFactors.Add(fromUnits,
                new Dictionary<Units, double>());
            }
            requestedConversionFactors[fromUnits].Add(toUnits, result);
          }
        }
      }

      return result;
    }
  }
}
