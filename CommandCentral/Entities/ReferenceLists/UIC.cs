﻿using FluentNHibernate.Mapping;

namespace CommandCentral.Entities.ReferenceLists
{
    /// <summary>
    /// Describes a single UIC.
    /// </summary>
    public class UIC : ReferenceListItemBase
    {
        /// <summary>
        /// Maps a UIC to the database.
        /// </summary>
        public class UICMapping : SubclassMap<UIC>
        {
        }
    }
}
