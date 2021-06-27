using System;
using System.Collections.Generic;
using System.Linq;

namespace N2Library
{
    public class Neuron
    {
        public static readonly double ETA = 0.345;
        public static readonly double ALPHA = 0.5;

        public int Id { get; set; }
        public int LayerIndex { get; set; }
        public double Activation { get; set; }
        public double Gradient{ get; set; }

        public List<N2Weight> Weights { get; set; }
        public Neuron(int id, int layerIndex)
        {
            Id = id;
            LayerIndex = layerIndex;
            if(Id < 0 || LayerIndex < 0)
            {
                throw new InvalidOperationException("Neither Id nor LayerIndex can be negative");
            }
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
            double toReturn = 0.0;
            Enumerable.Range(0, previous.Size())
                .ToList()
                .ForEach(f => toReturn = previous.At(f).Activation * previous.At(f).Weights.ElementAt(Id).Weight);
            Activation = ActivationFunction(toReturn);
        }

        public void UpdateInputWeights(N2Layer previous)
        {
            Enumerable.Range(0, previous.Size())
                .ToList()
                .ForEach(f =>{
                        Neuron cur = previous.At(f);
                        double oldDel = cur.Weights.ElementAt(Id).DeltaWeight;
                        double newDel = ETA * cur.Activation * Gradient + ALPHA * oldDel;
                        cur.Weights.ElementAt(Id).DeltaWeight = newDel;
                        cur.Weights.ElementAt(Id).Weight += newDel;
                    });
        }

        double ActivationFunction(double x) 
        { 
            return Math.Tanh(x); 
        }

        double ActivationDerivativeFunction(double x) 
        { 
            return (1.0 - x * x); 
        }

        void CalculateOutputGradient(double val) 
        { 
            Gradient = ((val - Activation) * ActivationDerivativeFunction(Activation)); 
        }

        double SumDOW(N2Layer next)
        {
            double toReturn = 0.0f;
            Enumerable.Range(0, next.Size() - 1)
                .ToList()
                .ForEach(f =>toReturn += Weights.ElementAt(f).Weight * next.At(f).Gradient);
            return toReturn;
        }

        void CalculateHiddenGradients(N2Layer next) 
        {
            Gradient = SumDOW(next) * ActivationDerivativeFunction(Activation); 
        }

    }
}
