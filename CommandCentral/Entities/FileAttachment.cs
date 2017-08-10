﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using FluentNHibernate.Mapping;
using System.IO;

namespace CommandCentral.Entities
{
    public class FileAttachment : Entity, IHazComments
    {
        public static string AttachmentsDirectory = Utilities.ConfigurationUtility.Configuration["Attachments"];

        public virtual IList<Comment> Comments { get; set; }

        public virtual IHazAttachments OwningEntity { get; set; }

        public virtual string AttachmentFilePath
        {
            get
            {
                return Path.Combine(Directory.GetCurrentDirectory(), AttachmentsDirectory, Id.ToString() + ".ccatt");
            }
        }

        public virtual string OverlayFilePath
        {
            get
            {
                return Path.Combine(Directory.GetCurrentDirectory(), AttachmentsDirectory, Id.ToString() + ".ccover");
            }
        }

        public bool CanPersonAccessComments(Person person)
        {
            throw new NotImplementedException();
        }

        public override ValidationResult Validate()
        {
            throw new NotImplementedException();
        }

        public class FileAttachmentMapping : ClassMap<FileAttachment>
        {
            public FileAttachmentMapping()
            {
                Id(x => x.Id).GeneratedBy.Assigned();

                ReferencesAny(x => x.OwningEntity)
                    .AddMetaValue<NewsItem>(typeof(Correspondence.CorrespondenceItem).Name)
                    //Uncomment this and the line below when adding comments to a Person breaks.  This is an experiment to make sure I understand this shit.
                    //.AddMetaValue<Person>(typeof(Person).Name)
                    .IdentityType<Guid>()
                    .EntityTypeColumn("OwningEntity_Type")
                    .EntityIdentifierColumn("OwningEntity_id")
                    .MetaType<string>();
            }
        }
    }
}
