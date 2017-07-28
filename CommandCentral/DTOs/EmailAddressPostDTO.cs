﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandCentral.DTOs
{
    public class EmailAddressPostDTO
    {
        public Guid Person { get; set; }
        public bool IsReleasableOutsideCoC { get; set; }
        public string Address { get; set; }
        public bool IsPreferred { get; set; }
    }
}
