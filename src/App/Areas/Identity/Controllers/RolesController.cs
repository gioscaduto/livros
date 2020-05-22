using App.Areas.Identity.ViewModels;
using App.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using X.PagedList;

namespace App.Areas.Identity.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Identity")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [Route("roles")]
        public IActionResult Index(int? pagina, [FromServices]PaginaUtil paginaUtil)
        {
            var roles = _roleManager.Roles;

            var rolesPagedList = roles.ToPagedList(pagina ?? 1, paginaUtil.QtdItensPagina);

            return View(rolesPagedList);
        }

        [HttpGet]
        [Route("nova-role")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("nova-role")]
        public async Task<IActionResult> Create(RoleViewModel role)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                   Name = role.Nome
                };

                var result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                {
                    foreach (var erro in result.Errors)
                        ModelState.AddModelError("", erro.Description);
                }

            }

            return View(role);
        }

        [HttpGet]
        [Route("editar-role")]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role != null)
            {
                var roleViewModel = new RoleViewModel
                {
                    Id = role.Id,
                    Nome = role.Name
                };

                return View(roleViewModel);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("editar-role")]
        public async Task<IActionResult> Edit(RoleViewModel role)
        {
            if (ModelState.IsValid)
            {
                var identityRole = await _roleManager.FindByIdAsync(role.Id);

                if (identityRole != null)
                {
                    identityRole.Name = role.Nome;
                    
                    var result = await _roleManager.UpdateAsync(identityRole);

                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (var erro in result.Errors)
                            ModelState.AddModelError("", erro.Description);
                    }
                }
            }

            return View(role);
        }

        [HttpGet]
        [Route("excluir-role")]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role != null)
            {
                var roleViewModel = new RoleViewModel
                {
                    Id = role.Id,
                    Nome = role.Name
                };

                return View(roleViewModel);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("excluir-confirmacao-role")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                {
                    foreach (var erro in result.Errors)
                        ModelState.AddModelError("", erro.Description);
                }

                return View(role);
            }

            return RedirectToAction("Index");
        }
    }
}