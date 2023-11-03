using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Common.Dtos
{
    public class CreateOrganizationUserCommand : IRequest<OrganizationUserResponseDto>
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string PasswordSalt { get; set; }
        [Required]
        public string Email { get; set; }
        public string? Phone { get; set; }
        public int RoleId { get; set; }
        public int OrganizationId { get; set; }
        public string? ImagePath { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
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
