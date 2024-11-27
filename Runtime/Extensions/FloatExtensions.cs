namespace VAT.Shared.Extensions
{
    public static class FloatExtensions
    {
        public static float Average(params float[] values)
        {
            return Sum(values) / values.Length;
        }

        public static float Sum(params float[] values)
        {
            float t = 0f;

            for (int i = 0; i < values.Length; i++)
            {
                t += values[i];
            }

            return t;
        }
    }
}
