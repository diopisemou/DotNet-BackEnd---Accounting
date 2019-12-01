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
  public class CompanyController : ControllerBase {
    private readonly MyDatabaseContext _context;
    private IUserService _userService;
    private IHttpContextAccessor _httpContextAccessor;
    private readonly IEventService _eventService;
    private string menu = "Company";
    public CompanyController (
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

    [HttpPost("createCompany")]
    public IActionResult CreateCompany ([FromBody] CompanyModel company) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "create");
        if (currentUser != null) {
          company.Createdby = currentUser.User_Id;
          _context.Add(company);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_CREATE, company.Name, "Company");
          return Ok(company);
        } else {
          return Ok(SendResult.SendError("You must login."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t create new complany"));
      }
    }
    [HttpPost("getUserCompanys")]
    public IActionResult getUserCompanys () {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          var result = _context.Companies.Where(c => c.Createdby == currentUser.User_Id).ToList<CompanyModel>();
          return Ok(result);
        } else {
          return Ok(SendResult.SendError("You must login."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        _eventService.SaveEvent(0, EventUserLog.EVENT_READ, EventUserLog.ACTIVITY_FAILED, "Company");
        return BadRequest(SendResult.SendError("You don`t create new complany"));
      }
    }
    [HttpPost("getCompany")]
    public IActionResult GetComany ([FromBody] int id) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "read");
        if (currentUser != null) {
          CompanyModel result = _context.Companies.FirstOrDefault<CompanyModel>(c => c.ID == id);
          return Ok(result);
        } else {
          return Ok(SendResult.SendError("You must login."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t create new complany"));
      }
    }
    [HttpPost("getAllCompanys")]
    public IActionResult getAllCompanys () {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "create");
        if (currentUser != null) {
          var result = _context.Companies.ToList<CompanyModel>();
          return Ok(result);
        } else {
          return Ok(SendResult.SendError("You must login."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t create new complany"));
      }
    }
    [HttpPost("updateCompany")]
    public IActionResult UpdateCompany ([FromBody] CompanyModel company) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "update");
        if (currentUser != null) {
          CompanyModel result = _context.Companies.FirstOrDefault<CompanyModel>(c => c.ID == company.ID);
          result.Name = company.Name;
          result.Address = company.Address;
          result.Pan = company.Pan;
          result.Regd = company.Regd;
          result.Telephone = company.Telephone;
          result.Mobile = company.Mobile;
          result.Email = company.Email;
          result.Website = company.Website;
          result.StateCity = company.StateCity;
          result.Country = company.Country;
          result.Logo = company.Logo;
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_UPDATE, result.Name, "Company");
          return Ok(result);
        } else {
          return Ok(SendResult.SendError("You must login."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t create new complany"));
      }
    }
    [HttpPost("updateLogo")]
    public IActionResult UpdateLogo ([FromBody] CompanyModel company) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "update");
        if (currentUser != null) {
          CompanyModel result = _context.Companies.FirstOrDefault<CompanyModel>(c => c.ID == company.ID);

          result.Logo = company.Logo;
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_UPDATE, result.Name + " Logo", "Company");
          return Ok(result);
        } else {
          return Ok(SendResult.SendError("You must login."));
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t create new complany"));
      }
    }
    [HttpPost("removeCompany")]
    public IActionResult getRemoveCompany ([FromBody] JObject data) {
      try {
        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, menu, "remove");
        if (currentUser != null) {
          int id = data["id"].Value<int>();
          CompanyModel result = _context.Companies.FirstOrDefault<CompanyModel>(c => c.ID == id);
          string name = result.Name;
          _context.Companies.Remove(result);
          _context.SaveChanges();
          _eventService.SaveEvent(currentUser.User_Id, EventUserLog.EVENT_REMOVE, name, "Company");
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