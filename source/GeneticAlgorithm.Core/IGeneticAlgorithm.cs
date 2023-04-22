﻿namespace GeneticAlgorithm.Core;

public interface IGeneticAlgorithm
{
    INeuralNetwork GetBestPerformer();

    void RunSimulation();
}