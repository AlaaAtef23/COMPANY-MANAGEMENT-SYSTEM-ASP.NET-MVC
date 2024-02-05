﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public class Department :ModelBase
    {
        //Model

        public string Code { get; set; }

        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }= DateTime.Now;

        //Navigational proprty => [Many]
        public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
