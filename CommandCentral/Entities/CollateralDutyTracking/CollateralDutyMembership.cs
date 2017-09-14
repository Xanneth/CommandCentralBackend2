﻿using System.Collections.Generic;
using System.Linq;
using CommandCentral.Authorization;
using CommandCentral.Enums;
using CommandCentral.Framework;
using CommandCentral.Framework.Data;
using FluentNHibernate.Mapping;
using FluentValidation;
using FluentValidation.Results;
using NHibernate.Linq;

namespace CommandCentral.Entities.CollateralDutyTracking
{
    /// <summary>
    /// A coll duty membership grants the associated person membership in the referenced collateral duty at a certain level and role.
    /// </summary>
    public class CollateralDutyMembership : Entity, IHazAttachments
    {
        #region Properties

        /// <summary>
        /// The person that actually has a membership to the collateral duty.
        /// </summary>
        public virtual Person Person { get; set; }

        /// <summary>
        /// The collateral duty to which this membership belongs.
        /// </summary>
        public virtual CollateralDuty CollateralDuty { get; set; }

        /// <summary>
        /// The role this membership is at in the command.
        /// </summary>
        public virtual CollateralRoles Role { get; set; }

        /// <summary>
        /// The level this collateral is at.
        /// </summary>
        public virtual CollateralLevels Level { get; set; }

        /// <summary>
        /// The list of file attachments on this membership.  This can be used to post a designation letter for the given position.
        /// </summary>
        public virtual IList<FileAttachment> Attachments { get; set; }

        /// <summary>
        /// Indicates that a designation letter has been received for this membership.
        /// </summary>
        public virtual bool HasDesignationLetter { get; set; }

        #endregion

        /// <summary>
        /// Validates this object.
        /// </summary>
        /// <returns></returns>
        public override ValidationResult Validate()
        {
            return new Validator().Validate(this);
        }

        /// <summary>
        /// Detetmines if the given person can see the file attachments on this membership.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public virtual bool CanPersonAccessAttachments(Person person)
        {
            return person.CanAccessSubmodules(SubModules.AdminTools) ||
                   SessionManager.GetCurrentSession().Query<CollateralDutyMembership>().Count(x =>
                       x.CollateralDuty == CollateralDuty &&
                       (x.Role == CollateralRoles.Primary || x.Role == CollateralRoles.Secondary) &&
                       x.Level == CollateralLevels.Command && x.Person == person) == 1;
        }

        /// <summary>
        /// Maps this object to the database.
        /// </summary>
        public class CollateralDutyMembershipMapping : ClassMap<CollateralDutyMembership>
        {
            /// <summary>
            /// Maps this object to the database.
            /// </summary>
            public CollateralDutyMembershipMapping()
            {
                Id(x => x.Id).GeneratedBy.Assigned();

                Map(x => x.Level).Not.Nullable().CustomType<GenericEnumMapper<CollateralLevels>>();
                Map(x => x.Role).Not.Nullable().CustomType<GenericEnumMapper<CollateralRoles>>();
                Map(x => x.HasDesignationLetter).Not.Nullable().Default(false.ToString());

                References(x => x.Person).Not.Nullable();
                References(x => x.CollateralDuty).Nullable();

                HasMany(x => x.Attachments)
                    .Cascade.AllDeleteOrphan()
                    .KeyColumn("OwningEntity_id")
                    .ForeignKeyConstraintName("none");
            }
        }

        /// <summary>
        /// Validates this object.
        /// </summary>
        public class Validator : AbstractValidator<CollateralDutyMembership>
        {
            /// <summary>
            /// Validates this object.
            /// </summary>
            public Validator()
            {
                RuleFor(x => x.Id).NotEmpty();
                RuleFor(x => x.CollateralDuty).NotEmpty();
            }
        }
    }
}