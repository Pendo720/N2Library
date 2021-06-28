using System;
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
            float xValue = 12.4f, fMin = 3.6f, fMax = 19.7f; 
            Field t = new Field("x", xValue);
            // act 
            t.Normalise(fMin, fMax);
            // assert
            Assert.True(t.Value.Equals((xValue-fMin)/fMax));
        }
    }

    public class NeuronTests
    {

        [Fact]
        void Successful_Creation()
        {
            // arrange
            Neuron t = new Neuron(10, 6);
            // act
            //assert
            Assert.True(t.Id == 10 && t.LayerIndex == 6 && t.Weights != null);
        }
        [Fact]
        void Invalid_Creation()
        {
            // arrange
            // act
            //assert            
            Assert.Throws<InvalidOperationException>(() =>new Neuron(-1, 0));
        }
    }

    public class N2WeightTests
    {
        [Fact]
        void Successful_Creation()
        {
            // arrange
            N2Weight t = new N2Weight(0.12, 0.23);
            
            // act

            // assert
            Assert.True(t.Weight != 0.0 && t.DeltaWeight != 0.0);
            Assert.True(t.Weight == 0.12 && t.DeltaWeight == 0.23);
        }
    }

    public class N2LayerTests
    {
        [Fact]
        void Successful_Creation()
        {
            // arrange
            N2Layer t = new N2Layer(0);
            // act 
            t.Add(new Neuron(id:0, layerIndex: 0));
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
            Assert.Throws<IndexOutOfRangeException>(()=>t.At(0));
        }
    }

    public class N2TrainerTests { }
    public class N2ModelTests { }
    public class N2PipelineTests { }
}
