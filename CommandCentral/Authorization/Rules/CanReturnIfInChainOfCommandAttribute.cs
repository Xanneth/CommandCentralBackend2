﻿using CommandCentral.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandCentral.Authorization.Rules
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class CanReturnIfInChainOfCommandAttribute : Attribute
    {
        public ChainsOfCommand ChainOfCommand { get; private set; }
        public ChainOfCommandLevels Level { get; private set; }

        public CanReturnIfInChainOfCommandAttribute(ChainsOfCommand coc, ChainOfCommandLevels level)
        {
            ChainOfCommand = coc;
            Level = level;
        }

    }
}
