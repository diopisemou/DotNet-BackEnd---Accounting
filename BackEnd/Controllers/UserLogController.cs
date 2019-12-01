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
  public class UserLogController : ControllerBase {
    private readonly MyDatabaseContext _context;
    private IUserService _userService;
    private IHttpContextAccessor _httpContextAccessor;
    private readonly IEventService _eventService;
    private string menu = "Product Group";
    public UserLogController (
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

    [HttpPost("createUserLog")]
    public IActionResult CreateUserLog ([FromBody] UserLogModel request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "create");
        if (currentUser != null) {
          request.Username = currentUser.User_Id;
          request.Datetime = DateTime.Now;
          _context.Add(request);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_CREATE, EventUserLog.ACTIVITY_SUCCESS, "UserLog");
          return Ok(request);
        } else {
          return Ok(SendResult.SendError("You don`t have create permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t create new UserLog"));
      }
    }

    [HttpPost("getUserLog")]
    public IActionResult getUserLog ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          UserLogModel model = _context.Userlogs.FirstOrDefault(p => p.SN == id);
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You don`t have read permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t get UserLog info"));
      }
    }

    [HttpPost("getUserLogs")]
    public IActionResult getUserLogs () {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          return Ok(_context.Userlogs.ToList<UserLogModel>());
        } else {
          return Ok(SendResult.SendError("You don`t have read permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t get UserLog list"));
      }
    }

    [HttpPost("updateUserLog")]
    public IActionResult updateUserLog ([FromBody] UserLogModel request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "update");
        if (currentUser != null) {
          UserLogModel model = _context.Userlogs.FirstOrDefault(p => p.SN == request.SN);
          model.Username = request.Username;
          model.Datetime = request.Datetime;
          model.Activity= request.Activity;
          model.Type= request.Type;
          model.Event= request.Event;
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_UPDATE, EventUserLog.ACTIVITY_SUCCESS, "UserLog");
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You don`t have update permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t update UserLog info"));
      }
    }

    [HttpPost("removeUserLog")]
    public IActionResult removeUserLog ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "delete");
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          UserLogModel model = _context.Userlogs.FirstOrDefault(p => p.SN == id);
          _context.Userlogs.Remove(model);
          _context.SaveChanges();
          //_eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_REMOVE, EventUserLog.ACTIVITY_SUCCESS, "UserLog");
          return Ok(true);
        } else {
          return Ok(SendResult.SendError("You don`t have delete permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`remove UserLog"));
      }
    }
  }
}