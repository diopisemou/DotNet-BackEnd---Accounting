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
  public class PurchaseController : ControllerBase {
    private readonly MyDatabaseContext _context;
    private IUserService _userService;
    private IHttpContextAccessor _httpContextAccessor;
    private readonly IEventService _eventService;
    private string menu = "Purchase";
    public PurchaseController (
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

    [HttpPost("createPurchase")]
    public IActionResult CreatePurchases ([FromBody] PurchaseModel request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "create");
        if (currentUser != null) {
          int inv_no = _context.Purchases.Any() ? _context.Purchases.Max(s => s.inv_no) : 1;
          request.inv_no = inv_no + 1;
          request.date = DateTime.Now;
          request.created_by = currentUser.User_Id;
          request.updated = "False";
          _context.Add(request);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_CREATE, request.inv_no.ToString(), "Purchase");
          return Ok(request);
        } else {
          return Ok(SendResult.SendError("You don`t have create permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t create new Purchases"));
      }
    }

    [HttpPost("getPurchase")]
    public IActionResult getPurchases ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          PurchaseModel model = _context.Purchases.FirstOrDefault(s => s.id == id);
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You don`t have read permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t get Purchases info"));
      }
    }

    [HttpPost("getPurchases")]
    public IActionResult getPurchases () {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          return Ok(_context.Purchases.ToList<PurchaseModel>());
        } else {
          return Ok(SendResult.SendError("You don`t have read permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t get Purchases list"));
      }
    }

    [HttpPost("updatePurchase")]
    public IActionResult updatePurchases ([FromBody] PurchaseModel request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "update");
        if (currentUser != null) {
          PurchaseModel model = _context.Purchases.FirstOrDefault(p => p.id == request.id);
          model.date = request.date;
          model.inv_no = request.inv_no;
          model.order_ref = request.order_ref;
          model.supp_id = request.supp_id;
          model.taxable = request.taxable;
          model.tax = request.tax;
          model.discount = request.discount;
          model.netamount = request.netamount;
          model.net_dues = request.net_dues;
          model.status = request.status;
          model.updated = request.updated;
          model.agent_id = request.agent_id;
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_UPDATE, model.inv_no.ToString(), "Purchase");
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You don`t have update permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t update Purchases info"));
      }
    }

    [HttpPost("removePurchase")]
    public IActionResult removePurchases ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "delete");
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          PurchaseModel model = _context.Purchases.FirstOrDefault(p => p.id == id);
          int inv_no = model.inv_no;
          PurchaseDetailModel[] models = _context.PurchaseDetails.Where(sd => sd.inv_no == inv_no).ToArray();
          _context.Purchases.Remove(model);
          _context.PurchaseDetails.RemoveRange(models);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_REMOVE, inv_no.ToString(), "Purchase");
          return Ok(true);
        } else {
          return Ok(SendResult.SendError("You don`t have delete permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`remove Purchases"));
      }
    }
  }
}