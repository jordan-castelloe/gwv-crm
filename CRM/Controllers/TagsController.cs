using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRM.Data;
using CRM.Models;

namespace CRM.Controllers
{
    public class TagsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> AddTag(string tagName, [FromRoute] int id)
        {
            Tag tagFromDB= await _context.Tag.FirstOrDefaultAsync(tag => tag.Name.ToLower() == tagName.ToLower());
            int tagId = tagFromDB != null ? tagFromDB.Id : 0;
            bool isItAlreadyAssigned = false;
            if (tagFromDB == null)
            {
                Tag newTag = new Tag()
                {
                    Name = tagName
                };
                _context.Add(newTag);
                await _context.SaveChangesAsync();
                tagId = newTag.Id;
            } else
            {
                isItAlreadyAssigned = _context.ContactTag
                     .Include(ct => ct.Tag)
                     .Any(ct => ct.Tag.Name == tagName);
            }
            
            if(tagId != 0 && !isItAlreadyAssigned)
            {
                ContactTag newAssignment = new ContactTag()
                {
                    TagId = tagId,
                    ContactId = id

                };
                _context.Add(newAssignment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", "Contacts", new { id = id.ToString()});


        }

        public async Task DeleteTag(int id)
        {
            var assignedTag = await _context.ContactTag.FindAsync(id);
            _context.ContactTag.Remove(assignedTag);
            await _context.SaveChangesAsync();
            
        }
        // GET: Tags
        public async Task<IActionResult> ListTags(string q)
        {
            List<Tag> tags = await _context.Tag.ToListAsync();

            if (q != "" && q != null)
            {
                tags = tags.Where(tag => tag.Name.Contains(q)).ToList();
            }
            
            return Ok(tags);
        }

        // GET: Tags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await _context.Tag
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // GET: Tags/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        // GET: Tags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await _context.Tag.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }

        // POST: Tags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Tag tag)
        {
            if (id != tag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TagExists(tag.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        // GET: Tags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tag = await _context.Tag
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tag = await _context.Tag.FindAsync(id);
            _context.Tag.Remove(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TagExists(int id)
        {
            return _context.Tag.Any(e => e.Id == id);
        }
    }
}
