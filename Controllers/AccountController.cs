using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Ptpn8.Areas.Referensi.Models.CRUD;
using Ptpn8.Models;
using Ptpn8.Models.CRUD;
using Ptpn8.UserManagement.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ptpn8.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Menu = CRUDMenu.CRUD.BuildMenu("Portal");
            ViewBag.LokasiKerja = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerja;
            return View();
        }


        [AllowAnonymous]
        public ActionResult KonfirmasiLogin(string returnUrl, string email, string password, bool rememberMe )
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Menu = CRUDMenu.CRUD.BuildMenu("Portal");
            ViewBag.LokasiKerja = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerja;
            var model = new KonfirmasiLoginViewModel();
            model.Email = email;
            model.Password = password;
            model.KonfirmasiPassword = "";
            model.RememberMe = rememberMe;
            return View(model);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            ViewBag.Menu = CRUDMenu.CRUD.BuildMenu("Portal");
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            if(model.KonfirmasiPassword=="NULL")
            {
                return RedirectToAction("KonfirmasiLogin", new { ReturnUrl = returnUrl, Email = model.Email, Password = model.Password, RememberMe = model.RememberMe });
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            try
            {
                var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        var user = SignInManager.UserManager.FindByEmail(model.Email);
                        if(user == null)
                        {
                            user = SignInManager.UserManager.FindByName(model.Email);
                        }
                        else 
                        {
                            CRUDImage.CRUD.ReadToView(user.Id + ".jpg", user.Foto);
                            user.TanggalAksesTerakhir = DateTime.Now;
                            CRUDApplicationUser.CRUD.Update(user);
                            ViewBag.LokasiKerja = CRUDLokasiKerja.CRUD.LoginLokasiKerja(user.UserName).PosisiLokasiKerja;
                            //createPenggunaPortal(user.UserName);
                        }
                        if (model.Password == "123456")
                        {
                            string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                            //var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id,  userName = user.UserName, code = code, eMail = user.Email, namaKaryawan = user.Nama, jabatanKaryawan = user.Jabatan, lokasiKerjaId = user.LokasiKerjaId, posisiLokasiKerjaId = user.PosisiLokasiKerjaId, Area = "" }, protocol: Request.Url.Scheme);
                            //await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                            //return RedirectToAction("ForgotPasswordConfirmation", "Account");
                            return RedirectToAction("ResetPassword", "Account", new { userId = user.Id, userName = user.UserName, code = code, eMail = user.Email, namaKaryawan = user.Nama, jabatanKaryawan = user.Jabatan, lokasiKerjaId = user.LokasiKerjaId, posisiLokasiKerjaId = user.PosisiLokasiKerjaId, Area = "" });
                        }
                        else
                        {
                            return RedirectToLocal(returnUrl); //RedirectToAction("Index", "Home", new { Area = "" }); 
                        }

                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                }

            }
            catch (Exception ex)
            {
                return View(model);
            }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> KonfirmasiLogin(KonfirmasiLoginViewModel model, string returnUrl)
        {
            ViewBag.Menu = CRUDMenu.CRUD.BuildMenu("Portal");
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    var user = SignInManager.UserManager.FindByEmail(model.Email);
                    if (user != null)
                    {
                        CRUDImage.CRUD.ReadToView(user.Id + ".jpg", user.Foto);
                        user.TanggalAksesTerakhir = DateTime.Now;
                        CRUDApplicationUser.CRUD.Update(user);
                        ViewBag.LokasiKerja = CRUDLokasiKerja.CRUD.LoginLokasiKerja(user.UserName).PosisiLokasiKerja;
                        //createPenggunaPortal(user.UserName);
                    }
                    return RedirectToLocal(returnUrl); //RedirectToAction("Index", "Home", new { Area = "" }); 
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.treeOrganisasi = CRUDLokasiKerja.CRUD.getTreeData();
            ViewBag.Menu = CRUDMenu.CRUD.BuildMenu("Portal");
            ViewBag.LokasiKerja = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerja;
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            ViewBag.Menu = CRUDMenu.CRUD.BuildMenu("Portal");
            ViewBag.LokasiKerja = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerja;
            ViewBag.treeOrganisasi = CRUDLokasiKerja.CRUD.getTreeData();
            ApplicationUser user = new ApplicationUser();

            if (ModelState.IsValid)
            {
                model.Foto = CRUDImage.CRUD.GetFromUpload(model.FileFoto, model.Foto);
                user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Jabatan = model.Jabatan,
                    LokasiKerjaId = model.LokasiKerjaId,
                    Nama = model.Nama,
                    NoIndukKaryawan = model.NoIndukKaryawan,
                    IsApprove = model.IsApprove,
                    Foto = model.Foto,
                    Telepon = model.Telepon,
                    TanggalAksesTerakhir = DateTime.Now,
                    PhoneNumber=model.Telepon
                    //PosisiLokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(model.LokasiKerjaId).PosisiLokasiKerjaId
                };


                
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    user.PosisiLokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(user).PosisiLokasiKerjaId;
                    CRUDApplicationUser.CRUD.Update(user);

                    HttpContext.Application["AspNetUsers"]=null;
                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code, string userName, string eMail = "", 
            string namaKaryawan = "",
            string jabatanKaryawan = "",
            string lokasiKerjaId = "00000000-0000-0000-0000-000000000000", 
            string posisiLokasiKerjaId = "00000000-0000-0000-0000-000000000000")
        {
            if (code == null)
                return View("Error");
            else
            {
                var model = new ResetPasswordViewModel();
                model.Email = eMail;
                model.UserName = userName;
                model.Code = code;
                model.NamaKaryawan = namaKaryawan;
                model.JabatanKaryawan = jabatanKaryawan;
                model.LokasiKerjaId = Guid.Parse(lokasiKerjaId);
                model.PosisiLokasiKerjaId = Guid.Parse(posisiLokasiKerjaId);
                return View(model);
            }
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                // tambahkan untuk mengupdate field LokasiKerjaId dan PosisiLokasiKerjaId
                ExecSQL("update [Identity].[dbo].[AspNetUsers] set Nama='"+model.NamaKaryawan+"', Jabatan='"+model.JabatanKaryawan+"', Email='"+model.Email+"', LokasiKerjaId = '"+model.LokasiKerjaId.ToString()+"', PosisiLokasiKerjaId = '"+model.PosisiLokasiKerjaId.ToString()+"' where UserName = '"+model.UserName.ToString()+"'");

                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {

            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback

        


        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login

            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    var key = loginInfo.DefaultUserName;
                    var user = SignInManager.UserManager.FindByEmail(loginInfo.Email);
                    if (user != null)
                    {
                        CRUDImage.CRUD.ReadToView(user.Id + ".jpg", user.Foto);
                        user.TanggalAksesTerakhir = DateTime.Now;
                        CRUDApplicationUser.CRUD.Update(user);
                        //createPenggunaPortal(user.UserName);
                    }
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    ViewBag.treeOrganisasi = CRUDLokasiKerja.CRUD.getTreeData();
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel
                    {
                        Email = loginInfo.Email != null ? loginInfo.Email : "",
                        UserName = loginInfo.DefaultUserName,
                        Jabatan = "",
                        LokasiKerjaId = Guid.Empty,
                        PosisiLokasiKerjaId = Guid.Empty,
                        Nama = "",
                        NoIndukKaryawan = "",
                        IsApprove = false,
                        Foto = null
                    });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            ViewBag.Menu = CRUDMenu.CRUD.BuildMenu("Portal");
            ViewBag.LokasiKerja = CRUDLokasiKerja.CRUD.LoginLokasiKerja(model.UserName).PosisiLokasiKerja;

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }

                var user = new ApplicationUser
                {
                    UserName = info.Login.LoginProvider=="Google" ? model.Email : model.UserName,
                    Email = model.Email,
                    Jabatan = model.Jabatan,
                    LokasiKerjaId = model.LokasiKerjaId,
                    Nama = model.Nama,
                    NoIndukKaryawan = model.NoIndukKaryawan,
                    IsApprove = model.IsApprove,
                    Foto = CRUDImage.CRUD.GetFromUpload(model.FileFoto, model.Foto),
                    TanggalAksesTerakhir = DateTime.Today
                };
                user.PosisiLokasiKerjaId = CRUDLokasiKerja.CRUD.ReadStatusLokasiKerja(user).PosisiLokasiKerjaId;

                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    List<ApplicationUser> listUser = (List<ApplicationUser>)HttpContext.Application["ListApplicationUser"];
                    listUser.Add(user);
                    HttpContext.Application["ListApplicationUser"] = listUser;
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                AddErrors(result);
            }
            ViewBag.treeOrganisasi = CRUDLokasiKerja.CRUD.getTreeData();
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken()]
        [Authorize]
        public ActionResult LogOff()
        {
            try
            {
                string namaKey = "";
                for(int i=0; i<HttpContext.Application.AllKeys.Count(); i++)
                {
                    try
                    {
                        namaKey = HttpContext.Application.Keys[i];
                        if (namaKey.Substring(0,5).ToLower() != "input")
                        {
                            if(namaKey== "PenggunaPortalYangAktif")
                            {
                                hapusPenggunaPortalYangAktif();
                            }
                            else
                            {
                                hapusListNoInput(namaKey);
                            }
                        }
                        else
                        {
                            // tidak ngapa-ngapain, yang ini harus manteng terus di server
                        }
                    }
                    catch
                    {

                    }
                }

            }
            catch
            {

            }
            finally
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            }
            
            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        public void createPenggunaPortal(string userName)
        {
            var current = Ptpn8.Models.CRUD.CRUDPenggunaPortalYangAktif.CRUD.Get1Record(userName);
            if (current == null)
            {
                current = new Ptpn8.Models.PenggunaPortalYangAktif();
            }
            current.PenggunaPortalYangAktifId = Guid.Parse(Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(userName).Id);
            current.UserName = userName;
            current.Nama = Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(userName).Nama;
            current.LokasiKerjaId = Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(userName).LokasiKerjaId;
            current.PosisiLokasiKerjaId = Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(userName).PosisiLokasiKerjaId;
            current.Tanggal = DateTime.Now;
            current.AplikasiId = Guid.Empty;
            current.Aplikasi = "Login";
            current.MenuId = Guid.Empty;
            current.Menu = "Login";
            Ptpn8.Models.CRUD.CRUDPenggunaPortalYangAktif.CRUD.Update(current);
        }
        public void hapusListNoInput(string namaList)
        {
            var content = (IList)HttpContext.Application[namaList];
            if (content != null)
            {
                for (int j = 0; j < content.Count; j++)
                {
                    var item = (IList)content[j];
                    if ((string)item[0] == HttpContext.User.Identity.Name)
                    {
                        content[j] = null;
                    }
                }

                List<object[]> results = new List<object[]>();
                for (int k = 0; k < content.Count; k++)
                {
                    if (content[k] != null)
                    {
                        object[] result = new object[3];
                        var ct = (IList)content[k];
                        result[0] = ct[0];
                        result[1] = ct[1];
                        result[2] = ct[2];
                        results.Add(result);
                    }
                }
                HttpContext.Application[namaList] = results;
            }
        }
        public void hapusPenggunaPortalYangAktif()
        {
            var current = Ptpn8.Models.CRUD.CRUDPenggunaPortalYangAktif.CRUD.GetAllRecord().Where(o => o.UserName == HttpContext.User.Identity.Name).ToList();
            if (current != null)
            {
                Ptpn8.Models.CRUD.CRUDPenggunaPortalYangAktif.CRUD.Delete(current);
            }
        }

        public void hapusPenggunaPortalYangAktif(string _menuId)
        {
            string namaMenu = CRUDMenu.CRUD.Get1Record(Guid.Parse(_menuId)).LinkText.ToString();
            var current = Ptpn8.Models.CRUD.CRUDPenggunaPortalYangAktif.CRUD.GetAllRecord().Where(o => o.UserName == HttpContext.User.Identity.Name && o.Menu == namaMenu && o.StatusBaru == false).ToList();
            if (current!=null)
            {
                Ptpn8.Models.CRUD.CRUDPenggunaPortalYangAktif.CRUD.Delete(current);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult HapusPenggunaPortalYangAktif(string _menuId)
        {
            hapusListNoInput(_menuId);
            hapusPenggunaPortalYangAktif(_menuId);
            return Json(new { });
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        [AllowAnonymous]
        [HttpPost]
        public ActionResult periksaEmail(string _userName, string _passWord)
        {
            string message="";
            bool result=false;
            var user = CRUDApplicationUser.CRUD.Get1Record(_userName);
            if (user == null)
            {
                message = "EMAIL";
                result = false;
                return Json(new object[] { result, message }, JsonRequestBehavior.AllowGet);
            }
            else if(user.PasswordHash==null)
            {
                message = "NULL";
                result = true;
                return Json(new object[] { result, message }, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("KonfirmasiLogin", "Account");
            }
            else
            {
                message = "OK";
                result = true;
                return Json(new object[] { result, message }, JsonRequestBehavior.AllowGet);
            }
            
        }

        [AllowAnonymous]
        [HttpPost]

        public ActionResult simpanEmail(string _userName, string _passWord)
        {

            var user = CRUDApplicationUser.CRUD.Get1Record(_userName);
            user.PasswordHash = HashPassword(_passWord);
            if(CRUDApplicationUser.CRUD.Update(user)) 
                return Json(true, JsonRequestBehavior.AllowGet);
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }

        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        public ActionResult ExecSQL(string scriptSQL)
        {
            List<object[]> Results = new List<object[]>();

            string connStr = ConfigurationManager.ConnectionStrings["csIdentity"].ConnectionString;
            string[] arrCS = new string[] { connStr };
            if (scriptSQL.ToLower().IndexOf("delete ") >= 0 ||
                scriptSQL.ToLower().IndexOf("drop tab") >= 0
                )
            {
                return Json(Results, JsonRequestBehavior.AllowGet);
            }

            SqlConnection con = new SqlConnection(arrCS[0]);
            try
            {
                con.Open();
                DataTable tap = new DataTable();
                new SqlDataAdapter(scriptSQL, con).Fill(tap);
                for (int i = 0; i < tap.Rows.Count; i++)
                {
                    object[] result = new object[tap.Columns.Count];
                    DataRow dr = tap.Rows[i];
                    for (int j = 0; j < dr.ItemArray.Length; j++)
                    {
                        if (dr[j].GetType().Name == "Byte[]")
                        {
                            CRUDImage.CRUD.RemoveFileView(HttpContext.User.Identity.Name.ToString() + "_" + tap.Columns[j].ToString() + ".jpg");
                            CRUDImage.CRUD.ReadToView(HttpContext.User.Identity.Name.ToString() + "_" + tap.Columns[j].ToString() + ".jpg", (byte[])dr[j]);
                        }
                        else if (tap.Columns[j].ToString().ToLower().Contains("img"))
                        {
                            CRUDImage.CRUD.RemoveFileView(HttpContext.User.Identity.Name.ToString() + "_" + tap.Columns[j].ToString() + ".jpg");
                        }
                        result[j] = dr[j].ToString();
                    }
                    Results.Add(result);
                }

            }
            catch (Exception ex)
            {
                //Exception   
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            return Json(Results, JsonRequestBehavior.AllowGet);
        }
    }
}