using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TokenScanner.Models;
using TokenScanner.Services.Export;

namespace TokenScanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly TokenContext _context;
        private readonly IExportService _exportService;

        public TokensController(TokenContext context, IExportService exportService)
        {
            _context = context;
            _exportService = exportService;
        }

        // GET: api/Tokens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Token>>> GetTokens()
        {
          if (_context.Tokens == null)
          {
              return NotFound();
          }
            return await _context.Tokens.ToListAsync();
        }

        // GET: api/Tokens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Token>> GetToken(int id)
        {
          if (_context.Tokens == null)
          {
              return NotFound();
          }
            var token = await _context.Tokens.FindAsync(id);

            if (token == null)
            {
                return NotFound();
            }

            return token;
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateToken(Token token)
        {
            if (_context.Tokens == null)
            {
                return Problem("Entity set 'TokenContext.Tokens'  is null.");
            }

            Token tokenToUpdate = _context.Tokens.AsNoTracking().FirstOrDefault(x => x.Symbol == token.Symbol);

            if (tokenToUpdate != null)
            {
                try
                {
                    token.Id = tokenToUpdate.Id;

                    _context.Entry(token).State = EntityState.Modified;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return NoContent();
            }
            else
            {
                _context.Tokens.Add(token);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetToken", new { id = token.Id }, token);
            }
        }

        // DELETE: api/Tokens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToken(int id)
        {
            if (_context.Tokens == null)
            {
                return NotFound();
            }
            var token = await _context.Tokens.FindAsync(id);
            if (token == null)
            {
                return NotFound();
            }

            _context.Tokens.Remove(token);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("export")]
        public FileResult ExportCsv()
        {
            byte[] fileBytes = _exportService.GenerateFileContent(_context.Tokens);

            return File(fileBytes, "text/csv", "tokens.csv");
        }
    }
}
