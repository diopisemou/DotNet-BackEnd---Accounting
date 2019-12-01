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
  public class FiscalController : ControllerBase {
    private readonly MyDatabaseContext _context;
    private IUserService _userService;
    private IHttpContextAccessor _httpContextAccessor;
    private readonly IEventService _eventService;
    private string menu = "Fiscal Year";
    public FiscalController (
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

    [HttpPost("createFiscal")]
    public IActionResult CreateFiscal ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "create");
        if (currentUser != null) {
          string[] fromdate = request["fromdate"].Value<string>().Split("-");
          string[] todate = request["todate"].Value<string>().Split("-");
          FiscalModel model = new FiscalModel();
          model.Fiscalyear = request["fiscalyear"].Value<string>();
          model.Fromdate = new DateTime(
            System.Convert.ToInt32(fromdate[0]),
            System.Convert.ToInt32(fromdate[1]),
            System.Convert.ToInt32(fromdate[2])
            );
          model.Todate = new DateTime(
            System.Convert.ToInt32(todate[0]),
            System.Convert.ToInt32(todate[1]),
            System.Convert.ToInt32(todate[2])
            );
          model.Status = request["status"].Value<bool>();
          model.Created_by = currentUser.User_Id;
          model.Created_date = DateTime.Now;
          if (model.Status) {
            FiscalModel[] activeFiscals = _context.Fiscals.Where(f => f.Status).ToArray();
            foreach (FiscalModel activeFiscal in activeFiscals) {
              activeFiscal.Status = false;
            }
          }
          _context.Add(model);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_CREATE, model.Fiscalyear, "Fiscal");
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You must login."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t create new complany"));
      }
    }

    [HttpPost("getFiscal")]
    public IActionResult getFiscal ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          FiscalModel model = _context.Fiscals.FirstOrDefault(f => f.Fiscal_Id == id);
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You must login."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t create new complany"));
      }
    }

    [HttpPost("getFiscals")]
    public IActionResult getFiscals () {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          return Ok(_context.Fiscals.ToList<FiscalModel>());
        } else {
          return Ok(SendResult.SendError("You must login."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t create new complany"));
      }
    }

    [HttpPost("updateFiscal")]
    public IActionResult updateFiscal ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "update");
        if (currentUser != null) {
          FiscalModel model = _context.Fiscals.FirstOrDefault(f => f.Fiscal_Id == request["fiscal_Id"].Value<int>());
          string[] fromdate = request["fromdate"].Value<string>().Split("-");
          string[] todate = request["todate"].Value<string>().Split("-");
          model.Fiscalyear = request["fiscalyear"].Value<string>();
          model.Fromdate = new DateTime(
            System.Convert.ToInt32(fromdate[0]),
            System.Convert.ToInt32(fromdate[1]),
            System.Convert.ToInt32(fromdate[2])
            );
          model.Todate = new DateTime(
            System.Convert.ToInt32(todate[0]),
            System.Convert.ToInt32(todate[1]),
            System.Convert.ToInt32(todate[2])
            );
          model.Status = request["status"].Value<bool>();
          model.Created_by = currentUser.User_Id;
          model.Created_date = DateTime.Now;
          if (model.Status) {
            FiscalModel[] activeFiscals = _context.Fiscals.Where(f => f.Status).ToArray();
            foreach(FiscalModel activeFiscal in activeFiscals) {
              activeFiscal.Status = false;
            }
          }
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_UPDATE, model.Fiscalyear, "Fiscal");
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You must login."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t create new complany"));
      }
    }

    [HttpPost("removeFiscal")]
    public IActionResult removeFiscal ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "delete");
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          FiscalModel model = _context.Fiscals.FirstOrDefault(f => f.Fiscal_Id == id);
          string fiscal = model.Fiscalyear;
          _context.Fiscals.Remove(model);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_REMOVE, fiscal, "Fiscal");
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
