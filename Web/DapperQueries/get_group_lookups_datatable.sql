Select Count(*) as "RecordsTotal"
FROM lookups."ApplicationGroupLookup" AS a
Where a."GroupId" in @selectedGroupIds;

Select Count(*) as "RecordsFiltered"
FROM public."ApplicationGroupLookup" AS a
WHERE a."GroupId" in @selectedGroupIds AND 
	(@SearchValue Is NULL OR @SearchValue = '' OR
    ((lower(a."GroupName") LIKE @SearchValue) OR
        (lower(a."Description") LIKE @SearchValue)));

Select 
    a."GroupId",
    a."GroupName",
    a."Description"
FROM public."ApplicationGroupLookup" AS a
WHERE a."GroupId" in @selectedGroupIds AND 
	(@SearchValue Is NULL OR @SearchValue = '' OR
    ((lower(a."GroupName") LIKE @SearchValue) OR
        (lower(a."Description") LIKE @SearchValue)))
ORDER BY a."{0}" {1}
LIMIT @PageSize OFFSET @Start;