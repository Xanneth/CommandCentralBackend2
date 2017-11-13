﻿using System;
using System.Collections.Generic;
using System.Linq;
using CommandCentral.Authorization;
using CommandCentral.Entities;
using CommandCentral.Enums;
using CommandCentral.Framework;
using Microsoft.AspNetCore.Mvc;

namespace CommandCentral.Controllers.CommandStructureControllers
{
    /// <summary>
    /// Provides access to the different commands supported by the api.  
    /// Does not provide access to all persons in a command.  GET /api/persons?command=commandId will provide you that functionality.  
    /// Due to the numerous places a command can be referenced, no command may be deleted.
    /// </summary>
    public partial class CommandsController : CommandCentralController
    {
        /// <summary>
        /// Retrieves all commands.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<DTOs.Command.Get>))]
        public IActionResult Get()
        {
            return Ok(DBSession.Query<Command>()
                .Select(item => new DTOs.Command.Get(item))
                .ToList());
        }

        /// <summary>
        /// Gets a command.
        /// </summary>
        /// <param name="id">The id of the command to get.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(DTOs.Command.Get))]
        public IActionResult Get(Guid id)
        {
            var item = DBSession.Get<Command>(id);
            if (item == null)
                return NotFoundParameter(id, nameof(id));

            return Ok(new DTOs.Command.Get(item));
        }

        /// <summary>
        /// Creates a new command.  Requires access to the admin tools submodule.
        /// </summary>
        /// <param name="dto">An object containing all of the information required to create a new command.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(DTOs.Command.Get))]
        public IActionResult Post([FromBody] DTOs.Command.Update dto)
        {
            if (dto == null)
                return BadRequestDTONull();

            if (!User.CanAccessSubmodules(SubModules.AdminTools))
                return Forbid();

            var item = new Command
            {
                Id = Guid.NewGuid(),
                Description = dto.Description,
                Name = dto.Name
            };

            var result = item.Validate();
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(x => x.ErrorMessage));

            DBSession.Save(item);

            CommitChanges();

            return CreatedAtAction(nameof(Get), new {id = item.Id}, new DTOs.Command.Get(item));
        }

        /// <summary>
        /// Modifies a command.  Requires access to the admin tools submodule.
        /// </summary>
        /// <param name="id">The id of the command to modify.</param>
        /// <param name="dto">A dto containing all the information required to update a command.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(201, Type = typeof(DTOs.Command.Get))]
        public IActionResult Put(Guid id, [FromBody] DTOs.Command.Update dto)
        {
            if (dto == null)
                return BadRequestDTONull();

            if (!User.CanAccessSubmodules(SubModules.AdminTools))
                return Forbid();

            var item = DBSession.Get<Command>(id);
            if (item == null)
                return NotFoundParameter(id, nameof(id));

            item.Description = dto.Description;
            item.Name = dto.Name;

            var result = item.Validate();
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(x => x.ErrorMessage));

            CommitChanges();

            return CreatedAtAction(nameof(Put), new {id = item.Id}, new DTOs.Command.Get(item));
        }
    }
}