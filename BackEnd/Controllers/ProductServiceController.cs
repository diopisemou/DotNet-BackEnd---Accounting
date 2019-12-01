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
  public class ProductServiceController : ControllerBase {
    private readonly MyDatabaseContext _context;
    private IUserService _userService;
    private IHttpContextAccessor _httpContextAccessor;
    private readonly IEventService _eventService;
    private string menu = "Product & Service";
    public ProductServiceController (
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

    [HttpPost("createProductService")]
    public IActionResult CreateProductService ([FromBody] ProductServiceModel request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "create");
        if (currentUser != null) {
          int ledger_code = _context.ChartOfAccounts.Max(t => t.ledger_Code);
          request.created_by = currentUser.User_Id;
          request.created_date = DateTime.Now;
          ChartOfAccountModel coa = new ChartOfAccountModel();
          coa.Created_by = currentUser.User_Id;
          coa.Ledger_name = request.d_name;
          coa.ledger_Code = ledger_code + 1;
          coa.is_ledger = false;
          coa.is_subledger = true;
          _context.Add(request);
          _context.Add(coa);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_CREATE, request.d_name, "ProductService");
          return Ok(request);
        } else {
          return Ok(SendResult.SendError("You don`t have create permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t create new ProductService"));
      }
    }

    [HttpPost("getProductService")]
    public IActionResult getProductService ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          ProductServiceModel model = _context.ProductServices.FirstOrDefault(p => p.id == id);
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You don`t have read permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t get ProductService info"));
      }
    }

    [HttpPost("getProductServices")]
    public IActionResult getProductServices () {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          return Ok(_context.ProductServices.ToList<ProductServiceModel>());
        } else {
          return Ok(SendResult.SendError("You don`t have read permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t get ProductService list"));
      }
    }

    [HttpPost("updateProductService")]
    public IActionResult updateProductService ([FromBody] ProductServiceModel request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "update");
        if (currentUser != null) {
          ProductServiceModel model = _context.ProductServices.FirstOrDefault(p => p.id == request.id);
          model.p_type = request.p_type;
          model.p_name = request.p_name;
          model.d_name = request.d_name;
          model.p_group = request.p_group;
          model.qtyonhand = request.qtyonhand;
          model.p_price = request.p_price;
          model.s_price = request.s_price;
          model.i_account = request.i_account;
          model.e_account = request.e_account;
          model.reorder = request.reorder;
          model.barcode = request.barcode;
          model.istaxable = request.istaxable;
          model.taxtype = request.taxtype;
          model.status = request.status;
          model.image = request.image;
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_UPDATE, request.d_name, "ProductService");
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You don`t have update permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t update ProductService info"));
      }
    }

    [HttpPost("removeProductService")]
    public IActionResult removeProductService ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "delete");
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          ProductServiceModel model = _context.ProductServices.FirstOrDefault(p => p.id == id);
          string name = model.d_name;
          _context.ProductServices.Remove(model);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_REMOVE, name, "ProductService");
          return Ok(true);
        } else {
          return Ok(SendResult.SendError("You don`t have delete permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`remove ProductService"));
      }
    }
  }
}