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
  public class BankController : ControllerBase {
    private readonly MyDatabaseContext _context;
    private IUserService _userService;
    private IHttpContextAccessor _httpContextAccessor;
    private readonly IEventService _eventService;
    private string menu = "Bank";
    public BankController (
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

    [HttpPost("createBank")]
    public IActionResult CreateBank ([FromBody] BankModel request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "create");
        if (currentUser != null) {
          request.created_by = currentUser.User_Id;
          request.created_date = DateTime.Now;
          _context.Add(request);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_CREATE, "Bank");
          return Ok(request);
        } else {
          return Ok(SendResult.SendError("You don`t have create permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t create new Bank"));
      }
    }

    [HttpPost("getBank")]
    public IActionResult getBank ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          BankModel model = _context.Banks.FirstOrDefault(p => p.id == id);
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You don`t have read permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t get Bank info"));
      }
    }

    [HttpPost("getBanks")]
    public IActionResult getBanks () {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          return Ok(_context.Banks.ToList<BankModel>());
        } else {
          return Ok(SendResult.SendError("You don`t have read permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t get Bank list"));
      }
    }

    [HttpPost("updateBank")]
    public IActionResult updateBank ([FromBody] BankModel request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "update");
        if (currentUser != null) {
          BankModel model = _context.Banks.FirstOrDefault(p => p.id == request.id);
          model.name = request.name;
          model.acc_type = request.acc_type;
          model.acc_number = request.acc_number;
          model.branch= request.branch;
          model.swift_code = request.swift_code;
          model.level_value = request.level_value;
          model.url = request.url;
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_UPDATE, "Bank");
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You don`t have update permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t update Bank info"));
      }
    }

    [HttpPost("removeBank")]
    public IActionResult removeBank ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "delete");
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          BankModel model = _context.Banks.FirstOrDefault(p => p.id == id);
          _context.Banks.Remove(model);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_REMOVE, "Bank");
          return Ok(true);
        } else {
          return Ok(SendResult.SendError("You don`t have delete permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`remove Bank"));
      }
    }
  }
}