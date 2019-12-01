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
  public class TransactionController : ControllerBase {
    private readonly MyDatabaseContext _context;
    private IUserService _userService;
    private IHttpContextAccessor _httpContextAccessor;
    private readonly IEventService _eventService;
    private string menu = "Transaction";
    public TransactionController (
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

    [HttpPost("createTransaction")]
    public IActionResult CreateTransaction ([FromBody] TransactionModel request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "create");
        if (currentUser != null) {
          request.Created_by = currentUser.User_Id;
          int trans_Id = _context.Transactions.Max(t => t.Trans_Id);
          request.Trans_Id = trans_Id + 1;
          request.Date = DateTime.Now;
          _context.Add(request);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_CREATE, request.Trans_Id.ToString(), "Transaction");
          return Ok(request);
        } else {
          return Ok(SendResult.SendError("You don`t have create permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t create new Transaction"));
      }
    }

    [HttpPost("getTransaction")]
    public IActionResult getTransaction ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          TransactionModel model = _context.Transactions.FirstOrDefault(p => p.Tr_Id == id);
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You don`t have read permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t get Transaction info"));
      }
    }

    [HttpPost("getTransactions")]
    public IActionResult getTransactions () {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          return Ok(_context.Transactions.ToList<TransactionModel>());
        } else {
          return Ok(SendResult.SendError("You don`t have read permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t get Transaction list"));
      }
    }

    [HttpPost("getTransactionsByType")]
    public IActionResult getTransactionsByType ([FromBody]JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          string type = request["type"].Value<string>();
          List<TransactionModel> lists = null;
          if (type == "debit") {
            lists = _context.Transactions.Where<TransactionModel>(t => t.Credit == 0).ToList();
          } else {
            lists = _context.Transactions.Where<TransactionModel>(t => t.Debit == 0).ToList();
          }
          return Ok(lists);
        } else {
          return Ok(SendResult.SendError("You don`t have read permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t get Transaction list"));
      }
    }

    [HttpPost("updateTransaction")]
    public IActionResult updateTransaction ([FromBody] TransactionModel request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "update");
        if (currentUser != null) {
          TransactionModel model = _context.Transactions.FirstOrDefault(p => p.Tr_Id == request.Tr_Id);
          model.ledger_Code = request.ledger_Code;
          model.Debit = request.Debit;
          model.Credit = request.Credit;
          model.Gl_type = request.Gl_type;
          model.Remarks = request.Remarks;
          model.Narration = request.Narration;
          model.Status = request.Status;
          model.Vouc_No = request.Vouc_No;
          model.Fiscal = request.Fiscal;
          model.Date = request.Date;
          model.tran_type = request.tran_type;
          model.branch_id = request.branch_id;
          model.project_id = request.project_id;
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_UPDATE, model.Trans_Id.ToString(), "Transaction");
          return Ok(model);
        } else {
          return Ok(SendResult.SendError("You don`t have update permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t update Transaction info"));
      }
    }

    [HttpPost("removeTransaction")]
    public IActionResult removeTransaction ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "delete");
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          TransactionModel model = _context.Transactions.FirstOrDefault(p => p.Tr_Id == id);
          string name = model.Trans_Id.ToString();
          _context.Transactions.Remove(model);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_REMOVE, name, "Transaction");
          return Ok(true);
        } else {
          return Ok(SendResult.SendError("You don`t have delete permision."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`remove Transaction"));
      }
    }
  }
}