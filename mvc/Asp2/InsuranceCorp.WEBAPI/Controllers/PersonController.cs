using InsuranceCorp.Data;
using InsuranceCorp.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsuranceCorp.WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private InsCorpDbContext db;
        public PersonController(InsCorpDbContext _db)
        {
            db = _db;
        }

        //[Route("list")]
        [HttpGet("list")]
        public ActionResult<List<Person>> List(int start, int take)
        {
            return db.Persons.Skip(start).Take(take).ToList();
        }

        [HttpGet("/{id:int}")]
        public ActionResult<Person> Get(int id)
        {
            var person = db.Persons.Include(x => x.Address).Include(x => x.Contracts).FirstOrDefault(x => x.Id == id);

            if (person == null)
            {
                return NotFound();
            }
            return person;
        }

        [HttpGet("findByEmail/{text:alpha}")]
        public ActionResult<List<Person>> FindByEmail(string text)
        {
            var persons = db.Persons.Where(x => !string.IsNullOrEmpty(x.Email) && x.Email.ToLower().Contains(text.ToLower())).ToList();

            if (persons.Count == 0)
            {
                return NotFound();
            }
            return persons;
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult CreatePerson(Person person)
        {
            db.Persons.Add(person);
            db.SaveChanges();
            return Created($"/api/person/{person.Id}", person);
        }

        [HttpPost("edit")]
        public ActionResult EditPerson(Person person)
        {
            db.Persons.Update(person);
            db.SaveChanges();
            return Created($"/api/person/{person.Id}", person);
        }
    }
}
