using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Common.Dtos
{
    public class UpdateApplicationUserProfileCommand: IRequest<GetApplicationUserResponseDto>
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        public string? ImagePath { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? StateCode { get; set; }
        public string? PostCode { get; set; }
        public string? Mobile { get; set; }
        public string? AlternateEmail { get; set; }
        public string? CountryCode { get; set; }
        public string? AlternateMobile { get; set; }
    }
}
