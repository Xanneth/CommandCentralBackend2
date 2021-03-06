﻿using CommandCentral.Enums;

namespace CommandCentral.DTOs.PhoneNumber
{
    public class Put
    { 
        public PhoneNumberTypes PhoneType { get; set; }
        public bool IsReleasableOutsideCoC { get; set; }
        public string Number { get; set; }
        public bool IsPreferred { get; set; }
    }
}
