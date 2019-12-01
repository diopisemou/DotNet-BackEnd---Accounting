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

namespace BackEnd.Controllers
{
  [Authorize]
  [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
    private readonly MyDatabaseContext _context;
    private IUserService _userService;
    private IHttpContextAccessor _httpContextAccessor;
    private readonly IEventService _eventService;
    private string menu = "Supplier";
    public SupplierController (
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

    [HttpPost("createSupplier")]
    public IActionResult CreateSupplier ([FromBody] SupplierModel request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "create");
        if (currentUser != null) {
          int ledger_code = _context.ChartOfAccounts.Max(t => t.ledger_Code);
          request.created_by = currentUser.User_Id;
          request.created_date = DateTime.Now;
          request.ledger = ledger_code + 1;
          ChartOfAccountModel coa = new ChartOfAccountModel();
          coa.Created_by = currentUser.User_Id;
          coa.Ledger_name = request.cust_name;
          coa.ledger_Code = ledger_code + 1;
          _context.Add(request);
          _context.Add(coa);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_CREATE, request.cust_name, "Supplier");
          return Ok(request);
        } else {
          return Ok(SendResult.SendError("You don`t have create permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t create new Supplier"));
      }
    }

    [HttpPost("getSupplier")]
    public IActionResult getSupplier ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          SupplierModel model = _context.Suppliers.FirstOrDefault(p => p.cust_id == id);
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You don`t have read permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t get Supplier info"));
      }
    }

    [HttpPost("getSuppliers")]
    public IActionResult getSuppliers () {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          return Ok(_context.Suppliers.ToList<SupplierModel>());
        } else {
          return Ok(SendResult.SendError("You don`t have read permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t get Supplier list"));
      }
    }

    [HttpPost("updateSupplier")]
    public IActionResult updateSupplier ([FromBody] SupplierModel request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "update");
        if (currentUser != null) {
          SupplierModel model = _context.Suppliers.FirstOrDefault(p => p.cust_id == request.cust_id);
          model.cust_name = request.cust_name;
          model.address = request.address;
          model.email = request.email;
          model.region = request.region;
          model.mob = request.mob;
          model.type = request.type;
          model.tel = request.tel;
          model.netdues = request.netdues;
          model.ledger = request.ledger;
          model.status = request.status;
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_UPDATE, model.cust_name, "Supplier");
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You don`t have update permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t update Supplier info"));
      }
    }

    [HttpPost("removeSupplier")]
    public IActionResult removeSupplier ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "delete");
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          SupplierModel model = _context.Suppliers.FirstOrDefault(p => p.cust_id == id);
          string name = model.cust_name;
          _context.Suppliers.Remove(model);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_REMOVE, name, "Supplier");
          return Ok(true);
        } else {
          return Ok(SendResult.SendError("You don`t have delete permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`remove Supplier"));
      }
    }
  }
}