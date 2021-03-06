﻿using System;

namespace CommandCentral.DTOs.Change
{
    public class Get
    {
        public Guid Id { get; set; }
        public Guid Editor { get; set; }
        public Guid Entity { get; set; }
        public string PropertyPath { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime ChangeTime { get; set; }

        public Get(Entities.Change change)
        {
            Id = change.Id;
            Editor = change.Editor.Id;
            Entity = change.Entity.Id;
            PropertyPath = change.PropertyPath;
            OldValue = change.OldValue;
            NewValue = change.NewValue;
            ChangeTime = change.ChangeTime;
        }
    }
}