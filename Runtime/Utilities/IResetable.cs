using System;
using System.Collections.Generic;

using UnityEngine;

namespace VAT.Shared.Utilities {
    /// <summary>
    /// Generic interface for implementing a reset method.
    /// </summary>
    public interface IResetable {
        /// <summary>
        /// Resets the state.
        /// </summary>
        void OnReset();
    }
}