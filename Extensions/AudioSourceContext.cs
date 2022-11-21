using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VAT.Shared.Extensions {
    public static partial class AudioSourceExtensions {
#if UNITY_EDITOR
        [MenuItem("CONTEXT/AudioSource/Realistic Rolloff")]
        public static void RealisticRolloff(MenuCommand command) {
            Undo.RecordObject(command.context, "AudioSource Realistic Rolloff");
            (command.context as AudioSource).SetRealisticRolloff();
            EditorUtility.SetDirty(command.context);
        }
#endif

        // Gives the audio source a more realistic doppler and rolloff
        public static void SetRealisticRolloff(this AudioSource source) {
            // Creates an animation curve for the audio rolloff using more realistic values
            AnimationCurve animCurve = new AnimationCurve(
                new Keyframe(source.minDistance, 1f),
                new Keyframe(source.minDistance + (source.maxDistance - source.minDistance) / 4f, .35f),
                new Keyframe(source.maxDistance, 0f));

            // Apply the rolloff changes
            source.rolloffMode = AudioRolloffMode.Custom;
            animCurve.SmoothTangents(1, .025f);
            source.SetCustomCurve(AudioSourceCurveType.CustomRolloff, animCurve);
            
            // Change doppler level and spread to be more realistic
            source.dopplerLevel = 0.5f;
            source.spread = 60f;
        }
    }
}