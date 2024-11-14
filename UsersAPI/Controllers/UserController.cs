using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using UsersAPI.Data;
using UsersAPI.Domain;
using FluentValidation.Results;

namespace UsersAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IValidator<User> _validator;
        private readonly ApiDbContext _context;
        private readonly ILogger<UserController> _logger;
        public UserController(ILogger<UserController> logger, IValidator<User> validator, ApiDbContext context)
        {
            _context = context;
            _validator = validator;
            _logger = logger;
        }

        [HttpPost]
        public JsonResult UpdateUser(User user)
        {
            ValidationResult result = _validator.Validate(user);
            if (!result.IsValid)
            {
                Log.Information("Update User Details: {@User}", user);
                return new JsonResult(BadRequest(result.Errors));
            }
            //Email must be unique in the database
            if (_context.Users.Any(u => u.Email == user.Email && u.Id != user.Id))
            {
                Log.Information("Update User Failed Duplication Email Details: {@User}", user);
                return new JsonResult(BadRequest("Duplicate Email"));
            }
            if (user.Id == 0)
            {
                _context.Users.Add(user);
            }
            else
            {
                var userInDb = _context.Users.Find(user.Id);

                if (userInDb == null)
                    return new JsonResult(NotFound());

                userInDb = user;
            }

            _context.SaveChanges();

            return new JsonResult(Ok(user));

        }
        [HttpPut]
        public JsonResult InsertUser(User user)
        {
            ValidationResult result = _validator.Validate(user);
            if (!result.IsValid)
            {
                Log.Information("Update User Details: {@Errors}", result.Errors);
                return new JsonResult(BadRequest(result.Errors));
            }
            //Email must be unique in the database
            if (_context.Users.Any(u => u.Email == user.Email && u.Id != user.Id))
            {
                Log.Information("Insert User Failed Duplication Email Details: {@User}", user);
                return new JsonResult(BadRequest("Duplicate Email"));
            }
            _context.Users.Add(user);
            _context.SaveChanges();
            Log.Information("Insert User Details: {@User}", user);

            return new JsonResult(Ok(user));

        }


        // Get
        [HttpGet]
        [Route("api/[controller]/{id}")]
        public JsonResult Get(int id)
        {
            var result = _context.Users.Find(id);

            if (result == null)
                return new JsonResult(NotFound());

            return new JsonResult(Ok(result));
        }

        // Delete
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            var result = _context.Users.Find(id);

            if (result == null)
            {
                Log.Information("Delete User Not Found: {@id}", id);
                return new JsonResult(NotFound());
            }
            _context.Users.Remove(result);
            _context.SaveChanges();
            Log.Information("Delete User Details: {@User}", result);

            return new JsonResult(NoContent());
        }

        // Get all
        [HttpGet()]
        [Route("api/[controller]/")]
        public JsonResult GetAll()
        {
            var result = _context.Users.ToList();
            Log.Information("Get All Users");

            return new JsonResult(Ok(result));
        }
    }
}
