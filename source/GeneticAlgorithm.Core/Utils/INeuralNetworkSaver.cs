﻿namespace GeneticAlgorithm.Core.Utils;

internal interface INeuralNetworkSaver
{
    string SaveNeuralNetwork(INeuralNetwork network, double networkEvaluation, int epoch);
}