using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace VAT.Shared.Math 
{
    // Represent a unit of measurement, regardless of system, regardless of type
    // It's up the scripts that use it to interpret it!
    //
    // LengthUnit:
    //  Meters
    //  Feet
    //
    // VolumeUnit:
    //  Liters
    //  US Gallons
    //
    // TemperatureUnit:
    //  Celsius
    //  Fahrenheit
    //  Kelvin
    public abstract class BaseUnit
    {
        public enum UnitSystem
        {
            Metric, Imperial, Scientific
        }

        // Used for formatting mainly
        public static UnitSystem CurrentUnitSystem = UnitSystem.Metric;

        [SerializeField] 
        protected float _value; // 1 unity unit = 1 meter

        public float RawValue { get => _value; }

        public BaseUnit(float u) => _value = u;
    }

    [Serializable]
    public class LengthUnit : BaseUnit
    {
        public LengthUnit(float u) : base(u) { }

        public const float METER_TO_FEET = 3.28084F;
        public const float FEET_TO_METER = 0.3048F;

        public float Meters
        {
            get => _value;
            set => _value = value;
        }

        public float Feet
        {
            get => _value * METER_TO_FEET;
            set => _value = value * FEET_TO_METER;
        }

        public (int, int) GetFeetInches()
        {
            float rawFeet = Feet;
            int feet = (int)System.MathF.Truncate(rawFeet);

            float rawInches = rawFeet - feet;
            int inches = Mathf.RoundToInt(rawInches * 12);

            return (feet, inches);
        }
        
        public override string ToString()
        {
            if (CurrentUnitSystem == UnitSystem.Metric || CurrentUnitSystem == UnitSystem.Scientific)
                return $"{Meters}m";
            else {
                var feetInches = GetFeetInches();
                return $"{feetInches.Item1}' {Mathf.Abs(feetInches.Item2)}\"";
            }
        }

        // When working with Unity operations, we should only use meters
        // When you want feet you need to explicitly request them!
        public static implicit operator float(LengthUnit u) => u.Meters;
        public static implicit operator LengthUnit(float u) => new(u);
    }

    [Serializable]
    public class VolumeUnit : BaseUnit
    {
        public VolumeUnit(float u) : base(u) { }
        public VolumeUnit(LengthUnit length, LengthUnit width, LengthUnit height) : base(length * width * height) { }

        public const float LITER_TO_GALLON = 0.264172F;
        public const float GALLON_TO_LITER = 3.78541F;

        public float Liters
        {
            get => _value;
            set => _value = value;
        }

        public float Gallons
        {
            get => _value * LITER_TO_GALLON;
            set => _value = value * GALLON_TO_LITER;
        }

        public override string ToString()
        {
            if (CurrentUnitSystem == UnitSystem.Metric || CurrentUnitSystem == UnitSystem.Scientific)
                return $"{Liters}L";
            else
                return $"{Gallons}gal";
        }

        // Once again, Unity is metric!
        public static implicit operator float(VolumeUnit u) => u.Liters;
        public static implicit operator VolumeUnit(float u) => new VolumeUnit(u);
    }

    [Serializable]
    public class TemperatureUnit : BaseUnit
    {
        public TemperatureUnit(float u) : base(u) { }

        public const float C_TO_F_OFFSET = 32F;
        public const float C_TO_F_RATIO = 5F / 9F;

        public const float F_TO_C_RATIO = 9F / 5F;

        public const float K_TO_C_OFFSET = 273.15F;

        public float Celsius
        {
            get => _value;
            set => _value = value;
        }

        public float Fahrenheit
        {
            get => (_value * F_TO_C_RATIO) + C_TO_F_OFFSET;
            set => _value = (value - C_TO_F_OFFSET) * C_TO_F_RATIO;
        }

        // Just in case you ever need kelvin :)
        public float Kelvin
        {
            get => _value + K_TO_C_OFFSET;
            set => _value = value - K_TO_C_OFFSET;
        }

        public override string ToString()
        {
            if (CurrentUnitSystem == UnitSystem.Metric)
                return $"{Celsius}C";
            else if (CurrentUnitSystem == UnitSystem.Scientific)
                return $"{Kelvin}K";
            else
                return $"{Fahrenheit}F";
        }

        // Unity is metric!!!
        public static implicit operator float(TemperatureUnit u) => u.Celsius;
        public static implicit operator TemperatureUnit(float u) => new(u);
    }
}