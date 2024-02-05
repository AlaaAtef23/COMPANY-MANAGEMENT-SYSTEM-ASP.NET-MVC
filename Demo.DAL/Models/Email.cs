using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
	public class Email : ModelBase
	{
        public String Subject { get; set; }
        public String Recipients { get; set; }
        public String Body { get; set; }
    }
}
