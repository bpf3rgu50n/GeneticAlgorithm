using GeneticAlgorithms.Core.Evaluatable;

namespace GeneticAlgorithms.Core.Tests;

public class MyEvaluatable : IEvaluatable
{
    private INeuralNetwork _neuralNetwork;
    private double[] _outputs = [];

    public MyEvaluatable(INeuralNetwork neuralNetwork)
    {
        _neuralNetwork = neuralNetwork;
    }

    public double GetEvaluation()
    {
        return _outputs.Sum();
    }

    public void RunEvaluation()
    {
        _neuralNetwork.SetInputs([0, 0, 0]);
        _neuralNetwork.Process();
        _outputs = _neuralNetwork.GetOutputs();
    }
}