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

  [Route("api/[controller]")]
  [Authorize]
  [ApiController]
  public class CustomerController : ControllerBase {
    private readonly MyDatabaseContext _context;
    private IUserService _userService;
    private IHttpContextAccessor _httpContextAccessor;
    private readonly IEventService _eventService;
    private string menu = "Customer";
    public CustomerController (
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

    [HttpPost("createCustomer")]
    public IActionResult CreateCustomer ([FromBody] CustomerModel request) {
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
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_CREATE, request.cust_name, "Customer");
          return Ok(request);
        } else {
          return Ok(SendResult.SendError("You don`t have create permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t create new Customer"));
      }
    }

    [HttpPost("getCustomer")]
    public IActionResult getCustomer ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          CustomerModel model = _context.Customers.FirstOrDefault(p => p.cust_id == id);
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You don`t have read permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t get Customer info"));
      }
    }

    [HttpPost("getCustomers")]
    public IActionResult getCustomers () {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          return Ok(_context.Customers.ToList<CustomerModel>());
        } else {
          return Ok(SendResult.SendError("You don`t have read permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t get Customer list"));
      }
    }

    [HttpPost("updateCustomer")]
    public IActionResult updateCustomer ([FromBody] CustomerModel request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "update");
        if (currentUser != null) {
          CustomerModel model = _context.Customers.FirstOrDefault(p => p.cust_id == request.cust_id);
          model.cust_name = request.cust_name;
          model.address = request.address;
          model.email = request.email;
          model.region = request.region;
          model.mob = request.mob;
          model.type = request.type;
          model.tel = request.tel;
          model.netdues = request.netdues;
          model.s_agent = request.s_agent;
          model.ledger = request.ledger;
          model.status = request.status;
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_UPDATE, model.cust_name, "Customer");
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You don`t have update permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t update Customer info"));
      }
    }

    [HttpPost("removeCustomer")]
    public IActionResult removeCustomer ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "delete");
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          CustomerModel model = _context.Customers.FirstOrDefault(p => p.cust_id == id);
          string name = model.cust_name;
          _context.Customers.Remove(model);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_REMOVE, name, "Customer");
          return Ok(true);
        } else {
          return Ok(SendResult.SendError("You don`t have delete permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`remove Customer"));
      }
    }
  }
}
