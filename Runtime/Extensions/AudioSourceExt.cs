using UnityEngine;

namespace VAT.Shared.Extensions {
    /// <summary>
    /// Extension methods for AudioSources.
    /// </summary>
    public static partial class AudioSourceExtensions {
        /// <summary>
        /// Plays a random Audio Clip without stopping current clips.
        /// </summary>
        /// <param name="src">The audio source.</param>
        /// <param name="clips">The array of audio clips.</param>
        /// <param name="volume">The volume to play.</param>
        public static void PlayRandomOneShot(this AudioSource src, AudioClip[] clips, float volume = 1f) {
            if (clips.Length <= 0) 
                return;
            src.PlayOneShot(clips.GetRandom(), volume);
        }

        /// <summary>
        /// Plays a random audio clip.
        /// </summary>
        /// <param name="src">The audio source.</param>
        /// <param name="clips">The array of audio clips.</param>
        public static void PlayRandom(this AudioSource src, AudioClip[] clips) {
            if (clips.Length <= 0) 
                return;

            src.clip = clips.GetRandom();
            src.Play();
        }

        /// <summary>
        /// Gets the percent of progress on fading in.
        /// </summary>
        /// <param name="src">The audio source.</param>
        /// <param name="fadeInLength">The length of fading in.</param>
        /// <returns>The percent of the fade in.</returns>
        public static float GetFadeInProgress(this AudioSource src, float fadeInLength) {
            if (!src.isPlaying || src.time > fadeInLength) return 1f;
            return src.time.DivNoNan(fadeInLength);
        }

        /// <summary>
        /// Gets the percent of progress on fading out.
        /// </summary>
        /// <param name="src">The audio source.</param>
        /// <param name="fadeOutLength">The length of fading out.</param>
        /// <returns>The percent of the fade out.</returns>
        public static float GetFadeOutProgress(this AudioSource src, float fadeOutLength) {
            if (!src.isPlaying || !src.clip || src.time < src.clip.length - fadeOutLength) return 1f;
            return (src.clip.length - src.time) / fadeOutLength;
        }

        /// <summary>
        /// Returns if this audio source has begun to fade in or out.
        /// </summary>
        /// <param name="src">The audio source.</param>
        /// <param name="isFadingIn">Are we checking for fading in or fading out?</param>
        /// <param name="length">The length of the fade.</param>
        /// <param name="progress">The output progress.</param>
        /// <returns>If the AudioSource is currently fading.</returns>
        public static bool IsFading(this AudioSource src, bool isFadingIn, float length, out float progress) => isFadingIn ? (progress = src.GetFadeInProgress(length)) > 0f : (progress = src.GetFadeOutProgress(length)) > 0f;
    }
}
