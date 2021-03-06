﻿using System;

namespace CommandCentral.DTOs.CorrespondenceItem
{
    public class Get : Post
    {
        public Guid Id { get; set; }
        public int SeriesNumber { get; set; }
        public Guid SubmittedBy { get; set; }
        public DateTime TimeSubmitted { get; set; }
        public bool HasBeenCompleted { get; set; }

        public Get(Entities.Correspondence.CorrespondenceItem item)
        {
            Id = item.Id;
            SeriesNumber = item.SeriesNumber;
            SubmittedFor = item.SubmittedFor.Id;
            SubmittedBy = item.SubmittedBy.Id;
            TimeSubmitted = item.TimeSubmitted;
            FinalApprover = item.FinalApprover.Id;
            HasBeenCompleted = item.HasBeenCompleted;
            Type = item.Type.Id;
            Body = item.Body;
            HasPhysicalCounterpart = item.HasPhysicalCounterpart;
        }
    }
}
