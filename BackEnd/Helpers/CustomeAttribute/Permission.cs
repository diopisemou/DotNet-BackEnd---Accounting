using BackEnd.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Helpers.CustomAttribute {
  [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
  public class Permission : Attribute {
    public readonly RequiredPermissionOption RequiredPermission;

    public Permission (RequiredPermissionOption requiredPermission) {
      RequiredPermission = requiredPermission;
    }
  }
}
