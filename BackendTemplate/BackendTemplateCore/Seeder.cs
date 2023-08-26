using BackendTemplateCore.Enums;
using BackendTemplateCore.Models;
using BackendTemplateCore.Models.Address;
using BackendTemplateCore.Models.Brigade;
using BackendTemplateCore.Models.Company;
using BackendTemplateCore.Models.Extensions;
using BackendTemplateCore.Models.User;

namespace BackendTemplateCore;

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
		SeedEnum<BrigadeStatus, BrigadeStatuses>(),
		SeedEnum<BrigadeType, BrigadeTypes>(),

		(typeof(Permission), Enum.GetValues<PermissionAreas>().Select(x =>
			Enum.GetValues<PermissionTypes>().Select(y =>
				new Permission
				{
					Id = (int) y + (((int) x - 1) * 6),
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
			new()
			{
				Id = 2,
				Name = "Bahoruco",
				CountryId = 1,
				CreatedOn = DateTime.MinValue,
			},
			new()
			{
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
			new()
			{
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
			new()
			{
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
				Name = "San Cristóbal",
				CountryId = 1,
				CreatedOn = DateTime.MinValue,
			},
			new()
			{
				Id = 25,
				Name = "San José de Ocoa",
				CountryId = 1,
				CreatedOn = DateTime.MinValue,
			},
			new()
			{
				Id = 26,
				Name = "San Juan",
				CountryId = 1,
				CreatedOn = DateTime.MinValue,
			},
			new()
			{
				Id = 27,
				Name = "San Pedro de Macorís",
				CountryId = 1,
				CreatedOn = DateTime.MinValue,
			},
			new()
			{
				Id = 28,
				Name = "Sánchez Ramírez",
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
	};

	private static (Type, object[]) SeedBrigadeStatuses() => (typeof(BrigadeStatus), new [] {
      new BrigadeStatus { Id = (int)BrigadeStatuses.EnTrabajo,    Description = "En trabajo"     },
      new BrigadeStatus { Id = (int)BrigadeStatuses.Almuerzo,     Description = "Almuerzo"       },
      new BrigadeStatus { Id = (int)BrigadeStatuses.Averiada,     Description = "Averiada"       },
      new BrigadeStatus { Id = (int)BrigadeStatuses.FinDeJornada, Description = "Fin de jornada" },
      new BrigadeStatus { Id = (int)BrigadeStatuses.Inactivo,     Description = "Inactivo"       }
   });

	private static (Type, object[]) SeedBrigadeTypes() => (typeof(BrigadeType), new[]
   {
	   new BrigadeType { Id = (int)BrigadeTypes.Ligera, Description = BrigadeTypes.Ligera.ToString()},
	   new BrigadeType { Id = (int)BrigadeTypes.Canasto, Description = BrigadeTypes.Canasto.ToString()},
	   new BrigadeType { Id = (int)BrigadeTypes.Grua, Description = BrigadeTypes.Grua.ToString()},
	   new BrigadeType { Id = (int)BrigadeTypes.Motor, Description = BrigadeTypes.Motor.ToString()},
	   new BrigadeType { Id = (int)BrigadeTypes.Persona, Description = BrigadeTypes.Persona.ToString()},
	   new BrigadeType { Id = (int)BrigadeTypes.Automovil, Description = BrigadeTypes.Automovil.ToString()},
   });
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
			  new (){
				  Id = 1,
				  Name = "Santo Domingo",
				  StateId = 5,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 2,
				  Name = "Boca Chica",
				  StateId = 31,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 3,
				  Name = "Los Alcarrizos",
				  StateId = 31,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 4,
				  Name = "Pedro Brand",
				  StateId = 31,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 5,
				  Name = "San Antonio de Guerra",
				  StateId = 31,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 6,
				  Name = "Santo Domingo Este",
				  StateId = 31,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 7,
				  Name = "Santo Domingo Norte",
				  StateId = 31,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 8,
				  Name = "Santo Domingo Oeste",
				  StateId = 31,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 9,
				  Name = "Puerto Plata",
				  StateId = 22,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 10,
				  Name = "Altamira",
				  StateId = 22,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 11,
				  Name = "Guananico",
				  StateId = 22,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 12,
				  Name = "Imbert",
				  StateId = 22,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 13,
				  Name = "Los Hidalgos",
				  StateId = 22,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 14,
				  Name = "Luperón",
				  StateId = 22,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 15,
				  Name = "Sosúa",
				  StateId = 22,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 16,
				  Name = "Villa Isabela",
				  StateId = 22,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 17,
				  Name = "Villa Montellano",
				  StateId = 22,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 18,
				  Name = "Baitoa",
				  StateId = 29,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 19,
				  Name = "Jánico",
				  StateId = 29,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 20,
				  Name = "Licey al Medio",
				  StateId = 29,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 21,
				  Name = "Puñal",
				  StateId = 29,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 22,
				  Name = "Sabana Iglesia",
				  StateId = 29,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 23,
				  Name = "San José de las Matas",
				  StateId = 29,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 24,
				  Name = "Santiago",
				  StateId = 29,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 25,
				  Name = "Tamboril",
				  StateId = 29,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 26,
				  Name = "Villa Bisonó",
				  StateId = 29,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 27,
				  Name = "Villa González",
				  StateId = 29,
				  CreatedOn = DateTime.MinValue,
			  },
		  }),
	};

	private static Guid guid(byte id) => new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, id);
}