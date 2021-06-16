using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BookKeeperBECommon.BusinessObjects;
using BookKeeperBECommon.Services;



namespace BookKeeperBERest.Controllers
{



    //[Route("api/contacts")]
    [ApiController]
    [Route("/api/contacts")]
    public class ContactController : ControllerBase
    {



        private readonly ILogger<ContactController> _logger;

        private readonly ContactService _contactService;



        public ContactController(ILogger<ContactController> logger)
        {
            _logger = logger;
            // Temporary solution
            _contactService = new ContactService();
        }



        // REST API path: GET /api/contacts
        // REST API path: GET /api/contacts/?contactname=ba
        //public IEnumerable<Contact> Get()
        [HttpGet]
        public IActionResult Get([FromQuery] Contact contact)
        {
            IEnumerable<Contact> contacts = _contactService.SearchContacts(contact);
            // HTTP status code: 200 (OK)
            return Ok(contacts);
            //return contacts;
        }



        // REST API path: GET /api/contacts/3
        //public Contact Get(int id)
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            Contact contact = new Contact { ID = id };
            try
            {
                contact = _contactService.LoadContact(id);
            }
            catch (Exception)
            {
                // No such contact (non-existing ID).
                // HTTP status code: 404 (Not Found)
                return NotFound(new { id = contact.ID });
            }
            // HTTP status code: 200 (OK)
            return Ok(contact);
            // return contact;
        }



        // REST API path: PUT /api/contacts
        // Data is in the request body in JSON format.
        // Therefore, we have an HTTP header of "Content-Type", with a value of "application/json".
        [HttpPut]
        public IActionResult Put(Contact contact)
        {
            _logger.LogInformation(contact.ToString());

            // Is there a contact with the given ID?
            bool exists = _contactService.ExistsContact(contact.ID);
            if ( ! exists)
            {
                // HTTP status code: 404 (Not Found)
                return NotFound(contact);
            }

            // Update the contact.
            _contactService.SaveContact(contact);

            // REST API recommends either a status code of 200 (OK) or 204 (No Content) to be returned.
            // HTTP status code: 200 (OK)
            //return Ok(contact);
            // HTTP status code: 204 (No Content)
            return NoContent();
        }



        // REST API path: POST /api/contacts
        // Data is in the request body in JSON format.
        // Therefore, we have an HTTP header of "Content-Type", with a value of "application/json".
        [HttpPost]
        public IActionResult Post(Contact contact)
        {
            _logger.LogInformation(contact.ToString());

            // Add a new contact.
            Contact newContact = _contactService.SaveContact(contact);

            // HTTP status code: 201 (Created)
            return Created(this.Request.Path, newContact);
        }



        // REST API path: DELETE /api/contacts/3
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation(id.ToString());

            // Is there a contact with the given ID?
            bool exists = _contactService.ExistsContact(id);
            if ( ! exists )
            {
                // HTTP status code: 404 (Not Found)
                return NotFound(new { id = id });
            }

            // Delete the contact.
            Contact contactDeleted = _contactService.DeleteContact(id);

            // HTTP status code: 200 (OK)
            return Ok(contactDeleted);
            //return Ok(new { id = contactDeleted.ID });
        }



    }



}
