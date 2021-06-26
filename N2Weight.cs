namespace N2Library
{
    public class N2Weight
    {
        public double Weight { set; get; }
        public double DeltaWeight { set; get; }

        public N2Weight(double weight, double deltaWeight)
        {
            Weight = weight;
            DeltaWeight = deltaWeight;
        }
    }
}