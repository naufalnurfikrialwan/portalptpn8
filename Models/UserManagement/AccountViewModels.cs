using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ptpn8.UserManagement.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        //[Display(Name = "Email")]
        public string Email { get; set; }
        public string UserName { get; set; }
        public string NoIndukKaryawan { get; set; }
        public string Nama { get; set; }
        public string Jabatan { get; set; }
        public string Telepon { get; set; }
        public Guid LokasiKerjaId { get; set; }
        public Guid PosisiLokasiKerjaId { get; set; }
        public bool IsApprove { get; set; }
        public DateTime TanggalAksesTerakhir { get; set; }
        [NotMapped]
        public string FileFoto { get; set; }

        [UIHint("imgUpload")]
        public byte[] Foto { get; set; }

    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        //[RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "Must be a valid email")]
        [Display(Name = "Email")]
        //[EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string KonfirmasiPassword { get; set; }
    }

    public class KonfirmasiLoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        //[RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "Must be a valid email")]
        [Display(Name = "Email")]
        //[EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string KonfirmasiPassword { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "Must be a valid email")]
        [Display(Name = "Alamat Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "No Induk Karyawan Harus Diisi")]
        [Display(Name = "No Induk Karyawan (NIK) ")]
        public string NoIndukKaryawan { get; set; }

        [Required(ErrorMessage = "Nama Harus Diisi")]
        [Display(Name = "Nama Lengkap")]
        public string Nama { get; set; }

        [Required(ErrorMessage = "Jabatan Harus Diisi")]
        [Display(Name = "Jabatan")]
        public string Jabatan { get; set; }

        [Required(ErrorMessage = "No Telepon/HP Diisi")]
        [Display(Name = "No Telepon/HP")]
        public string Telepon { get; set; }

        [Required(ErrorMessage = "Lokasi Kerja Harus Dipilih")]
        [Display(Name = "Lokasi Kerja")]
        public Guid LokasiKerjaId { get; set; }
        public Guid PosisiLokasiKerjaId { get; set; }
        public bool IsApprove { get; set; }

        [Required(ErrorMessage = "Foto dengan format JPEG harus di upload")]
        public String FileFoto { get; set; }

        [UIHint("imgUpload")]
        public byte[] Foto { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password Baru")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password Baru")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }

        [Required]
        [Display(Name = "Bagian/GM/Kebun/Unit")]
        public Guid PosisiLokasiKerjaId { get; set; }

        [Required]
        [Display(Name = "Sub Bagian/Bidang/Afdeling")]
        public Guid LokasiKerjaId { get; set; }

        [Required]
        [Display(Name = "Nama")]
        public string NamaKaryawan { get; set; }

        [Required]
        [Display(Name = "Jabatan")]
        public string JabatanKaryawan { get; set; }

    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
