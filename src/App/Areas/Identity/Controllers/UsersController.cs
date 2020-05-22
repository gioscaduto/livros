using App.Areas.Identity.ViewModels;
using App.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace App.Areas.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Identity")]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPasswordHasher<IdentityUser> _passwordHasher;

        public UsersController(UserManager<IdentityUser> userManager,
                               RoleManager<IdentityRole> roleManager,
                               IPasswordHasher<IdentityUser> passwordHasher)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
        }

        [Route("usuarios")]
        public IActionResult Index(int? pagina, [FromServices]PaginaUtil paginaUtil)
        {
            var usuarios = _userManager.Users;
            var usuariosPagedList = usuarios.ToPagedList(pagina ?? 1, paginaUtil.QtdItensPagina);

            return View(usuariosPagedList);
        }

        [HttpGet]
        [Route("novo-usuario")]
        public IActionResult Create()
        {
            ViewBag.Roles = _roleManager.Roles;
            return View();
        }

        [HttpPost]
        [Route("novo-usuario")]
        public  async Task<IActionResult> Create(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                IdentityUser identityUser = new IdentityUser
                {
                    UserName = user.Email,
                    Email = user.Email
                };

                var result = await _userManager.CreateAsync(identityUser, user.Senha);

                if (result.Succeeded)
                {
                    if (user.Roles != null && user.Roles.Length > 0)
                        result = await _userManager.AddToRolesAsync(identityUser, user.Roles);

                    if(result.Succeeded) return RedirectToAction("Index");
                }
                else
                {
                    foreach (var erro in result.Errors)
                        ModelState.AddModelError("", erro.Description);
                }
            }

            ViewBag.Roles = _roleManager.Roles;
            return View(user);
        }

        [HttpGet]
        [Route("editar-usuario")]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                
                var userViewModel = new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Roles = roles?.ToArray()
                };

                ViewBag.Roles = _roleManager.Roles;
                return View(userViewModel);
            }
           
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("editar-usuario")]
        public async Task<IActionResult> Edit(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                var identityUser = await _userManager.FindByIdAsync(user.Id);

                if (identityUser != null)
                {
                    identityUser.Email = user.Email;
                    identityUser.PasswordHash = _passwordHasher.HashPassword(identityUser, user.Senha);
                    
                    var result = await _userManager.UpdateAsync(identityUser);

                    if (result.Succeeded)
                    {
                        await AdicionarRoles(identityUser, user.Roles); 
                        
                        return RedirectToAction("Index");
                    }   
                    else
                    {
                        foreach (var erro in result.Errors)
                            ModelState.AddModelError("", erro.Description);
                    }
                }
            }

            ViewBag.Roles = _roleManager.Roles;
            return View(user);
        }

        private async Task AdicionarRoles(IdentityUser identityUser, string[] roles)
        {
            var rolesUser = await _userManager.GetRolesAsync(identityUser);
          
            if (rolesUser != null && rolesUser.Count > 0)
            {
                // Removendo Todas As Roles para Adicionar As Novas ou Re Adicionar As Mesmas
                await _userManager.RemoveFromRolesAsync(identityUser, _roleManager.Roles
                                                                                  .Where(r => rolesUser.Any(ru => ru == r.Name))
                                                                                  .Select(r => r.Name));
            }

            if (roles != null) 
                await _userManager.AddToRolesAsync(identityUser, roles.ToList());
        }

        [HttpGet]
        [Route("excluir-usuario")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                var userViewModel = new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email
                };

                return View(userViewModel);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("excluir-confirmacao-usuario")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                {
                    foreach (var erro in result.Errors)
                        ModelState.AddModelError("", erro.Description);
                }

                return View(user);
            }

            return RedirectToAction("Index");
        }
    }
}