Select Count(*) as "RecordsTotal"
FROM public."ApplicationUser" AS a
Where a."RoleId" = ANY(@RoleIds) AND (@OrganizationId Is NULL Or a."OrganizationId" = @OrganizationId);

Select Count(*) as "RecordsFiltered"
FROM public."ApplicationUser" AS a
INNER JOIN public."ApplicationUserDetails" AS a0 ON a."UserId" = a0."UserId"
WHERE a."RoleId" = ANY(@RoleIds) AND 
	(@OrganizationId Is NULL Or a."OrganizationId" = @OrganizationId) AND
	(@SearchValue Is NULL OR @SearchValue = '' OR
    ((lower(a."UserName") LIKE @SearchValue) OR
        (lower(a."Email") LIKE @SearchValue) OR 
        (lower(a0."FirstName") LIKE @SearchValue) OR 
        (lower(a0."LastName") LIKE @SearchValue)));

SELECT 
	t."UserId",
	t."UserName",
	t."ImagePath", 
	t."Email",
	t."FirstName",
	t."LastName",
	t."UserGuid", 
	t."IsEnabled", 
	t."IsPasswordExpired", 
	t."IsUserLocked",
	t."RoleId", 
	t."Role" as "RoleName",	
	t."CountryCode"	
FROM (
    SELECT a."UserId", a."UserGuid", a."Email", a."IsEnabled", a."IsPasswordExpired", a."IsUserLocked", a."RoleId", a."UserName", a0."CountryCode", a0."FirstName", a0."ImagePath", a0."LastName", u."Role"
    FROM public."ApplicationUser" AS a
    INNER JOIN public."ApplicationUserDetails" AS a0 ON a."UserId" = a0."UserId"
	INNER JOIN lookups."UserRoleLookup" AS u ON a."RoleId" = u."RoleId"
    WHERE a."RoleId" = ANY(@RoleIds) AND
		(@OrganizationId Is NULL Or a."OrganizationId" = @OrganizationId) AND
		(@SearchValue Is NULL OR @SearchValue = '' OR
		((lower(a."UserName") LIKE @SearchValue) OR
		 (lower(a."Email") LIKE @SearchValue) OR 
		 (lower(a0."FirstName") LIKE @SearchValue) OR 
		 (lower(a0."LastName") LIKE @SearchValue)))    
) AS t
ORDER BY t."{0}" {1}
LIMIT @PageSize OFFSET @Start; 

