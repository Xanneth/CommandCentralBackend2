﻿using System;
using System.Collections.Generic;
using System.Linq;
using CommandCentral.Entities.ReferenceLists;
using FluentNHibernate.Mapping;
using FluentValidation;
using NHibernate.Transform;
using NHibernate.Criterion;
using NHibernate.Linq;
using System.Reflection;
using NHibernate.Type;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using CommandCentral.Framework.Data;
using CommandCentral.Utilities;
using CommandCentral.Enums;
using CommandCentral.Framework;
using CommandCentral.Utilities.Types;
using CommandCentral.Authorization;
using CommandCentral.Authorization.Rules;

namespace CommandCentral.Entities
{
    /// <summary>
    /// Describes a single person and all their properties and data access methods.
    /// </summary>
    [HasPermissions]
    public class Person : CommentableEntity
    {

        #region Properties

        #region Main Properties
        
        /// <summary>
        /// The person's last name.
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        [CanEditIfSelf]
        public virtual string LastName { get; set; }

        /// <summary>
        /// The person's first name.
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        [CanEditIfSelf]
        public virtual string FirstName { get; set; }

        /// <summary>
        /// The person's middle name.
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        [CanEditIfSelf]
        public virtual string MiddleName { get; set; }

        /// <summary>
        /// The person's SSN.
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        [CanReturnIfSelf]
        [CanReturnIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        public virtual string SSN { get; set; }

        /// <summary>
        /// The person's DoD Id which allows us to communicate with other systems about this person.
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        [CanReturnIfSelf]
        [CanReturnIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        public virtual string DoDId { get; set; }

        /// <summary>
        /// The person's suffix.
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        [CanEditIfSelf]
        public virtual string Suffix { get; set; }

        /// <summary>
        /// The person's date of birth.
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        [CanEditIfSelf]
        [CanReturnIfSelf]
        [CanReturnIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        public virtual DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// The person's age.  0 if the date of birth isn't set.
        /// </summary>
        [CanNeverEdit]
        public virtual int Age
        {
            get
            {
                if (DateOfBirth == null || !DateOfBirth.HasValue)
                    return 0;

                if (DateTime.Today.Month < DateOfBirth.Value.Month ||
                    DateTime.Today.Month == DateOfBirth.Value.Month &&
                    DateTime.Today.Day < DateOfBirth.Value.Day)
                {
                    return DateTime.Today.Year - DateOfBirth.Value.Year - 1;
                }

                return DateTime.Today.Year - DateOfBirth.Value.Year;
            }
        }

        /// <summary>
        /// The person's sex.
        /// </summary>
        [CanEditIfSelf]
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        public virtual Sex Sex { get; set; }

        /// <summary>
        /// Stores the person's ethnicity.
        /// </summary>
        [CanEditIfSelf]
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        public virtual Ethnicity Ethnicity { get; set; }

        /// <summary>
        /// The person's religious preference
        /// </summary>
        [CanEditIfSelf]
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        [CanReturnIfSelf]
        [CanReturnIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        public virtual ReligiousPreference ReligiousPreference { get; set; }

        /// <summary>
        /// The person's paygrade (e5, O1, O5, CWO2, GS1,  etc.)
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        public virtual Paygrade Paygrade { get; set; }

        /// <summary>
        /// The person's Designation (CTI2, CTR1, 1114, Job title)
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        public virtual Designation Designation { get; set; }

        /// <summary>
        /// The person's division
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        public virtual Division Division { get; set; }

        /// <summary>
        /// The person's department
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        public virtual Department Department { get; set; }

        /// <summary>
        /// The person's command
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        public virtual Command Command { get; set; }

        /// <summary>
        /// The date this person received government travel card training.  Temporary and should be implemented in the training module.
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        public virtual DateTime? GTCTrainingDate { get; set; }

        /// <summary>
        /// The date on which ADAMS training was completed.  Temporary and should be implemented in the training module.
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        public virtual DateTime? ADAMSTrainingDate { get; set; }

        /// <summary>
        /// The date on which AWARE training was completed.  Temporary and should be implemented in the training module.
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        public virtual bool HasCompletedAWARE { get; set; }

        #endregion

        #region Work Properties

        /// <summary>
        /// The person's primary NEC.
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        public virtual NEC PrimaryNEC { get; set; }

        /// <summary>
        /// The list of the client's secondary NECs.
        /// </summary>
        [HiddenFromPermissions]
        public virtual IList<NEC> SecondaryNECs { get; set; }

        /// <summary>
        /// The person's supervisor
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        [CanEditIfSelf]
        public virtual string Supervisor { get; set; }

        /// <summary>
        /// The person's work center.
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        [CanEditIfSelf]
        public virtual string WorkCenter { get; set; }

        /// <summary>
        /// The room in which the person works.
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        [CanEditIfSelf]
        public virtual string WorkRoom { get; set; }

        /// <summary>
        /// A free form text field intended to let the client store the shift of a person - however the client wants to do that.
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        [CanEditIfSelf]
        public virtual string Shift { get; set; }

        /// <summary>
        /// The person's duty status
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        public virtual DutyStatus DutyStatus { get; set; }

        /// <summary>
        /// The person's UIC
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        public virtual UIC UIC { get; set; }

        /// <summary>
        /// The date/time that the person arrived at the command.
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        public virtual DateTime? DateOfArrival { get; set; }

        /// <summary>
        /// The client's job title.
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        [CanEditIfSelf]
        public virtual string JobTitle { get; set; }

        /// <summary>
        /// The date/time of the end of active obligatory service (EAOS) for the person.
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        public virtual DateTime? EAOS { get; set; }

        /// <summary>
        /// The member's projected rotation date.
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        public virtual DateTime? PRD { get; set; }

        /// <summary>
        /// The date/time that the client left/will leave the command.
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        public virtual DateTime? DateOfDeparture { get; set; }

        /// <summary>
        /// The person's watch qualification.
        /// </summary>
        [HiddenFromPermissions]
        public virtual IList<WatchQualification> WatchQualifications { get; set; }

        /// <summary>
        /// The type of billet this person is assigned to.
        /// </summary>
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        public virtual BilletAssignment BilletAssignment { get; set; }

        #endregion

        #region Contacts Properties

        /// <summary>
        /// The email addresses of this person.
        /// </summary>
        [CanEditIfSelf]
        [CanEditIfInChainOfCommand(ChainsOfCommand.Main, ChainOfCommandLevels.Division)]
        public virtual IList<EmailAddress> EmailAddresses { get; set; }

        /// <summary>
        /// The Phone Numbers of this person.
        /// </summary>
        public virtual IList<PhoneNumber> PhoneNumbers { get; set; }

        /// <summary>
        /// The Physical Addresses of this person
        /// </summary>
        public virtual IList<PhysicalAddress> PhysicalAddresses { get; set; }

        #endregion

        #region Account

        /// <summary>
        /// A boolean indicating whether or not this account has been claimed.
        /// </summary>
        [HiddenFromPermissions]
        public virtual bool IsClaimed { get; set; }

        /// <summary>
        /// The client's username.
        /// </summary>
        [CanReturnIfSelf]
        [CanEditIfSelf]
        public virtual string Username { get; set; }

        /// <summary>
        /// The client's hashed password.
        /// </summary>
        [HiddenFromPermissions]
        public virtual string PasswordHash { get; set; }

        /// <summary>
        /// The list of the person's permissions.  This is not persisted in the database.  Only the names are.
        /// </summary>
        [HiddenFromPermissions]
        public virtual IList<PermissionGroup> PermissionGroups { get; set; } = new List<PermissionGroup> ();

        /// <summary>
        /// A list containing account history events, these are events that track things like login, password reset, etc.
        /// </summary>
        [HiddenFromPermissions]
        public virtual IList<AccountHistoryEvent> AccountHistory { get; set; }

        /// <summary>
        /// A list containing all changes that have every occurred to the profile.
        /// </summary>
        [HiddenFromPermissions]
        public virtual IList<Change> Changes { get; set; }

        /// <summary>
        /// The list of those events to which this person is subscribed.
        /// </summary>
        [HiddenFromPermissions]
        public virtual IDictionary<Guid, ChainOfCommandLevels> SubscribedEvents { get; set; }
        
        #endregion

        #endregion

        #region Overrides

        /// <summary>
        /// Returns a friendly name for this user in the form: Atwood, Daniel Kurt Roger
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}, {1} {2}", LastName, FirstName, MiddleName);
        }

        #endregion

        #region Helper Methods

        public virtual Dictionary<ChainsOfCommand, Dictionary<ChainOfCommandLevels, List<Person>>> GetChainOfCommand()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a boolean indicating if this person is in the same command as the given person.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public virtual bool IsInSameCommandAs(Person person)
        {
            if (person == null || this.Command == null || person.Command == null)
                return false;

            return this.Command.Id == person.Command.Id;
        }

        /// <summary>
        /// Returns a boolean indicating that this person is in the same command and department as the given person.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public virtual bool IsInSameDepartmentAs(Person person)
        {
            if (person == null || this.Department == null || person.Department == null)
                return false;

            return IsInSameCommandAs(person) && this.Department.Id == person.Department.Id;
        }

        /// <summary>
        /// Returns a boolean indicating that this person is in the same command, department, and division as the given person.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public virtual bool IsInSameDivisionAs(Person person)
        {
            if (person == null || this.Division == null || person.Division == null)
                return false;

            return IsInSameDepartmentAs(person) && this.Division.Id == person.Division.Id;
        }

        public override bool CanPersonAccessComments(Person person)
        {
            return true;
        }

        #endregion

        /// <summary>
        /// Maps a person to the database.
        /// </summary>
        public class PersonMapping : ClassMap<Person>
        {
            /// <summary>
            /// Maps a person to the database.
            /// </summary>
            public PersonMapping()
            {
                Id(x => x.Id).GeneratedBy.Assigned();

                References(x => x.Ethnicity).Nullable();
                References(x => x.ReligiousPreference).Nullable();
                References(x => x.Designation).Nullable();
                References(x => x.Division).Nullable();
                References(x => x.Department).Nullable();
                References(x => x.Command).Nullable();
                References(x => x.UIC).Nullable();
                References(x => x.Paygrade).Not.Nullable();
                References(x => x.DutyStatus).Not.Nullable();
                References(x => x.Sex).Not.Nullable();
                References(x => x.BilletAssignment);

                Map(x => x.LastName).Not.Nullable().Length(40);
                Map(x => x.FirstName).Not.Nullable().Length(40);
                Map(x => x.MiddleName).Nullable().Length(40);
                Map(x => x.SSN).Not.Nullable().Length(40).Unique();
                Map(x => x.DoDId).Unique();
                Map(x => x.DateOfBirth).Not.Nullable();
                Map(x => x.Supervisor).Nullable().Length(40);
                Map(x => x.WorkCenter).Nullable().Length(40);
                Map(x => x.WorkRoom).Nullable().Length(40);
                Map(x => x.Shift).Nullable().Length(40);
                Map(x => x.DateOfArrival).Not.Nullable();
                Map(x => x.JobTitle).Nullable().Length(40);
                Map(x => x.EAOS).CustomType<UtcDateTimeType>();
                Map(x => x.PRD).CustomType<UtcDateTimeType>();
                Map(x => x.DateOfDeparture).Nullable().CustomType<UtcDateTimeType>();
                Map(x => x.IsClaimed).Not.Nullable().Default(false.ToString());
                Map(x => x.Username).Nullable().Length(40).Unique();
                Map(x => x.PasswordHash).Nullable().Length(100);
                Map(x => x.Suffix).Nullable().Length(40);
                Map(x => x.GTCTrainingDate).Nullable().CustomType<UtcDateTimeType>();
                Map(x => x.ADAMSTrainingDate).Nullable().CustomType<UtcDateTimeType>();
                Map(x => x.HasCompletedAWARE).Not.Nullable().Default(false.ToString());

                References(x => x.PrimaryNEC);
                HasManyToMany(x => x.SecondaryNECs).Cascade.All();

                HasMany(x => x.AccountHistory).Cascade.All();
                HasMany(x => x.Changes).Cascade.All().Inverse();
                HasMany(x => x.EmailAddresses).Cascade.All();
                HasMany(x => x.PhoneNumbers).Cascade.All();
                HasMany(x => x.PhysicalAddresses).Cascade.All();
                
                HasManyToMany(x => x.WatchQualifications);

                HasMany(x => x.SubscribedEvents)
                    .AsMap<string>(index =>
                        index.Column("ChangeEventId").Type<Guid>(), element =>
                        element.Column("Level").Type<ChainOfCommandLevels>())
                    .Cascade.All();

                HasMany(x => x.PermissionGroups).Table("persontopermissiongroups").Component(x =>
                {
                    x.Map(y => y.Name);
                });
            }
        }

        /// <summary>
        /// Validates a person object.
        /// </summary>
        public class PersonValidator : AbstractValidator<Person>
        {
            /// <summary>
            /// Validates a person object.
            /// </summary>
            public PersonValidator()
            {
                RuleFor(x => x.Id).NotEmpty();
                RuleFor(x => x.LastName).NotEmpty().Length(1, 40)
                    .WithMessage("The last name must not be left blank and must not exceed 40 characters.");
                RuleFor(x => x.FirstName).Length(0, 40)
                    .WithMessage("The first name must not exceed 40 characters.");
                RuleFor(x => x.MiddleName).Length(0, 40)
                    .WithMessage("The middle name must not exceed 40 characters.");
                RuleFor(x => x.Suffix).Length(0, 40)
                    .WithMessage("The suffix must not exceed 40 characters.");
                RuleFor(x => x.SSN).NotEmpty().Must(x => System.Text.RegularExpressions.Regex.IsMatch(x, @"^(?!\b(\d)\1+-(\d)\1+-(\d)\1+\b)(?!123-45-6789|219-09-9999|078-05-1120)(?!666|000|9\d{2})\d{3}(?!00)\d{2}(?!0{4})\d{4}$"))
                    .WithMessage("The SSN must be valid and contain only numbers.");
                RuleFor(x => x.DateOfBirth).NotEmpty()
                    .WithMessage("The DOB must not be left blank.");
                RuleFor(x => x.PRD).NotEmpty()
                    .WithMessage("The DOB must not be left blank.");
                RuleFor(x => x.Sex).NotNull()
                    .WithMessage("The sex must not be left blank.");
                RuleFor(x => x.Command).NotEmpty().WithMessage("A person must have a command.  If you are trying to indicate this person left the command, please set his or her duty status to 'LOSS'.");
                RuleFor(x => x.Department).NotEmpty().WithMessage("A person must have a department.  If you are trying to indicate this person left the command, please set his or her duty status to 'LOSS'.");
                RuleFor(x => x.Division).NotEmpty().WithMessage("A person must have a division.  If you are trying to indicate this person left the command, please set his or her duty status to 'LOSS'.");
                RuleFor(x => x.Ethnicity).Must(x =>
                    {
                        if (x == null)
                            return true;

                        Ethnicity ethnicity = SessionManager.CurrentSession.Get<Ethnicity>(x.Id);

                        if (ethnicity == null)
                            return false;

                        return ethnicity.Equals(x);
                    })
                    .WithMessage("The ethnicity wasn't valid.  It must match exactly a list item in the database.");
                RuleFor(x => x.ReligiousPreference).Must(x =>
                    {
                        if (x == null)
                            return true;

                        ReligiousPreference pref = SessionManager.CurrentSession.Get<ReligiousPreference>(x.Id);

                        if (pref == null)
                            return false;

                        return pref.Equals(x);
                    })
                    .WithMessage("The religious preference wasn't valid.  It must match exactly a list item in the database.");
                RuleFor(x => x.Designation).Must(x =>
                    {
                        if (x == null)
                            return true;

                        Designation designation = SessionManager.CurrentSession.Get<Designation>(x.Id);

                        if (designation == null)
                            return false;

                        return designation.Equals(x);
                    })
                    .WithMessage("The designation wasn't valid.  It must match exactly a list item in the database.");
                RuleFor(x => x.Division).Must((person, x) =>
                    {
                        if (x == null)
                            return true;

                        Division division = SessionManager.CurrentSession.Get<Division>(x.Id);

                        if (division == null)
                            return false;

                        return division.Equals(x);
                    })
                    .WithMessage("The division wasn't a valid division.  It must match exactly.");
                RuleFor(x => x.Department).Must(x =>
                    {
                        if (x == null)
                            return true;

                        Department department = SessionManager.CurrentSession.Get<Department>(x.Id);

                        if (department == null)
                            return false;

                        return department.Equals(x);
                    })
                    .WithMessage("The department was invalid.");
                RuleFor(x => x.Command).Must(x =>
                    {
                        if (x == null)
                            return true;

                        Command command = SessionManager.CurrentSession.Get<Command>(x.Id);

                        if (command == null)
                            return false;

                        return command.Equals(x);
                    })
                    .WithMessage("The command was invalid.");
                RuleFor(x => x.PrimaryNEC).Must((person, x) =>
                    {
                        if (x == null)
                            return true;

                        NEC nec = SessionManager.CurrentSession.Get<NEC>(x.Id);

                        if (nec == null)
                            return false;

                        if (!nec.Equals(x))
                            return false;

                        //Now let's also make sure this isn't in the secondary NECs.
                        if (person.SecondaryNECs.Any(y => y.Id == x.Id))
                            return false;

                        return true;
                    })
                    .WithMessage("The primary NEC must not exist in the secondary NECs list.");
                RuleFor(x => x.Supervisor).Length(0, 40)
                    .WithMessage("The supervisor field may not be longer than 40 characters.");
                RuleFor(x => x.WorkCenter).Length(0, 40)
                    .WithMessage("The work center field may not be longer than 40 characters.");
                RuleFor(x => x.WorkRoom).Length(0, 40)
                    .WithMessage("The work room field may not be longer than 40 characters.");
                RuleFor(x => x.Shift).Length(0, 40)
                    .WithMessage("The shift field may not be longer than 40 characters.");
                RuleFor(x => x.UIC).Must(x =>
                    {
                        if (x == null)
                            return true;

                        UIC uic = SessionManager.CurrentSession.Get<UIC>(x.Id);

                        if (uic == null)
                            return false;

                        return uic.Equals(x);
                    })
                    .WithMessage("The UIC was invalid.");
                RuleFor(x => x.JobTitle).Length(0, 40)
                    .WithMessage("The job title may not be longer than 40 characters.");

                When(x => x.IsClaimed, () =>
                {
                    RuleFor(x => x.EmailAddresses).Must((person, x) =>
                    {
                        return x.Any(y => y.IsDoDEmailAddress());
                    }).WithMessage("You must have at least one mail.mil address.");
                });

                RuleForEach(x => x.SubscribedEvents).Must((person, subEvent) =>
                {

                    if (person.SubscribedEvents.Count(x => x.Key == subEvent.Key) != 1)
                        return false;

                    var changeEvent = ChangeEvents.ChangeEventHelper.AllChangeEvents.FirstOrDefault(x => x.Id == subEvent.Key);

                    if (changeEvent == null)
                        return false;

                    if (!changeEvent.ValidLevels.Contains(subEvent.Value))
                        return false;

                    return true;
                })
                .WithMessage("One or more of your subscription events were not valid.");

                //Set validations
                RuleFor(x => x.EmailAddresses)
                    .SetCollectionValidator(new EmailAddress.EmailAddressValidator());
                RuleFor(x => x.PhoneNumbers)
                    .SetCollectionValidator(new PhoneNumber.PhoneNumberValidator());
                RuleFor(x => x.PhysicalAddresses)
                    .SetCollectionValidator(new PhysicalAddress.PhysicalAddressValidator());
            }

        }

        /// <summary>
        /// Provides searching strategies for the person object.
        /// </summary>
        public class PersonQueryProvider : QueryStrategyProvider<Person>
        {
            /// <summary>
            /// Provides searching strategies for the person object.
            /// </summary>
            public PersonQueryProvider()
            {
                ForProperties(
                    x => x.Id)
                .AsType(SearchDataTypes.String)
                .CanBeUsedIn(QueryTypes.Advanced)
                .UsingStrategy(token =>
                {
                    return CommonQueryStrategies.IdQuery(token.SearchParameter.Key.GetPropertyName(), token.SearchParameter.Value);
                });

                ForProperties(
                    x => x.SSN,
                    x => x.Suffix,
                    x => x.Supervisor,
                    x => x.WorkCenter,
                    x => x.WorkRoom,
                    x => x.Shift,
                    x => x.JobTitle,
                    x => x.DoDId)
                .AsType(SearchDataTypes.String)
                .CanBeUsedIn(QueryTypes.Advanced)
                .UsingStrategy(token =>
                {
                    return CommonQueryStrategies.StringQuery(token.SearchParameter.Key.GetPropertyName(), token.SearchParameter.Value);
                });
                              
                ForProperties(
                    x => x.LastName,
                    x => x.FirstName,
                    x => x.MiddleName)
                .AsType(SearchDataTypes.String)
                .CanBeUsedIn(QueryTypes.Advanced, QueryTypes.Simple)
                .UsingStrategy(token =>
                {
                    return CommonQueryStrategies.StringQuery(token.SearchParameter.Key.GetPropertyName(), token.SearchParameter.Value);
                });

                ForProperties(
                    x => x.HasCompletedAWARE)
                .AsType(SearchDataTypes.Boolean)
                .CanBeUsedIn(QueryTypes.Advanced)
                .UsingStrategy(token =>
                    {
                        return CommonQueryStrategies.BooleanQuery(token.SearchParameter.Key.GetPropertyName(), token.SearchParameter.Value);
                    });

                ForProperties(
                    x => x.DateOfBirth,
                    x => x.GTCTrainingDate,
                    x => x.ADAMSTrainingDate,
                    x => x.DateOfArrival,
                    x => x.EAOS,
                    x => x.DateOfDeparture,
                    x => x.PRD)
                .AsType(SearchDataTypes.DateTime)
                .CanBeUsedIn(QueryTypes.Advanced)
                .UsingStrategy(token =>
                {
                    return CommonQueryStrategies.DateTimeQuery(token.SearchParameter.Key.GetPropertyName(), token.SearchParameter.Value);
                });

                ForProperties(
                    x => x.Sex,
                    x => x.BilletAssignment,
                    x => x.Ethnicity,
                    x => x.ReligiousPreference,
                    x => x.DutyStatus)
                .AsType(SearchDataTypes.String)
                .CanBeUsedIn(QueryTypes.Advanced)
                .UsingStrategy(token =>
                {
                    return CommonQueryStrategies.ReferenceListValueQuery(token.SearchParameter.Key, token.SearchParameter.Value);
                });

                ForProperties(
                    x => x.Paygrade,
                    x => x.Designation,
                    x => x.Division,
                    x => x.Department,
                    x => x.Command,
                    x => x.UIC,
                    x => x.PrimaryNEC)
                .AsType(SearchDataTypes.String)
                .CanBeUsedIn(QueryTypes.Advanced, QueryTypes.Simple)
                .UsingStrategy(token =>
                {
                    return CommonQueryStrategies.ReferenceListValueQuery(token.SearchParameter.Key, token.SearchParameter.Value);
                });

                ForProperties(
                    x => x.SecondaryNECs)
                .AsType(SearchDataTypes.String)
                .CanBeUsedIn(QueryTypes.Advanced)
                .UsingStrategy(token =>
                {
                    NEC necAlias = null;

                    token.Query = token.Query.JoinAlias(x => x.SecondaryNECs, () => necAlias);

                    //First we need to get what the client gave us into a list of Guids.
                    if (token.SearchParameter.Value == null)
                        throw new CommandCentralException("You search value must not be null.", ErrorTypes.Validation);

                    var str = (string)token.SearchParameter.Value;

                    if (String.IsNullOrWhiteSpace(str))
                        throw new CommandCentralException("Your search value must be a string of values, delineated by white space, semicolons, or commas.", ErrorTypes.Validation);

                    List<string> values = new List<string>();
                    foreach (var value in str.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (String.IsNullOrWhiteSpace(value) || String.IsNullOrWhiteSpace(value.Trim()))
                            throw new CommandCentralException("One of your values was not vallid.", ErrorTypes.Validation);

                        values.Add(value.Trim());
                    }

                    var disjunction = new Disjunction();

                    foreach (var value in values)
                    {
                        disjunction.Add(Restrictions.On(() => necAlias.Value).IsInsensitiveLike(value, MatchMode.Anywhere));
                    }

                    return disjunction;
                });

                ForProperties(
                    x => x.WatchQualifications)
                .AsType(SearchDataTypes.String)
                .CanBeUsedIn(QueryTypes.Advanced)
                .UsingStrategy(token =>
                {
                    WatchQualification qualAlias = null;

                    token.Query = token.Query.JoinAlias(x => x.WatchQualifications, () => qualAlias);

                    //First we need to get what the client gave us into a list of Guids.
                    if (token.SearchParameter.Value == null)
                        throw new CommandCentralException("You search value must not be null.", ErrorTypes.Validation);

                    var str = (string)token.SearchParameter.Value;

                    if (String.IsNullOrWhiteSpace(str))
                        throw new CommandCentralException("Your search value must be a string of values, delineated by white space, semicolons, or commas.", ErrorTypes.Validation);

                    List<string> values = new List<string>();
                    foreach (var value in str.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (String.IsNullOrWhiteSpace(value) || String.IsNullOrWhiteSpace(value.Trim()))
                            throw new CommandCentralException("One of your values was not vallid.", ErrorTypes.Validation);

                        values.Add(value.Trim());
                    }

                    var disjunction = new Disjunction();

                    foreach (var value in values)
                    {
                        disjunction.Add(Restrictions.On(() => qualAlias.Value).IsInsensitiveLike(value, MatchMode.Anywhere));
                    }

                    return disjunction;
                });
                
                ForProperties(
                    x => x.EmailAddresses)
                .AsType(SearchDataTypes.String)
                .CanBeUsedIn(QueryTypes.Advanced)
                .UsingStrategy(token =>
                {
                    EmailAddress addressAlias = null;
                    token.Query = token.Query.JoinAlias(x => x.EmailAddresses, () => addressAlias);

                    //First we need to get what the client gave us into a list of Guids.
                    if (token.SearchParameter.Value == null)
                        throw new CommandCentralException("You search value must not be null.", ErrorTypes.Validation);

                    var str = (string)token.SearchParameter.Value;

                    if (String.IsNullOrWhiteSpace(str))
                        throw new CommandCentralException("Your search value must be a string of values, delineated by white space, semicolons, or commas.", ErrorTypes.Validation);

                    List<string> values = new List<string>();
                    foreach (var value in str.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (String.IsNullOrWhiteSpace(value) || String.IsNullOrWhiteSpace(value.Trim()))
                            throw new CommandCentralException("One of your values was not valid.", ErrorTypes.Validation);

                        values.Add(value.Trim());
                    }

                    var disjunction = new Disjunction();

                    foreach (var value in values)
                    {
                        disjunction.Add(Restrictions.On(() => addressAlias.Address).IsInsensitiveLike(value, MatchMode.Anywhere));
                    }

                    return disjunction;
                });

                ForProperties(
                    x => x.PhysicalAddresses)
                .AsType(SearchDataTypes.String)
                .CanBeUsedIn(QueryTypes.Advanced)
                .UsingStrategy(token =>
                {
                    PhysicalAddress addressAlias = null;
                    token.Query.JoinAlias(x => x.PhysicalAddresses, () => addressAlias);

                    var query = new PhysicalAddress.PhysicalAddressQueryProvider().CreateQuery(QueryTypes.Simple, token.SearchParameter.Value);

                    using (var session = SessionManager.CurrentSession)
                    {
                        var ids = query.GetExecutableQueryOver(session).Select(x => x.Id).List<Guid>();

                        return Restrictions.On(() => addressAlias.Id).IsIn(ids.ToList());
                    }

                });
            }
        }

    }
}
