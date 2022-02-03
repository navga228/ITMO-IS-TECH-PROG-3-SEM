namespace Banks
{
    public class Interest
    {
        public Interest(float firstValue, float secondValue, float percent)
        {
            Sum[0] = firstValue;
            Sum[1] = secondValue;
            Percent = percent;
        }

        public float[] Sum { get; set; } = new float[2];
        public float Percent { get; set; }
    }
}