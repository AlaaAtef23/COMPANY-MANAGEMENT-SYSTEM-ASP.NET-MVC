using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Demo.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentityRole> roleManager,IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string name)
        {
            if (string.IsNullOrEmpty(name)) //هيجيب كل اليوزرز اللى عندنا
            {
                var roles = await _roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName=R.Name,
                }).ToListAsync();
                return View(roles);
            }
            else
            {
                var roles = await _roleManager.FindByNameAsync(name);
                if(roles is not null)
                {
                    var mappedRole = new RoleViewModel()
                    {
                        Id = roles.Id,
                        RoleName = roles.Name
                    };
                    return View(new List<RoleViewModel>() { mappedRole });

                }
                return View(Enumerable.Empty<RoleViewModel>());

            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleVM)
        {
            if(ModelState.IsValid)
            {
                var mappedRole=_mapper.Map<RoleViewModel,IdentityRole>(roleVM);
                await _roleManager.CreateAsync(mappedRole);

                return RedirectToAction(nameof(Index));
            }
            return View(roleVM);
        }

        // /Role/Details/Guid
        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest(); //400

            var role = await _roleManager.FindByIdAsync(id);

            var mappedRole = _mapper.Map<IdentityRole, RoleViewModel>(role);

            if (role is null)
                return NotFound(); //404

            return View(viewName, mappedRole);
        }

        // /Role/Edit/Guid
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleViewModel updatedRole)
        {
            if (ModelState.IsValid)
            {
                if (id != updatedRole.Id)
                    return BadRequest();
                try
                {
                    //var mappedUser = _mapper.Map<UserViewModel, ApplicationUser>(updatedUser);

                    var role = await _roleManager.FindByIdAsync(id);

                    role.Name = updatedRole.RoleName;
                   

                    await _roleManager.UpdateAsync(role);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(updatedRole);
        }


        public async Task<IActionResult> Delete(string id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost(Name = ("Delete"))]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public async Task<IActionResult> ConfirmDelete(string id)
        {

            try
            {
                //var mappedUser = _mapper.Map<UserViewModel, ApplicationUser>(deletedUser);

                var role = await _roleManager.FindByIdAsync(id);

                await _roleManager.DeleteAsync(role);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 1. Log Excption
                // 2. Return Friendly Message

                ModelState.AddModelError(string.Empty, ex.Message);
                return RedirectToAction("Error", "Home");

            }

        }
    }
}
