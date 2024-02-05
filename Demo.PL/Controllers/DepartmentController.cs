using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Demo.PL.Controllers
{
	//Inhertiance : DepartmentController is a Controller
	//Composition (Association) : DepartmentController has a DepartmentRepsitory (when DepartmentController must has a DepartmentRepository) || --> (it can be (agregation) if DepartmentController may has DepartmentRepository or not maybe with null)
	
    [Authorize]
	public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        //private readonly IDepartmentRepository _departmentsRepo; //Ask CLR to create an object from class implmenting IDepartmentRepository

        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper/*, IDepartmentRepository departmentRepo*/)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;

            //_departmentsRepo = departmentRepo;
        }

        // /Depaetment/Index
        public IActionResult Index()
        {
            var departments= _unitOfWork.DepartmentRepository.GetAll();
            var mappedDept=_mapper.Map<IEnumerable<Department>,IEnumerable<DepartmentVeiwModel>>(departments);
            return View(mappedDept);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DepartmentVeiwModel departmentVM)
        {
            if (ModelState.IsValid)
            {
                var mappedDept = _mapper.Map<DepartmentVeiwModel,Department>(departmentVM) ;
                 _unitOfWork.DepartmentRepository.Add(mappedDept);
                var count = _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(departmentVM);
        }

        public IActionResult Details(int? id ,string viewName="Details")
        {
            if (!id.HasValue)
                return BadRequest(); //400

            var deparment = _unitOfWork.DepartmentRepository.Get(id.Value);

            var mappedDept = _mapper.Map<Department, DepartmentVeiwModel>(deparment);

            if (mappedDept is null)
                return NotFound(); //404

            return View(viewName, mappedDept);
        }

        public IActionResult Edit(int?id)
        {
            ///if(!id.HasValue)
            ///    return BadRequest();
            ///var departmnet = _departmentsRepo.Get(id.Value);
            ///
            ///if (departmnet is null)
            ///    return NotFound();
            ///
            ///return View(departmnet);

            return Details(id,"Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, DepartmentVeiwModel departmentVM)
        {
            if(ModelState.IsValid)
            {
                var mappedDept = _mapper.Map<DepartmentVeiwModel, Department>(departmentVM);

                if (id != mappedDept.Id)
                    return BadRequest();
                try
                {
                    _unitOfWork.DepartmentRepository.Update(mappedDept);
                    _unitOfWork.Complete();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(departmentVM);
        }

        // /Department/Delete
        // /Department/Delete/1
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute]int id,DepartmentVeiwModel depaetmentVM)
        {
            
            if (id != depaetmentVM.Id)
                return BadRequest();
            try
            {
                var mappedDept = _mapper.Map<DepartmentVeiwModel, Department>(depaetmentVM);
                _unitOfWork.DepartmentRepository.Delete(mappedDept);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 1. Log Excption
                // 2. Return Friendly Message

                ModelState.AddModelError(string.Empty,ex.Message);
                return View(depaetmentVM);
            }
        }


    }
}
