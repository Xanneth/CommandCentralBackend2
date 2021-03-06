﻿using CommandCentral.Email.Models;

namespace CommandCentral.Email
{
    /// <summary>
    /// A list of all templates used throughout the application.
    /// </summary>
    public static class Templates
    {
        /// <summary>
        /// The template meant to be used for the correspondence completed email.
        /// </summary>
        public static readonly CCEmailTemplate<CorrespondenceGeneric> CorrespondenceCompletedTemplate =
            new CCEmailTemplate<CorrespondenceGeneric>("CorrespondenceCompleted.cshtml");

        /// <summary>
        /// The template meant to be used for the correspondence created email.
        /// </summary>
        public static readonly CCEmailTemplate<CorrespondenceGeneric> CorrespondenceCreatedTemplate =
            new CCEmailTemplate<CorrespondenceGeneric>("CorrespondenceCreated.cshtml");

        /// <summary>
        /// The template meant to be used for the correspondence deleted email.
        /// </summary>
        public static readonly CCEmailTemplate<CorrespondenceGeneric> CorrespondenceDeletedTemplate =
            new CCEmailTemplate<CorrespondenceGeneric>("CorrespondenceDeleted.cshtml");

        /// <summary>
        /// The template meant to be used for the correspondence modified email.
        /// </summary>
        public static readonly CCEmailTemplate<CorrespondenceGeneric> CorrespondenceModifiedTemplate =
            new CCEmailTemplate<CorrespondenceGeneric>("CorrespondenceModified.cshtml");

        /// <summary>
        /// The template meant to be used for the correspondence email when it is routed to a new person.
        /// </summary>
        public static readonly CCEmailTemplate<CorrespondenceRouted> CorrespondenceRoutedTemplate =
            new CCEmailTemplate<CorrespondenceRouted>("CorrespondenceRouted.cshtml");

        /// <summary>
        /// The template meant to be used in the email for when a corr item is shared to new people.
        /// </summary>
        public static readonly CCEmailTemplate<CorrespondenceShared> CorrespondenceSharedTemplate =
            new CCEmailTemplate<CorrespondenceShared>("CorrespondenceShared.cshtml");

        /// <summary>
        /// The template meant to be used in the email for when a corr item unshared from a person.
        /// </summary>
        public static readonly CCEmailTemplate<CorrespondenceGeneric> CorrespondenceUnsharedTemplate =
            new CCEmailTemplate<CorrespondenceGeneric>("CorrespondenceUnshared.cshtml");

        /// <summary>
        /// The template meant to be used for when a corr item's review is modified.
        /// </summary>
        public static readonly CCEmailTemplate<CorrespondenceGeneric> ReviewModifiedTemplate =
            new CCEmailTemplate<CorrespondenceGeneric>("ReviewModified.cshtml");

        /// <summary>
        /// The template to be used when a review of a correspondence chain is deleted.
        /// </summary>
        public static readonly CCEmailTemplate<CorrespondenceGeneric> ReviewDeletedTemplate =
            new CCEmailTemplate<CorrespondenceGeneric>("ReviewDeleted.cshtml");

        /// <summary>
        /// The template meant to be used in the email for when account registration is started.
        /// </summary>
        public static readonly CCEmailTemplate<CollateralDeleted> CollateralDeletedTemplate =
            new CCEmailTemplate<CollateralDeleted>("CollateralDeleted.cshtml");

        /// <summary>
        /// The template meant to be used in the email for when a collateral duty is assigned.
        /// </summary>
        public static CCEmailTemplate<CollateralMembershipCreated> CollateralMembershipCreated =
            new CCEmailTemplate<CollateralMembershipCreated>("CollateralMembershipCreated.cshtml");
    }
}