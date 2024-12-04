using GeneticAlgorithms.Core.Evaluatable;

namespace GeneticAlgorithms.Core.Tests;

public class MyEvaluatableFactory : IEvaluatableFactory
{
    private static Lazy<IEvaluatableFactory> _evaluatableFactory = new(() => new MyEvaluatableFactory());

    public static IEvaluatableFactory GetInstance()
    {
        return _evaluatableFactory.Value;
    }

    public IEvaluatable Create(INeuralNetwork neuralNetwork)
    {
        return new MyEvaluatable(neuralNetwork);
    }
}