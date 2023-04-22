﻿namespace GeneticAlgorithm.Core.Evolution;

public interface IMutatorFactory
{
    IMutator Create(MutationConfigurationSettings config);

    IMutator Create();
}