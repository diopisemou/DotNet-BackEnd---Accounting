using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BackEnd.Data;
using BackEnd.Helpers;
using BackEnd.Models;
using BackEnd.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace BackEnd.Controllers {
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase {
    private readonly MyDatabaseContext _context;
    private IUserService _userService;
    private IHttpContextAccessor _httpContextAccessor;
    private IEventService _eventService;
    public AuthController (
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
    [HttpGet]
    public ActionResult<IEnumerable<User>> Create () {
      try {
        var items = _context.Users.ToList<User>();
        return items;
      } catch (Exception error) {
        throw error;
      }

    }
    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult Register ([Bind("username", "useremail", "password")] User user) {
      if (user.Useremail == null || user.Username == null || user.Password == null) {
        return Ok(SendResult.SendError("Please confirm your information"));
      }
      var savedUser = _userService.GetUserByEmail(user.Useremail);
      if (savedUser == null) {
        string password = user.Password;
        user.Password = Hashing.HashPassword(user.Password);
        UserRoleModel roleModel = _context.Roles.FirstOrDefault(r => r.Rollname == "guest");
        user.Roll_Id = roleModel.Roll_Id;
        _context.Add(user);
        int x = _context.SaveChanges();
        _eventService.SaveEvent(user.User_Id, EventUserLog.EVENT_CREATE, user.Username);
        return Ok(SendResult.SendUser(_userService.Authenticate(user.Useremail, password), _context));
      } else {
        return Ok(SendResult.SendError("Your email is already exist!"));
      }

    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public IActionResult Authentication ([Bind("useremail", "password", "branch_Id")] User user) {
      var result = _userService.Authenticate(user.Useremail, user.Password);
      if (user.Branch_Id == 0 || result.Branch_Id!=user.Branch_Id) {
        return Ok(SendResult.SendError("Please select correctly branch."));
      }
      if (result == null) {
        return Ok(SendResult.SendError("Please confirm your email and password."));
      } else {
        if (result.Status > 0) {
          _eventService.SaveEvent(result.User_Id, EventUserLog.EVENT_LOGIN, user.Username);
          return Ok(SendResult.SendUser(result, _context));
        } else {
          return Ok(SendResult.SendError("Your account is locked."));
        }
      }

    }

    [Authorize]
    [HttpPost("jwt")]
    public IActionResult JWTLogin () {
      var currentUser = _httpContextAccessor.HttpContext.User;
      if (currentUser.HasClaim(c => c.Type == ClaimTypes.Email)) {
        var user = _userService.GetUserByEmail(currentUser.FindFirst(ClaimTypes.Email).Value);
        if (user.Status > 0) {
          _eventService.SaveEvent(user.User_Id, EventUserLog.EVENT_LOGIN, user.Username);
          return Ok(SendResult.SendUser(user, _context));
        } else {
          return Ok(SendResult.SendError("Your account is locked"));
        }
      } else {
        return Ok(SendResult.SendError("You must login."));

      }
    }
  }
}