using Microsoft.EntityFrameworkCore.Migrations;

namespace BASE.Data.Repository
{
    public static class CreatePostgresqlFunction
    {
        public static void CreatePostgresFunction(this MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Create_Filter_Spatial_Prop();
			migrationBuilder.Create_Map_Filter();
			migrationBuilder.Create_Merge_Layer();
			migrationBuilder.Create_Radius_Degree();
			migrationBuilder.Create_Parse_Where_Clause();
			migrationBuilder.Create_Spatial_Search();
			migrationBuilder.Create_Utm2wgs();
			migrationBuilder.Create_Wgs2utm();
			migrationBuilder.Create_isnull();
		}

        private static void Create_Filter_Spatial_Prop(this MigrationBuilder migrationBuilder)
        {
            var sql=@"CREATE OR REPLACE FUNCTION geogis.filter_spatial_prop(
					shp text,
					r double precision,
					searchtype text,
					_tbname text,
					_values text,
					_lang character varying)
					RETURNS text
					LANGUAGE 'plpgsql'

					COST 100
					VOLATILE 
				AS $BODY$
				declare g text; tbid integer;kq text; coor text; _radius_dgree text; _order text; filter_out text;datatype text;
				begin
					if searchtype in('Box','Polygon','LineString') then
						g = shp;
						_radius_dgree = '';
					elsif searchtype in ('Point','Circle') then
						g = ST_astext(geogis.utm2wgs(ST_Buffer(geogis.wgs2utm(ST_GeomFromText(shp)),r)));
						_radius_dgree= geogis.radius_degree(shp, r);
					else 
					return 'invalid';
					end if;
	 
					execute 'select GeometryType(geom) as type from geogis.' || _tbname || ' limit 1' into datatype;
					execute 'select case when ''' || lower(datatype) || ''' = ' || '''point''' ||  ' then '  || ''', ST_X(geom) x, ST_Y(geom) y'''  || ' else ' || ''',ST_Astext(geom) geo'''  || ' end  from geogis.' ||_tbname || ' limit 1' into coor;
					select into _order filter -> 'order' as output from cms.layer where ""table"" = 'geogis.' ||  _tbname;
					--_order = replace((filter_out->'filter'->'order')::text, '""', '');
							raise notice '%','select * , ST_AsText(geom) geo  from geogis.' || _tbname || ' where ST_Intersects(geom,ST_GeomFromText(''' || g || ''',4326))' || case when _values<>'' then ' and ' || geogis.parse_where_clause(_values) else '' end || ')';
							execute 'with t as(select *,ST_AsText(geom) geo  from geogis.' || _tbname || ' where ST_Intersects(geom,ST_GeomFromText(''' || g || ''',4326))' || case when _values<>'' then ' and ' || geogis.parse_where_clause(_values) else '' end || ')  select  array_to_json(array_agg(row_to_json(t))) from t' into kq;
							return '{""circle"":""' || _radius_dgree || ',' || shp || '"", ""data"":' || kq || '}';

							end
					$BODY$;

							ALTER FUNCTION geogis.filter_spatial_prop(text, double precision, text, text, text, character varying)

					OWNER TO postgres;
			";
			migrationBuilder.Sql(sql);
		}

		private static void Create_Map_Filter(this MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION geogis.map_filter(
					tbname text,
					_values text,
					_lang character varying)
					RETURNS text
					LANGUAGE 'plpgsql'

					COST 100
					VOLATILE 
				AS $BODY$

				declare kq text;

				declare config text; idtable int; _order text;cols_output text; f json; datatype text;
				BEGIN
					execute 'select GeometryType(geom) as type from geogis. ' ||tbname || ' limit 1' into datatype;
					select into f (filter -> 'out') as output from cms.layer where ""table"" = tbname;
					config = f;
							_order = '{""filter"":' || f || '}';
							raise NOTICE '%', replace(_order, '""', '');
							execute 'select array_to_json(array_agg(row_to_json(t)))::text from (select * ' || case when lower(datatype) = 'point' then ', ST_x(geom) x, ST_Y(geom) y ' else ', ST_AsText(geom) geo ' end || ' from geogis.' || tbname || case when _values<> '' then ' where ' || geogis.parse_where_clause(_values) else '' end || ')t' into kq;
							raise NOTICE '%', public.""isnull""(kq,'[]'::text);
  					 return '{""config"":"""",""data"":'|| public.""isnull""(kq,'[]'::text) ||'}';
				END

				$BODY$;

				ALTER FUNCTION geogis.map_filter(text, text, character varying)
					OWNER TO postgres;
				");
        }

		private static void Create_Merge_Layer(this MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION geogis.merge_layer(
					tb text,
					_name_tb character varying,
					_planning_code character varying,
					_idmap bigint,
					_tiff_name character varying,
					_boundaries_file_name character varying)
					RETURNS text
					LANGUAGE 'plpgsql'

					COST 100
					VOLATILE 
				AS $BODY$
				declare q text;
				begin
				with t as (select regexp_split_to_table(tb,',') tbname)
				select 'select ST_AsText(ST_BuildArea(ST_Union(d))) geom_text,
				ST_Union(d) geom, '|| _idmap ||', '''|| _name_tb ||''', '''|| _planning_code ||''', '''|| _tiff_name ||''', '''|| _boundaries_file_name ||''' from(
				select ST_BuildArea(ST_Union(geom)) d from '|| tb ||'
				union all
				select ST_Union(ST_BuildArea(geom)) d from '|| tb ||')p' into q
				from t;
				raise notice '%', q;
				--create table if not exists geogis.boundaries(id serial, geom geometry, idmap bigint, name_tb character varying(255) , CONSTRAINT boundaries_pkey PRIMARY KEY (id));
				--idmap ='|| _idmap ||' or
				execute 'delete from geogis.boundaries where planning_code ='''|| _planning_code ||''';
				insert into geogis.boundaries(geom_text,geom, idmap, name_tb,planning_code, tiff_name, boundaries_file_name)
				' || q ;
				return '';
				END
				$BODY$;

				ALTER FUNCTION geogis.merge_layer(text, character varying, character varying, bigint, character varying, character varying)
					OWNER TO postgres;

				");
        }

		private static void Create_Radius_Degree(this MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"CREATE OR REPLACE FUNCTION geogis.radius_degree(
								shp character varying,
								_radius double precision)
								RETURNS text
								LANGUAGE 'plpgsql'
								COST 100
								VOLATILE 
							AS $BODY$
							declare s1 text; center geometry; circle geometry;
							begin
								center = ST_Transform(ST_SetSrid(ST_GeomFromText(shp),4326),32648);
								circle = ST_Transform(ST_Buffer(center,_radius),4326);
								with t as(select (ST_DumpPoints(circle)).geom g)
								,t1 as
								(
									select _radius,ST_Distance(ST_SetSrid(g,4326),ST_Transform(center,4326))::double precision r from t limit 1

								)
								select r into s1 from t1;
								return s1;
							end
							$BODY$;
							ALTER FUNCTION geogis.radius_degree(character varying, double precision)
								OWNER TO postgres;
							");
        }
		private static void Create_Parse_Where_Clause(this MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION geogis.parse_where_clause(
					ip text)
					RETURNS text
					LANGUAGE 'plpgsql'

					COST 100
					VOLATILE 
				AS $BODY$
				declare output text;
				begin
					with t as(select value j from json_array_elements(ip::json) ),
						k as(select j->>'col' col, (j->>'op') op,j->>'v' v , j->>'type' tp from t)
						,r as (select case when tp = 'date' or tp='number' or op='LESS' or op ='GREATER' or op='BETWEEN' then col else  'lower(' || col || '::text)::text'  end || ' '
							   || case when op ='LESS' then '<' when op='GREATER' then '>' else op end || ' '|| 
							   case when op='LIKE' then '''%'|| v ||'%'''  
							   when op='=' then case when tp='date' then 'to_timestamp(''' || v || ''',''DD/MM/YYYY'')' 
               										when tp='number' then v
												   else ''''||v||'''' end
							   when (op='LESS' or op='GREATER') then v
							   when (op='BETWEEN') then replace(btrim(replace(replace(v,'[',''),']',''),' '),' ',' and ')
							   end   s  from k)
						select array_to_string(array_agg(s),' and ') into output from r;
					return output;
					--select geogis.parse_where_clause('[{""col"":""dientich"",""op"":""GREATER"",""v"":""200""}]')
				end
				$BODY$;

							ALTER FUNCTION geogis.parse_where_clause(text)

					OWNER TO postgres;

			");

		}
		private static void Create_Spatial_Search(this MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"CREATE OR REPLACE FUNCTION geogis.spatial_search(
					shp text,
					r double precision,
					searchtype text,
					_tbname text,
					_lang character varying)
					RETURNS text
					LANGUAGE 'plpgsql'
					COST 100
					VOLATILE 
				AS $BODY$
				declare g text; tbid integer;kq text; coor text; _radius_dgree text; sql text;
				begin
					_radius_dgree = case when r = 0 then '' else geogis.radius_degree(shp, r) end ;
	
					if _radius_dgree = '' then
						g = shp;
						_radius_dgree = case when r = 0 then '' else geogis.radius_degree(shp, r) end ;
					else
						g = ST_astext(geogis.utm2wgs(ST_Buffer(geogis.wgs2utm(ST_GeomFromText(shp)),r)));
						_radius_dgree= geogis.radius_degree(shp, r);

					end if;
	
					--select case when type ='point' then ', ST_X(geom) x, ST_Y(geom) y ' else ' ,ST_Astext(geom) geo ' end 
					--from geogis.layers where tbname = 'c.' || _tbname into coor;
					coor = ',ST_Astext(geom) geo';
					sql ='with t as(select * '|| coor || ' from geogis.' || _tbname || ' where ST_Intersects(geom,ST_GeomFromText('''|| g ||''',4326)))  select  array_to_json(array_agg(row_to_json(t))) from t' ;

					raise notice '%', 'SQL  ' || sql;
					execute sql into kq;
					return '{""circle"":""' || _radius_dgree || ', '|| shp || '"", ""data"":' || kq || '}';
					--select * from geogis.spatial_search('LINESTRING(105.82107639312746 19.852226257324222,105.8043909072876 19.838733673095703,105.80928325653076 19.832468032836914,105.81220149993896 19.83332633972168)'
					--, 200, 'LineString', 'qhc_tpthanhhoa_chucnangsudungdat', 'vn')
				 --
				end
				$BODY$;
			ALTER FUNCTION geogis.spatial_search(text, double precision, text, text, character varying)
	OWNER TO postgres;
			");

		}
		private static void Create_Utm2wgs(this MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION geogis.utm2wgs(
					geom geometry)
					RETURNS geometry
					LANGUAGE 'plpgsql'

					COST 100
					VOLATILE 
				AS $BODY$
				begin
					return ST_Transform(ST_SetSrid(geom,32648),4326);
					end
				$BODY$;
				ALTER FUNCTION geogis.utm2wgs(geometry)
					OWNER TO postgres;

				");
        }
		private static void Create_Wgs2utm(this MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"
				CREATE OR REPLACE FUNCTION geogis.wgs2utm(
					geom geometry)
					RETURNS geometry
					LANGUAGE 'plpgsql'

					COST 100
					VOLATILE 
				AS $BODY$
				begin
					return ST_Transform(ST_SetSrid(geom,4326),32648);
					end
				$BODY$;
				ALTER FUNCTION geogis.wgs2utm(geometry)
					OWNER TO postgres;

			");
        }
		private static void Create_isnull(this MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"CREATE OR REPLACE FUNCTION public.""isnull""(
					a text,
					b text)
					RETURNS text
					LANGUAGE 'plpgsql'
					COST 100
					VOLATILE
				AS $BODY$
				begin
					return case when a is not null then a else b end;
							end
				$BODY$;
							ALTER FUNCTION public.""isnull""(text, text)
								OWNER TO postgres;");
		}
	}
}
