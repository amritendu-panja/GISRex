using Common.Dtos;
using Common.Settings;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.User.Models;

namespace Web.User.Helpers
{
	public class Mapper
	{
		public void Map(ApplicationUserResponseDto userResponseDto, AppUserProfileModel profileModel)
		{
			if (userResponseDto == null) { return; }
			if (profileModel == null)
			{
				profileModel = new AppUserProfileModel();
			}
			profileModel.UserId = userResponseDto.UserId;
			profileModel.Email = userResponseDto.Email;
			profileModel.UserName = userResponseDto.UserName;
			profileModel.AddressLine1 = userResponseDto.AddressLine1;
			profileModel.AddressLine2 = userResponseDto.AddressLine2;
			profileModel.AlternateEmail = userResponseDto.AlternateEmail;
			profileModel.AlternateMobile = userResponseDto.AlternateMobile;
			profileModel.Mobile = userResponseDto.Mobile;
			profileModel.City = userResponseDto.City;
			profileModel.CountryCode = userResponseDto.CountryCode;
			profileModel.FirstName = userResponseDto.FirstName;
			profileModel.LastName = userResponseDto.LastName;
			profileModel.StateCode = userResponseDto.StateCode;
			profileModel.PostCode = userResponseDto.PostCode;
			profileModel.ImagePath = string.IsNullOrEmpty(userResponseDto.ImagePath) ? Constants.DefaultProfileImage : userResponseDto.ImagePath;
		}

		public void Map(CountryLookupResponseDto countryLookupResponseDto, List<SelectListItem> listItems)
		{
			if (countryLookupResponseDto == null) { return; };
			if (listItems == null) { listItems = new List<SelectListItem>(); }
			if (countryLookupResponseDto.Success)
			{
				foreach (var lookup in countryLookupResponseDto.Countries)
				{
					SelectListItem item = new SelectListItem()
					{
						Value = lookup.CountryCode,
						Text = lookup.CountryName
					};
					listItems.Add(item);
				}
			}
		}

		public void Map(RegisterPartnerModel partnerModel, CreateApplicationOrganizationCommand partnerCommand)
		{
			if (partnerModel == null) { return; };

			if (partnerCommand == null) partnerCommand = new CreateApplicationOrganizationCommand();

			partnerCommand.UserName = partnerModel.UserName;
			partnerCommand.PasswordSalt = partnerModel.Password;
			partnerCommand.RoleId = (int)RoleTypes.Partner;
			partnerCommand.OrganizationName = partnerModel.OrganizationName;
			partnerCommand.Domain = partnerModel.Domain;
			partnerCommand.Description = partnerModel.Description;
			partnerCommand.Email = partnerModel.Email;
			partnerCommand.Phone = partnerModel.Phone;
			partnerCommand.CountryCode = partnerModel.CountryCode;
			partnerCommand.AddressLine1 = partnerModel.AddressLine1;
			partnerCommand.AddressLine2 = partnerModel.AddressLine2;
			partnerCommand.City = partnerModel.City;
			partnerCommand.StateCode = partnerModel.StateCode;
			partnerCommand.PostCode = partnerModel.PostCode;
		}

		public void Map(ApplicationOrganizationResponseDto dto, PartnerProfileModel model)
		{
			if (dto != null)
			{
				if (model == null) model = new PartnerProfileModel();

				model.UserId = dto.UserId;
				model.UserGuid = dto.UserGuid;
				model.UserName = dto.UserName;
				model.Email = dto.Email;
				model.Phone = dto.Phone;

				model.LogoUrl = string.IsNullOrEmpty(dto.LogoUrl) ? Constants.DefaultProfileImage : dto.LogoUrl;
				model.OrganizationId = dto.OrganizationId;
				model.Description = dto.Description;
				model.OrganizationName = dto.OrganizationName;

				model.AddressLine1 = dto.AddressLine1;
				model.AddressLine2 = dto.AddressLine2;
				model.City = dto.City;
				model.StateCode = dto.StateCode != null ? dto.StateCode.Value.ToString() : string.Empty;
				model.CountryCode = dto.CountryCode;
				model.PostCode = dto.PostCode;
			}
		}

		public void Map(GroupLookupResponseDto dto, ApplicationGroupModel model)
		{
			if (dto != null)
			{
				if (dto.Groups != null && dto.Groups.Count > 0)
				{
					if (model == null) model = new ApplicationGroupModel();
					var group = dto.Groups[0];

					model.GroupId = group.GroupId;
					model.GroupName = group.GroupName;
					model.Description = group.Description;
				}
			}
		}

		public void Map(IFormCollection form, DataTableRequestModel model)
		{
			if (model == null) model = new DataTableRequestModel();

			model.Draw = Convert.ToInt32(form["draw"].FirstOrDefault() ?? "1");
			model.SortColumn = form["columns[" + form["order[0][column]"].FirstOrDefault() + "][data]"].FirstOrDefault();
			model.SortDirection = form["order[0][dir]"].FirstOrDefault();
			model.SearchValue = form["search[value]"].FirstOrDefault();
			model.PageSize = Convert.ToInt32(form["length"].FirstOrDefault() ?? "0");
			model.Start = Convert.ToInt32(form["start"].FirstOrDefault() ?? "0");
		}

		public void Map(DataTableRequestModel model, DataTableRequestBase request)
		{
            request.Start = model.Start;
            request.PageSize = model.PageSize;
            request.SearchValue = model.SearchValue;
            request.SortColumn = model.SortColumn;
            request.SortDirection = model.SortDirection;
            request.Draw = model.Draw;
        }
	}
}
