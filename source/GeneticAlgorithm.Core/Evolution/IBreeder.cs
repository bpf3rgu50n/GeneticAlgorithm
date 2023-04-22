﻿namespace GeneticAlgorithm.Core.Evolution;

public interface IBreeder
{
    IList<INeuralNetwork> Breed(IList<ITrainingSession> sessions, int numToBreed);
}