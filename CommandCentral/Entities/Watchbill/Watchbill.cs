﻿using System;
using System.Collections.Generic;
using CommandCentral.Authorization;
using CommandCentral.Enums;
using CommandCentral.Framework;
using FluentNHibernate.Mapping;
using FluentValidation;
using FluentValidation.Results;

namespace CommandCentral.Entities.Watchbill
{
    /// <summary>
    /// A quarterdeck watchbill is used to encapsulate all the shifts and assignments that make up the schedule 
    /// for one month of watch standing.
    /// </summary>
    public class Watchbill : CommentableEntity
    {
        /// <summary>
        /// The title of this watchbill
        /// </summary>
        public virtual string Title { get; set; }
        
        /// <summary>
        /// The month of this watchbill
        /// </summary>
        public virtual int Month { get; set; }
        
        /// <summary>
        /// The year of this watchbill
        /// </summary>
        public virtual int Year { get; set; }
        
        /// <summary>
        /// The shifts contained in this watchbill
        /// </summary>
        public virtual IList<WatchShift> WatchShifts { get; set; }
        
        /// <summary>
        /// The command this watchbill is for
        /// </summary>
        public virtual Command Command { get; set; }
        
        /// <summary>
        /// The current phase of the watchbill
        /// </summary>
        public virtual WatchbillPhases Phase { get; set; }

        /// <summary>
        /// The person who created this watchbill.
        /// </summary>
        public virtual Person CreatedBy { get; set; }
        
        /// <summary>
        /// Validates this object.
        /// </summary>
        /// <returns></returns>
        public override ValidationResult Validate()
        {
            return new Validator().Validate(this);
        }

        /// <summary>
        /// Gets the first datetime of this watchbill as a UTC datetime.  Time is at midnight.
        /// </summary>
        /// <returns></returns>
        public virtual DateTime GetFirstDay()
        {
            return new DateTimeOffset(Year, Month, 1, 0, 0, 0, Command.GetTimeZoneInfo().BaseUtcOffset).UtcDateTime;
        }

        /// <summary>
        /// Gets the last datetime of this watchbill as a UTC datetime.  Time is at midnight.
        /// </summary>
        /// <returns></returns>
        public virtual DateTime GetLastDay()
        {
            return new DateTimeOffset(Year, Month, 1, 0, 0, 0, Command.GetTimeZoneInfo().BaseUtcOffset)
                .AddMonths(1)
                .UtcDateTime;
        }

        /// <summary>
        /// Maps this object to the database.
        /// </summary>
        public class WatchbillMapping : ClassMap<Watchbill>
        {
            /// <summary>
            /// Maps this object to the database.
            /// </summary>
            public WatchbillMapping()
            {
                Id(x => x.Id).GeneratedBy.Assigned();

                Map(x => x.Title).Not.Nullable();
                Map(x => x.Month).Not.Nullable();
                Map(x => x.Year).Not.Nullable();
                Map(x => x.Phase).Not.Nullable();

                References(x => x.Command).Not.Nullable();
                References(x => x.CreatedBy).Not.Nullable();

                HasMany(x => x.WatchShifts).Cascade.All();
            }
            
        }

        /// <summary>
        /// Validates the watchbill
        /// </summary>
        public class Validator : AbstractValidator<Watchbill>
        {
            /// <summary>
            /// Validates the watchbill
            /// </summary>
            public Validator()
            {
                RuleFor(x => x.Title).NotEmpty().Length(3, 50);
                RuleFor(x => x.Month).NotEmpty().InclusiveBetween(1, 12);
                RuleFor(x => x.Year).NotEmpty().GreaterThan(2016);
                RuleFor(x => x.Command).NotEmpty();
                RuleFor(x => x.CreatedBy).NotEmpty();
            }
        }

        /// <summary>
        /// Declares authorization rules for this object.
        /// </summary>
        public class Contract : RulesContract<Watchbill>
        {
            /// <summary>
            /// Declares authorization rules for this object.
            /// </summary>
            public Contract()
            {
                RulesFor()
                    .CanEdit((person, watchbill) =>
                        person.Division.Department.Command == watchbill.Command &&
                        person.IsInChainOfCommandAtLevel(ChainsOfCommand.QuarterdeckWatchbill,
                            ChainOfCommandLevels.Command))
                    .CanReturn((person, watchbill) =>
                        person.Division.Department.Command == watchbill.Command &&
                        person.IsInChainOfCommandAtLevel(ChainsOfCommand.QuarterdeckWatchbill,
                            ChainOfCommandLevels.Command));
            }
        }
    }
}