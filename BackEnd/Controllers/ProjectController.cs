using BackEnd.Data;
using BackEnd.Helpers;
using BackEnd.Helpers.CustomAttribute;
using BackEnd.Helpers.CustomeAttribute;
using BackEnd.Models;
using BackEnd.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Controllers {
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  [ServiceFilter(typeof(HasPermission))]
  //[MenuName("project")]
  public class ProjectController : ControllerBase {
    private readonly MyDatabaseContext _context;

    public ProjectController (
        MyDatabaseContext context
        ) {
      _context = context;
    }

    [HttpPost("[action]")]
    [Permission(RequiredPermissionOption.Create)]
    public IActionResult CreateProject ([FromBody] ProjectModel request) {
      try {
        //request.CreatedBy = request.CreatedBy;
        request.CreatedDate = DateTime.Now;
        _context.Add(request);
        _context.SaveChanges();
        return Ok(request);
      } catch (Exception ex) {
        Console.WriteLine(ex.Message);
        return BadRequest(SendResult.SendError("You don`t create new project"));
      }
    }

    [HttpPost("[action]")]
    [Permission(RequiredPermissionOption.Update)]
    public IActionResult UpdateProject ([FromBody] ProjectModel request) {
      try {
        _context.Update(request);
        _context.SaveChanges();
        return Ok(request);
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t update Project info"));
      }
    }

    [HttpPost("[action]/{id}")]
    [Permission(RequiredPermissionOption.Delete)]
    public IActionResult RemoveProject (int id) {
      try {
        ProjectModel project = _context.Projects.FirstOrDefault(x => x.Id == id);
        _context.Remove(project);
        _context.SaveChanges();
        return Ok(true);
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t delete project info"));
      }
    }

    [HttpPost("[action]/{id}")]
    [Permission(RequiredPermissionOption.Read)]
    public IActionResult GetProjectById (int id, [BindNever] GeneralData data) {
      try {
        ProjectModel project = _context.Projects.FirstOrDefault(x => x.Id == id);
        return Ok(project);
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t get project list"));
      }
    }

    [HttpPost("[action]")]
    [Permission(RequiredPermissionOption.Read)]
    public IActionResult GetProjects () {
      try {
        List<ProjectModel> projects = _context.Projects.ToList();
        return Ok(projects);
      } catch (Exception error) {
        Console.WriteLine(error);
        return BadRequest(SendResult.SendError("You don`t get project list"));
      }
    }
  }
}
