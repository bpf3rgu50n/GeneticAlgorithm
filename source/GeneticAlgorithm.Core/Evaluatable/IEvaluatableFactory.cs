﻿namespace GeneticAlgorithm.Core.Evaluatable;

public interface IEvaluatableFactory
{
    IEvaluatable Create(INeuralNetwork neuralNetwork);
}