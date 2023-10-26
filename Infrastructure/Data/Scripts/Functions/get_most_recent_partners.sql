-- FUNCTION: public.get_most_recent_partners(integer)

-- DROP FUNCTION public.get_most_recent_partners(integer);

CREATE OR REPLACE FUNCTION public.get_most_recent_partners(
	p_count integer)
    RETURNS TABLE(organizationid bigint, organizationname character varying, logourl character varying, countrycode character varying) 
    LANGUAGE 'plpgsql'

    COST 100
    VOLATILE 
    ROWS 1000
AS $BODY$
Declare
	partner_record record;
Begin
	For partner_record in (
		Select
			"OrganizationId",
			"OrganizationName",
			"LogoUrl",
			"CountryCode"
		From public."ApplicationPartnerOrganization"
		Limit p_count
	)
	Loop 
		OrganizationId:= partner_record."OrganizationId";
		OrganizationName:= partner_record."OrganizationName";
		LogoUrl:= partner_record."LogoUrl";
		CountryCode:= partner_record."CountryCode";
		Return NEXT;
	End Loop;
End;
$BODY$;