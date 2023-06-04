using FirstApi.Data;
using Microsoft.AspNetCore.Mvc;
using FirstApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {

        private readonly ContactsAPIDbContext dbContext1;
        public ContactsController(ContactsAPIDbContext dbContext)
        {
            this.dbContext1 = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            return Ok(await dbContext1.Contacts.ToListAsync());

        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var contact = await dbContext1.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }
        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactRequest contactRequest)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Address = contactRequest.Address,
                Email = contactRequest.Email,
                FullName = contactRequest.FullName,
                Phone = contactRequest.Phone
            };
            await dbContext1.Contacts.AddAsync(contact);
            await dbContext1.SaveChangesAsync();
            return Ok(contact);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest)

        {
            var contact = await dbContext1.Contacts.FindAsync(id);
            if (contact != null)
            {
                contact.FullName = updateContactRequest.FullName;
                contact.Email = updateContactRequest.Email;
                contact.Address = updateContactRequest.Address;
                contact.Phone = updateContactRequest.Phone;
                await dbContext1.SaveChangesAsync();
                return Ok(contact);
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contact = await dbContext1.Contacts.FindAsync(id);
            if (contact != null)
            {
                dbContext1.Remove(contact);
                await dbContext1.SaveChangesAsync();
                return Ok(contact);
            }
            return NotFound();
        }

    }
}