using UnityEngine;

namespace VAT.Shared.Extensions {
    public static partial class AudioSourceExtensions {
        // Plays a random audio clip from array without overriding
        public static void PlayRandomOneShot(this AudioSource src, AudioClip[] clips, float vol = 1f) {
            if (clips.Length <= 0) return;
            src.PlayOneShot(clips.GetRandom(), vol);
        }

        // Plays a random audio clip from array on source
        public static void PlayRandomOverride(this AudioSource src, AudioClip[] clips) {
            if (clips.Length <= 0) return;
            src.clip = clips.GetRandom();
            src.Play();
        }

        // Gets percentage of fading in based on length
        public static float ProgressFadeIn(this AudioSource src, float fadeInLength) {
            if (!src.isPlaying || src.time > fadeInLength) return 1f;
            return src.time.DivNoNan(fadeInLength);
        }

        // Gets percentage of fading out based on length
        public static float ProgressFadeOut(this AudioSource src, float fadeOutLength) {
            if (!src.isPlaying || !src.clip || src.time < src.clip.length - fadeOutLength) return 1f;
            return (src.clip.length - src.time) / fadeOutLength;
        }

        // Returns whether the audio source is in the fade length time
        public static bool IsFading(this AudioSource src, bool fadeIn, float fadeLength, out float progress) => fadeIn ? (progress = src.ProgressFadeIn(fadeLength)) > 0f : (progress = src.ProgressFadeOut(fadeLength)) > 0f;
    }
}
