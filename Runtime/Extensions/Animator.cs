using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace VAT.Shared.Extensions {
    public static partial class AnimatorExtensions {
        /// <summary>
        /// Attempts to get the bone id related to this bone. If none is found, LastBone is returned.
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="bone"></param>
        /// <returns></returns>
        public static HumanBodyBones GetBoneID(this Animator animator, Transform bone) {
            for (var i = 0; i < (int)HumanBodyBones.LastBone; i++) {
                if (animator.GetBoneTransform((HumanBodyBones)i) == bone)
                    return (HumanBodyBones)i;
            }

            return HumanBodyBones.LastBone;
        }
    }
}