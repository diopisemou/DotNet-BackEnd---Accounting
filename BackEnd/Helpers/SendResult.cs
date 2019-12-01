using BackEnd.Data;
using BackEnd.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Helpers {
  public class SendResult {
    public static JObject SendError (string error) {
      dynamic sendObject = new JObject();
      sendObject.error = error;
      return sendObject;
    }

    public static JObject SendUser (User user, MyDatabaseContext _context) {
      dynamic sendObject = new JObject();
      try {
        sendObject.id = user.User_Id;
        sendObject.username = user.Username;
        sendObject.useremail = user.Useremail;
        sendObject.token = user.Token;
        sendObject.role = _context.Roles.FirstOrDefault(role => role.Roll_Id == user.Roll_Id).Rollname;
      } catch (Exception error) {
        Console.WriteLine(error);
        sendObject.role = "";
      }

      return sendObject;
    }
  }
}
