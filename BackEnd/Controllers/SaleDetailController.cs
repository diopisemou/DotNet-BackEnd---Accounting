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
  public class SaleDetailController : ControllerBase {
    private readonly MyDatabaseContext _context;
    private IUserService _userService;
    private IHttpContextAccessor _httpContextAccessor;
    private readonly IEventService _eventService;
    private string menu = "SaleDetail";
    public SaleDetailController (
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

    [HttpPost("createSaleDetail")]
    public IActionResult CreateSaleDetail ([FromBody] SaleDetailModel request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "create");
        if (currentUser != null) {
          request.date = DateTime.Now;
          request.created_by = currentUser.User_Id;
          _context.Add(request);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_CREATE, request.inv_no.ToString(), "SaleDetail");
          return Ok(request);
        } else {
          return Ok(SendResult.SendError("You don`t have create permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t create new SaleDetail"));
      }
    }

    [HttpPost("getSaleDetail")]
    public IActionResult getSaleDetails ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          SaleDetailModel model = _context.SaleDetails.FirstOrDefault(s => s.id == id);
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You don`t have read permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t get SaleDetail info"));
      }
    }

    [HttpPost("getSaleDetails")]
    public IActionResult getSaleDetails () {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          return Ok(_context.SaleDetails.ToList<SaleDetailModel>());
        } else {
          return Ok(SendResult.SendError("You don`t have read permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t get SaleDetails list"));
      }
    }

    [HttpPost("getSaleDetailsByInv")]
    public IActionResult getSaleDetailsByInv ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          return Ok(_context.SaleDetails.Where(s=>s.inv_no==id).ToList<SaleDetailModel>());
        } else {
          return Ok(SendResult.SendError("You don`t have read permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t get SaleDetails list"));
      }
    }

    [HttpPost("updateSaleDetail")]
    public IActionResult updateSaleDetails ([FromBody] SaleDetailModel request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "update");
        if (currentUser != null) {
          SaleDetailModel model = _context.SaleDetails.FirstOrDefault(p => p.id == request.id);
          model.date = request.date;
          model.inv_no = request.inv_no;
          model.item_id = request.item_id;
          model.unit_id = request.unit_id;
          model.price = request.price;
          model.qty = request.qty;
          model.total = request.total;
          model.discount = request.discount;
          model.tax = request.tax;
          model.net_amount = request.net_amount;
          model.status = request.status;
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_UPDATE, model.inv_no.ToString(), "SaleDetail");
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You don`t have update permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t update SaleDetail info"));
      }
    }

    [HttpPost("removeSaleDetail")]
    public IActionResult removeSaleDetails ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "delete");
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          SaleDetailModel model = _context.SaleDetails.FirstOrDefault(p => p.id == id);
          string name = model.inv_no.ToString();
          _context.SaleDetails.Remove(model);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_REMOVE, name, "SaleDetails");
          return Ok(true);
        } else {
          return Ok(SendResult.SendError("You don`t have delete permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`remove SaleDetail"));
      }
    }
  }
}