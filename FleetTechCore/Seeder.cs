using FleetTechCore.Enums;
using FleetTechCore.Models;
using FleetTechCore.Models.Address;
using FleetTechCore.Models.Company;
using FleetTechCore.Models.Extensions;
using FleetTechCore.Models.Fleet;
using FleetTechCore.Models.Fuel;
using FleetTechCore.Models.User;
using System.ComponentModel;

namespace FleetTechCore;

public static class Seeder {
	private static (Type, object[]) SeedEnum<T, TEnum>()
		where TEnum : struct, Enum
	{
		var props = typeof(T).GetProperties();
		var description_prop = props.First(p => p.PropertyType == typeof(string));
		var value_prop = props.First(p => p.PropertyType == typeof(int));

		return (typeof(T), Enum.GetValues<TEnum>()
			.Select(e => {
				var obj = Activator.CreateInstance(typeof(T))!;
				description_prop.SetValue(obj, e.ToString().PascalCaseWithInitialsToTitleCase());
				value_prop.SetValue(obj, (int)(object)e);
				return obj;
			}).ToArray());
	}

	public static (Type type, object[] data)[] Seeds(Func<string, string> pass) => new[]
	{
		SeedEnum<BranchType, BranchTypes>(),
		SeedEnum<PermissionArea, PermissionAreas>(),
		SeedEnum<PermissionType, PermissionTypes>(),

		(typeof(Permission), Enum.GetValues<PermissionAreas>().Select(x =>
			Enum.GetValues<PermissionTypes>().Select(y =>
				new Permission
				{
					Id = (int)y + (((int) x - 1) * 6),
					PermissionAreaId = (int) x,
					PermissionTypeId = (int) y,
				}).ToArray()).SelectMany(x => x).ToArray()),

		(typeof(Role), new Role[]
		{
			new()
			{
				Id = 1,
				Name = "SuperAdmin",
			},
		}),
		(typeof(CompanySetting), new CompanySetting[]
		{
			new()
			{
				Id = 1,
				TimeZone = "America/Santo_Domingo",
				DatePattern = "dd/MM/yyyy",
				TimePattern = "hh:mm tt",
				CreatedOn = DateTime.MinValue,
			}
		}),
		(typeof(UserRole), new UserRole[]
		{
			new()
			{
				Id = 1,
				UserId = guid(1),
				RoleId = 1,
			}
		}),
		(typeof(User), new User[]
		{
			new()
			{
				Id = guid(1),
				FirstName = "Super",
				LastName = "Admin",
				Status = 1,
				Username = "superadmin",
				Email = "superadmin@gmail.com",
				PasswordHash = pass("123Password"),
				Phone = "(829) 123-4567"
			}
		}),
		(typeof(Country), new Country[]
		{
			new()
			{
				Id = 1,
				Name = "Republica Dominicana",
				Demonym = "Dominicano (a)",
				CreatedOn = DateTime.MinValue,
			},
			new()
			{
				Id = 2,
				Name = "Estados Unidos",
				Demonym = "Estadounidense",
				CreatedOn = DateTime.MinValue,
			},
			new()
			{
				Id = 3,
				Name = "Canada",
				Demonym = "Canadiense",
				CreatedOn = DateTime.MinValue,
			},
			new()
			{
				Id = 4,
				Name = "Mexico",
				Demonym = "Mexicano (a)",
				CreatedOn = DateTime.MinValue,
			},
			new()
			{
				Id = 5,
				Name = "Haiti",
				Demonym = "Haitiano (a)",
				CreatedOn = DateTime.MinValue,
			},
			new()
			{
				Id = 6,
				Name = "Puerto Rico",
				Demonym = "Puertorricense",
				CreatedOn = DateTime.MinValue,
			},
			new()
			{
				Id = 7,
				Name = "China",
				Demonym = "Chino (a)",
				CreatedOn = DateTime.MinValue,
			},
			new()
			{
				Id = 8,
				Name = "Venezuela",
				Demonym = "Venezolano (a)",
				CreatedOn = DateTime.MinValue,
			},
			new()
			{
				Id = 9,
				Name = "Colombia",
				Demonym = "Colombiano (a)",
				CreatedOn = DateTime.MinValue,
			},
		}),
		(typeof(State), new State[]
		{
            new() 
			{ 
				Id = 1,	  
				Name = "Azua",	
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() { 
				Id = 2,	  
				Name = "Bahoruco", 
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() { 
				Id = 3,	  
				Name = "Barahona", 
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{ 
				Id = 4,	  
				Name = "Dajabón", 
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() { 
				Id = 5,	  
				Name = "Distrito Nacional", 
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{ 
				Id = 6,	  
				Name = "Duarte", 
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{ 
				Id = 7,	  
				Name = "Elías Piña", 
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{ 
				Id = 8,	  
				Name = "El Seibo", 
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{ 
				Id = 9,	  
				Name = "Espaillat", 
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{ 
				Id = 10,  
				Name = "Hato Mayor", 
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{ 
				Id = 11,  
				Name = "Hermanas Mirabal", 
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{ 
				Id = 12,  
				Name = "Independencia", 
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{ 
				Id = 13,  
				Name = "La Altagracia", 
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() { 
				Id = 14,  
				Name = "La Romana", 
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{ 
				Id = 15,  
				Name = "La Vega", 
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{ 
				Id = 16,  
				Name = "María Trinidad Sánchez",
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{ 
				Id = 17,  
				Name = "Monseñor Nouel", 
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{ 
				Id = 18,  
				Name = "Monte Cristi", 
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{ 
				Id = 19,  
				Name = "Monte Plata", 
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{ 
				Id = 20,  
				Name = "Pedernales", 
				CountryId = 1,
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{ 
				Id = 21,  
				Name = "Peravia",
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{	
				Id = 22,  
				Name = "Puerto Plata", 
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{
				Id = 23,  
				Name = "Samaná", 
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{ 
				Id = 24,  
				Name = "Sánchez Ramírez", 
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{ 
				Id = 25,  
				Name = "San Cristóbal", 
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{ 
				Id = 26,  
				Name = "San José de Ocoa",
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{ 
				Id = 27,  
				Name = "San Juan", 
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{ 
				Id = 28,
				Name = "San Pedro de Macorís", 
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{ 
				Id = 29,
				Name = "Santiago", 
				CountryId = 1,
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{ 
				Id = 30,
				Name = "Santiago Rodríguez",
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{ 
				Id = 31,
				Name = "Santo Domingo",
				CountryId = 1, 
				CreatedOn = DateTime.MinValue,   
			},
			new() 
			{ 
				Id = 32,
				Name = "Valverde",
				CountryId = 1,
				CreatedOn = DateTime.MinValue,
			}
        }),

		(typeof(LicenseType), new[] {
			  new LicenseType { Id = 1, Description = "Permiso de aprendizaje"                   },
			  new LicenseType { Id = 2, Description = "01a motocicletas y tricículos livianos"   },
			  new LicenseType { Id = 3, Description = "01b motocicletas y tricículos pesados"    },
			  new LicenseType { Id = 4, Description = "02 vehículos livianos"                    },
			  new LicenseType { Id = 5, Description = "02+R vehículos livianos con remolque"     },
			  new LicenseType { Id = 6, Description = "03a vehículos pesados"                    },
			  new LicenseType { Id = 7, Description = "03+R vehículos pesados con remolque"      },
			  new LicenseType { Id = 8, Description = "04 vehículos pesados de carga (patanas)"  },
			  new LicenseType { Id = 9, Description = "05 vehículos especiales"                  }
	     }),
         (typeof(FuelType), new[] {
              new FuelType { Id = 1, Name = "Gasolina Premium"  },
              new FuelType { Id = 2, Name = "Gasolina Regular"  },
              new FuelType { Id = 3, Name = "Diesel Premium"    },
              new FuelType { Id = 4, Name = "Diesel Regular"    },
              new FuelType { Id = 5, Name = "Gasoil Optimo"     },
              new FuelType { Id = 6, Name = "Gasoil Regular"    },
              new FuelType { Id = 7, Name = "Kerosene"			},
              new FuelType { Id = 8, Name = "Gas Licuado (GLP)" },
              new FuelType { Id = 9, Name = "Gas Natural (GNV)" },
         }),

    };
   public static (Type type, object[] data)[] DevelopmentSeeds() => new (Type, object[])[] {
		(typeof(Role), new Role[]
		{
			new ()
			{
				Id = 2,
				Name = "Cajero",
			},
			new ()
			{
				Id = 3,
				Name = "Atención al Cliente",
			},
			new ()
			{
				Id = 4,
				Name = "Enc. Comercial",
			},
			new ()
			{
				Id = 5,
				Name = "Analista de Recaudo",
			},
			new ()
			{
				Id = 6,
				Name = "Enc. de Recaudo",
			},
			new ()
			{
				Id = 7,
				Name = "Enc. de Facturación",
			},
		}),
		(typeof(RolePermission), new RolePermission[]
		{
			new ()
			{
				Id = 1,
				RoleId = 2,
				PermissionId = 108
			},
			new ()
			{
				Id = 2,
				RoleId = 2,
				PermissionId = 156,
			},
			new ()
			{
				Id = 3,
				RoleId = 2,
				PermissionId = 158,
			},
			new ()
			{
				Id = 4,
				RoleId = 2,
				PermissionId = 159,
			},
			new ()
			{
				Id = 5,
				RoleId = 2,
				PermissionId = 78,
			},
			new ()
			{
				Id = 6,
				RoleId = 2,
				PermissionId = 208,
			},
			// 
		}),
		(typeof(Company), new Company[]
		{
			new ()
			{
				Id = 1,
				Name = "DobarTec",
				Code = "DT",
				TaxRegistrationNumber = "9999",
				AddressLine1 = "Ave. Abraham Lincoln",
				AddressLine2 = "Edificio Blue Mall",
				AddressLine3 = "Piso 23",
				CityId = 1,
				Region= "",
				PostalCode = "10130",
				Phone = "8095864007",
				Email = "ppe@prueba.com",
				CompanySettingsId = 1,
				CreatedOn = DateTime.MinValue,
			}
		}),
		(typeof(City), new City[] {
            new ()
            {
                   Id = 1,
                   Name = "Distrito Nacional",
                   StateId = 5,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 2,
                   Name = "Azua de Compostela",
                   StateId = 1,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 3,
                   Name = "Estebanía",
                   StateId = 1,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 4,
                   Name = "Guayabal",
                   StateId = 1,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 5,
                   Name = "Las Charcas",
                   StateId = 1,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 6,
                   Name = "Las Yayas de Viajama",
                   StateId = 1,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 7,
                   Name = "Padre Las Casas",
                   StateId = 1,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 8,
                   Name = "Peralta",
                   StateId = 1,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 9,
                   Name = "Pueblo Viejo",
                   StateId = 1,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 10,
                   Name = "Sabana Yegua",
                   StateId = 1,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 11,
                   Name = "Tábara Arriba",
                   StateId = 1,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 12,
                   Name = "Neiba",
                   StateId = 2,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 13,
                   Name = "Galván",
                   StateId = 2,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 14,
                   Name = "Los Ríos",
                   StateId = 2,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 15,
                   Name = "Tamayo",
                   StateId = 2,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 16,
                   Name = "Villa Jaragua",
                   StateId = 2,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 17,
                   Name = "Barahona",
                   StateId = 3,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 18,
                   Name = "Cabral",
                   StateId = 3,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 19,
                   Name = "El Peñón",
                   StateId = 3,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 20,
                   Name = "Enriquillo",
                   StateId = 3,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 21,
                   Name = "Fundación",
                   StateId = 3,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 22,
                   Name = "Jaquimeyes",
                   StateId = 3,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 23,
                   Name = "La Ciénaga",
                   StateId = 3,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 24,
                   Name = "Las Salinas",
                   StateId = 3,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 25,
                   Name = "Paraíso",
                   StateId = 3,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 26,
                   Name = "Polo",
                   StateId = 3,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 27,
                   Name = "Vicente Noble",
                   StateId = 3,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 28,
                   Name = "Dajabón",
                   StateId = 4,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 29,
                   Name = "El Pino",
                   StateId = 4,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 30,
                   Name = "Loma de Cabrera",
                   StateId = 4,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 31,
                   Name = "Partido",
                   StateId = 4,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 32,
                   Name = "Restauración",
                   StateId = 4,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 33,
                   Name = "San Francisco de Macorís",
                   StateId = 6,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 34,
                   Name = "Arenoso",
                   StateId = 6,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 35,
                   Name = "Castillo",
                   StateId = 6,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 36,
                   Name = "Eugenio María de Hostos",
                   StateId = 6,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 37,
                   Name = "Las Guáranas",
                   StateId = 6,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 38,
                   Name = "Pimentel",
                   StateId = 6,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 39,
                   Name = "Villa Riva",
                   StateId = 6,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 40,
                   Name = "El Seibo",
                   StateId = 8,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 41,
                   Name = "Miches",
                   StateId = 8,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 42,
                   Name = "Comendador",
                   StateId = 7,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 43,
                   Name = "Bánica",
                   StateId = 7,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 44,
                   Name = "El Llano",
                   StateId = 7,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 45,
                   Name = "Hondo Valle",
                   StateId = 7,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 46,
                   Name = "Juan Santiago",
                   StateId = 7,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 47,
                   Name = "Pedro Santana",
                   StateId = 7,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 48,
                   Name = "Moca",
                   StateId = 9,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 49,
                   Name = "Cayetano Germosén",
                   StateId = 9,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 50,
                   Name = "Gaspar Hernández",
                   StateId = 9,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 51,
                   Name = "Jamao al Norte",
                   StateId = 9,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 52,
                   Name = "Hato Mayor del Rey",
                   StateId = 10,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 53,
                   Name = "El Valle",
                   StateId = 10,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 54,
                   Name = "Sabana de la Mar",
                   StateId = 10,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 55,
                   Name = "Salcedo",
                   StateId = 11,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 56,
                   Name = "Tenares",
                   StateId = 11,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 57,
                   Name = "Villa Tapia",
                   StateId = 11,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 58,
                   Name = "Jimaní",
                   StateId = 12,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 59,
                   Name = "Cristóbal",
                   StateId = 12,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 60,
                   Name = "Duvergé",
                   StateId = 12,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 61,
                   Name = "La Descubierta",
                   StateId = 12,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 62,
                   Name = "Mella",
                   StateId = 12,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 63,
                   Name = "Postrer Río",
                   StateId = 12,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 64,
                   Name = "Higüey",
                   StateId = 13,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 65,
                   Name = "San Rafael del Yuma",
                   StateId = 13,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 66,
                   Name = "La Romana",
                   StateId = 14,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 67,
                   Name = "Guaymate",
                   StateId = 14,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 68,
                   Name = "Villa Hermosa",
                   StateId = 14,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 69,
                   Name = "La Concepción de La Vega",
                   StateId = 15,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 70,
                   Name = "Constanza",
                   StateId = 15,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 71,
                   Name = "Jarabacoa",
                   StateId = 15,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 72,
                   Name = "Jima Abajo",
                   StateId = 15,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 73,
                   Name = "Nagua",
                   StateId = 16,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 74,
                   Name = "Cabrera",
                   StateId = 16,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 75,
                   Name = "El Factor",
                   StateId = 16,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 76,
                   Name = "Río San Juan",
                   StateId = 16,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 77,
                   Name = "Bonao",
                   StateId = 17,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 78,
                   Name = "Maimón",
                   StateId = 17,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 79,
                   Name = "Piedra Blanca",
                   StateId = 17,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 80,
                   Name = "Montecristi",
                   StateId = 18,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 81,
                   Name = "Castañuela",
                   StateId = 18,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 82,
                   Name = "Guayubín",
                   StateId = 18,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 83,
                   Name = "Las Matas de Santa Cruz",
                   StateId = 18,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 84,
                   Name = "Pepillo Salcedo",
                   StateId = 18,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 85,
                   Name = "Villa Vásquez",
                   StateId = 18,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 86,
                   Name = "Monte Plata",
                   StateId = 19,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 87,
                   Name = "Bayaguana",
                   StateId = 19,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 88,
                   Name = "Peralvillo",
                   StateId = 19,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 89,
                   Name = "Sabana Grande de Boyá",
                   StateId = 19,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 90,
                   Name = "Yamasá",
                   StateId = 19,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 91,
                   Name = "Pedernales",
                   StateId = 20,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 92,
                   Name = "Oviedo",
                   StateId = 20,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 93,
                   Name = "Baní",
                   StateId = 21,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 94,
                   Name = "Nizao",
                   StateId = 21,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 95,
                   Name = "Puerto Plata",
                   StateId = 22,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 96,
                   Name = "Altamira",
                   StateId = 22,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 97,
                   Name = "Guananico",
                   StateId = 22,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 98,
                   Name = "Imbert",
                   StateId = 22,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 99,
                   Name = "Los Hidalgos",
                   StateId = 22,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 100,
                   Name = "Luperón",
                   StateId = 22,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 101,
                   Name = "Sosúa",
                   StateId = 22,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 102,
                   Name = "Villa Isabela",
                   StateId = 22,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 103,
                   Name = "Villa Montellano",
                   StateId = 22,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 104,
                   Name = "Samaná",
                   StateId = 23,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 105,
                   Name = "Las Terrenas",
                   StateId = 23,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 106,
                   Name = "Sánchez",
                   StateId = 23,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 107,
                   Name = "San Cristóbal",
                   StateId = 25,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 108,
                   Name = "Bajos de Haina",
                   StateId = 25,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 109,
                   Name = "Cambita Garabito",
                   StateId = 25,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 110,
                   Name = "Los Cacaos",
                   StateId = 25,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 111,
                   Name = "Sabana Grande de Palenque",
                   StateId = 25,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 112,
                   Name = "San Gregorio de Nigua",
                   StateId = 25,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 113,
                   Name = "Villa Altagracia",
                   StateId = 25,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 114,
                   Name = "Yaguate",
                   StateId = 25,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 115,
                   Name = "San José de Ocoa",
                   StateId = 26,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 116,
                   Name = "Rancho Arriba",
                   StateId = 26,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 117,
                   Name = "Sabana Larga",
                   StateId = 26,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 118,
                   Name = "San Juan de la Maguana",
                   StateId = 27,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 119,
                   Name = "Bohechío",
                   StateId = 27,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 120,
                   Name = "El Cercado",
                   StateId = 27,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 121,
                   Name = "Juan de Herrera",
                   StateId = 27,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 122,
                   Name = "Las Matas de Farfán",
                   StateId = 27,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 123,
                   Name = "Vallejuelo",
                   StateId = 27,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 124,
                   Name = "San Pedro de Macorís",
                   StateId = 28,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 125,
                   Name = "Consuelo",
                   StateId = 28,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 126,
                   Name = "Guayacanes",
                   StateId = 28,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 127,
                   Name = "Quisqueya",
                   StateId = 28,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 128,
                   Name = "Ramón Santana",
                   StateId = 28,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 129,
                   Name = "San José de Los Llanos",
                   StateId = 28,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 130,
                   Name = "Cotuí",
                   StateId = 24,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 131,
                   Name = "Cevicos",
                   StateId = 24,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 132,
                   Name = "Fantino",
                   StateId = 24,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 133,
                   Name = "La Mata",
                   StateId = 24,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 134,
                   Name = "Santiago",
                   StateId = 29,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 135,
                   Name = "Bisonó",
                   StateId = 29,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 136,
                   Name = "Jánico",
                   StateId = 29,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 137,
                   Name = "Licey al Medio",
                   StateId = 29,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 138,
                   Name = "Puñal",
                   StateId = 29,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 139,
                   Name = "Sabana Iglesia",
                   StateId = 29,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 140,
                   Name = "San José de las Matas",
                   StateId = 29,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 141,
                   Name = "Tamboril",
                   StateId = 29,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 142,
                   Name = "Villa González",
                   StateId = 29,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 143,
                   Name = "San Ignacio de Sabaneta",
                   StateId = 30,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 144,
                   Name = "Los Almácigos",
                   StateId = 30,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 145,
                   Name = "Monción",
                   StateId = 30,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 146,
                   Name = "Santo Domingo Este",
                   StateId = 31,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 147,
                   Name = "Boca Chica",
                   StateId = 31,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 148,
                   Name = "Los Alcarrizos",
                   StateId = 31,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 149,
                   Name = "Pedro Brand",
                   StateId = 31,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 150,
                   Name = "San Antonio de Guerra",
                   StateId = 31,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 151,
                   Name = "Santo Domingo Norte",
                   StateId = 31,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 152,
                   Name = "Santo Domingo Oeste",
                   StateId = 31,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 153,
                   Name = "Mao",
                   StateId = 32,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 154,
                   Name = "Esperanza",
                   StateId = 32,
                   CreatedOn = DateTime.MinValue,
            },
            new ()
            {
                   Id = 155,
                   Name = "Laguna Salada",
                   StateId = 32
            }
        
        }),
	};

	private static Guid guid(byte id) => new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, id);
}