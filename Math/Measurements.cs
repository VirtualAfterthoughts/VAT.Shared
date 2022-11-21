using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace VAT.Shared.Math {
    public static partial class Measurements {
        public static string FormatMetric(float meters) => $"{meters}m";

        public static string FormatImperial(float meters) {
            float sign = Mathf.Sign(meters);

            meters = Mathf.Abs(meters);

            float total = ConvertToFeet(meters);

            float feet = Mathf.Floor(total);
            float inches = Mathf.Floor((total - feet) * 12f);

            return $"{feet * sign}' {inches * sign}\" ";
        }

        public static float ConvertToFeet(float meters) {
            return meters * 3.281f;
        }
    }
}