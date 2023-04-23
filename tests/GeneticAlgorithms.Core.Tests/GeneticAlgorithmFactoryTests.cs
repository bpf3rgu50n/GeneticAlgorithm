using GeneticAlgorithms.Core.Evaluatable;
using GeneticAlgorithms.Core.Evolution;
using GeneticAlgorithms.Core.Utils;
using NeuralNetworks.Core.ActivationFunctions;
using NeuralNetworks.Core.Factories;
using NeuralNetworks.Core.WeightInitializer;

namespace GeneticAlgorithms.Core.Tests
{
    public class GeneticAlgorithmFactoryTests
    {
        [Fact]
        public void Test1()
        {
            int numInputs = 3;
            int numOutputs = 1;
            int numHiddenLayers = 1;
            int numNeuronsInHiddenLayer = 5;

            INeuralNetworkFactory factory = NeuralNetworkFactory.GetInstance();
            INeuralNetwork network = factory.Create(numInputs, numOutputs, numHiddenLayers, numNeuronsInHiddenLayer);

            network.Should().NotBeNull();
            network.Should().BeAssignableTo<INeuralNetwork>();
            network.Should().BeOfType<NeuralNetwork>();
        }

        [Fact]
        public void Test2()
        {
            Mock<IEvaluatableFactory> mock = new();

            IEvaluatableFactory evaluatableFactory = mock.Object;

            NeuralNetworkConfigurationSettings networkConfig = new()
            {
                NumInputNeurons = 3,
                NumOutputNeurons = 1,
                NumHiddenLayers = 2,
                NumHiddenNeurons = 3,
                SummationFunction = new SimpleSummation(),
                ActivationFunction = new TanhActivationFunction()
            };

            IGeneticAlgorithmFactory factory = GeneticAlgorithmFactory.GetInstance(evaluatableFactory);
            IGeneticAlgorithm evolver = factory.Create(networkConfig);

            evolver.Should().NotBeNull();
            evolver.Should().BeAssignableTo<IGeneticAlgorithm>();
            evolver.Should().BeOfType<GeneticAlgorithm>();
        }

        [Fact]
        public void Test3()
        {
            Mock<IEvaluatableFactory> mock = new();

            IEvaluatableFactory evaluatableFactory = mock.Object;

            var networkFactory = NeuralNetworkFactory.GetInstance();
            var evalWorkingSetFactory = EvalWorkingSetFactory.GetInstance();

            var randomInit = new RandomWeightInitializer(new Random());
            var breederFactory = BreederFactory.GetInstance(networkFactory, randomInit);
            var mutatorFactory = MutatorFactory.GetInstance(networkFactory, randomInit);
            IGeneticAlgorithmFactory factory = GeneticAlgorithmFactory.GetInstance(networkFactory, evalWorkingSetFactory, evaluatableFactory, breederFactory, mutatorFactory);

            NeuralNetworkConfigurationSettings networkConfig = new()
            {
                NumInputNeurons = 3,
                NumOutputNeurons = 1,
                NumHiddenLayers = 2,
                NumHiddenNeurons = 3,
                SummationFunction = new SimpleSummation(),
                ActivationFunction = new TanhActivationFunction()
            };

            IGeneticAlgorithm evolver = factory.Create(networkConfig);

            evolver.Should().NotBeNull();
            evolver.Should().BeAssignableTo<IGeneticAlgorithm>();
            evolver.Should().BeOfType<GeneticAlgorithm>();
        }
    }
}