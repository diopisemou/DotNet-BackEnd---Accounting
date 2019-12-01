using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BackEnd.Data;
using BackEnd.Helpers;
using BackEnd.Models;
using BackEnd.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class RoleController : ControllerBase {
    private readonly MyDatabaseContext _context;
    private IUserService _userService;
    private IHttpContextAccessor _httpContextAccessor;
    private IEventService _eventService;

    public RoleController (
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


    [Authorize]
    [HttpPost("getRoles")]
    public IActionResult GetRoles () {
      try {
        User currentUser = _userService.CheckAdmin(_httpContextAccessor.HttpContext.User);
        if (currentUser != null) {
          return Ok(_context.Roles.ToList<UserRoleModel>());
        } else {
          return Ok(SendResult.SendError("You must login."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return Ok(SendResult.SendError("You don`t create new role"));
      }
    }

    [Authorize]
    [HttpPost("createRole")]
    public IActionResult CreateRole ([Bind("rollname", "status")] UserRoleModel role) {
      try {
        User currentUser = _userService.CheckAdmin(_httpContextAccessor.HttpContext.User);
        if (currentUser != null) {
          role.Created_by = currentUser.User_Id;
          _context.Add(role);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_CREATE, role.Rollname, "User Role");
          return Ok(_context.Roles.ToList<UserRoleModel>());
        } else {
          return Ok(SendResult.SendError("You must login."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return Ok(SendResult.SendError("You don`t create new role"));
      }
    }

    [Authorize]
    [HttpPost("updateRole")]
    public IActionResult UpdateRole ([Bind("roll_Id", "rollname", "status")] UserRoleModel role) {
      try {
        User currentUser = _userService.CheckAdmin(_httpContextAccessor.HttpContext.User);
        if (currentUser != null) {
          UserRoleModel model = _context.Roles.FirstOrDefault(r => r.Roll_Id == role.Roll_Id);
          if (model.Rollname=="admin" || model.Rollname == "customer" || model.Rollname == "guest") {
            return Ok(SendResult.SendError("Can not change default role"));
          }
          model.Rollname = role.Rollname;
          model.Status = role.Status;
          model.Created_by = currentUser.User_Id;
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_UPDATE, model.Rollname, "User Role");
          return Ok(_context.Roles.ToList<UserRoleModel>());
        } else {
          return Ok(SendResult.SendError("You must login."));
        }
      } catch (Exception error) {
        return Ok(SendResult.SendError(error.ToString()));
      }
    }

    [Authorize]
    [HttpPost("deleteRole")]
    public IActionResult deleteRole ([Bind("roll_Id")] UserRoleModel role) {
      try {
        User currentUser = _userService.CheckAdmin(_httpContextAccessor.HttpContext.User);
        if (currentUser != null) {
          UserRoleModel model = _context.Roles.FirstOrDefault(r => r.Roll_Id == role.Roll_Id);
          if (model.Rollname == "admin" || model.Rollname == "customer" || model.Rollname == "guest") {
            return Ok(SendResult.SendError("Can not remove default role"));
          }
          string name = model.Rollname;
          _context.Roles.Remove(model);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_DELETE, name, "User Role");
          return Ok(_context.Roles.ToList<UserRoleModel>());
        } else {
          return Ok(SendResult.SendError("You must login."));
        }
      } catch (Exception error) {
        return Ok(SendResult.SendError(error.ToString()));
      }
    }
  }
}