namespace VAT.Shared.Data
{
    /// <summary>
    /// The base of a mesh built from a curve.
    /// </summary>
    public struct CurveMesh
    {
        /// <summary>
        /// The curve that will be used as a base.
        /// </summary>
        public ICurve Curve;

        /// <summary>
        /// The local transform of the curve.
        /// </summary>
        public SimpleTransform Transform;

        /// <summary>
        /// Whether or not the curve is filled.
        /// </summary>
        public bool IsFilled;

        public CurveMesh(ICurve curve, SimpleTransform transform)
        {
            Curve = curve;
            Transform = transform;
            IsFilled = false;
        }

        public CurveMesh(ICurve curve) : this(curve, SimpleTransform.Identity) { }
    }
}
