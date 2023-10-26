Select Count(*) as "RecordsTotal"
FROM public."ApplicationUser" AS a
Where a."RoleId" = 2;

Select Count(*) as "RecordsFiltered"
FROM public."ApplicationUser" AS a
INNER JOIN public."ApplicationPartnerOrganization" AS a0 
ON a."UserId" = a0."UserId"
WHERE a."RoleId" = 2 AND 
	(@SearchValue Is NULL OR @SearchValue = '' OR
    ((lower(a."UserName") LIKE @SearchValue) OR
        (lower(a."Email") LIKE @SearchValue) OR 
        (lower(a0."OrganizationName") LIKE @SearchValue) OR 
        (lower(a0."DomainName") LIKE @SearchValue)));

SELECT 
	t."UserId",
	t."UserName",
	t."LogoUrl", 
	t."Email",
    t."OrganizationId",
	t."OrganizationName",
	t."DomainName",	
	t."IsEnabled", 
	t."IsPasswordExpired", 
	t."IsUserLocked",	
	t."CountryCode"	
FROM (
    SELECT a."UserId", a."UserName", a0."LogoUrl", a."Email", a0."OrganizationId", a0."OrganizationName", a0."DomainName", a."IsEnabled", a."IsPasswordExpired", a."IsUserLocked", a0."CountryCode"
    FROM public."ApplicationUser" AS a
    INNER JOIN public."ApplicationPartnerOrganization" AS a0 
    ON a."UserId" = a0."UserId"	
    WHERE a."RoleId" = 2 AND 
		(@SearchValue Is NULL OR @SearchValue = '' OR
		((lower(a."UserName") LIKE @SearchValue) OR
		 (lower(a."Email") LIKE @SearchValue) OR 
		 (lower(a0."OrganizationName") LIKE @SearchValue) OR 
		 (lower(a0."DomainName") LIKE @SearchValue)))    
) AS t
ORDER BY t."{0}" {1}
LIMIT @PageSize OFFSET @Start; 