using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using WebApi.Models;
using WebApi.Models.ViewModels;

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Documents")]
    public class DocumentsController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public DocumentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Documents
        [HttpGet]
        public IActionResult GetDocuments(string order="Name", string sort = "ASC", int offset = 0, int limit = 5, string filter = "{}")
        {
            IEnumerable<Document> documents = Filter(sort, order, offset, limit, filter);
            var data = documents.ToArray();
            var dict = new Dictionary<object, object>();
            dict["Rows"] = data;
            dict["RowsPerPage"] = limit - offset;
            dict["TotalRows"] = _context.Documents.Count();
            return BuildJsonResponse(200, "SUCCESSFUL_REQUEST", dict);
        }

        private IEnumerable<Document> Filter(string sort, string order, int offset, int limit, string filter)
        {
            IEnumerable<Document> List = _context.Documents;

            if (sort == "ASC")
            {
                switch (order)
                {
                    case "Name":
                        List = List.OrderBy(x => x.Name);
                        break;
                    case "PublishedAt":
                        List = List.OrderBy(x => x.PublishedAt);
                        break;
                }

            }

            else
            {
                switch (order)
                {
                    case "Name":
                        List = List.OrderByDescending(x => x.Name);
                        break;
                    case "PublishedAt":
                        List = List.OrderByDescending(x => x.PublishedAt);
                        break;
                }
            }

            if (filter != "{}")
            {
                var jo = JObject.Parse(filter);
                DateTime publishedAt = DateTime.Now;
                string value = "";
                if (jo.GetValue("Name") != null)
                {
                    value = jo["Name"].ToString();
                    List = List.Where(x => x.Name.ToLower().Contains(value.ToLower()));
                }
                if (jo.GetValue("PublishedAt") != null)
                {
                    DateTime.TryParse(jo["PublishedAt"].ToString(), out publishedAt);
                    List = List.Where(x => x.PublishedAt >= publishedAt || x.PublishedAt <= publishedAt);
                }


            }

            List = List.Skip(offset).Take(limit);
            return List;
        }

        // GET: api/Documents/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocument([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var document = await _context.Documents.SingleOrDefaultAsync(m => m.Id == id);

            if (document == null)
            {
                return NotFound();
            }

            return BuildJsonResponse(200, "SUCCESSFUL_REQUEST", document);
        }

        // PUT: api/Documents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocument([FromRoute] int id, [FromBody] Document document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != document.Id)
            {
                return BadRequest();
            }

            _context.Entry(document).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentExists(id))
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
        [HttpPost]
        [Route("upload")]
        public void PostFile(IFormFile uploadedFile)
        {
            //TODO: Save file
        }
        // POST: api/Documents
        [HttpPost]
        public async Task<IActionResult> PostDocument([FromForm] DocumentViewModel document)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (document.uploadedFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    document.uploadedFile.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    document.FileData = fileBytes;

                }
            }
            
            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDocument", new { id = document.Id }, document);
        }

        // DELETE: api/Documents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var document = await _context.Documents.SingleOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();

            return Ok(document);
        }

        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.Id == id);
        }
    }
}