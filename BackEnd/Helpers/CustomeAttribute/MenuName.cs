using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Helpers.CustomeAttribute
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class MenuName : Attribute
    {
        public string menu;

        public MenuName(string menu)
        {
            this.menu = menu;
        }
    }
}
