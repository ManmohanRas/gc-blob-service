using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using morrisTestAPI.Context;
using morrisTestAPI.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace morrisTestAPI.Controllers
{
     [Authorize(Policy = "PLIApiReader")]
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class AgenciesController : ControllerBase
    {
        private readonly PLIDbContext _context;

        public AgenciesController(PLIDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Select all Agencies.
        /// </summary>
        [Authorize(Policy = "Consumer")]
        [HttpGet]
       
        public async Task<ActionResult<IEnumerable<Agency>>> GetAgencies()
        {
            return await _context.Agency.ToListAsync();
        }

        /// <summary>
        /// Select an Agency by AgencyID.
        /// </summary>
        /// <param name="id"></param>   
        [Authorize(Policy = "Consumer")]
        [HttpGet("{id}")]
        
        public async Task<ActionResult<Agency>> GetAgency(string id)
        {
            var agency = await _context.Agency
                 .Where(a => a.AgencyId == id)
               // .Include("Tract")
                .FirstAsync();
            ;

            if (agency == null)
            {
                return NotFound();
            }

            return Ok(agency);
        }

        /// <summary>
        /// Select Agency by Governemt Level.
        /// </summary>
        /// <param name="id"></param> 
        [Authorize(Policy = "Consumer")]
        [HttpGet("government{id}")]
               public async Task<ActionResult<Agency>> GetGovernmentAgency(string id)
        {
            var agency = await _context.Agency
                .Where(a => a.AgencyType == id.Substring(0, 1).ToUpper())
                .OrderBy(a => a.AgencyLabel)
             //   .Include("Tract")
                .ToListAsync();

            if (agency == null)
            {
                return NotFound();
            }

            return Ok(agency);
        }


        //// PUT: api/Agencies/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutAgency(string id, Agency agency)
        //{
        //    if (id != agency.AgencyId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(agency).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!AgencyExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Agencies
        //[HttpPost]
        //public async Task<ActionResult<Agency>> PostAgency(Agency agency)
        //{
        //    _context.Agencies.Add(agency);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (AgencyExists(agency.AgencyId))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetAgency", new { id = agency.AgencyId }, agency);
        //}

        //// DELETE: api/Agencies/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Agency>> DeleteAgency(string id)
        //{
        //    var agency = await _context.Agencies.FindAsync(id);
        //    if (agency == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Agencies.Remove(agency);
        //    await _context.SaveChangesAsync();

        //    return agency;
        //}

        private bool AgencyExists(string id)
        {
            return _context.Agency.Any(e => e.AgencyId == id);
        }
    }
}