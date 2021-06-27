using System.Collections.Generic;

namespace N2Library
{
    public class N2Layer
    {
        public static readonly int WEIGHTS_START_INDEX = 5;
        public int LayerId { get; set; }
        private List<Neuron> mNetNeurons;

        public N2Layer(int id)
        {
            LayerId = id;
            mNetNeurons = new List<Neuron>();
        }

        public N2Layer load(List<double> contents)
        {
            Neuron neuron = new Neuron(LayerId, (int)contents.ToArray()[1]);
            neuron.Activation = contents.ToArray()[2];
            neuron.Gradient = contents.ToArray()[3];
            int connections = (int)contents.ToArray()[4];
            for (int i = 0; i < connections * 2;)
            {
                N2Weight con = new N2Weight(contents.ToArray()[WEIGHTS_START_INDEX + i], contents.ToArray()[WEIGHTS_START_INDEX + 1 + i]);
                neuron.Weights.Add(con);
                i += 2;
            }
            mNetNeurons.Add(neuron);
            return this;
        }

        public Neuron At(int index) => mNetNeurons.ToArray()[index];

        public void Add(Neuron n) => mNetNeurons.Add(n);

        public int Size() => mNetNeurons.Count;
    }
}