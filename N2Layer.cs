using System.Collections.Generic;

namespace N2Library
{
    public class N2Layer
    {
        public int LayerId { get; set; }
        private List<Neuron> mNetNeurons;

        public N2Layer(int id)
        {
            LayerId = id;
            mNetNeurons = new List<Neuron>();
        }

        public N2Layer load(List<double> contents)
        {
            /*Neuron neuron = new Neuron(LayerId, contents.ToArray()get(1).intValue());
            neuron.setActivationValue(contents.get(2));
            neuron.setGradient(contents.get(3));
            int connections = contents.get(4).intValue();
            for (int i = 0; i < connections * 2;)
            {
                N2Weight con = new N2Weight(contents.get(WEIGHTS_ARE_FROM + i), contents.get(WEIGHTS_ARE_FROM + 1 + i));
                neuron.getWeights().add(con);
                i += 2;
            }
            mNetNeurons.Add(neuron);
*/
            return this;
        }

        public Neuron get(int index) => mNetNeurons.ToArray()[index];

        public void add(Neuron n) => mNetNeurons.Add(n);

        public int size() => mNetNeurons.Count;
    }
}