using BackEnd.Data;
using BackEnd.Helpers;
using BackEnd.Helpers.CustomeAttribute;
using BackEnd.Models;
using BackEnd.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BackEnd.Helpers.CustomAttribute {
  public class HasPermission : ActionFilterAttribute {
    private readonly MyDatabaseContext _context;
    private IUserService _userService;
    private IHttpContextAccessor _httpContextAccessor;
    private readonly IEventService _eventService;

    private int userId;
    private string eventUserLog;
    private string menuName;
    private string loginfo;

    public HasPermission (
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

    public override void OnActionExecuting (ActionExecutingContext context) {
      var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
      if (descriptor != null) {
        var parameters = descriptor.MethodInfo.GetCustomAttributes(typeof(Permission), false).FirstOrDefault() as Permission;
        RequiredPermissionOption RequiredPermission = parameters.RequiredPermission;
        string type = string.Empty;

        switch (RequiredPermission) {
          case RequiredPermissionOption.Create:
            type = "create";
            eventUserLog = EventUserLog.EVENT_CREATE;
            break;
          case RequiredPermissionOption.Delete:
            type = "delete";
            eventUserLog = EventUserLog.EVENT_DELETE;
            break;
          case RequiredPermissionOption.Update:
            type = "update";
            eventUserLog = EventUserLog.EVENT_UPDATE;
            break;
          case RequiredPermissionOption.Read:
            type = "read";
            eventUserLog = EventUserLog.EVENT_READ;
            break;
          case RequiredPermissionOption.None:
            type = "none";
            break;
          default:
            break;
        }

        string controllerName = descriptor.ControllerName;

        try {
          MenuName menuName = descriptor.ControllerTypeInfo.GetCustomAttributes(typeof(MenuName), false).FirstOrDefault() as MenuName;

          if (menuName != null) {
            controllerName = menuName.menu;
          }

          this.menuName = controllerName;
        } catch (Exception ex) {

        }

        User currentUser = _userService.CheckUseRole(_httpContextAccessor.HttpContext.User, controllerName, type);
        if (currentUser == null) {
          string message = $"You don`t have {type} permision.";
          context.Result = new BadRequestObjectResult(message);
        } else {
          GeneralData generalData = new GeneralData {
            BranchId = currentUser.Branch_Id,
            CreatedBy = currentUser.Created_by,
            EmpId = currentUser.Emp_Id,
            Password = currentUser.Password,
            RollId = currentUser.Roll_Id,
            Status = currentUser.Status,
            Token = currentUser.Token,
            Useremail = currentUser.Useremail,
            UserId = currentUser.User_Id,
            Username = currentUser.Username
          };
          this.userId = currentUser.User_Id;
          context.ActionArguments.Add("data", generalData);
        }
      }
    }

    public override void OnResultExecuted (ResultExecutedContext context) {
      if (context.Exception == null) {
        _eventService.SaveEvent(this.userId, this.eventUserLog, EventUserLog.ACTIVITY_SUCCESS, this.menuName);
      }
    }
  }
}
