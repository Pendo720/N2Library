using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace N2Library
{
    public class N2Model
    {
        public static readonly double AverageSmoothingFactor = 100.0;
        public List<N2Layer> Layers { set; get; }
        public double Error { set; get; }
        public double AverageError { set; get; }
        public List<int> Topology { set; get; }
        string mPath;

        public N2Model(string filePath)
        {
            mPath = filePath;
            LoadModel(mPath);
        }

        public N2Model(List<int> topology, string filePath)
        {
            Topology = topology;            
            mPath = filePath;
            Layers = new List<N2Layer>();
            Enumerable.Range(0, Topology.Count)
                .ToList()
                .ForEach(f =>
                {
                    Layers.Add(new N2Layer(f));
                    int outputs = f == Topology.Count - 1 ? 0 : Topology.ToArray()[f + 1];

                    for (int n = 0; n <= Topology.ToList().ElementAt(f); ++n)
                    {
                        Layers.ElementAt(Layers.Count - 1).Add(new Neuron(outputs, f, n));
                    }

                    N2Layer netLayer = Layers.ElementAt(Layers.Count - 1);
                    netLayer.At(netLayer.Size() - 1).Activation = 1.0;
                });
        }

        void FeedForward(List<double> inputs)
        {
            Enumerable.Range(0, inputs.Count)
                .ToList()
                .ForEach(f => Layers.ElementAt(0).At(f).Activation = inputs.ElementAt(f));

            Enumerable.Range(1, Layers.Count)
                .ToList()
                .ForEach(f => {
                    N2Layer prevLayer = Layers.ElementAt(f - 1);
                    Enumerable.Range(0, Layers.ElementAt(f).Size() - 1)
                    .ToList()
                    .ForEach(d => Layers.ElementAt(f).At(d).FeedForward(prevLayer));                    
                });
        }

        void BackPropagate(List<double> targets)
        {
            Error = 0.0;
            // calculate net error
            N2Layer outputLayer = Layers.ElementAt(Layers.Count - 1);

            Enumerable.Range(0, outputLayer.Size() - 1)
                .ToList()
                .ForEach(f => Error += Math.Pow(targets.ElementAt(f) - outputLayer.At(f).Activation, 2));

            Error /= outputLayer.Size() - 1;
            Error = Math.Sqrt(Error);
            AverageError = ((AverageError * AverageSmoothingFactor + Error) / (AverageSmoothingFactor + 1.0));

            Enumerable.Range(0, outputLayer.Size() - 1)
                .ToList()
                .ForEach(f => outputLayer.At(f).CalculateOutputGradient(targets.ElementAt(f)));

            //calculate gradients on hidden layer
            for (int lay = Layers.Count - 2; lay > 0; --lay)
            {
                N2Layer hiddenLayer = Layers.ElementAt(lay);
                N2Layer nextLayer = Layers.ElementAt(lay + 1);
                for (int n = 0; n < hiddenLayer.Size(); ++n)
                {
                    hiddenLayer.At(n).CalculateHiddenGradients(nextLayer);
                }
            }

            // for all layers from output to first hidden layer, updated connection weights
            for (int lay = Layers.Count - 1; lay > 0; --lay)
            {
                N2Layer cur = Layers.ElementAt(lay);
                N2Layer prev = Layers.ElementAt(lay - 1);
                for (int n = 0; n < cur.Size() - 1; ++n)
                {
                    cur.At(n).UpdateInputWeights(prev);
                }
            }
        }

        void GetResults(List<double> results)
        {
            results.Clear();
            Enumerable.Range(0, Layers.ElementAt(Layers.Count - 1).Size() - 1)
                        .ToList()
                        .ForEach(f => results.Add(Layers.ElementAt(Layers.Count-1).At(f).Activation));
        }

        void ExportNetwork(string modelFile)
        {

/*            try
            {
                File fw = new File(modelFile);
                FileOutputStream handle = new FileOutputStream(fw);
                if (Topology != null)
                {
                    string[] sLine = { "topology: "};
                    IntStream.range(0, mTopology.size())
                            .forEach(i->sLine[0] += String.format(Locale.UK, "%d ", mTopology.get(i)));

                    sLine[0] += String.format(Locale.UK, "\n%4.3f | % 4.3f | % 12.11f | %3.1f | % 12.11f", N2Neuron.mEta, N2Neuron.mAlpha, mAverageError, mAverageSmoothingFactor, mError);
                    IntStream.range(0, mLayers.size())
                            .forEach(i->sLine[0] += mLayers.get(i).export());
                    handle.write(sLine[0].getBytes());
                }
                handle.close();
            }
            catch (IOException e)
            {
                e.printStackTrace();
            }*/
        }

        public void LoadModel(string modelFile)
        {
            /*try
            {
                BufferedReader reader = new BufferedReader(new InputStreamReader(new FileInputStream(modelFile)));
                String sLine = reader.readLine();
                if (sLine != null && sLine.contains("topology:"))
                {
                    sLine = sLine.substring(sLine.indexOf(':') + 1);
                    mTopology = Stream.of(sLine.split(" "))
                            .filter(s->!s.isEmpty())
                            .map(Integer::valueOf).collect(Collectors.toList());

                    mLayers = new ArrayList<>();
                    sLine = reader.readLine();
                    List<Double> parameters = Stream.of(sLine.split("\\s\\|"))
                                                .filter(s->!s.isEmpty() && !s.contains("|"))
                                                .map(Double::valueOf).collect(Collectors.toList());

                    N2Neuron.mEta = parameters.get(0);
                    N2Neuron.mAlpha = parameters.get(1);
                    mAverageError = parameters.get(2);
                    mAverageSmoothingFactor = parameters.get(3);
                    mError = parameters.get(4);

                    reader.lines()
                        .forEach(l-> {
                        if (!l.isEmpty())
                        {
                            List < Double > params =
                               Stream.of(l.split("\\s"))
                                       .filter(s-> !s.isEmpty() && !s.contains("|"))
                                       .map(Double::valueOf).collect(Collectors.toList());

                            int layer = params.get(0).intValue();
                            N2Layer current = this.mLayers.stream().filter(i->i.getLayerId() == layer).findAny().orElse(null);
                            if (current != null)
                            {
                                this.mLayers.get(layer).load(params);
                            }
                            else
                            {
                                N2Layer nl = new N2Layer(layer);
                                this.mLayers.add(nl.load(params));
                            }
                        }
                    });
                }
                reader.close();
                //            Uncomment the following line verify correct loading by capturing the network state
                //            exportNetwork(mPath + "/new_snapshot.txt");
            }
            catch (IOException e)
            {
                e.printStackTrace();
            }*/
        }


        public List<double> check(int input)
        {
            List<double> results = new List<double>();
            //FeedForward(N2Utils.int2BitDoubles(input, Topology.ElementAt(0)));
            GetResults(results);
            return results;
        }
    }
}
