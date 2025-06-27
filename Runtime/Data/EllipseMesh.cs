namespace VAT.Shared.Data
{
    public struct EllipseMesh
    {
        public Ellipse Ellipse;

        public SimpleTransform Transform;

        public bool IsFilled;

        public EllipseMesh(Ellipse ellipse, SimpleTransform transform)
        {
            Ellipse = ellipse;
            Transform = transform;
            IsFilled = false;
        }

        public EllipseMesh(Ellipse ellipse) : this(ellipse, SimpleTransform.Identity) { }
    }
}
