using BackendTemplateCore.Models;
using BackendTemplateCore.Models.Billings;
using BackendTemplateCore.Models.Customers;
using BackendTemplateCore.Models.FieldServices;
using BackendTemplateCore.Models.FieldServices.Orders;
using BackendTemplateCore.Models.FieldServices.Readouts;
using BackendTemplateCore.Models.FieldServices.Relationships;
using BackendTemplateCore.Models.Invoices;
using BackendTemplateCore.Models.Payments;
using BackendTemplateCore.Models.Products;
using BackendTemplateCore.Models.Subscriptions;

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

	public static (Type type, object[] data)[] Seeds(Func<string, string> pass) => new[] {
		SeedEnum<AreaType, AreaTypes>(),
		SeedEnum<NCFType, NCFTypes>(),
		SeedEnum<DocumentType, DocumentTypes>(),
		SeedEnum<PaymentMethodType, PaymentMethodTypes>(),
		SeedEnum<PaymentStatus, PaymentStatuses>(),
		SeedEnum<InvoiceStatus, InvoiceStatuses>(),
		SeedEnum<Frequency, Frequencies>(),
		SeedEnum<ContactType, ContactTypes>(),
		SeedEnum<SubscriptionType, SubscriptionTypes>(),
		SeedEnum<DeliveryMethod, DeliveryMethods>(),
		SeedEnum<BillingType, BillingTypes>(),
		SeedEnum<PriceType, PriceTypes>(),
		SeedEnum<ConnectionType, ConnectionTypes>(),
		SeedEnum<BranchType, BranchTypes>(),
		SeedEnum<ReadingType, ReadingTypes>(),
		SeedEnum<MeasuringType, MeasuringTypes>(),
		SeedEnum<PermissionArea, PermissionAreas>(),
		SeedEnum<PermissionType, PermissionTypes>(),

		SeedIssueTypes(),
		SeedOrderTasks(),
		SeedOrderTypes(),
		SeedOrderStatuses(),
		SeedOrderTypeOrderTaskRelationships(),
		SeedBrigadeStatuses(),
		SeedBrigadeTypes(),
		
		(typeof(Permission), Enum.GetValues<PermissionAreas>().Select(x =>
			Enum.GetValues<PermissionTypes>().Select(y => 
				new Permission
				{
						Id = (int)y + (((int)x - 1)  * 6),
						PermissionAreaId = (int)x,
						PermissionTypeId = (int)y,
				}).ToArray()).SelectMany(x => x).ToArray()),
		
		(typeof(Extension), new Extension[]
		{
			new ()
			{
				Id = (int)ExtensionIdentifiers.ReadoutService,
				Name = "Repositorio de Lectura",
				Status = 0,
				CreatedOn = DateTime.MinValue
			},
			new ()
			{
				Id = (int)ExtensionIdentifiers.MunicipiaService,
				Name = "Municipia",
				Status = 0,
				CreatedOn = DateTime.MinValue
			},
			new ()
			{
				Id = (int)ExtensionIdentifiers.SicflexService,
				Name = "Sicflex",
				Status = 0,
				CreatedOn = DateTime.MinValue
			},
		}),
		
		(typeof(AnomalyType), Enum.GetValues<AnomalyTypes>().Select(x =>
			new AnomalyType
			{
				Id = (int)x,
				Name = x.ToString().PascalCaseWithInitialsToTitleCase(),
			}).ToArray()),
		
		(typeof(AnomalyStatus), Enum.GetValues<AnomalyStatuses>().Select(x =>
			new AnomalyStatus
			{
				Id = (int)x,
				Name = x.ToString().PascalCaseWithInitialsToTitleCase(),
			}).ToArray()),
		
		(typeof(AnomalyResolutionType), Enum.GetValues<AnomalyResolutionTypes>().Select(x =>
			new AnomalyResolutionType
			{
				Id = (int)x,
				Name = x.ToString().PascalCaseWithInitialsToTitleCase(),
			}).ToArray()),
		
		(typeof(ERPCodeDescription), new ERPCodeDescription[]
		{
			new () { Id = 1, Code = 113103, Description ="IMPUESTO SOBRE LAS OPERACIONES INMOBILIARIAS"},
			new () { Id = 2, Code = 113111, Description ="IMPUESTO SOBRE TERRENOS NO URBANIZADOS"},
			new () { Id = 3, Code = 113112, Description ="IMPUESTO SOBRE SOLARES NO EDIFICADOS"},
			new () { Id = 4,  Code = 113113, Description ="CONTRIBUCIONES MUNICIPALES"},
			new () { Id = 5, Code = 113217, Description ="RECARGOS POR MORA, MULTAS Y SANCIONES CONTRIBUCIONES MUNICIPALES"},
			new () { Id = 6, Code = 114103, Description ="IMPUESTO SOBRE VENTAS CONDICIONALES DE MUEBLES"},
			new () { Id = 7, Code = 114302, Description ="DERECHO DE CIRCULACIÓN VEHÍCULOS DE MOTOR"},
			new () { Id = 8, Code = 114316, Description ="SOLICITUD DE ARRENDAMIENTO EDIFICIOS MUNICIPALES"},
			new () { Id = 9, Code = 114318, Description ="ANUNCIOS, MUESTRAS Y CARTELES"},
			new () { Id = 10, Code = 114319, Description ="RODAJE Y TRANSPORTE DE MATERIALES VARIOS"},
			new () { Id = 11, Code = 114320, Description ="HOTELES, MOTELES Y APART- HOTELES Y ESTABLECIMIENTOS SIMILARES"},
			new () { Id = 12, Code = 114321, Description ="CERTIFICACIONES DE ANIMALES"},
			new () { Id = 13, Code = 114323, Description ="MERCADO MÓVIL(CHIMI, HOTDOG Y OTROS)"},
			new () { Id = 14, Code = 114324, Description ="AUTORIZACIÓN PARA PODA Y CORTE DE ARBOLES"},
			new () { Id = 15, Code = 114325, Description ="REGISTRO Y ORGANIZACIÓN SINDICATO DE CHOFERES"},
			new () { Id = 16, Code = 114326, Description ="FUNCIONAMIENTO CAR WASH"},
			new () { Id = 17, Code = 114327, Description ="PARQUEOS"},
			new () { Id = 18, Code = 114328, Description ="IMPUESTO SOBRE TRAMITACIÓN DE DOCUMENTOS"},
			new () { Id = 19, Code = 114329, Description ="IMPUESTO SOBRE REGISTRO DE DOCUMENTOS"},
			new () { Id = 20, Code = 114330, Description ="IMPUESTO LIDIAS DE GALLO"},
			new () { Id = 21, Code = 114331, Description ="IMPUESTO SOBRE BILLARES"},
			new () { Id = 22, Code = 114332, Description ="ESPECTÁCULOS PUBLICOS CON O SIN BOLETA DE ENTRADA"},
			new () { Id = 23, Code = 114333, Description ="LICENCIA DE CONSTRUCCIÓN"},
			new () { Id = 24, Code = 114334, Description ="PERMISO CONSTRUCCIÓN POZOS FILTRANTES"},
			new () { Id = 25, Code = 114335, Description ="PERMISO PARA ROMPER PAVIMENTO DE LA VÍA PUBLICA"},
			new () { Id = 26, Code = 114336, Description ="INSTALACIÓN ENVASADORA DE GAS Y ESTACIÓN DE COMBUSTIBLE."},
			new () { Id = 27, Code = 114337, Description ="OCUPACIÓN VÍA PUBLICA COMERCIO INFORMAL"},
			new () { Id = 28, Code = 114338, Description ="PERMISO PARA OCUPAR VÍA PUBLICA CON MATERIALES DE CONSTRUCCIÓN"},
			new () { Id = 29, Code = 114339, Description ="PERMISO POR USUFRUCTO VÍA PUBLICA CARGA Y DESCARGA DE MERCANCÍAS"},
			new () { Id = 30, Code = 114340, Description ="INSTALACIÓN CAR WASH"},
			new () { Id = 31, Code = 114342, Description ="CONSTRUCCIÓN: NICHOS, FOSAS Y PANTEONES"},
			new () { Id = 32, Code = 114343, Description ="CONSTRUCCIÓN DE RAMPA CON EXCESO DE METROS LINEALES"},
			new () { Id = 33, Code = 114344, Description ="LICENCIA PARA INSTALACIÓN DE TELECOMUNICACIONES"},
			new () { Id = 34, Code = 114345, Description ="PERMISO PARA DEMOLICIÓN DE CONSTRUCCIONES"},
			new () { Id = 35, Code = 114346, Description ="PERMISO PARA OPERACIÓN DE MERCADOS"},
			new () { Id = 36, Code = 114347, Description ="PARADA Y TERMINAR DE AUTOBUSES"},
			new () { Id = 37, Code = 116101, Description ="COMPENSACIÓN POR DAÑOS AL MEDIO AMBIENTE Y VÍA PUBLICA"},
			new () { Id = 38, Code = 116201, Description ="RECARGOS POR MORA"},
			new () { Id = 39, Code = 119103, Description ="COMPENSACIÓN SOBRE EL PAGO DE FACTURACIÓN, ENERGÍA ELECTRICA 3%"},
			new () { Id = 40, Code = 119104, Description ="OTROS ARBITRIOS DIVERSOS"},
			new () { Id = 41, Code = 119105, Description ="USO DE APARATOS REPRODUCTORES DE MUSICA DIVERSOS"},
			new () { Id = 42, Code = 119199, Description ="OTROS IMPUESTOS DIVERSOS"},
			new () { Id = 43, Code = 131301, Description ="DONACIONES CORRIENTES EN DINEROS DEL SECTOR PRIVADO"},
			new () { Id = 44, Code = 144322, Description ="TRASPASO DE SOLARES Y TERRENOS RURALES"},
			new () { Id = 45, Code = 151220, Description ="USO DE RAMPAS"},
			new () { Id = 46, Code = 151309, Description ="TRAMITACIÓN DE PLANO"},
			new () { Id = 47, Code = 151311, Description ="SERVICIOS FUNERARIOS"},
			new () { Id = 48, Code = 151312, Description ="SUPERVISIÓN Y FISCALIZACIÓN DE OBRAS"},
			new () { Id = 49, Code = 151313, Description ="LIMPIEZA DE SOLARES YERMOS"},
			new () { Id = 50, Code = 151314, Description ="INHUMACIÓN Y EXHUMACIÓN"},
			new () { Id = 51, Code = 151315, Description ="EXPEDICIÓN CERTIFICACIONES"},
			new () { Id = 52, Code = 151316, Description ="ESTUDIO DE USO DE SUELO"},
			new () { Id = 53, Code = 151317, Description ="GARAJES"},
			new () { Id = 54, Code = 151318, Description ="CERTIFICACIONES VIDA Y COSTUMBRE"},
			new () { Id = 55, Code = 151320, Description ="RECOLECCIÓN DESECHOS SÓLIDOS"},
			new () { Id = 56, Code = 151322, Description ="TASAS POR DECLARACIÓN TARDÍA ZONA RURAL"},
			new () { Id = 57, Code = 151327, Description ="CERTIFICACIÓN USO DE SUELO"},
			new () { Id = 58, Code = 151328, Description ="CERTIFICACIÓN PARA CAMBIO DE USO DE SUELO"},
			new () { Id = 59, Code = 151431, Description ="ESTACIONAMIENTO VÍA PUBLICA"},
			new () { Id = 60, Code = 151432, Description ="REGISTROS DE ACTOS CIVILES"},
			new () { Id = 61, Code = 151439, Description ="CASETAS FIJAS Y MÓVILES"},
			new () { Id = 62, Code = 151504, Description ="LOCALES Y CASETAS A BUHONEROS"},
			new () { Id = 63, Code = 151508, Description ="MERCADOS Y HOSPEDAJES"},
			new () { Id = 64, Code = 151510, Description ="ALQUILER NICHOS DE CEMENTERIO"},
			new () { Id = 65, Code = 151513, Description ="ALQUILER BALNEARIOS"},
			new () { Id = 66, Code = 151518, Description ="TARDANZA POR PAGOS DE ARRENDAMIENTOS"},
			new () { Id = 67, Code = 163108, Description ="MULTAS DIVERSAS"},
			new () { Id = 68, Code = 163109, Description ="PAGO TARDÍO RECOLECCIÓN DESECHOS SOLIDOS"},
			new () { Id = 69, Code = 163111, Description ="MULTAS ADMINISTRATIVAS"},
			new () { Id = 70, Code = 163112, Description ="MULTAS POR CONSTRUCCIÓN ILEGAL"},
			new () { Id = 71, Code = 163113, Description ="MULTA POR TIRADA DE ESCOMBROS EN VÍAS PUBLICAS"},
			new () { Id = 72, Code = 174301, Description ="VENTAS DE TERRENOS EN CEMENTERIOS"},
			new () { Id = 73, Code = 152503, Description ="INGRESO CAPITAL"},
			new () { Id = 74, Code = 141503, Description ="INGRESO CORRIENTE"},
		}),
		
		(typeof(Product), new Product[] {
			new()
			{
				Id = 1,
				Name = "Producto Capital",
				Barcode = "000001",
				Description = "Producto de Capital",
				TaxScheduleId = 1,
				PriceTypeId = (int)PriceTypes.Fijo,
				UnitPrice = 1,
				SubscriptionApply = false,
				MeasurementRequired = false,
				ERPCode = "PRD000001",
				Status = (int)GenericStatus.Activo,
				CreatedOn = DateTime.MinValue,
			}, new()
			{
				Id = 2,
				Name = "Producto Intereses",
				Barcode = "000002",
				Description = "Producto de Intereses",
				TaxScheduleId = 1,
				PriceTypeId = (int)PriceTypes.Fijo,
				UnitPrice = 1,
				SubscriptionApply = false,
				MeasurementRequired = false,
				ERPCode = "PRD000002",
				Status = (int)GenericStatus.Activo,
				CreatedOn = DateTime.MinValue,
			}, 
			new ()
			{
				Id = 3,
				Name = "Producto Mora",
				Barcode = "000003",
				Description = "Producto de Mora",
				TaxScheduleId = 1,
				PriceTypeId = (int)PriceTypes.Fijo,
				UnitPrice = 1,
				SubscriptionApply = false,
				MeasurementRequired = false,
				ERPCode = "PRD000003",
				Status = (int)GenericStatus.Activo,
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 4,
				Name = "Producto Cargo Fijo",
				Barcode = "000004",
				Description = "Producto de Cargo Fijo",
				TaxScheduleId = 1,
				PriceTypeId = (int)PriceTypes.Fijo,
				UnitPrice = 1,
				SubscriptionApply = false,
				MeasurementRequired = false,
				ERPCode = "PRD000004",
				Status = (int)GenericStatus.Activo,
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 5,
				Name = "Producto Descuento",
				Barcode = "000005",
				Description = "Producto de Descuento",
				TaxScheduleId = 1,
				PriceTypeId = (int)PriceTypes.Fijo,
				UnitPrice = 1,
				SubscriptionApply = false,
				MeasurementRequired = false,
				ERPCode = "PRD000005",
				Status = (int)GenericStatus.Activo,
				CreatedOn = DateTime.MinValue,
			},
		}),
		(typeof(Role), new Role[] {
			new ()
			{
				Id = 1,
				Name = "SuperAdmin",
			},
		}),
		(typeof(CompanySetting), new CompanySetting[] {
			new ()
			{
				Id = 1,
				TimeZone = "America/Santo_Domingo",
				DatePattern = "dd/MM/yyyy",
				TimePattern = "hh:mm tt",
				LateFeeProductId = 1,
				CreatedOn = DateTime.MinValue,
			}
		}),
		(typeof(UserRole), new UserRole[] {
			new ()
			{
				Id = 1,
				UserId = guid(1),
				RoleId = 1,
			}
		}),
		(typeof(Bank), new Bank[] {
			new ()
			{
				Id = 1,
				Name = "Banreservas",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 2,
				Name = "Banco Popular",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 3,
				Name = "Banco BHD",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 4,
				Name = "Scotiabank",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 5,
				Name = "Asociación Popular",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 6,
				Name = "Banco Santa Cruz",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 7,
				Name = "Asociación Cibao",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 8,
				Name = "Banco Promerica",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 9,
				Name = "Banesco",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 10,
				Name = "Asociación La Nacional",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 11,
				Name = "Banco Caribe",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 12,
				Name = "Banco BDI",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 13,
				Name = "Citibank",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 14,
				Name = "Banco López de Haro",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 15,
				Name = "Banco Ademi",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 16,
				Name = "Banco Vimenca",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 17,
				Name = "Banco Lafise",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 18,
				Name = "Alaver",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 19,
				Name = "Bandex",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 20,
				Name = "Motor Crédito",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 21,
				Name = "Banfondesa",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 22,
				Name = "Banco Adopem",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 23,
				Name = "Asociación Duarte",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 24,
				Name = "Asociación Mocana",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 25,
				Name = "Bellbank",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 26,
				Name = "ABONAP",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 27,
				Name = "Banco Unión",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 28,
				Name = "JMMB Bank",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 29,
				Name = "Asociación Peravia",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 30,
				Name = "Banco Confisa",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 31,
				Name = "Asociación Romana",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 32,
				Name = "Banco BACC",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 33,
				Name = "Banco Fihogar",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 34,
				Name = "Asociación Maguana",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 35,
				Name = "Banco Activo",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 36,
				Name = "Bancotui",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 37,
				Name = "Qik Banco Digital",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 38,
				Name = "Leasing Confisa",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 39,
				Name = "Banco Gruficorp",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 40,
				Name = "Banco Atlántico",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 41,
				Name = "Corporación de Crédito Nordestana",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 42,
				Name = "Banco Óptima de Ahorro y Crédito",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 43,
				Name = "Banco Cofaci",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 44,
				Name = "Bonanza Banco",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 45,
				Name = "Corporación de Crédito Monumental",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 46,
				Name = "Banco Empire",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 47,
				Name = "Corporación de Crédito Oficorp",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 48,
				Name = "Bancamérica",
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 49,
				Name = "Banco Agrícola",
				CreatedOn = DateTime.MinValue,
			},
		}),
		(typeof(User), new User[] {
			new() {
				 Id           = guid(1),
				 FirstName    = "Super",
				 LastName     = "Admin",
				 Status       = 1,
				 Username     = "superadmin",
				 Email        = "superadmin@gmail.com",
				 PasswordHash = pass("123Password"),
				 Phone        = "(829) 123-4567"
			}
		}),
		(typeof(Currency), new Currency[] {
			  new ()
			  {
				  Id = 1,
				  Code = "DOP",
				  Name = "Peso Dominicano",
				  Symbol = "RD$",
				  Status = (int)GenericStatus.Activo,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 2,
				  Code = "USD",
				  Name = "Dolar Americano",
				  Symbol = "$",
				  Status = (int)GenericStatus.Activo,
				  CreatedOn = DateTime.MinValue,
			  }
		}),
		(typeof(CurrencyRate), new CurrencyRate[] {
		  new ()
		  {
			  Id = 1,
			  CurrencyId = 1,
			  ExchangeRate = 1m,
			  StartDate = new DateTime(2023, 1, 1),
			  EndDate = new DateTime(2024, 1, 1),
			  CreatedOn = DateTime.MinValue,
		  },
		  new ()
		  {
			  Id = 2,
			  CurrencyId = 2,
			  ExchangeRate = 56.8m,
			  StartDate = new DateTime(2023, 1, 1),
			  EndDate = new DateTime(2024, 1, 1),
			  CreatedOn = DateTime.MinValue,
		  }
		}),
		(typeof(Country), new Country[] {
		 new ()
		 {
			 Id = 1,
			 Name = "Republica Dominicana",
			 Demonym = "Dominicano (a)",
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 2,
			 Name = "Estados Unidos",
			 Demonym = "Estadounidense",
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 3,
			 Name = "Canada",
			 Demonym = "Canadiense",
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 4,
			 Name = "Mexico",
			 Demonym = "Mexicano (a)",
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 5,
			 Name = "Haiti",
			 Demonym = "Haitiano (a)",
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 6,
			 Name = "Puerto Rico",
			 Demonym = "Puertorricense",
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 7,
			 Name = "China",
			 Demonym = "Chino (a)",
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 8,
			 Name = "Venezuela",
			 Demonym = "Venezolano (a)",
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 9,
			 Name = "Colombia",
			 Demonym = "Colombiano (a)",
			 CreatedOn = DateTime.MinValue,
		 },
		}),
		(typeof(State), new State[] {
		 new ()
		 {
			 Id = 1,
			 Name = "Azua",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 2,
			 Name = "Bahoruco",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 3,
			 Name = "Barahona",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 4,
			 Name = "Dajabón",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 5,
			 Name = "Distrito Nacional",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 6,
			 Name = "Duarte",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 7,
			 Name = "Elías Piña",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 8,
			 Name = "El Seibo",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 9,
			 Name = "Espaillat",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 10,
			 Name = "Hato Mayor",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 11,
			 Name = "Hermanas Mirabal",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 12,
			 Name = "Independencia",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 13,
			 Name = "La Altagracia",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 14,
			 Name = "La Romana",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 15,
			 Name = "La Vega",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 16,
			 Name = "María Trinidad Sánchez",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 17,
			 Name = "Monseñor Nouel",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 18,
			 Name = "Monte Cristi",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 19,
			 Name = "Monte Plata",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 20,
			 Name = "Pedernales",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 21,
			 Name = "Peravia",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 22,
			 Name = "Puerto Plata",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 23,
			 Name = "Samaná",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 24,
			 Name = "San Cristóbal",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 25,
			 Name = "San José de Ocoa",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 26,
			 Name = "San Juan",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 27,
			 Name = "San Pedro de Macorís",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 28,
			 Name = "Sánchez Ramírez",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 29,
			 Name = "Santiago",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 30,
			 Name = "Santiago Rodríguez",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 31,
			 Name = "Santo Domingo",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 32,
			 Name = "Valverde",
			 CountryId = 1,
			 CreatedOn = DateTime.MinValue,
		 }
		}),
		(typeof(CustomerTicketType), new CustomerTicketType[] {
		 new ()
		 {
			 Id = 1,
			 Name = "Denuncia",
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 2,
			 Name = "Peticion",
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 3,
			 Name = "Queja",
			 CreatedOn = DateTime.MinValue,
		 },
		 new ()
		 {
			 Id = 4,
			 Name = "Reclamo",
			 CreatedOn = DateTime.MinValue,
		 }
		}),
		(typeof(PropertyType), new PropertyType[] {
		  new (){
			  Id = 1,
			  Name = "Vivienda Individual",
			  Status = (int)GenericStatus.Activo,
			  CreatedOn = DateTime.MinValue,
		  },
		  new (){
			  Id = 2,
			  Name = "Comercial",
			  Status = (int)GenericStatus.Activo,
			  CreatedOn = DateTime.MinValue,
		  },
		  new (){
			  Id = 3,
			  Name = "Edificio / Torre / Condominio",
			  Status = (int)GenericStatus.Activo,
			  CreatedOn = DateTime.MinValue,
		  },
		  new (){
			  Id = 4,
			  Name = "Plaza Comercial",
			  Status = (int)GenericStatus.Activo,
			  CreatedOn = DateTime.MinValue,
		  },
		  new (){
			  Id = 5,
			  Name = "Agrupación de Viviendas",
			  Status = (int)GenericStatus.Activo,
			  CreatedOn = DateTime.MinValue,
		  },
		  new (){
			  Id = 6,
			  Name = "Agrupación Mixta",
			  Status = (int)GenericStatus.Activo,
			  CreatedOn = DateTime.MinValue,
		  },
		  new (){
			  Id = 7,
			  Name = "Solar",
			  Status = (int)GenericStatus.Activo,
			  CreatedOn = DateTime.MinValue,
		  },
		  new (){
			  Id = 8,
			  Name = "Institución Religiosa",
			  Status = (int)GenericStatus.Activo,
			  CreatedOn = DateTime.MinValue,
		  },
		  new (){
			  Id = 9,
			  Name = "Institución Gubernamental",
			  Status = (int)GenericStatus.Activo,
			  CreatedOn = DateTime.MinValue,
		  },
		  new (){
			  Id = 10,
			  Name = "Puesto de Mercado",
			  Status = (int)GenericStatus.Activo,
			  CreatedOn = DateTime.MinValue,
		  },
		  new (){
			  Id = 11,
			  Name = "Otros",
			  Status = (int)GenericStatus.Activo,
			  CreatedOn = DateTime.MinValue,
		  }
		}),
		(typeof(TaxSchedule), new TaxSchedule[] {
		  new ()
		  {
			  Id = 1,
			  Name = "Contado",
			  TaxRate = 0,
			  CreatedOn = DateTime.MinValue,
		  },
		  new ()
		  {
			  Id = 2,
			  Name = "ITBIS",
			  TaxRate = 18,
			  CreatedOn = DateTime.MinValue,
		  }
		}),
		(typeof(CustomerType), new CustomerType[] {
		  new ()
		  {
			  Id = 1,
			  Name = "Residencial",
			  Status = (int)GenericStatus.Activo,
			  CreatedOn = DateTime.MinValue,
		  },
		  new ()
		  {
			  Id = 2,
			  Name = "Comercial",
			  Status = (int)GenericStatus.Activo,
			  CreatedOn = DateTime.MinValue,
		  },
		  new ()
		  {
			  Id = 3,
			  Name = "Industrial",
			  Status = (int)GenericStatus.Activo,
			  CreatedOn = DateTime.MinValue,
		  },
		  new ()
		  {
			  Id = 4,
			  Name = "Gubernamental",
			  Status = (int)GenericStatus.Activo,
			  CreatedOn = DateTime.MinValue,
		  },
		  new ()
		  {
			  Id = 5,
			  Name = "Conjunto Residencial",
			  Status = (int)GenericStatus.Activo,
			  CreatedOn = DateTime.MinValue,
		  },
		  new ()
		  {
			  Id = 6,
			  Name = "Institución Religiosa",
			  Status = (int)GenericStatus.Activo,
			  CreatedOn = DateTime.MinValue,
		  },
		  new ()
		  {
			  Id = 7,
			  Name = "Sucursal Ayuntamiento",
			  Status = (int)GenericStatus.Activo,
			  CreatedOn = DateTime.MinValue,
		  },
		  new ()
		  {
			  Id = 8,
			  Name = "Otros",
			  Status = (int)GenericStatus.Activo,
			  CreatedOn = DateTime.MinValue,
		  },
		}),
		
		(typeof(PaymentMethod), new PaymentMethod[] {
			new ()
			{
				Id = 1,
				Name = "Efectivo",
				PaymentMethodTypeId = 1,
				Status = (int)GenericStatus.Activo,
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 2,
				Name = "Tarjeta",
				PaymentMethodTypeId = 2,
				Status = (int)GenericStatus.Activo,
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 3,
				Name = "Transferencia",
				PaymentMethodTypeId = 3,
				Status = (int)GenericStatus.Activo,
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 4,
				Name = "Cheque",
				PaymentMethodTypeId = 4,
				Status = (int)GenericStatus.Activo,
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 5,
				Name = "Generico",
				PaymentMethodTypeId = 5,
				Status = (int)GenericStatus.Activo,
				CreatedOn = DateTime.MinValue,
			},
		})
	};

	private static (Type, object[]) SeedIssueTypes() => (typeof(IssueType), new[] {
      new IssueType { Id = (int)IssueTypes.PredioMalUbicado,                     IsAnomaly = true,  Description = IssueTypes.PredioMalUbicado                    .ToString().PascalCaseWithInitialsToTitleCase() },
      new IssueType { Id = (int)IssueTypes.PredioNoEncontrado,                   IsAnomaly = true,  Description = IssueTypes.PredioNoEncontrado                  .ToString().PascalCaseWithInitialsToTitleCase() },
      new IssueType { Id = (int)IssueTypes.PredioAccesoImposible,                IsAnomaly = true,  Description = IssueTypes.PredioAccesoImposible               .ToString().PascalCaseWithInitialsToTitleCase() },
      new IssueType { Id = (int)IssueTypes.PredioDeshabitado,                    IsAnomaly = false, Description = IssueTypes.PredioDeshabitado                   .ToString().PascalCaseWithInitialsToTitleCase() },
      new IssueType { Id = (int)IssueTypes.PuntoDeMedidaNoEncontrado,            IsAnomaly = true,  Description = IssueTypes.PuntoDeMedidaNoEncontrado           .ToString().PascalCaseWithInitialsToTitleCase() },
      new IssueType { Id = (int)IssueTypes.MedidorConAlturaFueraDeLoEstablecido, IsAnomaly = false, Description = IssueTypes.MedidorConAlturaFueraDeLoEstablecido.ToString().PascalCaseWithInitialsToTitleCase() },
      new IssueType { Id = (int)IssueTypes.AnomaliaEnBase,                       IsAnomaly = false, Description = IssueTypes.AnomaliaEnBase                      .ToString().PascalCaseWithInitialsToTitleCase() },
      new IssueType { Id = (int)IssueTypes.MedidorCambiado,                      IsAnomaly = true,  Description = IssueTypes.MedidorCambiado                     .ToString().PascalCaseWithInitialsToTitleCase() },
      new IssueType { Id = (int)IssueTypes.PresuntoFraude,                       IsAnomaly = false, Description = IssueTypes.PresuntoFraude                      .ToString().PascalCaseWithInitialsToTitleCase() },
      new IssueType { Id = (int)IssueTypes.ContadorRotoOAveriado,                IsAnomaly = true,  Description = IssueTypes.ContadorRotoOAveriado               .ToString().PascalCaseWithInitialsToTitleCase() },
      new IssueType { Id = (int)IssueTypes.MedidorInexistente,                   IsAnomaly = true,  Description = IssueTypes.MedidorInexistente                  .ToString().PascalCaseWithInitialsToTitleCase() },
      new IssueType { Id = (int)IssueTypes.ContadorConDisplayApagado,            IsAnomaly = true,  Description = IssueTypes.ContadorConDisplayApagado           .ToString().PascalCaseWithInitialsToTitleCase() },
      new IssueType { Id = (int)IssueTypes.IrregularidadEnSello,                 IsAnomaly = false, Description = IssueTypes.IrregularidadEnSello                .ToString().PascalCaseWithInitialsToTitleCase() },
      new IssueType { Id = (int)IssueTypes.TiempoDeLecturaAgotado,               IsAnomaly = true,  Description = IssueTypes.TiempoDeLecturaAgotado              .ToString().PascalCaseWithInitialsToTitleCase() },
      new IssueType { Id = (int)IssueTypes.MedidorNoComunica,                    IsAnomaly = true,  Description = IssueTypes.MedidorNoComunica                   .ToString().PascalCaseWithInitialsToTitleCase() },
      new IssueType { Id = (int)IssueTypes.LecturaFueraDeRango,                  IsAnomaly = true,  Description = IssueTypes.LecturaFueraDeRango                 .ToString().PascalCaseWithInitialsToTitleCase() }
   });

	private static (Type, object[]) SeedOrderTasks() => (typeof(WorkOrderTask), new[] {
      new WorkOrderTask { Id = (int)OrderTasks.Meter,                   Description = "Medidor",                                        Automatic = true  },
      new WorkOrderTask { Id = (int)OrderTasks.MeterBrand,              Description = "Marca de medidor",                               Automatic = true  },
      new WorkOrderTask { Id = (int)OrderTasks.ActiveEnergyReadout,     Description = "Lectura de energía activa",                      Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.PowerReadout,            Description = "Lectura de potencia",                            Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.InstalledSeal,           Description = "Registro de sello instalado",                    Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.RemovedSeal,             Description = "Registro de sello removido",                     Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.Situation,               Description = "Reporte de situación",                           Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.IntegrationTest,         Description = "Prueba de integración",                          Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.MeterState,              Description = "Captura de estado del medidor",                  Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.ClientPresent,           Description = "Indicación cliente esta presente",               Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.MeterShouldBeChanged,    Description = "Reporte medidor debe ser cambiado",              Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.SealFound,               Description = "Registro de sello encontrado",                   Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.MeterOnSite,             Description = "Indicación medidor en predio",                   Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.PreviousMeter,           Description = "Registro medidor anterior",                      Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.PosteriorMeter,          Description = "Registro medidor posterior",                     Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.Address,                 Description = "Captura de dirección",                           Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.SiteType,                Description = "Tipo de predio",                                 Automatic = true  },
      new WorkOrderTask { Id = (int)OrderTasks.Reference,               Description = "Registro de referencias",                        Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.InstalledMeterCode,      Description = "Registro código de medidor",                     Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.InstalledMeterBrand,     Description = "Registro marca de medidor",                      Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.ConnectionType,          Description = "Tipo de conexión",                               Automatic = true  },
      new WorkOrderTask { Id = (int)OrderTasks.MeterVoltage,            Description = "Voltaje",                                        Automatic = true  },
      new WorkOrderTask { Id = (int)OrderTasks.ReadingType,             Description = "Indicación de tipo de medición",                 Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.Multiplier,              Description = "Indicación de mútiplo de medición",              Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.NewMeterLocation,        Description = "Registro de ubicación medidor instalado",        Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.Transformer,             Description = "Registro de transformador asociado a dirección", Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.RegisterLoad,            Description = "Registro de Carga (A)",                          Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.OfflineLoad,             Description = "Carga en vacío (A)",                             Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.MeasurementPattern,      Description = "Resultado patrón de medida",                     Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.RanIntoComplication,     Description = "Situación encontrada",                           Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.TakenAction,             Description = "Acción realizada",                               Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.MeterStatus,             Description = "Estado del medidor",                             Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.MaterialRequirement,     Description = "Requerimiento de material",                      Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.SupervisorRequired,      Description = "Intervención de supervisor requerida",           Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.Geolocalization,         Description = "Registrar Geolocalización",                      Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.ClientId,                Description = "Id de Cliente",                                  Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.RegisterPhoto,           Description = "Registro fotografico",                           Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.RegisterSignature,       Description = "Registro de firma",                              Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.OwnerName,               Description = "Nombre del propietario",                         Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.EngOrArchInCharge,       Description = "Ing. o Arq. encargado",                          Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.CollegeNumber,           Description = "No. Colegiatura",                                Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.Progress,                Description = "% Avance",                                       Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.ConstructionType,        Description = "Tipo de obra",                                   Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.Area,                    Description = "Area",                                           Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.CodeViolations,          Description = "Violaciones de ley",                             Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.HoursTillRegularization, Description = "Tiempo para presentacion en oficina DPU",        Automatic = false },
      new WorkOrderTask { Id = (int)OrderTasks.ReceivedBy,              Description = "Recibido por",                                   Automatic = false },
   });

	private static (Type, object[]) SeedOrderTypes() => (typeof(WorkOrderType), new [] {
      new WorkOrderType { Id = (int)OrderTypes.CortePorImpago,                  Description = OrderTypes.CortePorImpago.ToString().PascalCaseWithInitialsToTitleCase(),                  DurationInHours = 24, MaterialCost = false, CreatedOn = DateTime.MinValue },
      new WorkOrderType { Id = (int)OrderTypes.Reconexion,                      Description = OrderTypes.Reconexion.ToString().PascalCaseWithInitialsToTitleCase(),                      DurationInHours = 12, MaterialCost = false, CreatedOn = DateTime.MinValue },
      new WorkOrderType { Id = (int)OrderTypes.BajaDeServicio,                  Description = OrderTypes.BajaDeServicio.ToString().PascalCaseWithInitialsToTitleCase(),                  DurationInHours = 24, MaterialCost = false, CreatedOn = DateTime.MinValue },
      new WorkOrderType { Id = (int)OrderTypes.PrimeraRevisionDeCorte,          Description = OrderTypes.PrimeraRevisionDeCorte.ToString().PascalCaseWithInitialsToTitleCase(),          DurationInHours = 24, MaterialCost = false, CreatedOn = DateTime.MinValue },
      new WorkOrderType { Id = (int)OrderTypes.SegundaRevisionDeCorte,          Description = OrderTypes.SegundaRevisionDeCorte.ToString().PascalCaseWithInitialsToTitleCase(),          DurationInHours = 24, MaterialCost = false, CreatedOn = DateTime.MinValue },
      new WorkOrderType { Id = (int)OrderTypes.TerceraRevisionDeCorte,          Description = OrderTypes.TerceraRevisionDeCorte.ToString().PascalCaseWithInitialsToTitleCase(),          DurationInHours = 12, MaterialCost = false, CreatedOn = DateTime.MinValue },
      new WorkOrderType { Id = (int)OrderTypes.ReclamacionPorConsumo,           Description = OrderTypes.ReclamacionPorConsumo.ToString().PascalCaseWithInitialsToTitleCase(),           DurationInHours = 48, MaterialCost = false, CreatedOn = DateTime.MinValue },
      new WorkOrderType { Id = (int)OrderTypes.TomaDeLectura,                   Description = OrderTypes.TomaDeLectura.ToString().PascalCaseWithInitialsToTitleCase(),                   DurationInHours = 24, MaterialCost = false, CreatedOn = DateTime.MinValue },
      new WorkOrderType { Id = (int)OrderTypes.InspeccionNuevoServicio,         Description = OrderTypes.InspeccionNuevoServicio.ToString().PascalCaseWithInitialsToTitleCase(),         DurationInHours = 24, MaterialCost = false, CreatedOn = DateTime.MinValue },
      new WorkOrderType { Id = (int)OrderTypes.Averia,                          Description = OrderTypes.Averia.ToString().PascalCaseWithInitialsToTitleCase(),                          DurationInHours =  8, MaterialCost = false, CreatedOn = DateTime.MinValue },
      new WorkOrderType { Id = (int)OrderTypes.CambioMedidorRoto,               Description = OrderTypes.CambioMedidorRoto.ToString().PascalCaseWithInitialsToTitleCase(),               DurationInHours = 48, MaterialCost = false, CreatedOn = DateTime.MinValue },
      new WorkOrderType { Id = (int)OrderTypes.CambioMedidorPorCambioVoltaje,   Description = OrderTypes.CambioMedidorPorCambioVoltaje.ToString().PascalCaseWithInitialsToTitleCase(),   DurationInHours = 72, MaterialCost = false, CreatedOn = DateTime.MinValue },
      new WorkOrderType { Id = (int)OrderTypes.InstalacionNuevoServicio,        Description = OrderTypes.InstalacionNuevoServicio.ToString().PascalCaseWithInitialsToTitleCase(),        DurationInHours = 48, MaterialCost = true, CreatedOn = DateTime.MinValue  },
      new WorkOrderType { Id = (int)OrderTypes.VerificacionPosibilidadDeFraude, Description = OrderTypes.VerificacionPosibilidadDeFraude.ToString().PascalCaseWithInitialsToTitleCase(), DurationInHours = 48, MaterialCost = false, CreatedOn = DateTime.MinValue },
      new WorkOrderType { Id = (int)OrderTypes.VerificacionSIE,                 Description = "Verificación SIE",                                                                              DurationInHours = 72, MaterialCost = false, CreatedOn = DateTime.MinValue },
      new WorkOrderType { Id = (int)OrderTypes.ConexionNuevoServicio,           Description = OrderTypes.ConexionNuevoServicio.ToString().PascalCaseWithInitialsToTitleCase(),           DurationInHours = 24, MaterialCost = false, CreatedOn = DateTime.MinValue },
      new WorkOrderType { Id = (int)OrderTypes.CambioUbicacionMedidor,          Description = OrderTypes.CambioUbicacionMedidor.ToString().PascalCaseWithInitialsToTitleCase(),          DurationInHours = 48, MaterialCost = true, CreatedOn = DateTime.MinValue },
      new WorkOrderType { Id = (int)OrderTypes.NotificacionObra,                Description = OrderTypes.NotificacionObra.ToString().PascalCaseWithInitialsToTitleCase(),                DurationInHours =  2, MaterialCost = false, CreatedOn = DateTime.MinValue },
   });

	private static (Type, object[]) SeedOrderStatuses() => (typeof(WorkOrderStatus), new[] {
      new WorkOrderStatus { Id = (int)OrderStatuses.Generated,         Name = "Generada"             },
      new WorkOrderStatus { Id = (int)OrderStatuses.AssignedToBrigade, Name = "Asignada a brigada"   },
      new WorkOrderStatus { Id = (int)OrderStatuses.OnRoute,           Name = "En ruta"              },
      new WorkOrderStatus { Id = (int)OrderStatuses.OnExecution,       Name = "En ejecución"         },
      new WorkOrderStatus { Id = (int)OrderStatuses.Rejected,          Name = "Rechazada"            },
      new WorkOrderStatus { Id = (int)OrderStatuses.PendingClient,     Name = "Pendiente de cliente" },
      new WorkOrderStatus { Id = (int)OrderStatuses.NotResolved,       Name = "No resuelta"          },
      new WorkOrderStatus { Id = (int)OrderStatuses.SentToBackoffice,  Name = "Enviado a backoffice" },
      new WorkOrderStatus { Id = (int)OrderStatuses.Resolved,          Name = "Resuelta"             }
   });

	private static (Type, object[]) SeedOrderTypeOrderTaskRelationships() => (typeof(OrderTypeOrderTask), new[] {
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CortePorImpago, TaskId = (int)OrderTasks.Meter               },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CortePorImpago, TaskId = (int)OrderTasks.MeterBrand          },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CortePorImpago, TaskId = (int)OrderTasks.ActiveEnergyReadout },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CortePorImpago, TaskId = (int)OrderTasks.PowerReadout        },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CortePorImpago, TaskId = (int)OrderTasks.InstalledSeal       },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CortePorImpago, TaskId = (int)OrderTasks.RemovedSeal         },

      new OrderTypeOrderTask { TypeId = (int)OrderTypes.Reconexion, TaskId = (int)OrderTasks.Meter               },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.Reconexion, TaskId = (int)OrderTasks.MeterBrand          },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.Reconexion, TaskId = (int)OrderTasks.ActiveEnergyReadout },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.Reconexion, TaskId = (int)OrderTasks.PowerReadout        },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.Reconexion, TaskId = (int)OrderTasks.Situation           },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.Reconexion, TaskId = (int)OrderTasks.InstalledSeal       },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.Reconexion, TaskId = (int)OrderTasks.RemovedSeal         },

      new OrderTypeOrderTask { TypeId = (int)OrderTypes.BajaDeServicio, TaskId = (int)OrderTasks.Meter               },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.BajaDeServicio, TaskId = (int)OrderTasks.MeterBrand          },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.BajaDeServicio, TaskId = (int)OrderTasks.ActiveEnergyReadout },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.BajaDeServicio, TaskId = (int)OrderTasks.PowerReadout        },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.BajaDeServicio, TaskId = (int)OrderTasks.Situation           },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.BajaDeServicio, TaskId = (int)OrderTasks.InstalledSeal       },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.BajaDeServicio, TaskId = (int)OrderTasks.RemovedSeal         },

      new OrderTypeOrderTask { TypeId = (int)OrderTypes.PrimeraRevisionDeCorte, TaskId = (int)OrderTasks.Meter               },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.PrimeraRevisionDeCorte, TaskId = (int)OrderTasks.MeterBrand          },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.PrimeraRevisionDeCorte, TaskId = (int)OrderTasks.ActiveEnergyReadout },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.PrimeraRevisionDeCorte, TaskId = (int)OrderTasks.PowerReadout        },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.PrimeraRevisionDeCorte, TaskId = (int)OrderTasks.Situation           },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.PrimeraRevisionDeCorte, TaskId = (int)OrderTasks.InstalledSeal       },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.PrimeraRevisionDeCorte, TaskId = (int)OrderTasks.RemovedSeal         },

      new OrderTypeOrderTask { TypeId = (int)OrderTypes.SegundaRevisionDeCorte, TaskId = (int)OrderTasks.Meter               },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.SegundaRevisionDeCorte, TaskId = (int)OrderTasks.MeterBrand          },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.SegundaRevisionDeCorte, TaskId = (int)OrderTasks.ActiveEnergyReadout },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.SegundaRevisionDeCorte, TaskId = (int)OrderTasks.PowerReadout        },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.SegundaRevisionDeCorte, TaskId = (int)OrderTasks.Situation           },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.SegundaRevisionDeCorte, TaskId = (int)OrderTasks.InstalledSeal       },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.SegundaRevisionDeCorte, TaskId = (int)OrderTasks.RemovedSeal         },

      new OrderTypeOrderTask { TypeId = (int)OrderTypes.TerceraRevisionDeCorte, TaskId = (int)OrderTasks.Meter               },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.TerceraRevisionDeCorte, TaskId = (int)OrderTasks.MeterBrand          },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.TerceraRevisionDeCorte, TaskId = (int)OrderTasks.ActiveEnergyReadout },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.TerceraRevisionDeCorte, TaskId = (int)OrderTasks.PowerReadout        },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.TerceraRevisionDeCorte, TaskId = (int)OrderTasks.Situation           },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.TerceraRevisionDeCorte, TaskId = (int)OrderTasks.InstalledSeal       },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.TerceraRevisionDeCorte, TaskId = (int)OrderTasks.RemovedSeal         },

      new OrderTypeOrderTask { TypeId = (int)OrderTypes.ReclamacionPorConsumo, TaskId = (int)OrderTasks.Meter                },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.ReclamacionPorConsumo, TaskId = (int)OrderTasks.MeterBrand           },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.ReclamacionPorConsumo, TaskId = (int)OrderTasks.ActiveEnergyReadout  },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.ReclamacionPorConsumo, TaskId = (int)OrderTasks.PowerReadout         },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.ReclamacionPorConsumo, TaskId = (int)OrderTasks.Situation            },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.ReclamacionPorConsumo, TaskId = (int)OrderTasks.IntegrationTest      },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.ReclamacionPorConsumo, TaskId = (int)OrderTasks.MeterState           },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.ReclamacionPorConsumo, TaskId = (int)OrderTasks.OfflineLoad          },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.ReclamacionPorConsumo, TaskId = (int)OrderTasks.ClientPresent        },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.ReclamacionPorConsumo, TaskId = (int)OrderTasks.MeterShouldBeChanged },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.ReclamacionPorConsumo, TaskId = (int)OrderTasks.InstalledSeal        },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.ReclamacionPorConsumo, TaskId = (int)OrderTasks.RemovedSeal          },

      new OrderTypeOrderTask { TypeId = (int)OrderTypes.TomaDeLectura, TaskId = (int)OrderTasks.Meter               },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.TomaDeLectura, TaskId = (int)OrderTasks.MeterBrand          },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.TomaDeLectura, TaskId = (int)OrderTasks.ActiveEnergyReadout },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.TomaDeLectura, TaskId = (int)OrderTasks.PowerReadout        },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.TomaDeLectura, TaskId = (int)OrderTasks.Situation           },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.TomaDeLectura, TaskId = (int)OrderTasks.InstalledSeal       },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.TomaDeLectura, TaskId = (int)OrderTasks.RemovedSeal         },

      new OrderTypeOrderTask { TypeId = (int)OrderTypes.InspeccionNuevoServicio, TaskId = (int)OrderTasks.InstalledMeterCode  },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.InspeccionNuevoServicio, TaskId = (int)OrderTasks.InstalledMeterBrand },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.InspeccionNuevoServicio, TaskId = (int)OrderTasks.ActiveEnergyReadout },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.InspeccionNuevoServicio, TaskId = (int)OrderTasks.PowerReadout        },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.InspeccionNuevoServicio, TaskId = (int)OrderTasks.Situation           },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.InspeccionNuevoServicio, TaskId = (int)OrderTasks.SealFound           },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.InspeccionNuevoServicio, TaskId = (int)OrderTasks.MeterOnSite         },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.InspeccionNuevoServicio, TaskId = (int)OrderTasks.PreviousMeter       },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.InspeccionNuevoServicio, TaskId = (int)OrderTasks.PosteriorMeter      },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.InspeccionNuevoServicio, TaskId = (int)OrderTasks.Address             },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.InspeccionNuevoServicio, TaskId = (int)OrderTasks.SiteType            },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.InspeccionNuevoServicio, TaskId = (int)OrderTasks.Reference           },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.InspeccionNuevoServicio, TaskId = (int)OrderTasks.ConnectionType      },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.InspeccionNuevoServicio, TaskId = (int)OrderTasks.MeterVoltage        },

      new OrderTypeOrderTask { TypeId = (int)OrderTypes.Averia, TaskId = (int)OrderTasks.Meter               },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.Averia, TaskId = (int)OrderTasks.MeterBrand          },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.Averia, TaskId = (int)OrderTasks.ActiveEnergyReadout },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.Averia, TaskId = (int)OrderTasks.PowerReadout        },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.Averia, TaskId = (int)OrderTasks.Situation           },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.Averia, TaskId = (int)OrderTasks.InstalledSeal       },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.Averia, TaskId = (int)OrderTasks.RemovedSeal         },

      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioMedidorRoto, TaskId = (int)OrderTasks.Meter               },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioMedidorRoto, TaskId = (int)OrderTasks.MeterBrand          },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioMedidorRoto, TaskId = (int)OrderTasks.ActiveEnergyReadout },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioMedidorRoto, TaskId = (int)OrderTasks.PowerReadout        },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioMedidorRoto, TaskId = (int)OrderTasks.InstalledMeterCode  },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioMedidorRoto, TaskId = (int)OrderTasks.InstalledMeterBrand },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioMedidorRoto, TaskId = (int)OrderTasks.Situation           },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioMedidorRoto, TaskId = (int)OrderTasks.InstalledSeal       },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioMedidorRoto, TaskId = (int)OrderTasks.RemovedSeal         },

      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioMedidorPorCambioVoltaje, TaskId = (int)OrderTasks.Meter               },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioMedidorPorCambioVoltaje, TaskId = (int)OrderTasks.MeterBrand          },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioMedidorPorCambioVoltaje, TaskId = (int)OrderTasks.ActiveEnergyReadout },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioMedidorPorCambioVoltaje, TaskId = (int)OrderTasks.PowerReadout        },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioMedidorPorCambioVoltaje, TaskId = (int)OrderTasks.InstalledMeterCode  },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioMedidorPorCambioVoltaje, TaskId = (int)OrderTasks.InstalledMeterBrand },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioMedidorPorCambioVoltaje, TaskId = (int)OrderTasks.ReadingType         },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioMedidorPorCambioVoltaje, TaskId = (int)OrderTasks.Multiplier          },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioMedidorPorCambioVoltaje, TaskId = (int)OrderTasks.Situation           },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioMedidorPorCambioVoltaje, TaskId = (int)OrderTasks.InstalledSeal       },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioMedidorPorCambioVoltaje, TaskId = (int)OrderTasks.RemovedSeal         },

      new OrderTypeOrderTask { TypeId = (int)OrderTypes.InstalacionNuevoServicio, TaskId = (int)OrderTasks.InstalledMeterCode  },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.InstalacionNuevoServicio, TaskId = (int)OrderTasks.InstalledMeterBrand },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.InstalacionNuevoServicio, TaskId = (int)OrderTasks.MeterVoltage        },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.InstalacionNuevoServicio, TaskId = (int)OrderTasks.ActiveEnergyReadout },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.InstalacionNuevoServicio, TaskId = (int)OrderTasks.PowerReadout        },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.InstalacionNuevoServicio, TaskId = (int)OrderTasks.ReadingType         },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.InstalacionNuevoServicio, TaskId = (int)OrderTasks.Multiplier          },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.InstalacionNuevoServicio, TaskId = (int)OrderTasks.Situation           },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.InstalacionNuevoServicio, TaskId = (int)OrderTasks.NewMeterLocation    },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.InstalacionNuevoServicio, TaskId = (int)OrderTasks.InstalledSeal       },

      new OrderTypeOrderTask { TypeId = (int)OrderTypes.VerificacionPosibilidadDeFraude, TaskId = (int)OrderTasks.Meter               },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.VerificacionPosibilidadDeFraude, TaskId = (int)OrderTasks.MeterBrand          },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.VerificacionPosibilidadDeFraude, TaskId = (int)OrderTasks.ActiveEnergyReadout },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.VerificacionPosibilidadDeFraude, TaskId = (int)OrderTasks.PowerReadout        },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.VerificacionPosibilidadDeFraude, TaskId = (int)OrderTasks.Situation           },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.VerificacionPosibilidadDeFraude, TaskId = (int)OrderTasks.SealFound           },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.VerificacionPosibilidadDeFraude, TaskId = (int)OrderTasks.InstalledSeal       },

      new OrderTypeOrderTask { TypeId = (int)OrderTypes.VerificacionSIE, TaskId = (int)OrderTasks.Meter                },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.VerificacionSIE, TaskId = (int)OrderTasks.MeterBrand           },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.VerificacionSIE, TaskId = (int)OrderTasks.ActiveEnergyReadout  },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.VerificacionSIE, TaskId = (int)OrderTasks.PowerReadout         },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.VerificacionSIE, TaskId = (int)OrderTasks.Situation            },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.VerificacionSIE, TaskId = (int)OrderTasks.IntegrationTest      },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.VerificacionSIE, TaskId = (int)OrderTasks.MeterState           },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.VerificacionSIE, TaskId = (int)OrderTasks.OfflineLoad          },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.VerificacionSIE, TaskId = (int)OrderTasks.ClientPresent        },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.VerificacionSIE, TaskId = (int)OrderTasks.MeterShouldBeChanged },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.VerificacionSIE, TaskId = (int)OrderTasks.InstalledSeal        },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.VerificacionSIE, TaskId = (int)OrderTasks.RemovedSeal          },

      new OrderTypeOrderTask { TypeId = (int)OrderTypes.ConexionNuevoServicio, TaskId = (int)OrderTasks.Meter               },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.ConexionNuevoServicio, TaskId = (int)OrderTasks.MeterBrand          },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.ConexionNuevoServicio, TaskId = (int)OrderTasks.ActiveEnergyReadout },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.ConexionNuevoServicio, TaskId = (int)OrderTasks.PowerReadout        },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.ConexionNuevoServicio, TaskId = (int)OrderTasks.Situation           },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.ConexionNuevoServicio, TaskId = (int)OrderTasks.NewMeterLocation    },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.ConexionNuevoServicio, TaskId = (int)OrderTasks.SealFound           },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.ConexionNuevoServicio, TaskId = (int)OrderTasks.InstalledSeal       },

      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioUbicacionMedidor, TaskId = (int)OrderTasks.Meter               },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioUbicacionMedidor, TaskId = (int)OrderTasks.MeterBrand          },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioUbicacionMedidor, TaskId = (int)OrderTasks.ActiveEnergyReadout },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioUbicacionMedidor, TaskId = (int)OrderTasks.PowerReadout        },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioUbicacionMedidor, TaskId = (int)OrderTasks.Situation           },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioUbicacionMedidor, TaskId = (int)OrderTasks.NewMeterLocation    },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.CambioUbicacionMedidor, TaskId = (int)OrderTasks.InstalledSeal       },

      new OrderTypeOrderTask { TypeId = (int)OrderTypes.NotificacionObra, TaskId = (int)OrderTasks.Geolocalization         },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.NotificacionObra, TaskId = (int)OrderTasks.OwnerName               },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.NotificacionObra, TaskId = (int)OrderTasks.EngOrArchInCharge       },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.NotificacionObra, TaskId = (int)OrderTasks.CollegeNumber           },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.NotificacionObra, TaskId = (int)OrderTasks.Address                 },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.NotificacionObra, TaskId = (int)OrderTasks.Progress                },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.NotificacionObra, TaskId = (int)OrderTasks.ConstructionType        },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.NotificacionObra, TaskId = (int)OrderTasks.Area                    },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.NotificacionObra, TaskId = (int)OrderTasks.CodeViolations          },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.NotificacionObra, TaskId = (int)OrderTasks.HoursTillRegularization },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.NotificacionObra, TaskId = (int)OrderTasks.ReceivedBy              },
      new OrderTypeOrderTask { TypeId = (int)OrderTypes.NotificacionObra, TaskId = (int)OrderTasks.RegisterSignature       },
   });

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
		(typeof(Product), new Product[]
		{
			new ()
			{
				Id = 6,
				Name = "BTS",
				Barcode = "BTS",
				Description = "Tarifa base BTS",
				TaxScheduleId = 2,
				PriceTypeId = (int)PriceTypes.Escalonado,
				UnitPrice = 0,
				SubscriptionApply = true,
				MeasurementRequired = true,
				ERPCode = "BTS01",
				Status = (int)GenericStatus.Activo,
				CreatedOn = default,
			},
			new ()
			{
				Id = 7,
				CreatedBy = default,
				CreatedOn = default,
				Name = "BTD",
				Barcode = "BTD",
				Description = "Tarifa base BTD",
				TaxScheduleId = 2,
				PriceTypeId = (int)PriceTypes.Escalonado,
				UnitPrice = 0,
				SubscriptionApply = true,
				MeasurementRequired = true,
				ERPCode = "BTD01",
				Status = (int)GenericStatus.Activo,
			},
			new ()
			{
				Id = 8,
				CreatedBy = default,
				CreatedOn = default,
				Name = "MTD",
				Barcode = "MTD",
				Description = "Tarifa base MTD",
				TaxScheduleId = 2,
				PriceTypeId = (int)PriceTypes.Escalonado,
				UnitPrice = 0,
				SubscriptionApply = true,
				MeasurementRequired = true,
				ERPCode = "MTD01",
				Status = (int)GenericStatus.Activo,
			}
		}),
		(typeof(ProductRate), new ProductRate[]
		{
			new ()
			{
				Id = 1,
				CreatedOn = default,
				Name = "Escala BTS 2023",
				Description = "Escala aplicada para tarifa BTS periodo año 2023",
				UnitOfMeasure = "kWh",
				IsFixed = false,
				IsLastOverride = false,
				StartDate = default,
				EndDate = new DateTime(2023, 12, 31),
				ProductId = 6,
			},
			new ()
			{
				Id = 2,
				CreatedOn = default,
				Name = "Escala BTD kWh 2023",
				Description = "Escala aplicada para tarifa BTD periodo año 2023",
				UnitOfMeasure = "kWh",
				IsFixed = false,
				IsLastOverride = false,
				StartDate = default,
				EndDate = new DateTime(2023, 12, 31),
				ProductId = 7,
			},
			new ()
			{
				Id = 3,
				CreatedOn = default,
				Name = "Escala BTD kW 2023",
				Description = "Escala aplicada para tarifa BTD periodo año 2023",
				UnitOfMeasure = "kW",
				IsFixed = false,
				IsLastOverride = false,
				StartDate = default,
				EndDate = new DateTime(2023, 12, 31),
				ProductId = 7,
			},
			new ()
			{
				Id = 4,
				CreatedOn = default,
				Name = "Escala MTD kWh 2023",
				Description = "Escala aplicada para tarifa MTD periodo año 2023",
				UnitOfMeasure = "kWh",
				IsFixed = false,
				IsLastOverride = false,
				StartDate = default,
				EndDate = new DateTime(2023, 12, 31),
				ProductId = 8,
			},
			new ()
			{
				Id = 5,
				CreatedOn = default,
				Name = "Escala MTD kW 2023",
				Description = "Escala aplicada para tarifa MTD periodo año 2023",
				UnitOfMeasure = "kW",
				IsFixed = false,
				IsLastOverride = false,
				StartDate = default,
				EndDate = new DateTime(2023, 12, 31),
				ProductId = 8,
			}
		}),
		(typeof(ProductRateScale), new ProductRateScale[]
		{
			new ()
			{
				Id = 1,
				CreatedOn = default,
				LowerQuantity = 0,
				UpperQuantity = 300,
				Price = new decimal(8.26),
				ProductRateId = 1,
			},
			new ()
			{
				Id = 2,
				CreatedOn = default,
				LowerQuantity = 301,
				UpperQuantity = null,
				Price = new decimal(13.84),
				ProductRateId = 1,
			},
			new ()
			{
				Id = 3,
				CreatedOn = default,
				LowerQuantity = 0,
				UpperQuantity = null,
				Price = new decimal(9.85),
				ProductRateId = 2,
			},
			new ()
			{
				Id = 4,
				CreatedOn = default,
				LowerQuantity = 0,
				UpperQuantity = null,
				Price = new decimal(1338.56),
				ProductRateId = 3,
			},
			new ()
			{
				Id = 5,
				CreatedOn = default,
				LowerQuantity = 0,
				UpperQuantity = null,
				Price = new decimal(10.82),
				ProductRateId = 4,
			},
			new ()
			{
				Id = 6,
				CreatedOn = default,
				LowerQuantity = 0,
				UpperQuantity = null,
				Price = new decimal(683.52),
				ProductRateId = 5,
			}
		}),
		(typeof(ProductRateExtraCharge), new ProductRateExtraCharge[]
		{
			new ()
			{
				Id = 1,
				CreatedOn = default,
				ProductRateId = 1,
				ProductId = 4,
				Quantity = new decimal(128.16),
			},
			new ()
			{
				Id = 2,
				CreatedOn = default,
				ProductRateId = 2,
				ProductId = 4,
				Quantity = new decimal(262.33),
			},
			new ()
			{
				Id = 3,
				CreatedOn = default,
				ProductRateId = 4,
				ProductId = 4,
				Quantity = new decimal(223.18),
			},
		}),
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
		(typeof(Voltage), new Voltage[]
		{
			new ()
			{
				Id = 1,
				Name = "120v"
			},
			new ()
			{
				Id = 2,
				Name = "120v/240v"
			},
			new ()
			{
				Id = 3,
				Name = "208v"
			},
			new ()
			{
				Id = 4,
				Name = "480v"
			},
		}),
		(typeof(ReceivableReason), new ReceivableReason[]
		{
			new ()
			{
				Id = 1,
				DocumentType = (int)ReceivableReasonTypes.CreditMemo,
				Name = "Alta Facturación",
				Description = "Alta Facturación",
				Status = 1,
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 2,
				DocumentType = (int)ReceivableReasonTypes.DebitMemo,
				Name = "Baja Facturación",
				Description = "Baja Facturación",
				Status = 1,
				CreatedOn = DateTime.MinValue,
			},
			new ()
			{
				Id = 3,
				DocumentType = (int)ReceivableReasonTypes.Payment,
				Name = "Multa Indebida",
				Description = "Multa Indebida",
				Status = 1,
				CreatedOn = DateTime.MinValue,
			},
		}),
		(typeof(Company), new Company[]
		{
			new ()
			{
				Id = 1,
				Name = "Puerto Plata de Electricidad",
				Code = "PPE",
				TaxRegistrationNumber = "9999",
				AddressLine1 = "Calle Juan Garcia Bonelly No. 1",
				AddressLine2 = "Esq. prof. Emilio Aparicio, Ens. Julieta Santo Domingo",
				AddressLine3 = "",
				CityId = 1,
				Region= "San Felipe de Puerto Plata",
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
		  
		  (typeof(NCFSequenceSetting), new NCFSequenceSetting[]
		  {
			  new ()
			  {
				  Id = 1,
				  Series = "B",
				  NCFTypeId = (int)NCFTypes.CreditoFiscal,
				  MaxSequence = 429100,
				  LastSequence = 0,
				  DueDate = new DateTime(DateTime.Now.Year, 12, 31),
				  CreatedOn = DateTime.MinValue,
				  Status = (int)GenericStatus.Activo
			  },
			  new ()
			  {
				  Id = 2,
				  Series = "B",
				  NCFTypeId = (int)NCFTypes.ConsumidorFinal,
				  MaxSequence = 429100,
				  LastSequence = 0,
				  DueDate = new DateTime(DateTime.Now.Year, 12, 31),
				  CreatedOn = DateTime.MinValue,
				  Status = (int)GenericStatus.Activo
			  },
			  new ()
			  {
				  Id = 3,
				  Series = "B",
				  NCFTypeId = (int)NCFTypes.RegimenesEspeciales,
				  MaxSequence = 429100,
				  LastSequence = 0,
				  DueDate = new DateTime(DateTime.Now.Year, 12, 31),
				  CreatedOn = DateTime.MinValue,
				  Status = (int)GenericStatus.Activo
			  },
			  new ()
			  {
				  Id = 4,
				  Series = "B",
				  NCFTypeId = (int)NCFTypes.Gubernamental,
				  MaxSequence = 429100,
				  LastSequence = 0,
				  DueDate = new DateTime(DateTime.Now.Year, 12, 31),
				  CreatedOn = DateTime.MinValue,
				  Status = (int)GenericStatus.Activo
			  },
			  new ()
			  {
				  Id = 5,
				  Series = "B",
				  NCFTypeId = (int)NCFTypes.NotaDeDebito,
				  MaxSequence = 0,
				  LastSequence = 0,
				  DueDate = new DateTime(DateTime.Now.Year, 12, 31),
				  CreatedOn = DateTime.MinValue,
				  Status = (int)GenericStatus.Activo
			  },
			  new ()
			  {
				  Id = 6,
				  Series = "B",
				  NCFTypeId = (int)NCFTypes.NotaDeCredito,
				  MaxSequence = 0,
				  LastSequence = 0,
				  DueDate = new DateTime(DateTime.Now.Year, 12, 31),
				  CreatedOn = DateTime.MinValue,
				  Status = (int)GenericStatus.Activo
			  }
		  }),
		  
		  (typeof(CustomerAddress), new CustomerAddress[]
		  {
			  new ()
			  {
				  Id = 1,
				  Name = "Prueba 1",
				  Status = (int)GenericStatus.Activo,
				  ContactName = "Prueba 1 Contacto",
				  Latitude = 0,
				  Longitude = 0,
				  CompanyName = "Prueba 1 Empresa",
				  AddressLine1 = "Prueba 1 Linea 1",
				  AddressLine2 = "Prueba 1 Linea 2",
				  AddressLine3 = "Prueba 1 Linea 3",
				  PostalCode = "00000",
				  Email = "prueba@prueba.com",
				  Phone = "809-000-0000",
				  CityId = 1,
				  PropertyTypeId = 2,
				  AreaTypeId = (int)AreaTypes.ZonaUrbana,
				  CreatedOn = DateTime.MinValue,
				  PlainAddress = "Prueba 1 Linea 1, Prueba 1 Linea 2, Prueba 1 Linea 3, Santo Domingo",
				  Transformer = "1"
			  },
			  new ()
			  {
				  Id = 2,
				  Name = "Prueba 2",
				  Status = (int)GenericStatus.Activo,
				  ContactName = "Prueba 2 Contacto",
				  Latitude = 0,
				  Longitude = 0,
				  CompanyName = "Prueba 2 Empresa",
				  AddressLine1 = "Prueba 2 Linea 1",
				  AddressLine2 = "Prueba 2 Linea 2",
				  AddressLine3 = "Prueba 2 Linea 3",
				  PostalCode = "00000",
				  Email = "prueba@prueba.com",
				  Phone = "809-000-0000",
				  CityId = 1,
				  PropertyTypeId = 2,
				  AreaTypeId = (int)AreaTypes.ZonaUrbana,
				  CreatedOn = DateTime.MinValue,
				  PlainAddress = "Prueba 2 Linea 1, Prueba 2 Linea 2, Prueba 2 Linea 3, Santo Domingo",
				  Transformer = "1"
			  }
		  }),
		  
		  
		  (typeof(Customer), new Customer[]
		  {
			  new ()
			  {
				  Id = 1,
				  Name = "Raul Sanchez",
				  DocumentNumber = "40218206254",
				  CustomerTypeId = 1,
				  NCFTypeId = 1,
				  Email = "raulenrique6@hotmail.com",
				  MainPhone = "8295462490",
				  DocumentTypeId = 1,
				  BillingAddressId = 1,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 2,
				  Name = "Gorky Rojas",
				  DocumentNumber = "40218206255",
				  CustomerTypeId = 1,
				  NCFTypeId = 1,
				  Email = "grojas@gmail.com",
				  MainPhone = "8295462491",
				  DocumentTypeId = 1,
				  BillingAddressId = 1,
				  CreatedOn = DateTime.MinValue,
			  }
		  }),
		  (typeof(CustomerCustomerAddress), new CustomerCustomerAddress[]
		  {
			  new ()
			  {
				  Id = 1,
				  CustomerId = 1,
				  CustomerAddressId = 1,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 2,
				  CustomerId = 2,
				  CustomerAddressId = 2,
				  CreatedOn = DateTime.MinValue,
			  }
		  }),
		  (typeof(PaymentTerm), new PaymentTerm[]
		  {
			 new ()
			 {
				 Id = 1,
				 Name = "Contado",
				 DueDays = 0,
				 LateFeeDays = 0,
				 LateFeeRate = 0,
				 CreatedOn = DateTime.MinValue,
			 },
			 new ()
			 {
				 Id = 2,
				 Name = "Neto 30 Dias",
				 DueDays = 30,
				 LateFeeDays = 0,
				 LateFeeRate = 0,
				 CreatedOn = DateTime.MinValue,
			 },
		  }),
		  (typeof(BillingSchedule), new BillingSchedule[]
		  {
			  new ()
			  {
				  Id = 1,
				  Name = "Prueba Schedule",
				  Description = "Programacion de prueba",
				  FrequencyId = (int)Frequencies.Anual,
				  Status = (int)GenericStatus.Activo,
				  CreatedOn = DateTime.MinValue,
			  }
		  }),
		  (typeof(BillingCycle), new BillingCycle[]
		  {
			 new ()
			 {
				 Id = 1,
				 Name = "Prueba Cycle",
				 Description = "Ciclo de Prueba",
				 BillingScheduleId = 1,
				 Status = (int)GenericStatus.Activo,
				 CreatedOn = DateTime.MinValue,
			 }
		  }),
		  (typeof(Subscription), new Subscription[]
		  {
			  new ()
			  {
				  Id = 1,
				  SubscriptionNumber = "1",
				  SubscribedDate = new DateTime(2023, 4, 1),
				  NextDate = new DateTime(2023, 4, 30),
				  IsElectrical = false,
				  CustomerId = 1,
				  BillingAddressId = 1,
				  ShippingAddressId = 2,
				  PaymentTermId = 1,
				  BillingCycleId = 1,
				  SubscriptionTypeId = (int)SubscriptionTypes.Ilimitada,
				  Status = (int)GenericStatus.Activo,
				  DeliveryMethodId = (int)DeliveryMethods.Digital,
				  FrequencyId = (int)Frequencies.Anual,
				  BillingTypeId = (int)BillingTypes.Pospago,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 2,
				  SubscriptionNumber = "2",
				  SubscribedDate = new DateTime(2023, 4, 1),
				  NextDate = new DateTime(2023, 4, 30),
				  IsElectrical = false,
				  CustomerId = 2,
				  BillingAddressId = 2,
				  ShippingAddressId = 1,
				  PaymentTermId = 1,
				  BillingCycleId = 1,
				  SubscriptionTypeId = (int)SubscriptionTypes.Ilimitada,
				  Status = (int)GenericStatus.Activo,
				  DeliveryMethodId = (int)DeliveryMethods.Digital,
				  FrequencyId = (int)Frequencies.Anual,
				  BillingTypeId = (int)BillingTypes.Pospago,
				  CreatedOn = DateTime.MinValue,
			  }
		  }),
		  (typeof(SubscriptionLine), new SubscriptionLine[]
		  {
			  new ()
			  {
				  Id = 1,
				  SubscriptionId = 1,
				  ProductId = 1,
				  Quantity = 5,
				  CreatedOn = DateTime.MinValue,
			  },
			  new ()
			  {
				  Id = 2,
				  SubscriptionId = 1,
				  ProductId = 1,
				  Quantity = 10,
				  CreatedOn = DateTime.MinValue,
			  }
		  }),
		  
	};

	private static Guid guid(byte id) => new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, id);
}