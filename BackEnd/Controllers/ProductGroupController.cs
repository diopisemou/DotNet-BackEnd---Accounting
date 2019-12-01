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
  public class ProductGroupController : ControllerBase {
    private readonly MyDatabaseContext _context;
    private IUserService _userService;
    private IHttpContextAccessor _httpContextAccessor;
    private readonly IEventService _eventService;
    private string menu = "Product Group";
    public ProductGroupController (
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

    [HttpPost("createProductGroup")]
    public IActionResult CreateProductGroup ([FromBody] ProductGroupModel request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "create");
        if (currentUser != null) {
          int ledger_code = _context.ChartOfAccounts.Max(t => t.ledger_Code);
          request.created_by = currentUser.User_Id;
          request.created_date = DateTime.Now;
          request.ledger_code = ledger_code + 1;
          ChartOfAccountModel coa = new ChartOfAccountModel();
          coa.Created_by = currentUser.User_Id;
          coa.Ledger_name = request.group_name;
          coa.ledger_Code = ledger_code + 1;
          _context.Add(request);
          _context.Add(coa);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_CREATE, request.group_name, "ProductGroup");
          return Ok(request);
        } else {
          return Ok(SendResult.SendError("You don`t have create permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t create new ProductGroup"));
      }
    }

    [HttpPost("getProductGroup")]
    public IActionResult getProductGroup ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          ProductGroupModel model = _context.ProductGroups.FirstOrDefault(p => p.id == id);
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You don`t have read permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t get ProductGroup info"));
      }
    }

    [HttpPost("getProductGroups")]
    public IActionResult getProductGroups () {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          return Ok(_context.ProductGroups.ToList<ProductGroupModel>());
        } else {
          return Ok(SendResult.SendError("You don`t have read permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t get ProductGroup list"));
      }
    }

    [HttpPost("updateProductGroup")]
    public IActionResult updateProductGroup ([FromBody] ProductGroupModel request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "update");
        if (currentUser != null) {
          ProductGroupModel model = _context.ProductGroups.FirstOrDefault(p => p.id == request.id);
          model.group_name = request.group_name;
          model.ledger_code = request.ledger_code;
          model.status = request.status;
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_UPDATE, model.group_name, "ProductGroup");
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You don`t have update permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t update ProductGroup info"));
      }
    }

    [HttpPost("removeProductGroup")]
    public IActionResult removeProductGroup ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "delete");
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          ProductGroupModel model = _context.ProductGroups.FirstOrDefault(p => p.id == id);
          string name = model.group_name;
          _context.ProductGroups.Remove(model);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_REMOVE, name, "ProductGroup");
          return Ok(true);
        } else {
          return Ok(SendResult.SendError("You don`t have delete permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`remove ProductGroup"));
      }
    }
  }
}