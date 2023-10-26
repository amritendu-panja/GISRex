Select
	a."UserId",
	a."UserName",
    a."UserGuid",
    b."FirstName",
    b."LastName",
	b."ImagePath",
	b."CountryCode",
	c."RoleId",
	c."Role"
From public."ApplicationUser" a
Left Join public."ApplicationUserDetails" b
on a."UserId" = b."UserId"
Left Join lookups."UserRoleLookup" c
on a."RoleId" = c."RoleId"
Where a."RoleId" in (1, 3)
Order by a."ModifiedDate" Desc
Limit @p_count;