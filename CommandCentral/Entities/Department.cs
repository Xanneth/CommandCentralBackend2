﻿using System.Collections.Generic;
using CommandCentral.Authorization;
using CommandCentral.Enums;
using CommandCentral.Framework;
using FluentNHibernate.Mapping;
using FluentValidation;
using FluentValidation.Results;

namespace CommandCentral.Entities
{
    /// <summary>
    /// Describes a single Department and all of its divisions.
    /// </summary>
    public class Department : Entity
    {
        #region Properties
        
        /// <summary>
        /// The name of this department.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// A brief description of this department.
        /// </summary>
        public virtual string Description { get; set; }


        /// <summary>
        /// The command to which this department belongs.
        /// </summary>
        public virtual Command Command { get; set; }

        /// <summary>
        /// A list of those divisions that belong to this department.
        /// </summary>
        public virtual IList<Division> Divisions { get; set; } = new List<Division>();

        #endregion

        #region Overrides

        /// <summary>
        /// Validates this department object.
        /// </summary>
        /// <returns></returns>
        public override ValidationResult Validate()
        {
            return new Validator().Validate(this);
        }
        
        #endregion

        /// <summary>
        /// Maps a department to the database.
        /// </summary>
        public class DepartmentMapping : ClassMap<Department>
        {
            /// <summary>
            /// Maps a department to the database.
            /// </summary>
            public DepartmentMapping()
            {
                Id(x => x.Id).GeneratedBy.Assigned();

                Map(x => x.Name).Not.Nullable().Unique();
                Map(x => x.Description);

                HasMany(x => x.Divisions).Cascade.All();

                References(x => x.Command);

                Cache.ReadWrite();
            }
        }

        /// <summary>
        /// Validates the Department.
        /// </summary>
        public class Validator : AbstractValidator<Department>
        {
            /// <summary>
            /// Validates the Department.
            /// </summary>
            public Validator()
            {
                RuleFor(x => x.Description).Length(0, 255)
                    .WithMessage("The description of a department must be no more than 255 characters.");
                RuleFor(x => x.Name).NotEmpty()
                    .WithMessage("The value must not be empty.");
            }
        }
        
        /// <summary>
        /// Rules for this object.
        /// </summary>
        public class Contract : RulesContract<Department>
        {
            /// <summary>
            /// Rules for this object.
            /// </summary>
            public Contract()
            {
                RulesFor()
                    .CanEdit((person, department) => person.SpecialPermissions.Contains(SpecialPermissions.AdminTools))
                    .CanReturn((person, department) => true);
            }
        }
    }
}
