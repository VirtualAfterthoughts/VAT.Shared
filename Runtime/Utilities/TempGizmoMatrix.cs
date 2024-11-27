using System;

using UnityEngine;

namespace VAT.Shared
{
    public class TempGizmoMatrix : IDisposable
    {
        private Matrix4x4 _matrix;

        public TempGizmoMatrix() 
        {
            _matrix = Gizmos.matrix;
        }

        public void Dispose()
        {
            Gizmos.matrix = _matrix;
        }
    }
}
