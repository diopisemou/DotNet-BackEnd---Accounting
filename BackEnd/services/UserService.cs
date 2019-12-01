using BackEnd.Data;
using BackEnd.Helpers;
using BackEnd.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.services {
  public interface IUserService {
    User Authenticate (string useremail, string password);
    User GetUserByEmail (string email);
    User CheckAdmin (ClaimsPrincipal currentUser);
    User GetCurrentUser (ClaimsPrincipal currentUser);
    User CheckUseRole (ClaimsPrincipal currentUser, string menu, string function);
    User CheckSelf (ClaimsPrincipal currentUser, int user_id);
    JwtSecurityToken JWTLogin (string token);
  }

  public class UserService : IUserService {
    private readonly MyDatabaseContext _context;
    private readonly AppSettings _appSettings;
    public User CheckAdmin (ClaimsPrincipal currentUser) {
      try {
        if (currentUser.HasClaim(c => c.Type == ClaimTypes.Email)) {
          var user = GetUserByEmail(currentUser.FindFirst(ClaimTypes.Email).Value);
          var userRole = _context.Roles.FirstOrDefault(r => r.Roll_Id == user.Roll_Id);
          if (userRole.Rollname == "admin") {
            return user;
          } else {
            return null;
          }
        } else {
          return null;
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return null;
      }
    }
    public User GetCurrentUser (ClaimsPrincipal currentUser) {
      try {
        if (currentUser.HasClaim(c => c.Type == ClaimTypes.Email)) {
          var user = GetUserByEmail(currentUser.FindFirst(ClaimTypes.Email).Value);
          return user;
        } else {
          return null;
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return null;
      }
    }
    public User CheckSelf (ClaimsPrincipal currentUser, int user_id) {
      try {
        if (currentUser.HasClaim(c => c.Type == ClaimTypes.Email)) {
          var user = GetUserByEmail(currentUser.FindFirst(ClaimTypes.Email).Value);
          if (user_id == user.User_Id) {
            return user;
          } else {
            return null;
          }
        } else {
          return null;
        }
      } catch (Exception error) {
        Console.WriteLine(error);
        return null;
      }
    }
    public User CheckUseRole (ClaimsPrincipal currentUser, string menu, string function) {
      try {
        if (currentUser.HasClaim(c => c.Type == ClaimTypes.Email)) {
          var user = GetUserByEmail(currentUser.FindFirst(ClaimTypes.Email).Value);
          if (user != null) {
            AuthRoleModel roles = _context.AuthRoles.FirstOrDefault(a => a.roll_id == user.Roll_Id && menu == a.menu);
            switch (function) {
              case "create":
                if (roles.saveRole) return user;
                break;
              case "read":
                if (roles.viewRole) return user;
                break;
              case "update":
                if (roles.updateRole) return user;
                break;
              case "delete":
                if (roles.deleteRole) return user;
                break;
              default:
                return null;
            }
            return null;
          } else {
            return null;
          }
        } else {
          return null;
        }
      } catch (Exception ex) {
        Console.Write(ex.ToString());
        return null;
      }
    }
    public UserService (IOptions<AppSettings> appSettings, MyDatabaseContext context) {
      _appSettings = appSettings.Value;
      _context = context;
    }

    //get User with email and password
    public User Authenticate (string useremail, string password) {
      var user = _context.Users.FirstOrDefault(u => u.Useremail == useremail);
      // return null if user not found
      if (user == null)
        return null;
      if (Hashing.ValidatePassword(password, user.Password)) {
        user.Token = GetUserToken(user.Username, user.Useremail);
        return user;
      }
      return null;
    }

    //get User with email
    public User GetUserByEmail (string useremail) {
      var user = _context.Users.FirstOrDefault(u => u.Useremail == useremail);
      if (user == null) {
        return null;
      }
      user.Token = GetUserToken(user.Username, user.Useremail);
      return user;
    }

    public JwtSecurityToken JWTLogin (string token) {
      var newToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
      return newToken;
    }

    private string GetUserToken (string username, string email) {
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

      var claims = new[] {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, email),
            };

      var token = new JwtSecurityToken(
          _appSettings.JwtIssuer, _appSettings.JwtIssuer,
          claims,
          expires: DateTime.Now.AddMinutes(120),
          signingCredentials: credentials);

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
