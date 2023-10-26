Select
	"OrganizationId",
	"OrganizationName",
	"LogoUrl",
	"CountryCode"
From public."ApplicationPartnerOrganization"
Order by "ModifiedDate" Desc
Limit @p_count;