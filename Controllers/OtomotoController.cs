using Microsoft.AspNetCore.Mvc;
using OtomotoSimpleBackend.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OtomotoSimpleBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtomotoController : ControllerBase
    {
        private readonly OtomotoContext _context;

        public OtomotoController(OtomotoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var offers = _context.Offers.ToList(); // Pobranie wszystkich ofert z bazy danych

            return Ok(offers); // Zwrócenie ofert jako odpowiedź HTTP 200 OK
        }

        [HttpPost("Owner")]
        public IActionResult AddOwner(Owner owner)
        {
            _context.Owners.Add(owner); // Dodanie nowego właściciela do lokalnego kontekstu bazy danych
            _context.SaveChanges(); // Zapisanie zmian w bazie danych

            return Ok(owner); // Zwrócenie dodanego właściciela jako odpowiedź HTTP 200 OK
        }

        // POST: api/Otomoto/Offer
        [HttpPost("Offer")]
        public IActionResult AddOffer(Offer offer)
        {
            _context.Offers.Add(offer); // Dodanie nowej oferty do lokalnego kontekstu bazy danych
            _context.SaveChanges(); // Zapisanie zmian w bazie danych

            return Ok(offer); // Zwrócenie dodanej oferty jako odpowiedź HTTP 200 OK
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
