using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApi.Models;


namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/type-documents")]
    public class TypeDocumentsController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public TypeDocumentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TypeDocuments
        [HttpGet]
        public IActionResult GetTypeDocuments(string order = "Name", string sort = "ASC", int offset = 0, int limit = 5, string filter = "{}")
        {
            IEnumerable<TypeDocument> typeDocuments = Filter(sort, order, offset, limit, filter);
            var data = typeDocuments.ToArray();
            var dict = new Dictionary<object, object>();
            dict["Rows"] = data;
            dict["RowsPerPage"] = limit - offset;
            dict["TotalRows"] = _context.TypeDocuments.Count();
            return BuildJsonResponse(200, "SUCCESSFUL_REQUEST", dict);
        }

        private IEnumerable<TypeDocument> Filter(string sort, string order, int offset, int limit, string filter)
        {
            IEnumerable<TypeDocument> List = _context.TypeDocuments;

            if (sort == "ASC")
                List = List.OrderBy(x => x.Name);
            else
                List = List.OrderByDescending(x => x.Name);

            if (filter != "{}")
            {
                var jo = JObject.Parse(filter);
                var value = jo["Name"].ToString();
                List = List.Where(x => x.Name.ToLower().Contains(value.ToLower()));

            }

            List = List.Skip(offset).Take(limit);
            return List;
        }



        // GET: api/TypeDocuments/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTypeDocument([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var typeDocument = await _context.TypeDocuments.SingleOrDefaultAsync(m => m.Id == id);

            if (typeDocument == null)
            {
                return NotFound();
            }

            return BuildJsonResponse(200, "SUCCESSFUL_REQUEST", typeDocument); //Ok(typeDocument);
        }

        // PUT: api/TypeDocuments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypeDocument([FromRoute] int id, [FromBody] TypeDocument typeDocument)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != typeDocument.Id)
            {
                return BadRequest();
            }

            _context.Entry(typeDocument).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TypeDocumentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TypeDocuments
        [HttpPost]
        public async Task<IActionResult> PostTypeDocument([FromBody] TypeDocument typeDocument)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TypeDocuments.Add(typeDocument);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTypeDocument", new { id = typeDocument.Id }, typeDocument);
        }

        // DELETE: api/TypeDocuments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypeDocument([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var typeDocument = await _context.TypeDocuments.SingleOrDefaultAsync(m => m.Id == id);
            if (typeDocument == null)
            {
                return NotFound();
            }

            _context.TypeDocuments.Remove(typeDocument);
            await _context.SaveChangesAsync();

            return Ok(typeDocument);
        }

        private bool TypeDocumentExists(int id)
        {
            return _context.TypeDocuments.Any(e => e.Id == id);
        }
    }
}