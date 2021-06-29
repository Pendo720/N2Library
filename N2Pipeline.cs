using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace N2Library
{
    public class N2Pipeline<T>
    {
        public ImmutableList<T> Data { get; }
        public List<T> Training { get; set; }
        public List<T> Testing { get; set; }
        public List<T> CrossValidation { get; set; }

        public N2Pipeline(List<T> data, float trainRatio = 0.6f)
        {
            Data = data.ToImmutableList();
            Split(trainRatio);
        }

        //     Splits the data into training, testing and cross validation sets
        //     The default splitting ratio is 60%, 20% and 20%
        private void Split(float training = 0.6f)
        {
            int count = (int)(training * Data.Count);
            Training = Data.Take(count).ToList();
            
            count = (Data.Count - Training.Count) /2;
            Testing = Data.Skip(Training.Count).Take(count).ToList();
            
            CrossValidation = Data.Skip(Training.Count + Testing.Count).Take(count).ToList();
        }

        /*public static List<T> ShuffleAndDuplicate(List<T> items, int times)
        {
            items.OrderBy(f => new Random().Next());
            Enumerable.Range(0, times / 2).ToList().ForEach(p =>
                {
                    items.AddRange(items);
                });
            return items;
        }*/

        // Normalise feature in readiness for processing
        public static List<Feature<T>> Normalise(List<Feature<T>> items)
        {
            if (items != null && items.Count > 1)
            {
                items.ForEach(x => Console.WriteLine(x.ToString().Trim()));
                var all = new List<T>();
                items.ForEach(i => all.AddRange(i.Fields));

                var minmax = all.GroupBy(p => (p as Field).Name)
                                .ToList()
                                .Select(p => new { Name = p.Key, Min = p.Min(s => (s as Field).Value), Max = p.Max(s => (s as Field).Value) })
                                .ToList();
                items.ForEach(f =>
                {
                    for (int i = 0; i < f.Fields.Count; i++)
                    {
                        var limits = minmax.ElementAt(i);
                        var field = f.Fields.ElementAt(i) as Field;
                        if (field.Name == limits.Name)
                        {
                            field.Value = field.Normalise(limits.Min, limits.Max);
                        }
                    }
                });

                Console.WriteLine("Normalised features");
                items.ForEach(x => Console.WriteLine(x.ToString().TrimEnd()));
            }

            return items;
        }

        // Imports input data from a csv file
        // returns List of features contained in the the file with each line 
        // corresponding to an item
        public static List<Feature<T>> ImportCSV(string csvfile, ref List<string> CentroidTags)
        {
            var items = new List<Feature<T>>();
            try
            {
                if (File.Exists(csvfile))
                {
                    using (StreamReader sr = File.OpenText(csvfile))
                    {
                        string s = string.Empty;
                        CentroidTags = sr.ReadLine().Split(',').ToList();
                        string[] sFields = sr.ReadLine().Split(',');
                        while ((s = sr.ReadLine()) != null)
                        {
                            List<Field> feature = new List<Field>();
                            string[] values = s.Split(',');
                            sFields.Select((f, i) => (f, i))
                                .ToList()
                                .ForEach(c =>feature.Add(new Field(sFields[c.i], float.Parse(values[c.i]))));                            
                            items.Add(feature as Feature<T>);
                        }
                    }
                }
                else
                {
                    Console.WriteLine(csvfile + " does not exists");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return Normalise(items);
        }
    }
}
