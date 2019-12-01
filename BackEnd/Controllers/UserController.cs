using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data;
using BackEnd.Helpers;
using BackEnd.Models;
using BackEnd.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace BackEnd.Controllers {
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class UserController : ControllerBase {
    private readonly MyDatabaseContext _context;
    private IUserService _userService;
    private IHttpContextAccessor _httpContextAccessor;
    private IEventService _eventService;
    private string menu = "Users";
    public UserController (
        MyDatabaseContext context,
        IUserService userService,
        IHttpContextAccessor httpContextAccessor,
        IEventService eventService
        ) {
      _context = context;
      _userService = userService;
      _httpContextAccessor = httpContextAccessor;
      _eventService = eventService;
    }
    // GET: api/User
    [HttpPost("getUser")]
    public IActionResult Get ([Bind("user_Id")] User user) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          return Ok(_context.Users.FirstOrDefault(u => u.User_Id == user.User_Id));
        } else {
          return Ok(SendResult.SendError("You don`t have permision"));
        }
      } catch (Exception error) {
        Console.Write(error);
        return Ok(SendResult.SendError("You don`t have permision"));
      }
    }

    [HttpPost("getUsers")]
    public IActionResult GetUsers () {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          return Ok(_context.Users.ToList<User>());
        } else {
          return Ok(SendResult.SendError("You don`t have permision"));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return Ok(SendResult.SendError("You don`t have permision"));
      }
    }

    [HttpPost("createUser")]
    public IActionResult CreateUser ([FromBody] User request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "create");
        if (currentUser != null) {
          request.Created_by = currentUser.User_Id;
          request.Status = 0;
          request.Password = Hashing.HashPassword(request.Password);
          _context.Add(request);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_CREATE, request.Username, "User");
          return Ok(request);
        } else {
          return Ok(SendResult.SendError("You don`t have permision"));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("Can not create user"));
      }
    }

    [HttpPost("updateUser")]
    public IActionResult UpdateUser ([FromBody] User user) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "update");
        int user_Id = user.User_Id;
        if (currentUser != null || _userService.CheckSelf(_httpContextAccessor.HttpContext.User, user_Id) != null) {
          User model = _context.Users.FirstOrDefault(u => u.User_Id == user_Id);
          model.Username = user.Username;
          model.Useremail = user.Useremail;
          //model.Password = Hashing.HashPassword(user.Password);
          model.Roll_Id = user.Roll_Id;
          model.Branch_Id = user.Branch_Id;
          model.Created_by = currentUser.User_Id;
          model.Status = user.Status;
          model.Emp_Id = user.Emp_Id;
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_UPDATE, model.Username, "User");
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You don`t have permision"));
        }
      } catch (Exception error) {
        return BadRequest(SendResult.SendError(error.ToString()));
      }
    }

    [HttpPost("updatePassword")]
    public IActionResult UpdatePassword ([Bind(
            "user_Id",
            "password"
            )] User user) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "update");
        if (currentUser != null || _userService.CheckSelf(_httpContextAccessor.HttpContext.User, user.User_Id) != null) {
          User model = _context.Users.FirstOrDefault(u => u.User_Id == user.User_Id);
          model.Password = Hashing.HashPassword(user.Password);
          model.Created_by = currentUser.User_Id;
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_UPDATE, model.Username + " Password", "User");
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You don`t have permision"));
        }
      } catch (Exception error) {
        return Ok(SendResult.SendError(error.ToString()));
      }
    }

    [HttpPost("deleteUser")]
    public IActionResult deleteUser ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "delete");
        int id = request["id"].Value<int>();
        if (currentUser != null || _userService.CheckSelf(_httpContextAccessor.HttpContext.User, id) != null) {

          User model = _context.Users.FirstOrDefault(r => r.User_Id == id);
          string name = model.Username;
          _context.Users.Remove(model);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_DELETE, name, "User");
          return Ok(true);
        } else {
          return Ok(SendResult.SendError("You don`t have permision"));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return Ok(SendResult.SendError("You don`t have permision"));
      }
    }
  }
}

