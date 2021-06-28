using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace N2Library
{
    public class N2Trainer<T>
    {
        public N2Model Model {set; get;}
        private bool LoggingOn { set; get; }
        private Double Threshold { set; get; }
        public long IterationCount { set; get; }

        public int FeatureFields { set; get; }
        
        public N2Pipeline<T> Pipeline { set; get; }
        
        public N2Trainer(string filePath, string fileName, bool loggingOn)
        {
            Model = new N2Model(filePath);
            LoggingOn = loggingOn;
            IterationCount = 1L;
        }

        public N2Trainer(N2Model model, List<T> items, double threshold, bool logTraining)
        {
            Model = model;
            LoggingOn = logTraining;
            Threshold = threshold;
            IterationCount = 1L;
            FeatureFields = Model.Topology.ElementAt(0);
        }

        public void Train()
        {
            List<double> results = new List<double>();
            bool modelOptimised = false;

            Pipeline.Training.ForEach(async f => {

                if (!modelOptimised)
                {
                    StringBuilder sLine = new StringBuilder();
                    int input = Convert.ToInt32(f);
                    List<double> inputs = N2Common.IntBitsAsDoubles(input, FeatureFields), 
                                targets = N2Common.IntBitsAsDoubles(input, FeatureFields);

                    Model.FeedForward(inputs);
                    Model.GetResults(results);
                    Model.BackPropagate(targets);

                    sLine.Append("(")
                            .Append(N2Common.StringifyList(inputs))
                            .Append(" ) => [")
                            .Append(N2Common.StringifyList(results)).Append("] | [")
                            .Append(String.Format("%6.5f,% 6.5f]", Model.Error, Model.AverageError));
                    
                    await LogLineAsync(sLine.ToString());

                    IterationCount += 1;
                    
                    if (IterationCount % 10L == 0L)
                    {
                        if (CrossValidate())
                        {
                            modelOptimised = Tests();
                            if (modelOptimised)
                            {
                                EndTraining();
                            }
                        }
                    }
                }
            });
         
            EndTraining();
        }

        private void EndTraining()
        {
            ExportModel();
        }

        private async Task LogLineAsync(string sLine)
        {
            if (LoggingOn)
            {
                sLine += "\n";
                await using (FileStream fs = File.Create(Model.Path + "/" + "training.log"))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(sLine);
                    fs.Write(info, 0, info.Length);
                }
            }
        }

        public bool Tests()
        {
            List<string> result = new List<string>();
            bool passed = true;

            Pipeline.Testing.ForEach(s =>
            {
                int input = Convert.ToInt32(s);
                List<double> current = Test(input);               
                List<double> exploded = N2Common.IntBitsAsDoubles(input, current.Count);
                passed &= current.ToString().Equals(exploded.ToString());
                result.Add(passed ? "Pass" : "Fail");
            });

            return passed;
        }
        
        public List<double> Test(int x) => Model.Check(x).Select(f => (f >= Threshold ? 1.0 : 0.0)).ToList();

        public bool CrossValidate()
        {
            List<string> result = new List<string>();
            bool passed = true;
            Pipeline.CrossValidation.ForEach(f =>
            {
                int input = Convert.ToInt32(f);
                List<Double> current = Test(input);
                List<Double> exploded = N2Common.IntBitsAsDoubles(input, current.Count);
                passed &= current.ToString().Equals(exploded.ToString());
                result.Add(passed ? "Pass" : "Fail");
            });

            return passed;
        }

        public void ExportModel()
        {
            _ = Model.ExportAsync(Model.Path + "/" + SavedModelFilename(Model.Topology));
        }

        public List<double> ConvergenceErrors() => new List<double> { Model.Error, Model.AverageError}; 

        public static string SavedModelFilename(List<int> topology)
        {
            string sModel = string.Empty;
            topology.ForEach(c => sModel += $"{c}");
            sModel += ".txt";
            return sModel;
        }
    }
}
