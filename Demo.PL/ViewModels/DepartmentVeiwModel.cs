using Demo.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace Demo.PL.ViewModels
{
    public class DepartmentVeiwModel
    {
        //Model
        public int Id { get; set; }

        [Required(ErrorMessage = "Code is Required !!!")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name is Required !!!")]
        public string Name { get; set; }
        [Display(Name = "Date of creation")]
        public DateTime DateOfCreation { get; set; }

        //Navigational proprty => [Many]
        //public ICollection<Employee> Employees { get; set; } = new HashSet<Employee>();
    }
}
