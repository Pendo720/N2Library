using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace N2Library
{
    public class N2Model
    {
        public static readonly double AverageSmoothingFactor = 100.0;
        public List<N2Layer> Layers { set; get; }
        public double Error { set; get; }
        public double AverageError { set; get; }
        public List<int> Topology { set; get; }
        public string Path { set; get; }

        public N2Model(string filePath)
        {
            Path = filePath;
            ImportAsync(Path);
        }

        public N2Model(List<int> topology, string filePath)
        {
            Topology = topology;            
            Path = filePath;
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

        public void FeedForward(List<double> inputs)
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

        public void BackPropagate(List<double> targets)
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

        public void GetResults(List<double> results)
        {
            results.Clear();
            Enumerable.Range(0, Layers.ElementAt(Layers.Count - 1).Size() - 1)
                        .ToList()
                        .ForEach(f => results.Add(Layers.ElementAt(Layers.Count-1).At(f).Activation));
        }

        public async Task ExportAsync(string modelFile)
        {
            // Create the file, or overwrite if the file exists.
            await using (FileStream fs = File.Create(modelFile))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(JsonSerializer.Serialize(this).ToString());
                fs.Write(info, 0, info.Length);
            }
        }

        public Task ImportAsync(string modelFile)
        {
            using StreamReader sr = File.OpenText(modelFile);
            string sText = sr.ReadToEndAsync().Result;
            N2Model model = JsonSerializer.Deserialize<N2Model>(sText);
            return Task.CompletedTask;           
        }

        public List<double> Check(int input)
        {
            List<double> results = new List<double>();
            FeedForward(N2Common.IntBitsAsDoubles(input, Topology.ElementAt(0)));
            GetResults(results);
            return results;
        }
    }
}
