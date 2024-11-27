using System;

using UnityEngine;

namespace VAT.Shared.Utilities
{
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
