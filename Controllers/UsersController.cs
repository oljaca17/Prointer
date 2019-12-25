using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestniZadatak.Dtos;
using TestniZadatak.Models;
using TestniZadatak.Services;

namespace TestniZadatak.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)

        {
            _userRepository = userRepository;
        }

        //api/users
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
        public IActionResult GetUsers()
        {

            var users = _userRepository.GetUsers().ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var usersDto = new List<UserDto>();
            foreach (var user in users)
            {
                usersDto.Add(new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    City = user.City,
                    Country = user.Country,
                    DateOfBirth = user.DateOfBirth,
                    Phone = user.Phone,
                    AccountBalance = user.AccountBalance,
                });
            }
            return Ok(users);


        }

        //api/users/userId
        [HttpGet("{userId}", Name = "GetUser")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        public IActionResult GetUser(int userId)
        {
            if (!_userRepository.UserExists(userId))
                return NotFound();

            var user = _userRepository.GetUser(userId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var userDto = new UserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                City = user.City,
                Country = user.Country,
                DateOfBirth = user.DateOfBirth,
                Phone = user.Phone,
                AccountBalance = user.AccountBalance,
            };


            return Ok(userDto);
        }


        //api/users
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody]User userToCreate)
        {
            if (userToCreate == null)
                return BadRequest(ModelState);

            var user = _userRepository.GetUsers()
                .Where(u => u.Email.Trim().ToUpper() == userToCreate.Email.Trim().ToUpper()).FirstOrDefault(); //validacija za email, ako korisnik pokusa da unese email koji vec postoji

            if (user != null)
            {
                ModelState.AddModelError("", $"User with this email {userToCreate.Email} already exists!");
                return StatusCode(400, $"User with this email {userToCreate.Email} already exists!");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userRepository.CreateUser(userToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong creating user");
                return StatusCode(400, ModelState);
            }
            return CreatedAtRoute("GetUser", new { userId = userToCreate }, userToCreate);

        }


        //api/users/userId
        [HttpPatch("{userId}")]
        [ProducesResponseType(204)] //no content
        [ProducesResponseType(400)]
        public IActionResult UpdateUser(int userId, [FromBody]User updatedUserInfo)
        {
            if (updatedUserInfo == null)
                return BadRequest(ModelState);

            if (userId != updatedUserInfo.Id)
                return BadRequest(ModelState);

            if (!_userRepository.UserExists(userId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userRepository.UpdateUser(updatedUserInfo))
            {
                ModelState.AddModelError("", $"Something went wrong updating user");
                return StatusCode(400, ModelState);
            }

            
            return NoContent();
        }


        //api/users/userId
        [HttpDelete("{userId}")]
        [ProducesResponseType(204)] //no content
        [ProducesResponseType(404)]

        public IActionResult DeleteUser(int userId)
        {
            if (!_userRepository.UserExists(userId))
                return NotFound();

            var userToDelete = _userRepository.GetUser(userId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_userRepository.DeleteUser(userToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting user");
                return StatusCode(400, ModelState);
            }

            return NoContent();

        }

    }
}
