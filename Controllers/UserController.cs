using DispesasEmpresa.Context;
using DispesasEmpresa.Model.User;
using DispesasEmpresa.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DispesasEmpresa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DispesasEmpresaContext _context;

        public UserController(DispesasEmpresaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _context.Users.ToListAsync();

            return Ok(users);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLogin login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == login.Email && x.Password == login.Password);
            
            if(user == null)
            {
                return NotFound();
            }

            var token = TokenService.GenerateToken(user);

            return Ok(new {Token = token});
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateUser createUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = new User(createUser.Name, createUser.Email, createUser.Password);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, CreateUser createUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            user.Update(createUser.Name, createUser.Email, createUser.Password);
            _context.Update(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            _context.Remove(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
