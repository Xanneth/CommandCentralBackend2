﻿using CommandCentral.Entities.Muster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandCentral.Events.Args
{
    public class MusterFinalizedEventArgs : EventArgs
    {

        public MusterCycle MusterCycle { get; set; }

    }
}
