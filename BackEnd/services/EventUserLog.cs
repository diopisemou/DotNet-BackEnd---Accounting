using BackEnd.Data;
using BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BackEnd.services {
    public interface IEventService {
        void SaveEvent (int userName, string eventName, string activity, string type="Tran");
    }
    public class EventUserLog : IEventService {
        public static string EVENT_READ = "read";
        public static string EVENT_CREATE = "create";
        public static string EVENT_UPDATE = "update";
        public static string EVENT_DELETE = "delete";
        public static string EVENT_REMOVE = "remove";
        public static string EVENT_LOGIN = "login";
        public static string ACTIVITY_SUCCESS= "success";
        public static string ACTIVITY_FAILED = "failed";

        private readonly MyDatabaseContext _context;
        public EventUserLog (MyDatabaseContext context) {
            _context = context;
        }
        public void SaveEvent (int userName, string eventName, string activity, string type) {
            try {
                UserLogModel model = new UserLogModel {
                    Username = userName,
                    Event = eventName,
                    Activity = activity,
                    Type = type,
                    Datetime = DateTime.Now
                };
                _context.Add(model);
                _context.SaveChanges();
            } catch (Exception error) {
                Console.WriteLine(error);
            }
        }
    }
}
