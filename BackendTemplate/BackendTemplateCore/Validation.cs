using System.Text.RegularExpressions;
using BackendTemplateCore.Errors;

namespace BackendTemplateCore;

public class Validation
{
    static void Validate<T>(T data, string parameter_name, Func<T, bool> validator, string error_message) {
        if (!validator(data))
            throw new InvalidParameter($"Parámetro inválido: {parameter_name}\n - {error_message}");
    }

    static bool IsNull<T>(T data) =>
        data is null
        || (data is string  && string.IsNullOrWhiteSpace(data as string))
        || (data is int     && (data as int?) == 0)
        || (data is float   && (data as float?) == 0)
        || (data is double  && (data as double?) == 0)
        || (data is decimal && (data as decimal?) == 0);

    static void Require<T>(T data, string parameter_name) {
        if (IsNull(data))
            throw new InvalidParameter($"Parámetro inválido: {parameter_name}\n - Requerido.");
    }
    static Func<T, bool> IsNullOr<T>(Func<T, bool> check) => (T data) => IsNull(data) || check(data);

    static readonly Regex DocumentFormat    = new(@"^\s*\d{3}-?\d{7}-?\d\s*$");
    static readonly Regex PhoneFormat       = new(@"^\s*\(?\d{3}\)?[-\s]?\d{3}[-\s]?\d{4}\s*$");
    static readonly Regex EmailFormat       = new(@"^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$");
    
    public static void ValidateEmail(string email) {
        Require(email, "Correo electrónico");
        Validate(email, "Correo electrónico", EmailFormat.IsMatch, "Correo inválido.");
    }
    public static void ValidateUserData(UserData data) {
        Require(data.FirstName, "Nombres");
        Require(data.LastName, "Apellidos");
        Require(data.Password, "Contraseña");
        Validate(data, "Datos de acceso", _ => _.Email?.NullIfNothing() is not null, "Es necesario que el usuario tenga un correo" );
        Validate(data.Password, "Contraseña", _ => !(_.StartsWith(" ") || _.EndsWith(" ")), "No puede empezar ni terminar con espacios.");
        Validate(data.Password, "Contraseña", _ => _.Length >= 8, "Debe tener al menos 8 caracteres.");
        Validate(data.Email, "Correo electrónico", IsNullOr<string?>(EmailFormat.IsMatch!), "Correo inválido.");
        Validate(data.Phone, "Teléfono/Celular", IsNullOr<string?>(PhoneFormat.IsMatch!), "Teléfono inválido.");
    }
    public static void ValidateUserUpdateData(UserUpdateData data)
    {
        Require(data.FirstName, "Nombres");
        Require(data.LastName, "Apellidos");
        Validate(data, "Datos de acceso", _ => _.Email?.NullIfNothing() is not null || _.Phone?.NullIfNothing() is not null, "Es necesario registrarse con correo o numero de celular" );
        Validate(data.Email, "Correo electrónico", IsNullOr<string?>(EmailFormat.IsMatch!), "Correo inválido.");
        Validate(data.Phone, "Teléfono/Celular", IsNullOr<string?>(PhoneFormat.IsMatch!), "Teléfono inválido.");
    }

    public static void ValidateCityData(CityData data)
    {
        Require(data.Name, "Nombre");
        Require(data.StateId, "Provincia/Estado");
    }

    public static void ValidatePaymentTermData(PaymentTermData data)
    {
        Require(data.Name, "Nombre");
        Require(data.DueDays, "Días de vencimiento");
        Require(data.LateFeeDays, "Días de mora");
        Require(data.LateFeeRate, "Tasa de mora");
        Validate(data.DueDays, "Días de vencimiento", _ => _ > 0, "Debe ser mayor a 0.");
        Validate(data.LateFeeDays, "Días de mora", _ => _ > 0, "Debe ser mayor a 0.");
        Validate(data.LateFeeRate, "Tasa de mora", _ => _ > 0, "Debe ser mayor a 0.");
    }

    public static void ValidateStateData(StateData data)
    {
        Require(data.Name, "Nombre");
        Require(data.CountryId, "Provincia/Estado");
    }

    public static void ValidateReceivableReasonData(ReceivableReasonData data)
    {
        Require(data.Name, "Nombre");
        Require(data.DocumentTypeId, "Tipo de documento");
    }

    public static void ValidateBrigadeSpecialtyData(BrigadeSpecialtyData data)
    {
        Require(data.Code, "Código");
        Require(data.Name, "Nombre");
    }

    public static void ValidateTaxScheduleData(TaxScheduleData data)
    {
        Require(data.Name, "Nombre");
        Require(data.Rate, "Tasa de impuesto");
    }

    public static void ValidateNCFSequenceData(NCFSequenceData data)
    {
        Require(data.Series, "Serie");
        Require(data.TypeId, "Tipo de NCF");
        Require(data.Min, "Número de inicio");
        Require(data.Max, "Número de fin");
        Require(data.DueDate, "Fecha de vencimiento");
    }

    public static void ValidateBranchData(BranchData data)
    {
        Require(data.Code, "Código");
        Require(data.CityId, "Id de ciudad");
        Require(data.BranchTypeId, "Tipo de sucursal");
        Require(data.Locality, "Localidad");
        Require(data.Address, "Dirección");
        Require(data.Phone, "Teléfono");
        Validate(data.Phone, "Teléfono/Celular", IsNullOr<string?>(PhoneFormat.IsMatch!), "Teléfono inválido.");
    }

    public static void ValidateCompanyData(CompanyData data)
    {
        Require(data.Name, "Nombre");
        Require(data.TaxRegistrationNumber, "RNC");
        Require(data.AddressLine1, "Dirección");
        Require(data.AddressLine2, "Dirección");
        Require(data.CityId, "Ciudad");
        Require(data.Phone, "Teléfono");
        ValidateEmail(data.Email);
        Validate(data.Phone, "Teléfono/Celular", IsNullOr<string?>(PhoneFormat.IsMatch!), "Teléfono inválido.");
    }

    public static void ValidateCompanySettingsData(CompanySettingsData data)
    {
        Require(data.TimeZone, "Zona horaria");
        Require(data.DatePattern, "Formato de fecha");
        Require(data.TimePattern, "Formato de hora");
        Require(data.LateFeeProductId, "Producto de mora");
    }

    public static void ValidateCurrencyData(CurrencyData data)
    {
        Require(data.Code, "Código");
        Require(data.Name, "Nombre");
        Require(data.Symbol, "Símbolo");
    }
    public static void ValidateCurrencyRateData(CurrencyRateData data)
    {
        Require(data.StartDate, "Fecha de inicio");
        Require(data.EndDate, "Fecha de fin");
        Require(data.Rate, "Tasa de cambio");
    }

    public static void ValidateElectricalEquipmentData(ElectricalEquipmentData data)
    {
        Require(data.Code, "Código");
        Require(data.Name, "Nombre");
        Require(data.Power, "Potencia");
        Require(data.UsedHours, "Horas de uso");
        Require(data.DailyUsage, "Consumo diario(Kwh)");
        Require(data.MonthlyUsage, "Consumo mensual(Kwh)");
    }

    public static void ValidateCustomerData(CustomerData data)
    {
        Require(data.TypeId, "Tipo de cliente");
        Require(data.DocumentTypeId, "Tipo de documento");
        Require(data.Document, "Número de documento");
        Require(data.Name, "Nombre");
        Require(data.NCFTypeId, "Tipo de NCF");
        switch (data.DocumentTypeId)
        {
            case (int)DocumentTypes.Cedula:
                Validate(data.Document, "Número de documento", _ => _.Length == 11, "El número de documento debe tener 11 dígitos.");
                break;
            case (int)DocumentTypes.RNC:
                Validate(data.Document, "Número de documento", _ => _.Length == 9, "El número de documento debe tener 9 dígitos.");
                Validate(data.Representative, "Representante Legal", _ => _?.DocumentTypeId is not null && !string.IsNullOrWhiteSpace(_?.Document) && !string.IsNullOrWhiteSpace(_?.Name), "Debe ingresar datos de representante legal.");
                switch (data.Representative?.DocumentTypeId)
                {
                    case (int)DocumentTypes.Cedula:
                        Validate(data.Representative?.Document, "Representante: Número de documento", _ => _.Length == 11, "El número de documento debe tener 11 dígitos.");
                        break;
                    case (int)DocumentTypes.RNC:
                        Validate(data.Representative?.Document, "Representante: Número de documento", _ => _.Length == 9, "El número de documento debe tener 9 dígitos.");
                        break;
                    case (int)DocumentTypes.Pasaporte:
                        Require(data.Representative?.CountryId, "País");
                        Validate(data.Representative?.Document, "Representante: Número de documento", _ => _.Length == 9, "El número de documento debe tener 9 dígitos.");
                        break;
                }
                break;
            case (int)DocumentTypes.Pasaporte:
                Require(data.CountryId, "País");
                Validate(data.Document, "Número de documento", _ => _.Length == 9, "El número de documento debe tener 9 dígitos.");
                break;
        }
    }

    public static void ValidateCustomerAddressData(CustomerAddressData data)
    {
        Require(data.CustomerId, "Cliente");
        Require(data.SiteTypeId, "Tipo de propiedad");
        Require(data.ZoneTypeId, "Tipo de área");
        Require(data.Line1, "Dirección Linea 1");
        Require(data.Line2, "Dirección Linea 2");
        Require(data.CityId, "Ciudad");
        //Validate(data, "Datos de contacto", _ => _.Contact?.Name.NullIfNothing() is not null && (_.Contact?.Email?.NullIfNothing() is not null || _.Contact?.Phone?.NullIfNothing() is not null), "Es necesario registrarse con correo o numero de celular" );
        Validate(data.Contact?.Email, "Correo electrónico", IsNullOr<string?>(EmailFormat.IsMatch!), "Correo inválido.");
        Validate(data.Contact?.Phone, "Teléfono/Celular", IsNullOr<string?>(PhoneFormat.IsMatch!), "Teléfono inválido.");
        Validate(data, "Número de medidor", _ => (_.MeterNumber is null && data.SerialNumber is null && data.MeterModelId is null) || (_.MeterNumber is not null && data.SerialNumber is not null && data.MeterModelId is not null), "Es necesario registrar el número de serie y modelo del medidor.");
        
    }

    public static void ValidateProductData(ProductData data)
    {
        Require(data.Name, "Nombre");
        Require(data.Barcode, "Código de barras");
        Require(data.Price, "Precio unitario");
        Require(data.AppliesToSubscription, "Aplica a suscripción");
        Require(data.RequiresMeasurement, "Medición requerida");
        Require(data.PriceTypeId, "Tipo de precio");
        Require(data.TaxTypeId, "Tipo de impuesto");
    }

    public static void ValidateProductRateData(ProductRateData data, bool inProduct = false)
    {
        Require(data.Name, "Nombre");
        Require(data.DateStart, "Fecha de inicio");
        Require(data.DateEnd, "Fecha de fin");
        Validate(data, "Escalones", _ => _.Scales?.Count > 0, "Debe registrar al menos un escalón.");
        // must check if the scales dont overlap and dont overlap with each other
        Validate(data, "Escalones", _ => _.Scales?.All(s => s.LowerQuantity < s.UpperQuantity || s.UpperQuantity is null) == true, "Los escalones deben ser consecutivos.");
        //Validate(data, "Escalones", _ => _.Scales?.All(s => s.LowerQuantity > 0) == true, "Los escalones deben ser mayores a cero.");
        Validate(data, "Escalones", _ => _.Scales?.All(s => s.Price > 0) == true, "Los precios deben ser mayores a cero.");
        Validate(data, "Escalones", _ => _.Scales?.All(s => s.UpperQuantity is null or > 0) == true, "Los escalones deben ser mayores a cero.");
        // if there is a scale with null upper quantity, it must be the last one
        Validate(data, "Escalones", _ => _.Scales?.Where(s => s.UpperQuantity is null).Count() == 0 || (_.Scales?.Last().UpperQuantity is null && _.Scales?.Where(s => s.UpperQuantity is null).Count() == 1), "Solo el último escalón puede tener cantidad máxima nula.");
        if (!inProduct)
            Require(data.ProductId, "Producto");
        
    }

    public static void ValidateProductRateScaleData(ProductRateScaleData data, bool inProductRate = false)
    {
        Validate(data.LowerQuantity, "Cantidad mínima", _ => _ >= 0, "Cantidad mínima no puede ser menor a cero");
        Require(data.Price, "Precio");
    }

    public static void ValidateProductRateExtraChargeData(ProductRateExtraChargeData data, bool inProductRate = false)
    {
        Require(data.ProductId, "Producto");
        Require(data.Quantity, "Cantidad");
        
    }

    public static void ValidateSubscriptionData(SubscriptionData data)
    {
        Require(data.CustomerId, "Cliente");
        Require(data.BillingAddressId, "Dirección de facturación");
        Require(data.ShippingAddressId, "Dirección de envío");
        Require(data.DateStart, "Fecha de suscripción");
        Require(data.TypeId, "Tipo de suscripción");
        Require(data.TermId, "Plazo de pago");
        Require(data.FrequencyId, "Frecuencia");
        Require(data.IsElectrical, "Es eléctrico");
        Require(data.BillingTypeId, "Tipo de facturación");
        Require(data.DeliveryMethodId, "Método de entrega");
        Validate(data.DateStart, "Fecha de suscripción", _ => _.Date <= DateTime.Now.Date && _.Date != default, "La fecha de suscripción no puede ser mayor a la fecha actual.");
        Validate(data, "Datos electricos", _ => _.IsElectrical is false || (_.ElectricalData?.Power != null && _.ElectricalData?.ConnectionTypeId is not null && _.ElectricalData?.VoltageId is not null && _.ElectricalData?.BranchId is not null), "Debe ingresar los datos tecnicos si es una suscripcion electrica.");
        Validate(data, "Datos de suscripción", _ => _.TypeId != (int)SubscriptionTypes.Limitada || _.DateEnd.HasValue, "Debe ingresar una fecha de vencimiento.");
        
    }

    public static void ValidateRouteData(RouteData data)
    {
        Require(data.Name, "Nombre");
        Require(data.TypeId, "Tipo de ruta");
        Require(data.BillingCycleId, "Ciclo de facturación");
    }

    public static void ValidateInvoiceData(InvoiceData data)
    {
        Require(data.CustomerId, "Cliente");
        Require(data.TermId, "Plazo de pago");
        Require(data.NCFTypeId, "Tipo de NCF");
        Validate(data.Details, "Líneas de factura", _ => _.Any(), "Debe ingresar al menos una línea de factura.");
        Validate(data.Details, "Líneas de factura", _ => _.Select(s => s.ProductId).Distinct().Count() == _.Count, "No puede haber dos líneas de factura con el mismo producto.");
        Validate(data.Details, "Líneas de factura", _ => _.All(s => s.ProductId is not 0), "Debe ingresar un producto."); 
        Validate(data.Details, "Líneas de factura", _ => _.All(s => s.Quantity > 0), "La cantidad debe ser mayor a cero.");
    }

    public static void ValidateChargeData(ChargeData data)
    {
        Require(data.CustomerId, "Cliente");
        Require(data.SubscriptionId, "Suscripción");
        Require(data.ProductId, "Producto");
        Require(data.Quantity, "Cantidad");
        Validate(data.Quantity, "Cantidad", _ => _ > 0, "La cantidad debe ser mayor a cero.");
    }

    public static void ValidateInvoiceSearchData(InvoiceSearchData data)
    {
        Validate(data, "Datos de consulta de pago", _ => _.CustomerId.HasValue || !IsNull(_.CustomerDocument) || !IsNull(_.SubscriptionNumber) || !IsNull(_.Filter), "Debe ingresar al menos un criterio de búsqueda.");
    }

    public static void ValidateCreditMemoData(MemoData data)
    {
        Require(data.CustomerId, "Cliente");
        Require(data.ReasonId, "Motivo");
        Validate(data.Invoices, "Aplicaciones de Pago", _ => _.Any(), "Debe ingresar al menos una aplicación.");
        Validate(data.Invoices, "Aplicaciones de Pago", _ => _.All(s => s.AmountToPay > 0), "El monto de la aplicación debe ser mayor a cero.");
        
    }
    
    public static void ValidateDebitMemoData(MemoData data)
    {
        Require(data.CustomerId, "Cliente");
        Require(data.ReasonId, "Motivo");
        Validate(data.Invoices, "Aplicaciones de Pago", _ => _.Any(), "Debe ingresar al menos una aplicación.");
        Validate(data.Invoices, "Aplicaciones de Pago", _ => _.All(s => s.AmountToPay > 0), "El monto de la aplicación debe ser mayor a cero.");
    }

    public static void ValidateBillingScheduleData(BillingScheduleData data)
    {
        Require(data.Name, "Nombre");
        Require(data.FrequencyId, "Frecuencia");
    }

    public static void ValidateBillingCycleData(BillingCycleData data)
    {
        Require(data.Name, "Nombre");
        Require(data.BillingScheduleId, "Programación de ciclo de facturación");
    }

    public static void ValidateCustomerTicketData(CustomerTicketData data)
    {
        Require(data.ContactTypeId, "Tipo de contacto");
        Require(data.Name, "Nombre completo");
        switch (data.ContactTypeId)
        {
            case (int)ContactTypes.CorreoElectronico:
                Require(data.CustomerId, "Cliente");
                break;
            case (int)ContactTypes.Telefonico:
                Require(data.Phone, "Teléfono");
                Validate(data.Phone, "Teléfono", IsNullOr<string?>(PhoneFormat.IsMatch!), "El formato del teléfono no es válido.");
                break;
        }
        Require(data.CategoryId, "Tipo de ticket");
        Require(data.Subject, "Asunto");
        Validate(data , "Datos de contacto", _ => !IsNull(_.CustomerId) || !IsNull(_.Phone), "Debe ingresar al menos un medio de contacto.");
    }

    public static void ValidateClaimData(ClaimData data)
    {
        Require(data.CustomerId, "Cliente");
        Require(data.SubscriptionId, "Suscripción");
        Require(data.TypeId, "Tipo de reclamo");
        Require(data.MotiveId, "Motivo de reclamo");
        Require(data.LegalInstanceId, "Instancia legal");
        Require(data.InvoiceIds, "Facturas");
        Validate(data.InvoiceIds, "Facturas", _ => _.Any(), "Debe ingresar al menos una factura.");
    }

    public static void ValidatePaymentData(PaymentData data)
    {
        Require(data.MethodId, "Método de pago");
        Require(data.CurrencyId, "Moneda");
        Require(data.Invoices, "Aplicaciones de Pago");
        Validate(data.Invoices, "Aplicaciones de Pago", _ => _.Any(), "Debe ingresar al menos una aplicación.");
        Validate(data.Invoices, "Aplicaciones de Pago", _ => _.All(s => s.AmountToPay > 0), "El monto de la aplicación debe ser mayor a cero.");
        switch (data.MethodId)
        {
            case (int)PaymentMethodTypes.Efectivo:
                Require(data.CashAmount, "Monto en efectivo");
                Validate(data.CashAmount, "Monto en efectivo", _ => _ > 0, "El monto en efectivo debe ser mayor a cero.");
                Validate(data.CashAmount, "Monto en efectivo", _ => _ >= data.Invoices.Sum(a => a.AmountToPay), "El monto en efectivo debe ser mayor o igual al monto de las aplicaciones.");
                break;
            case (int)PaymentMethodTypes.Tarjeta:
                Require(data.ApprovalNumber, "Número de aprobación");
                Require(data.Lot, "Número de lote");
                Require(data.CardTerminal, "Número de tarjeta");
                Validate(data.CardTerminal, "Número de tarjeta", _ => _.All(char.IsDigit), "El número de tarjeta debe contener solo números.");
                break;
            case (int)PaymentMethodTypes.Cheque or (int)PaymentMethodTypes.Transferencia:
                Require(data.BankId, "Banco");
                Require(data.Reference, "Referencia");
                break;
            default:
                Require(data.Reference, "Referencia");
                break;
        }
        
    }

    public static void ValidateAdvancedPaymentData(AdvancedPaymentData data)
    {
        Require(data.MethodId, "Método de pago");
        Require(data.SubscriptionId, "Suscripción");
        Require(data.Amount, "Monto");
        Validate(data.Amount, "Monto", _ => _ > 0, "El monto debe ser mayor a cero.");
        switch (data.MethodId)
        {
            case (int)PaymentMethodTypes.Efectivo:
                Require(data.CashAmount, "Monto en efectivo");
                Validate(data.CashAmount, "Monto en efectivo", _ => _ > 0, "El monto en efectivo debe ser mayor a cero.");
                Validate(data.CashAmount, "Monto en efectivo", _ => _ >= data.Amount, "El monto en efectivo debe ser mayor o igual al monto de las aplicaciones.");
                break;
            case (int)PaymentMethodTypes.Tarjeta:
                Require(data.ApprovalNumber, "Número de aprobación");
                Require(data.Lot, "Número de lote");
                Require(data.CardNumber, "Número de tarjeta");
                break;
            case (int)PaymentMethodTypes.Cheque or (int)PaymentMethodTypes.Transferencia:
                Require(data.BankId, "Banco");
                Require(data.Reference, "Referencia");
                break;
            default:
                Require(data.Reference, "Referencia");
                break;
        }
    }

    public static void ValidateRelatedPersonData(RelatedPersonData data)
    {
        Require(data.Name, "Nombre completo");
        Require(data.Document, "Número de documento");
        Require(data.DocumentTypeId, "Tipo de documento");
        Require(data.RelationshipId, "Parentesco");
        Validate(data, "Datos de persona relacionada", _ => !IsNull(_.Email) || !IsNull(_.Phone), "Debe ingresar al menos un medio de contacto.");
        if (!IsNull(data.Email))
            Validate(data.Email, "Correo electrónico", IsNullOr<string?>(EmailFormat.IsMatch!), "El formato del correo electrónico no es válido.");
        if (!IsNull(data.Phone))
            Validate(data.Phone, "Teléfono", IsNullOr<string?>(PhoneFormat.IsMatch!), "El formato del teléfono no es válido.");
    }

    public static void ValidateEquipmentData(EquipmentData data)
    {
        Require(data.SubscriptionId, "Suscripción");
        Require(data.Type, "Tipo de equipo");
        Require(data.Brand, "Marca");
        Require(data.Number, "Modelo");
        Require(data.Reading, "Lectura");
        Require(data.ReadingType, "Tipo de lectura");
        Require(data.ReadingDate, "Fecha de lectura");
        Require(data.Consumption, "Consumo");
        Require(data.InstallationDate, "Fecha de instalación");
        Validate(data.ReadingDate, "Fecha de lectura", _ => _ != default, "La fecha de lectura no es válida.");
        Validate(data.InstallationDate, "Fecha de instalación", _ => _ != default, "La fecha de instalación no es válida.");
    }

    public static void ValidatePaymentAgreementData(PaymentAgreementData data)
    {
        Require(data.CustomerId, "Cliente");
        Require(data.Duration, "Duración");
        Require(data.InterestRate, "Tasa de interés");
        Require(data.DateStart, "Fecha de inicio");
        Require(data.InvoiceIds, "Facturas");
        Validate(data.InvoiceIds, "Facturas", _ => _.Count > 0, "Debe seleccionar al menos una factura.");
        Validate(data.DateStart, "Fecha de inicio", _ => _ != default, "La fecha de inicio no es válida.");
    }

    public static void ValidateWorkOrderData(WorkOrderData data)
    {

        Validate(data, "Nueva orden de trabajo", _ => !IsNull(_.SubscriptionId) || !IsNull(_.AddressId), "Debe ingresar al menos una suscripción o dirección"); 
        Require(data.TypeId, "Tipo de orden de trabajo");
    }

    public static void ValidateNonDebtCertificateData(NonDebtCertificateData data)
    {
        Require(data.SubscriptionId, "Suscripción");
    }

    public static void ValidateWorkOrderTypeData(WorkOrderTypeData data)
    {
        Require(data.Name, "Nombre");
    }

    public static void ValidateMeterData(MeterData data)
    {
        Require(data.MeterModelId, "Modelo de medidor");
        Require(data.CustomerAddressId, "Dirección");
        Require(data.SerialNumber, "Número de serie");
        Require(data.MeterNumber, "Número de medidor");
    }

    public static void ValidateMeterModelData(MeterModelData data)
    {
        Require(data.Brand, "Marca");
        Require(data.Model, "Modelo");
        Require(data.Telemetry, "Telemetría");
        Require(data.ReadingTypeIds, "Tipos de lectura");
        Validate(data.ReadingTypeIds, "Tipos de lectura", _ => _.Any(), "Debe seleccionar al menos un tipo de lectura.");
    }
}