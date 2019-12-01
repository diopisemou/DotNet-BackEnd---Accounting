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
  public class ChartOfAccountController : ControllerBase {
    private readonly MyDatabaseContext _context;
    private IUserService _userService;
    private IHttpContextAccessor _httpContextAccessor;
    private readonly IEventService _eventService;
    private string menu = "Product & Service";
    public ChartOfAccountController (
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

    [HttpPost("createChartOfAccount")]
    public IActionResult CreateChartOfAccount ([FromBody] ChartOfAccountModel request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "create");
        if (currentUser != null) {
          request.Created_by = currentUser.User_Id;
          request.created_date = DateTime.Now;
          _context.Add(request);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_CREATE, request.Ledger_name, "ChartOfAccount");
          return Ok(request);
        } else {
          return Ok(SendResult.SendError("You don`t have create permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t create new ChartOfAccount"));
      }
    }

    [HttpPost("getChartOfAccount")]
    public IActionResult getChartOfAccount ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          ChartOfAccountModel model = _context.ChartOfAccounts.FirstOrDefault(p => p.id == id);
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You don`t have read permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t get ChartOfAccount info"));
      }
    }

    [HttpPost("getChartOfAccounts")]
    public IActionResult getChartOfAccounts () {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          return Ok(_context.ChartOfAccounts.ToList<ChartOfAccountModel>());
        } else {
          return Ok(SendResult.SendError("You don`t have read permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t get ChartOfAccount list"));
      }
    }

    [HttpPost("updateChartOfAccount")]
    public IActionResult updateChartOfAccount ([FromBody] ChartOfAccountModel request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "update");
        if (currentUser != null) {
          ChartOfAccountModel model = _context.ChartOfAccounts.FirstOrDefault(p => p.id == request.id);
          model.Mast_Code = request.Mast_Code;
          model.ledger_Code = request.ledger_Code;
          model.ledger_Group = request.ledger_Group;
          model.Ledger_name= request.Ledger_name;
          model.is_ledger = request.is_ledger;
          model.is_subledger= request.is_subledger;
          model.Status = request.Status;
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_UPDATE, model.Ledger_name, "ChartOfAccount");
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You don`t have update permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t update ChartOfAccount info"));
      }
    }

    [HttpPost("removeChartOfAccount")]
    public IActionResult removeChartOfAccount ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "delete");
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          ChartOfAccountModel model = _context.ChartOfAccounts.FirstOrDefault(p => p.id == id);
          string ledgerName = model.Ledger_name;
          _context.ChartOfAccounts.Remove(model);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_REMOVE, ledgerName, "ChartOfAccount");
          return Ok(true);
        } else {
          return Ok(SendResult.SendError("You don`t have delete permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`remove ChartOfAccount"));
      }
    }
  }
}