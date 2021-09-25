using FirstWebApi.Data;
using FirstWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstWebApi.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public UserController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            if (!_dbContext.Users.Any())
            {
                _dbContext.Users.Add(new User { Name = "FirstUser", Age = 20 });
                _dbContext.Users.Add(new User { Name = "SecondUser", Age = 30 });
                _dbContext.SaveChanges();
            }
        }


        [HttpGet]
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u=>u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return new ObjectResult(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> AddUser(User user)
        {
            if (user==null)
            {
                return BadRequest();
            }
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPut]
        public async Task<ActionResult<User>> UpdateUser(User user)
        {
            if (user==null)
            {
                return BadRequest();
            }
            if (!_dbContext.Users.Any(u=>u.Id==user.Id))
            {
                return NotFound();
            }

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user==null)
            {
                return NotFound();
            }
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
            return Ok(user);
        }

    }
}
