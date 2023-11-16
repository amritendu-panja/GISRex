Select
	a."UserId",
	a."UserName",
    a."UserGuid",
    b."FirstName",
    b."LastName",
	b."ImagePath",
	b."CountryCode",
	c."RoleId",
	c."Role",
	d."OrganizationId",
	d."OrganizationName",
	d."LogoUrl"
From public."ApplicationUser" a
Left Join public."ApplicationUserDetails" b
on a."UserId" = b."UserId"
Left Join lookups."UserRoleLookup" c
on a."RoleId" = c."RoleId"
Left Join public."ApplicationPartnerOrganization" d
on a."OrganizationId" = d."OrganizationId"
Where a."RoleId" = 3
and a."OrganizationId" = @org_id
Order by a."ModifiedDate" Desc
Limit @p_count;