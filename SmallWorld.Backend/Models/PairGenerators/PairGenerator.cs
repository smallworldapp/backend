﻿using System;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Database.Entities;

namespace SmallWorld.Models.PairGenerators
{
    public class PairGenerator
    {
        private readonly IServiceProvider provider;

        public PairGenerator(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public GeneratedPairs For(Pairing pairing)
        {
            GeneratedPairs gen;

            if (pairing.Type == PairingType.Auto)
                gen = ActivatorUtilities.CreateInstance<AutoGeneratedPairs>(provider);
            else
                gen = ActivatorUtilities.CreateInstance<ManualGeneratedPairs>(provider);

            gen.Generate(pairing);

            return gen;
        }
    }
}