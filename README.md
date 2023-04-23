# GeneticAlgorithm
A genetic algorithm for evolving neural networks based on [NeuralNetwork](https://github.com/jobeland/NeuralNetwork) framework

### Creating the Genetic Algorithm

Creating an instance of IGeneticAlgorithm can be done using an instance of GeneticAlgorithmFactory which implements IGeneticAlgorithmFactory.

##### The Short Way
The short way to create one is to use the default values, where all you pass in is your implementation of `IEvaluatableFactory`, and your `NeuralNetworkConfigurationSettings`:
```c#
var evaluatableFactory = MyEvaluatableFactory.GetInstance();
NeuralNetworkConfigurationSettings networkConfig = new NeuralNetworkConfigurationSettings
            {
                NumInputNeurons = 3,
                NumOutputNeurons = 1,
                NumHiddenLayers = 2,
                NumHiddenNeurons = 3,
                SummationFunction = new SimpleSummation(),
                ActivationFunction = new TanhActivationFunction()
            };
IGeneticAlgorithm evolver = GeneticAlgorithmFactory.GetInstance(evaluatableFactory).Create(networkConfig);
```

This will create an instance of `IGeneticAlgorithm` for your networks that will have 3 inputs, 1 output, and 2 hidden layers each containing 3 neurons. 

##### The Long Way
If you wish to override some of the inner functionality, you can do so by extending the dependent interface factories and injecting them. Below are the default values that are set the same as if you used the short way, just explicitly injected:
```c#
var networkFactory = NeuralNetworkFactory.GetInstance();
var evalWorkingSetFactory = EvalWorkingSetFactory.GetInstance();
var evaluatableFactory = MyEvaluatableFactory.GetInstance();
var randomInit = new RandomWeightInitializer(new Random());
var breederFactory = BreederFactory.GetInstance(networkFactory, randomInit);
var mutatorFactory = MutatorFactory.GetInstance(networkFactory, randomInit);
IGeneticAlgorithmFactory factory = GeneticAlgorithmFactory.GetInstance(networkFactory, evalWorkingSetFactory, evaluatableFactory, breederFactory, mutatorFactory);

NeuralNetworkConfigurationSettings networkConfig = new NeuralNetworkConfigurationSettings
            {
                NumInputNeurons = 3,
                NumOutputNeurons = 1,
                NumHiddenLayers = 2,
                NumHiddenNeurons = 3,
                SummationFunction = new SimpleSummation(),
                ActivationFunction = new TanhActivationFunction()
            };
IGeneticAlgorithm evolver = factory.Create(networkConfig);
```

We can also go a step further by also specifying all of the settings for the algorithm:
```c#
GenerationConfigurationSettings generationSettings = new GenerationConfigurationSettings
            {
                UseMultithreading = true,
                GenerationPopulation = 1000
            };
EvolutionConfigurationSettings evolutionSettings = new EvolutionConfigurationSettings
            {
                NormalMutationRate = 0.05,
                HighMutationRate = 0.5,
                GenerationsPerEpoch = 10,
                NumEpochs = 1000
            };
IGeneticAlgorithm evolver = factory.Create(networkConfig, generationSettings, evolutionSettings);
```

Additionally, we can override the factories for `IBreeder`, `IMutator`, and `IEvalWorkingSet` by injecting the objects to the `Create()` method. A good reason for doing this is if you desire to override the default mutation settings, you can do so specifying your own `MutationConfigurationSettings`:
```c#
MutationConfigurationSettings mutationSettings = new MutationConfigurationSettings
            {
                MutateAxonActivationFunction = true,
                MutateNumberOfHiddenLayers = true,
                MutateNumberOfHiddenNeuronsInLayer = true,
                MutateSomaBiasFunction = true,
                MutateSomaSummationFunction = true,
                MutateSynapseWeights = true
            };
            
var random = new RandomWeightInitializer(new Random());
INeuralNetworkFactory factory = NeuralNetworkFactory.GetInstance();
IBreeder breeder = BreederFactory.GetInstance(factory, random).Create();
IMutator mutator = MutatorFactory.GetInstance(factory, random).Create(mutationSettings);
IEvalWorkingSet history = EvalWorkingSetFactory.GetInstance().Create(50);
IEvaluatableFactory evaluatableFactory = new GameEvaluationFactory();

var GAFactory = GeneticAlgorithmFactory.GetInstance(evaluatableFactory);
IGeneticAlgorithm evolver = GAFactory.Create(networkConfig, generationSettings, evolutionSettings, factory, breeder, mutator, history, evaluatableFactory);
```

### Using the Genetic Algorithm
Once created, using the algorithm requires running it, and getting the result.
```c#
evolver.RunSimulation();
...
INeuralNetwork best = evolver.GetBestPerformer();
```

### Creating IEvaluatable and IEvaluatableFactory
In order for the Genetic Algorithm to evolve your network, it needs a means of running the scenario you wish to optimize for, and a way to evaluate its performance of that scenario. The way to do this is by implementing `IEvaluatable`, and passing in an `IEvaluatableFactory` to the Genetic Algorithm. 
##### IEvaluatable
The IEvaluatable interface contains the following two methods:
```c#
public interface IEvaluatable
{
    void RunEvaluation();
    double GetEvaluation();
}
```
Where `RunEvaluation()` is called by the algorithm to run your given scenario, and `GetEvaluation()` is called afterwards to determine the performance of that given scenario. An example implemenation can be found in the [BasicGameNeuralNetworkTrainer](https://github.com/jobeland/BasicGameNeuralNetworkTrainer) repo in [GameEvaluation.cs](https://github.com/jobeland/BasicGameNeuralNetworkTrainer/blob/master/Trainer/GameEvaluation.cs)
##### IEvaluatableFactory
The IEvaluatableFactory is used by the Genetic Algorithm during each training session to create a new instance of the scenario to run and evaluate. Your factory only needs to implement the following method:
```c#
public interface IEvaluatableFactory
{
    IEvaluatable Create(INeuralNetwork neuralNetwork);
}
```
`Create()` only requires the `INeuralNetwork` to use for that session to be passed in, the rest can be handled by your factory. An example of this can be found in the [BasicGameNeuralNetworkTrainer](https://github.com/jobeland/BasicGameNeuralNetworkTrainer) repo in [GameEvaluationFactory.cs](https://github.com/jobeland/BasicGameNeuralNetworkTrainer/blob/master/Trainer/GameEvaluationFactory.cs)







