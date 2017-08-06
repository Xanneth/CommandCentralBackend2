﻿using CommandCentral.Entities.ReferenceLists;
using CommandCentral.Utilities.Types;
using CommandCentral.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using FluentNHibernate.Mapping;
using FluentValidation;
using NHibernate.Type;

namespace CommandCentral.Entities.Muster
{
    /// <summary>
    /// Represents a status period which is used to indicate a person will be something other than present for a given period of time.
    /// </summary>
    public class StatusPeriod : CommentableEntity
    {
        #region Properties

        /// <summary>
        /// The person for whom this status period was submitted.
        /// </summary>
        public virtual Person Person { get; set; }

        /// <summary>
        /// The person who submitted this status period.
        /// </summary>
        public virtual Person SubmittedBy { get; set; }

        /// <summary>
        /// The time this status period was submitted.
        /// </summary>
        public virtual DateTime DateSubmitted { get; set; }

        /// <summary>
        /// The last person to modify this status period.
        /// </summary>
        public virtual Person LastModifiedBy { get; set; }

        /// <summary>
        /// The last time this status period was modified.
        /// </summary>
        public virtual DateTime DateLastModified { get; set; }

        /// <summary>
        /// Indicates if this status period will also exempt the person from watch.
        /// </summary>
        public virtual bool ExemptsFromWatch { get; set; }

        /// <summary>
        /// The date range of this status period.
        /// </summary>
        public virtual TimeRange Range { get; set; }

        /// <summary>
        /// The <seealso cref="StatusPeriodReason"/> for this status period.
        /// </summary>
        public virtual StatusPeriodReason Reason { get; set; }

        #endregion

        #region CommentableEntity Members  

        /// <summary>
        /// The comments associated with this status period.
        /// </summary>
        public override IList<Comment> Comments { get; set; }

        /// <summary>
        /// Determines if the given person can return comments for this status period.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public override bool CanPersonAccessComments(Person person)
        {
            return person.GetFieldPermissions<Person>(this.Person).CanReturn(x => x.StatusPeriods);
        }

        /// <summary>
        /// Validates this object.
        /// </summary>
        /// <returns></returns>
        public override ValidationResult Validate()
        {
            return new Validator().Validate(this);
        }

        #endregion

        /// <summary>
        /// Maps this object to the database.
        /// </summary>
        public class StatusPeriodMapping : ClassMap<StatusPeriod>
        {
            /// <summary>
            /// Maps this object to the database.
            /// </summary>
            public StatusPeriodMapping()
            {
                Id(x => x.Id).GeneratedBy.Assigned();

                Map(x => x.DateSubmitted).Not.Nullable();
                Map(x => x.DateLastModified).Not.Nullable();
                Map(x => x.ExemptsFromWatch).Not.Nullable();
                Component(x => x.Range, map =>
                {
                    map.Map(x => x.End).Not.Nullable().CustomType<UtcDateTimeType>();
                    map.Map(x => x.Start).Not.Nullable().CustomType<UtcDateTimeType>();
                });

                References(x => x.Person).Not.Nullable().Column("Person_id");
                References(x => x.SubmittedBy).Not.Nullable();
                References(x => x.LastModifiedBy).Not.Nullable();
                References(x => x.Reason).Not.Nullable();
            }
        }

        /// <summary>
        /// Validates this object.
        /// </summary>
        public class Validator : AbstractValidator<StatusPeriod>
        {
            /// <summary>
            /// Validates this object.
            /// </summary>
            public Validator()
            {
                RuleFor(x => x.Person).NotEmpty();
                RuleFor(x => x.SubmittedBy).NotEmpty();
                RuleFor(x => x.DateSubmitted).NotEmpty().InclusiveBetween(DateTime.MinValue, DateTime.UtcNow);
                RuleFor(x => x.LastModifiedBy).NotEmpty();
                RuleFor(x => x.DateLastModified).NotEmpty();
                RuleFor(x => x.Range)
                    .Must(range => range.Start <= range.End && range.Start != default(DateTime) && range.End != default(DateTime))
                        .WithMessage("A status period must start before it ends.")
                    .Must((period, range) => range.Start >= period.DateSubmitted)
                        .WithMessage("A status period must start after or at the same time it was submitted.  For example, you may not submit a retroactive status period.")
                    .Must((period, range) => range.End >= period.DateLastModified)
                        .WithMessage("A status period must end after it was submitted or after it was last modified.  For example, you may not modify a status period to end before now.");

                RuleFor(x => x.Reason).NotEmpty()
                    .Must(x => !x.Value.Equals("Present", StringComparison.CurrentCultureIgnoreCase))
                    .WithMessage("A status period's reason may not be 'Present'.  You can not project that someone is going to be present for a given period of time."); ;
            }
        }
    }
}
