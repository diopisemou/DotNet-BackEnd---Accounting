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
  public class BranchController : ControllerBase {
    private readonly MyDatabaseContext _context;
    private IUserService _userService;
    private IHttpContextAccessor _httpContextAccessor;
    private IEventService _eventService;
    public BranchController (
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

    [HttpPost("createBranch")]
    public IActionResult CreateBranch ([FromBody] Branch branch) {
      try {
        User currentUser = _userService.CheckAdmin(_httpContextAccessor.HttpContext.User);
        if (currentUser != null) {
          branch.Created_date = DateTime.Now;
          _context.Add(branch);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_CREATE, branch.Branch_Name, "Branch");
          return Ok(branch);
        } else {
          return Ok(SendResult.SendError("You must login."));
        }
      } catch (Exception error) {
        Console.Write(error);
        return BadRequest();
      }
    }

    [HttpPost("getBranch")]
    public IActionResult getBranch ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckAdmin(_httpContextAccessor.HttpContext.User);
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          Branch branch = _context.Branches.FirstOrDefault(b => b.Branch_Id == id);
          return Ok(branch);
        } else {
          return Ok(SendResult.SendError("You must login."));
        }
      } catch (Exception error) {
        Console.Write(error);
        return BadRequest();
      }
    }

    [AllowAnonymous]
    [HttpPost("getAllBranch")]
    public ActionResult<IEnumerable<Branch>> Create () {
      try {
        var items = _context.Branches.ToList<Branch>();
        return items;
      } catch (Exception error) {
        throw error;
      }

    }

    [HttpPost("updateBranch")]
    public IActionResult updateBranch ([FromBody] Branch branch) {
      try {
        User currentUser = _userService.CheckAdmin(_httpContextAccessor.HttpContext.User);
        if (currentUser != null) {
          Branch model = _context.Branches.FirstOrDefault(b => b.Branch_Id == branch.Branch_Id);
          model.Address = branch.Address;
          model.Branch_Code = branch.Branch_Code;
          model.Branch_Name = branch.Branch_Name;
          model.Email_Id = branch.Email_Id;
          model.Telephone_No = branch.Telephone_No;
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_UPDATE, model.Branch_Name, "Branch");
          return Ok(branch);
        } else {
          return Ok(SendResult.SendError("You must login."));
        }
      } catch (Exception error) {
        Console.Write(error);
        return BadRequest();
      }
    }

    [HttpPost("removeBranch")]
    public IActionResult removeBranch ([FromBody] JObject request) {
      try {
        User currentUser = _userService.CheckAdmin(_httpContextAccessor.HttpContext.User);
        if (currentUser != null) {
          int id = request["id"].Value<int>();
          Branch model = _context.Branches.FirstOrDefault(b => b.Branch_Id == id);
          string modelName = model.Branch_Name;
          _context.Branches.Remove(model);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_REMOVE, modelName, "Branch");
          return Ok(true);
        } else {
          return Ok(SendResult.SendError("You must login."));
        }
      } catch (Exception error) {
        Console.Write(error);
        return BadRequest();
      }
    }
  }
}