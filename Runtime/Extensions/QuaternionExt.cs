using UnityEngine;
using Unity.Burst;

using static Unity.Mathematics.math;

namespace VAT.Shared.Extensions {
	using Unity.Mathematics;

	/// <summary>
	/// Extension methods for Quaternions.
	/// </summary>
    public static partial class QuaternionExtensions {
		/// <summary>
		/// Returns a version of this displacement quat that is ensured to be the shortest distance.
		/// </summary>
		/// <param name="q"></param>
		/// <returns></returns>
		public static Quaternion Shortest(this Quaternion q) {
			if (q.w < 0)
				q = new Quaternion(-q.x, -q.y, -q.z, -q.w);
			return q;
		}

		/// <summary>
		/// Returns a version of this displacement quat that is ensured to be the shortest distance.
		/// </summary>
		/// <param name="q"></param>
		/// <returns></returns>
		public static quaternion shortest(this quaternion q) {
			if (q.value.w < 0)
				q = new quaternion(-q.value.x, -q.value.y, -q.value.z, -q.value.w);
			return q;
		}

		// Thanks to https://gist.github.com/aeroson/043001ca12fe29ee911e for the implementation of base quaternion functions

		/// <summary>
		/// Quaternion.ToAngleAxis but for the Mathematics package
		/// </summary>
		/// <param name="q"></param>
		/// <param name="angle"></param>
		/// <param name="axis"></param>
		public static void toangleaxis(this quaternion q, out float angle, out float3 axis) {
			q.toaxisangle(out axis, out angle);
			angle = degrees(angle);
		}

		/// <summary>
		/// Quaternion.ToAngleAxis in radians but for the Mathematics package, following a similar naming scheme to quaternion.AxisAngle which is in radians.
		/// </summary>
		/// <param name="q"></param>
		/// <param name="angle"></param>
		/// <param name="axis"></param>
		public static void toaxisangle(this quaternion q, out float3 axis, out float angle) {
			if (abs(q.value.w) > 1.0f)
				q = normalize(q);
			angle = 2.0f * acos(q.value.w); // angle
			float den = (float)sqrt(1.0 - q.value.w * q.value.w);
			if (den > 0.0001f) {
				axis = q.value.xyz / den;
			}
			else {
				// This occurs when the angle is zero. 
				// Not a problem: just set an arbitrary normalized axis.
				axis = new Vector3(1, 0, 0);
			}
		}
	}

	[BurstCompile(FloatMode = FloatMode.Fast)]
	public static partial class BurstCompiled_QuaternionExtensions {
		/// <summary>
		/// Quaternion.ToAngleAxis but for the Mathematics package (BURST).
		/// </summary>
		/// <param name="q"></param>
		/// <param name="angle"></param>
		/// <param name="axis"></param>
		[BurstCompile(FloatMode = FloatMode.Fast)]
		public static void BurstCompiled_toangleaxis(in quaternion q, out float angle, out float3 axis) {
			BurstCompiled_toaxisangle(q, out axis, out angle);
			angle = degrees(angle);
		}

		/// <summary>
		/// Quaternion.ToAngleAxis in radians but for the Mathematics package, following a similar naming scheme to quaternion.AxisAngle which is in radians (BURST).
		/// </summary>
		/// <param name="q"></param>
		/// <param name="angle"></param>
		/// <param name="axis"></param>
		[BurstCompile(FloatMode = FloatMode.Fast)]
		public static void BurstCompiled_toaxisangle(in quaternion q, out float3 axis, out float angle) {
			var rot = q;
			if (abs(rot.value.w) > 1.0f)
				rot = normalize(rot);
			angle = 2.0f * acos(rot.value.w); // angle
			float den = (float)sqrt(1.0 - rot.value.w * rot.value.w);
			if (den > 0.0001f) {
				axis = rot.value.xyz / den;
			}
			else {
				// This occurs when the angle is zero. 
				// Not a problem: just set an arbitrary normalized axis.
				axis = right();
			}
		}
	}
}
