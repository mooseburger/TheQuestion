﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheQuestion.Models.Generic;
using TheQuestion.Models.User;
using TheQuestion.Repositories;

namespace TheQuestion.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(IUserRepository userRepository, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPage([FromQuery] PaginatedRequest request)
        {
            var result = await _userRepository.GetUserPage(request);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var model = new CreateUser();
            model.SetRoles(roles);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUser user)
        {
            var roles = await _roleManager.Roles.ToListAsync();
            user.SetRoles(roles);

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            if (user.Password != user.ConfirmPassword)
            {
                user.Errors = new List<IdentityError>() { new IdentityError() {
                    Code = "PassNotConfirm",
                    Description = "Passwords don't match."
                } };

                return View(user);
            }

            var identityUser = new IdentityUser()
            {
                UserName = user.Username,
                Email = user.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(identityUser, user.Password);
            if (!result.Succeeded)
            {
                user.Errors = result.Errors;
                return View(user);
            }

            result = await _userManager.AddToRoleAsync(identityUser, user.RoleName);
            if (!result.Succeeded)
            {
                user.Errors = result.Errors;
                return View(user);
            }

            return Redirect($"edit/{user.Username}");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit([FromRoute(Name = "id")] string username)
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var user = await _userManager.FindByNameAsync(username);
            var model = new EditUser(user);
            model.RoleName = (await _userManager.GetRolesAsync(user))?.FirstOrDefault();
            model.SetRoles(roles);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUser user)
        {
            var roles = await _roleManager.Roles.ToListAsync();
            user.SetRoles(roles);

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            if (!string.IsNullOrWhiteSpace(user.Password) && user.Password != user.ConfirmPassword)
            {
                user.Errors = new List<IdentityError>() { new IdentityError() {
                    Code = "PassNotConfirm",
                    Description = "Passwords don't match."
                } };

                return View(user);
            }

            IdentityResult result;

            var identityUser = await _userManager.FindByNameAsync(user.OriginalUsername);

            var currentRole = (await _userManager.GetRolesAsync(identityUser))?.FirstOrDefault();
            if (currentRole != user.RoleName)
            {
                if (currentRole != null)
                {
                    result = await _userManager.RemoveFromRoleAsync(identityUser, currentRole);
                    if (!result.Succeeded)
                    {
                        user.Errors = result.Errors;
                        return View(user);
                    }
                }

                if (!string.IsNullOrWhiteSpace(user.RoleName))
                {
                    result = await _userManager.AddToRoleAsync(identityUser, user.RoleName);
                    if (!result.Succeeded)
                    {
                        user.Errors = result.Errors;
                        return View(user);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(identityUser);
                result = await _userManager.ResetPasswordAsync(identityUser, resetToken, user.Password);
                if (!result.Succeeded)
                {
                    user.Errors = result.Errors;
                    return View(user);
                }
            }

            identityUser.Email = user.Email;
            identityUser.UserName = user.Username;
            identityUser.LockoutEnd = user.Lockout ? DateTimeOffset.MaxValue : null;

            result = await _userManager.UpdateAsync(identityUser);
            if (!result.Succeeded)
            {
                user.Errors = result.Errors;
                return View(user);
            }

            return Redirect($"/user/edit/{user.Username}");
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute(Name = "id")] string username)
        {
            if (HttpContext.User.Identity?.Name == username)
            {
                return Ok(new
                {
                    Succeeded = false,
                    Errors = new List<string> { "You can't delete yourself." }
                });
            }

            var user = await _userManager.FindByNameAsync(username);
            var result = await _userManager.DeleteAsync(user);

            return Ok(result);
        }
    }
}
