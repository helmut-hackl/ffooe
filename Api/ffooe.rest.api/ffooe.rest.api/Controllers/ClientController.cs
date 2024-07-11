using ffooe.db.context;
using ffooe.db.entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimplePatch;

namespace ffooe.rest.api.Controllers
{
    [Authorize(Roles="ADMIN")]
    [ApiController]
    [Route("api/v1.0/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;
        private readonly FFOOEContext _context;
        public ClientController(ILogger<ClientController> logger, FFOOEContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: api/M_Client
        [HttpGet]
        public async Task<ActionResult<IEnumerable<M_Client>>> Get_Clients()
        {
            return await _context.M_Clients.ToListAsync();
        }

        // GET: api/M_Client/5
        [HttpGet("{id}")]
        public async Task<ActionResult<M_Client>> Get_Client(int id)
        {
            var m_Client = await _context.M_Clients.FindAsync(id);

            if (m_Client == null)
            {
                return NotFound();
            }

            return m_Client;
        }

        // PUT: api/M_Client/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Put_Client(int id, M_Client m_Client)
        {
            if (id != m_Client.Id)
            {
                return BadRequest();
            }

            _context.Entry(m_Client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!M_ClientExists(id))
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

        // POST: api/M_Client
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<M_Client>> Post_Client(M_Client m_Client)
        {
            _context.M_Clients.Add(m_Client);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetM_Client", new { id = m_Client.Id }, m_Client);
        }

        // DELETE: api/M_Client/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete_Client(int id)
        {
            var m_Client = await _context.M_Clients.FindAsync(id);
            if (m_Client == null)
            {
                return NotFound();
            }

            _context.M_Clients.Remove(m_Client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/M_Client/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch_Client(int id, Delta<M_Client> m_Client)
        {
            // Determines the entity to be updated according to the id parameter
            var m_ClientToPatch = await _context.M_Clients.FindAsync(id);
            if (m_ClientToPatch == null) return NotFound();
            m_Client.Patch(m_ClientToPatch);
            _context.Entry(m_ClientToPatch).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!M_ClientExists(id))
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

        private bool M_ClientExists(int id)
        {
            return _context.M_Clients.Any(e => e.Id == id);
        }
    }
}
