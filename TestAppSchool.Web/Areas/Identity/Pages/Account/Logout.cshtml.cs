// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestAppSchool.Data;
using TestAppSchool.Data.Models;

namespace TestAppSchool.Web.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly TestAppSchoolDbContext _db;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(TestAppSchoolDbContext db, SignInManager<User> signInManager, ILogger<LogoutModel> logger)
        {
            _db = db;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            var user = _db.Users.SingleOrDefault(x => x.UserName == User.Identity.Name);
            user.IsActive = false;
            _db.Users.Update(user);
            _db.SaveChanges();

            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage();
            }
        }
    }
}
