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
  public class SaleController : ControllerBase {
    private readonly MyDatabaseContext _context;
    private IUserService _userService;
    private IHttpContextAccessor _httpContextAccessor;
    private readonly IEventService _eventService;
    private string menu = "Sale";
    public SaleController (
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

    [HttpPost("createSale")]
    public IActionResult CreateSales ([FromBody] SaleModel request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "create");
        if (currentUser != null) {
          int inv_no = _context.Sales.Any()?_context.Sales.Max(s => s.inv_no):1;
          request.inv_no = inv_no + 1;
          request.date = DateTime.Now;
          request.created_by = currentUser.User_Id;
          request.updated = "False";
          _context.Add(request);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_CREATE, request.inv_no.ToString(), "Sale");
          return Ok(request);
        } else {
          return Ok(SendResult.SendError("You don`t have create permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t create new Sales"));
      }
    }

    [HttpPost("getSale")]
    public IActionResult getSales ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          SaleModel model = _context.Sales.FirstOrDefault(s => s.id == id);
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You don`t have read permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t get Sales info"));
      }
    }

    [HttpPost("getSales")]
    public IActionResult getSales () {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          return Ok(_context.Sales.ToList<SaleModel>());
        } else {
          return Ok(SendResult.SendError("You don`t have read permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t get Sales list"));
      }
    }

    [HttpPost("updateSale")]
    public IActionResult updateSales ([FromBody] SaleModel request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "update");
        if (currentUser != null) {
          SaleModel model = _context.Sales.FirstOrDefault(p => p.id == request.id);
          model.date= request.date;
          model.inv_no = request.inv_no;
          model.order_ref= request.order_ref;
          model.cust_id= request.cust_id;
          model.taxable= request.taxable;
          model.tax= request.tax;
          model.discount= request.discount;
          model.netamount= request.netamount;
          model.net_dues= request.net_dues;
          model.status= request.status;
          model.updated= request.updated;
          model.agent_id= request.agent_id;
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_UPDATE, model.inv_no.ToString(), "Sale");
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You don`t have update permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t update Sales info"));
      }
    }

    [HttpPost("removeSale")]
    public IActionResult removeSales ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "delete");
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          SaleModel model = _context.Sales.FirstOrDefault(p => p.id == id);
          int inv_no = model.inv_no;
          SaleDetailModel[] models = _context.SaleDetails.Where(sd => sd.inv_no == inv_no).ToArray();
          _context.Sales.Remove(model);
          _context.SaleDetails.RemoveRange(models);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_REMOVE, inv_no.ToString(), "Sale");
          return Ok(true);
        } else {
          return Ok(SendResult.SendError("You don`t have delete permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`remove Sales"));
      }
    }
  }
}