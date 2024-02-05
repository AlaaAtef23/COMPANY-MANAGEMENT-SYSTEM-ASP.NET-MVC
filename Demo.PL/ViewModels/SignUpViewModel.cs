using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class SignUpViewModel
	{
		[Required(ErrorMessage ="UserName is required")]
        public string UserName { get; set; }

		[Required(ErrorMessage ="First name is required")]
        public string FName { get; set; }

		[Required(ErrorMessage = "last name is required")]
		public string LName { get; set; }

        [Required(ErrorMessage ="Email is required")]
		[EmailAddress(ErrorMessage ="Invalid Email")]
        public string Email { get; set; }

		[Required(ErrorMessage ="Password is required")]
		[MinLength(5,ErrorMessage ="MinLength is 5")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password is required")]
		[Compare(nameof(Password),ErrorMessage ="confirm password does not match password")]
		[DataType(DataType.Password)]  
		public string ConfirmPassword { get; set; }

		public bool IsAgree { get; set; }
    }
}
