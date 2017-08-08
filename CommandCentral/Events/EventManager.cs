﻿using CommandCentral.Events.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandCentral.Events
{
    public static class EventManager
    {
        #region Muster

        public static event EventHandler<MusterFinalizedEventArgs> MusterFinalized;
        public static void OnMusterFinalized(MusterFinalizedEventArgs e, object sender = null)
        {
            MusterFinalized?.Invoke(sender, e);
        }

        public static event EventHandler<MusterOpenedEventArgs> MusterOpened;
        public static void OnMusterOpened(MusterOpenedEventArgs e, object sender = null)
        {
            MusterOpened?.Invoke(sender, e);
        }

        #endregion

        public static event EventHandler<LoginFailedEventArgs> LoginFailed;
        public static void OnLoginFailed(LoginFailedEventArgs e, object sender = null)
        {
            LoginFailed?.Invoke(sender, e);
        }

    }
}
