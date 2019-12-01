using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Data
{
    public class GeneralData
    {
        public int UserId { get; set; }
        public int RollId { get; set; }
        public int BranchId { get; set; }
        public int EmpId { get; set; }
        public string Username { get; set; }
        public string Useremail { get; set; }
        public string Password { get; set; }
        public int CreatedBy { get; set; }
        public int Status { get; set; }
        public string Token { get; set; }
    }
}
