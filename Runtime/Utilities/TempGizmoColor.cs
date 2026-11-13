using System;

using UnityEngine;

namespace VAT.Shared.Utilities
{
    /// <summary>
    /// A disposable Gizmos color holder, which automatically resets the Gizmos color after it is released.
    /// </summary>
    public class TempGizmoColor : IDisposable
    {
        private Color _color;

        public TempGizmoColor() 
        {
            _color = Gizmos.color;
        }

        public void Dispose()
        {
            Gizmos.color = _color;
        }
    }
}
