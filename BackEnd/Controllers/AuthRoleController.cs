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
  public class AuthRoleController : ControllerBase {
    private readonly MyDatabaseContext _context;
    private IUserService _userService;
    private IHttpContextAccessor _httpContextAccessor;
    private readonly IEventService _eventService;
    public AuthRoleController (
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

    [HttpPost("createAuthRole")]
    public IActionResult CreateMenuRole ([FromBody] AuthRoleModel request) {
      try {
        User currentUser = _userService.CheckAdmin(_httpContextAccessor.HttpContext.User);
        if (currentUser != null) {
          request.created_by = currentUser.User_Id;
          request.created_date = DateTime.Now;
          _context.Add(request);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_CREATE, "AuthRole");
          return Ok(request);
        } else {
          return Ok(SendResult.SendError("You must login."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t create new complany"));
      }
    }

    [HttpPost("getAuthRole")]
    public IActionResult GetAuthRole ([FromBody] JObject request) {
      try {
        User currentUser = _userService.GetCurrentUser(_httpContextAccessor.HttpContext.User);
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          AuthRoleModel model = _context.AuthRoles.FirstOrDefault(f => f.id == id);
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You must login."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t create new complany"));
      }
    }

    [HttpPost("getUserRole")]
    public IActionResult GetUserRole () {
      try {
        User currentUser = _userService.GetCurrentUser(_httpContextAccessor.HttpContext.User);
        if (currentUser != null) {
          return Ok(_context.AuthRoles.Where(role => role.roll_id == currentUser.Roll_Id).ToList<AuthRoleModel>());
        } else {
          return Ok(SendResult.SendError("You must login."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t create new complany"));
      }
    }

    [HttpPost("getAuthRoles")]
    public IActionResult GetAuthRoles ([FromBody] JObject request) {
      try {
        User currentUser = _userService.GetCurrentUser(_httpContextAccessor.HttpContext.User);
        if (currentUser != null) {
          int id = request["roll_id"].Value<int>();
          return Ok(_context.AuthRoles.Where(role => role.roll_id == id).ToList<AuthRoleModel>());
        } else {
          return Ok(SendResult.SendError("You must login."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t create new complany"));
      }
    }

    [HttpPost("updateAuthRole")]
    public IActionResult UpdateAuthRole ([FromBody] AuthRoleModel request) {
      try {
        User currentUser = _userService.CheckAdmin(_httpContextAccessor.HttpContext.User);
        if (currentUser != null) {
          AuthRoleModel model = _context.AuthRoles.FirstOrDefault(f => f.id == request.id);
          UserRoleModel role = _context.Roles.FirstOrDefault(r => r.Roll_Id == model.roll_id);
          if (role.Rollname == "admin") {
            return Ok(SendResult.SendError("Can not Change admin role."));
          }
          model.menu = request.menu;
          model.saveRole = request.saveRole;
          model.deleteRole = request.deleteRole;
          model.updateRole = request.updateRole;
          model.viewRole = request.viewRole;
          model.printRole = request.printRole;
          model.status = request.status;
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_UPDATE, "AuthRole");
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You must login."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t create new complany"));
      }
    }

    [HttpPost("removeAuthRole")]
    public IActionResult RemoveAuthRole ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckAdmin(_httpContextAccessor.HttpContext.User);
        if (currentUser != null) {
          UserRoleModel role = _context.Roles.FirstOrDefault(r => r.Roll_Id == currentUser.Roll_Id);
          if (role.Rollname == "admin") {
            return Ok(SendResult.SendError("Can not Change admin role."));
          }
          int id = request["id"].Value<int>();
          AuthRoleModel model = _context.AuthRoles.FirstOrDefault(f => f.id == id);
          _context.AuthRoles.Remove(model);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_REMOVE, "AuthRole");
          return Ok(true);
        } else {
          return Ok(SendResult.SendError("You must login."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t create new complany"));
      }
    }
  }
}