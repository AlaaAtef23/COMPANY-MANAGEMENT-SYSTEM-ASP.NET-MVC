using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IEmployeeRepository _employeesRepo; //Ask CLR to create an object from class implmenting IEmployeeRepository
        //private readonly IDepartmentRepository _departmentRepo;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper/*, IEmployeeRepository EmployeeRepo, IDepartmentRepository departmentRepo*/)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;

            //_employeesRepo = EmployeeRepo;
            //_departmentRepo = departmentRepo;
        }

        // /Depaetment/Index
        public IActionResult Index(string searchInp)
        {
            var employees= Enumerable.Empty<Employee>();

            if (string.IsNullOrEmpty(searchInp)) 
            {
                employees = _unitOfWork.EmployeeRepository.GetAll();
            }
            else
            {
                employees= _unitOfWork.EmployeeRepository.SearchByName(searchInp);
            }

            var mappedEmps = _mapper.Map <IEnumerable<Employee>, IEnumerable<EmployeeViewModel>> (employees);

            return View(mappedEmps);

            TempData.Keep();
            // Binding through view's Dectionary  : Transfer data from action to view [one way]

            // 1.ViewData
            // 1. ViewData is a Dictionary Type Property (introduced in ASP.NET Framework 3.5) 
            // => It helps us to transfer the data from controller [Action] to View
            
            ///ViewData["Message"] = "Hello view Data";

            // 2. ViewBag 
            // 2. ViewBag is a Dynamic Type Property (introduced in ASP.NET Framework 4.0 based on dynamic feature) 
            // => It helps us to transfer the data from controller [Action] to view

            ///ViewBag.Message = "Hello ViewBag";


            
        }

        public IActionResult Create()
        {
            //ViewData["Departments"] = _departmentRepo.GetAll();
            //ViewBag.Departments=_departmentRepo.GetAll();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                employeeVM.ImageName= DocumentSettings.UploadFile(employeeVM.Image, "images");
                var mappedEmp= _mapper.Map<EmployeeViewModel,Employee>(employeeVM);

                 _unitOfWork.EmployeeRepository.Add(mappedEmp);
                var count = _unitOfWork.Complete();
                // 3. TempData

                if (count > 0)
                    TempData["Message"] = "Employee is created successfully";

                else
                    TempData["Message"] = "An Error has occured , Employee is not Created :( !!";

                return RedirectToAction(nameof(Index));

            }
            return View(employeeVM);
        }

        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (!id.HasValue)
                return BadRequest(); //400

            var employee = _unitOfWork.EmployeeRepository.Get(id.Value);

            var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(employee);

            if (employee is null)
                return NotFound(); //404

            return View(viewName, mappedEmp);
        }

        public IActionResult Edit(int? id)
        {
            ///if(!id.HasValue)
            ///    return BadRequest();
            ///var departmnet = _EmployeesRepo.Get(id.Value);
            ///
            ///if (departmnet is null)
            ///    return NotFound();
            ///
            ///return View(departmnet);

            //ViewBag.Departments = _departmentRepo.GetAll();


            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                if (id != employeeVM.Id)
                    return BadRequest();
                try
                {
                    var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                    _unitOfWork.EmployeeRepository.Update(mappedEmp);
                    _unitOfWork.Complete();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(employeeVM);
        }

        // /Employee/Delete
        // /Employee/Delete/1
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public IActionResult Delete([FromRoute] int id, EmployeeViewModel employeeVM)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            try
            {
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);

                _unitOfWork.EmployeeRepository.Delete(mappedEmp);
                var count=_unitOfWork.Complete();
                if (count > 0)
                    DocumentSettings.DeleteFile(employeeVM.ImageName, "images");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 1. Log Excption
                // 2. Return Friendly Message

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(employeeVM);

            }

        }
    }
}
