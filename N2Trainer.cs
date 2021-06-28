using System;
using System.Collections.Generic;

namespace N2Library
{
    public class N2Trainer<T>
    {
        private N2Model mNetwork;
        private bool LoggingOn { set; get; }
        private Double Threshold { set; get; }
        public long IterationCount { set; get; }
        public N2Trainer(string filePath, string fileName, bool loggingOn)
        {
            this.mNetwork = new N2Model(filePath);
            this.LoggingOn = loggingOn;
            this.IterationCount = 1L;
        }

        public N2Trainer(N2Model _network, List<T> items, double threshold, bool logTraining)
        {
            this.mNetwork = _network;
            this.LoggingOn = logTraining;
            this.Threshold = threshold;
            int mInputs = this.mNetwork.Topology.ToArray()[0];
            this.IterationCount = 1L;
        }

        public void train()
        {
            /*
            mState = N2State.Training;
            if (mLoggingOn)
            {
                try
                {
                    mLogHandle = new FileOutputStream(mNetwork.mPath + "/training.log");
                }
                catch (FileNotFoundException e)
                {
                    e.printStackTrace();
                }
            }

            List<Double> results = new ArrayList<>();
            final boolean[] modelOptimised = { false};
            mDataFactory.duplicate(mDataFactory.train(), 1000)
                .forEach(i-> {
                if (!modelOptimised[0])
                {
                    T code = (T)mDataFactory.nextCodedFeature();
                    List<Double> inputs = mDataFactory.getValues(), targets;
                    StringBuilder sLine = new StringBuilder();
                    targets = mDataFactory.getLabel((Integer)code);
                    mNetwork.feedForward(inputs);
                    mNetwork.getResults(results);
                    mNetwork.backPropagate(targets);
                    sLine.append("(")
                            .append(N2Utils.stringifyList(inputs))
                            .append(" ) => [")
                            .append(N2Utils.stringifyList(results)).append("] | [")
                            .append(String.format(Locale.UK, "%6.5f,% 6.5f]", mNetwork.getError(), mNetwork.getAverageError()));
                    logLine(sLine.toString());
                    this.mIterationCount += 1;
                    mContext.sendBroadcast(new Intent(TRAINING_STATUS));
                    if (this.mIterationCount % 10L == 0L)
                    {
                        if (crossValidate())
                        {
                            modelOptimised[0] = tests();
                            if (modelOptimised[0])
                            {
                                Log.d("Trainer", "train: Broken out of the iterative looping after " + mIterationCount + " iterations");
                                Log.d("Trainer", "train: Average learning error: " + String.format(Locale.UK, "%.06f", mNetwork.getAverageError()));
                                endTraining();
                            }
                        }
                    }
                }
            });
            */
            EndTraining();
        }

        private void EndTraining()
        {
            /*
            if (LoggingOn)
            {
                try
                {
                    mLogHandle.close();
                }
                catch (IOException e)
                {
                    e.printStackTrace();
                }
            }
            */
        }

        private void LogLine(String sLine)
        {
            if (LoggingOn)
            {
                sLine += "\n";
                /*
                try
                {
                    mLogHandle.write(sLine.getBytes());
                }
                catch (IOException e)
                {
                    e.printStackTrace();
                }*/
            }
        }

        public bool Tests()
        {
            List<string> result = new List<string>();
            bool[] passed = { true};
            /*mDataFactory.test().forEach(i->{
                List<Double> current = test((Integer)i);
                List<Double> exploded = N2Utils.int2BitDoubles((Integer)i, current.size());
                passed[0] &= current.toString().equals(exploded.toString());
                result.add(passed[0] ? "Pass" : "Fail");
            });

            Log.d("Trainer", "tests : " + (passed[0] ? " PASSED " : " FAILED ") + result);*/
            return passed[0];
        }

        
        public List<double> Test(int x) 
        {
            return new List<double>(); // mNetwork.check(x).stream().map(d->(d >= mThreshold) ? 1.0 : 0.0).collect(toList()); 
        }

        public bool CrossValidate()
        {
            List<string> result = new List<string>();
            bool [] passed = { true};
/*            mDataFactory.crossValidation().forEach(i->{
                List<Double> current = test((Integer)i);
                List<Double> exploded = N2Utils.int2BitDoubles((Integer)i, current.size());
                passed[0] &= current.toString().equals(exploded.toString());
                result.add(passed[0] ? "Pass" : "Fail");
            });

            Log.d("Trainer", "crossValidate : Iteration:" + mIterationCount.toString() + (passed[0] ? " - PASSED " : " - FAILED ") + result);
*/            return passed[0];
        }

        public void ExportModel()
        {
            //mNetwork.exportNetwork(mNetwork.mPath + "/" + savedModelFilename(mNetwork.getTopology(), mContext.getString(R.string.n2_model)));
        }

        public List<double> ConvergenceErrors() 
        {
            return new List<double>();// Arrays.asList(mNetwork.getError(), mNetwork.getAverageError()); 
        }

        public static string SavedModelFilename(List<int> topology, string format)
        {
            string[] sModel = { format};
            //topology.ForEach(c => sModel[0] += string.format(Locale.UK, "_%02d", c));
            sModel[0] += ".txt";
            return sModel[0];
        }
    }
}
