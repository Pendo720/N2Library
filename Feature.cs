using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace N2Library
{

    public sealed class Feature<T> : IDistance<Feature<T>>
    {
        public List<T> Fields { get; set; }
        public Feature(List<T> fields) => Fields = fields;

        public Feature() => Fields = new List<T>();

        public List<T> Add(T toAdd)
        {
            Fields.Add(toAdd);
            return Fields;
        }

        public Feature<T> Reset()
        {
            Fields.Clear();
            return this;
        }
        public override string ToString()
        {
            string toReturn = "(";
            Fields.Select(n => toReturn += n.ToString().Trim() + ", ").ToList();
            if (toReturn.Length > 2)
            {
                toReturn = toReturn.Remove(toReturn.Length - 2) + ")\n";
            }
            return toReturn;
        }
        public string asJson() => JsonSerializer.Serialize(this);
        public float distanceTo(Feature<T> other)
        {
            float toReturn = 0.0f;
            if (other != null && other.Fields.Count > 0)
            {
                List<T> otherFields = other.Fields;
                for (int i = 0; i < Fields.Count; i++)
                {
                    var ff = Fields.ElementAt(i) as Field;
                    var otherF = otherFields.ElementAt(i) as Field;

                    if (ff != null && otherF != null)
                    {
                        float d = 0.0f;
                        if (otherF != null)
                        {

                            d = (ff.Value - otherF.Value);
                            toReturn += d * d;
                        }
                    }
                }
            }

            return toReturn;
        }

        public List<float> GetValues()
        {
            List<float> toReturn = new List<float>();
            (Fields as List<Field>).ForEach(f =>
            {
                toReturn.Add(f.Value);
            });

            return toReturn;
        }
        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            bool toReturn = Fields.Count > 0;

            if ((obj as Feature<T>) != null && toReturn)
            {
                for (int i = 0; i < Fields.Count; i++)
                {
                    Feature<T> other = (obj as Feature<T>);
                    if (other != null && other.Fields.Count > 0)
                    {
                        toReturn &= Fields.ElementAt(i).Equals((obj as Feature<T>).Fields.ElementAt(i));
                    }
                    else
                    {
                        toReturn &= false;
                    }
                }

            }
            // TODO: write your implementation of Equals() here
            return toReturn;
        }

        // override object.GetHashCode
        public override int GetHashCode() => base.GetHashCode();
    }
}
