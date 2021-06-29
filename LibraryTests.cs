using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace N2Library
{
    public class FieldTests
    {
        [Fact]
        void Successful_Field_Creation()
        {
            // arrange
            Field t = new Field();
            // act 

            // assert
            Assert.True(t.Value.Equals(float.MinValue));
            Assert.True(t.Name.Equals("None"));
        }
        [Fact]
        void Normalise_Field()
        {
            // arrange
<<<<<<< HEAD
            float xValue = 12.4f, fMin = 3.6f, fMax = 19.7f; 
=======
            float xValue = 12.4f, fMin = 3.6f, fMax = 19.7f;
>>>>>>> neuron
            Field t = new Field("x", xValue);
            // act 
            t.Normalise(fMin, fMax);
            // assert
<<<<<<< HEAD
            Assert.True(t.Value.Equals((xValue-fMin)/fMax));
=======
            Assert.True(t.Value.Equals((xValue - fMin) / fMax));
>>>>>>> neuron
        }
    }

    public class NeuronTests
    {

        [Fact]
        void Creation_Test()
        {
            // arrange
            Neuron t = new Neuron(10, 6);
            // act
            //assert
            Assert.True(t.Id == 10 && t.LayerIndex == 6 && t.Weights != null);
        }
        [Fact]
        void InvalidOperationException_Creation_Test()
        {
            // arrange
            // act
            //assert            
            Assert.Throws<InvalidOperationException>(() => new Neuron(-1, 0));
        }
    }

    public class N2WeightTests
    {
        [Fact]
        void Creation_Test()
        {
            // arrange
            float wght = 0.12f, delta = 0.23f;
            // act
            N2Weight t = new N2Weight(wght, delta);

            // assert
            Assert.True(t.Weight != 0.0 && t.DeltaWeight != 0.0);
            Assert.True(t.Weight == wght && t.DeltaWeight == delta);
        }
    }

    public class N2LayerTests
    {
        [Fact]
        void Creation_Test()
        {
            // arrange
            N2Layer t = new N2Layer(0);
            // act 
            t.Add(new Neuron(id: 0, layerIndex: 0));
            // assert
            Assert.NotNull(t);
            Assert.NotNull(t.LayerNeurons);
            Assert.True(1 == t.LayerNeurons.Count);
            Assert.True(t.LayerId == 0);
        }

        [Fact]
        void Valid_Element_At()
        {
            // arrange
            N2Layer t = new N2Layer(0);
            // act 
            t.Add(new Neuron(id: 0, layerIndex: 0));
            // assert
            Assert.True(t.At(0) != null);
            Assert.True(t.At(0).Id == 0);
            Assert.True(t.At(0).LayerIndex == 0);
        }
        [Fact]
        void Invalid_Element_At()
        {
            // arrange
            N2Layer t = new N2Layer(0);
            // act 
            // assert
            Assert.Throws<IndexOutOfRangeException>(() => t.At(0));
        }
    }

    public class N2TrainerTests { 
    
    }

    public class N2ModelTests { 
    
    }

    public class N2PipelineTests { 
        [Fact]
        void Default_Ratio_Test() {
            // arrange
            List<int> data = Enumerable.Range(0, 10).ToList();
            // act
            N2Pipeline<int> t = new N2Pipeline<int>(data);
            // assert
            Assert.Equal(t.Training.Count, (int)(0.6f*t.Data.Count));
            Assert.Equal(t.Testing.Count, t.CrossValidation.Count);
            Assert.Equal(t.Testing.Count + t.CrossValidation.Count + t.Training.Count, t.Data.Count);
        }

        [Fact]
        void None_Default_Ratio_Test()
        {
            // arrange
            List<int> data = Enumerable.Range(0, 10).ToList();
            // act
            N2Pipeline<int> t = new N2Pipeline<int>(data, 0.4f);
            // assert
            Assert.Equal(t.Training.Count, (int)(0.4f * t.Data.Count));
            Assert.Equal(t.Testing.Count, t.CrossValidation.Count);
            Assert.Equal(t.Testing.Count + t.CrossValidation.Count + t.Training.Count, t.Data.Count);

        }
    }

    public class N2TrainerTests { }
    public class N2ModelTests { }
    public class N2PipelineTests { }
}
