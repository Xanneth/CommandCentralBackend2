﻿using System.Linq;
using CommandCentral.Authorization;
using CommandCentral.Entities;
using CommandCentral.Enums;
using CommandCentral.Events.Args;
using CommandCentral.Framework.Data;
using NHibernate.Linq;

namespace CommandCentral.Events.Handlers.Email
{
    public class MusterHandler : IEventHandler
    {
        public MusterHandler()
        {
            EventManager.MusterOpened += OnMusterOpened;
            EventManager.MusterFinalized += OnMusterFinalized;
            EventManager.MusterReopened += OnMusterReopened;
            EventManager.MusterEntrySubmitted += OnMusterEntrySubmitted;
            EventManager.MusterEntryDeleted += OnMusterEntryDeleted;
        }

        private void OnMusterOpened(object sender, MusterCycleEventArgs e)
        {
            //TODO
        }

        private void OnMusterFinalized(object sender, MusterCycleEventArgs e)
        {
            //TODO
        }

        private void OnMusterReopened(object sender, MusterCycleEventArgs e)
        {
            //TODO
        }

        private void OnMusterEntrySubmitted(object sender, MusterEntryEventArgs e)
        {
            //TODO
        }

        private void OnMusterEntryDeleted(object sender, MusterEntryEventArgs e)
        {
            //TODO
        }
    }
}