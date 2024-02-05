using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class ResetPasswordViewModel
	{
		[Required(ErrorMessage = "Password is required")]
		[MinLength(5, ErrorMessage = "MinLength is 5")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "Confirm Password is required")]
		[Compare(nameof(NewPassword), ErrorMessage = "confirm password does not match password")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }

		
    }
}
