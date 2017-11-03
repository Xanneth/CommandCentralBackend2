﻿using System;
using CommandCentral.Enums;

namespace CommandCentral.DTOs.Watchbill
{
    public class Get
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public Entities.Command Command { get; set; }
        public WatchbillPhases Phase { get; set; }

        public Get(Entities.Watchbill.Watchbill item)
        {
            Id = item.Id;
            Title = item.Title;
            Month = item.Month;
            Year = item.Year;
            Command = item.Command;
            Phase = item.Phase;
        }
    }
}