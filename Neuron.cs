using System;
using System.Collections.Generic;
using System.Linq;

namespace N2Library
{
    public class Neuron
    {
        public static double ETA = 0.345;
        public static double ALPHA = 0.5;

        public int Id { get; set; }
        public int LayerIndex { get; set; }
        public double Activation { get; set; }
        public double Gradient{ get; set; }

        public List<N2Weight> Weights { get; set; }
        public Neuron(int id, int layerIndex)
        {
            Id = id;
            LayerIndex = layerIndex;
            Gradient = 0.0;
            Weights = new List<N2Weight>();
        }
        Neuron(int outputs, int layer, int id)
        {
            Id = id;
            LayerIndex = layer;
            Weights = new List<N2Weight>();
            Gradient = 0.0;
            Enumerable.Range(0, outputs)
                        .ToList()
                        .ForEach(f =>Weights.Add(new N2Weight(new Random().NextDouble(), 0.0)));
        }


        public void Feedforeward(N2Layer previous)
        {

        }

        public void UpdateInputWeights(N2Layer previous)
        {

        }

        double ActivationFunction(double x) { return Math.Tanh(x); }

        double ActivationDerivativeFunction(double x) { return (1.0 - x * x); }

        void CalculateOutputGradient(double val) { Gradient = ((val - Activation) * ActivationDerivativeFunction(Activation)); }

        double SumDOW(N2Layer next)
        {
            double toReturn = 0.0f;
            Enumerable.Range(0, next.size() - 1)
                .ToList()
                .ForEach(f =>toReturn += Weights.ElementAt(f).Weight * next.get(f).Gradient);
            return toReturn;
        }
    }
}
