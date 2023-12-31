﻿using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Common.Dtos
{
    public class CreateApplicationOrganizationCommand: IRequest<GetApplicationOrganizationResponseDto>
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string PasswordSalt { get;  set; }
        [Required]
        public string Email { get;  set; }

		public string? Phone { get; set; }
		public int RoleId { get;  set; }
        [Required]
        public string OrganizationName { get;  set; }
        [Required]
        public string Domain { get; set; }
        public string? Description { get;  set; }
        public string? LogoUrl { get; set; }
        public string? AddressLine1 { get;  set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public int? StateCode { get; set; }
        public string? PostCode { get;  set; }
        [Required]
        public string? CountryCode { get; set; }
    }
}
