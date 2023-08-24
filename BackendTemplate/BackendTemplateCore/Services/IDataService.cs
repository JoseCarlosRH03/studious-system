using System.Linq.Expressions;
using BackendTemplateCore.Models;
using BackendTemplateCore.Models.Billings;
using BackendTemplateCore.Models.Invoices;
using BackendTemplateCore.Models.Payments;
using BackendTemplateCore.Models.Subscriptions;
using BillingBatchDetailedView = Core.DTOs.BillfastNGDTOs.BillingBatchDetailedView;

namespace BackendTemplateCore.Services;

public interface IDataService {

   Task<T?> GetByIdAsync<T>(params object[] keys) where T : class;
   Task<T?> GetAsync<T>(Expression<Func<T, bool>>? expression = null, params Expression<Func<T, object>>[] includes)
      where T : class;
   // Task<List<T>?> GetAll<T>(Expression<Func<T, bool>>? expression = null) where T : class;
   Task<List<T>> GetAll<T>(Expression<Func<T, bool>>? expression = null, params Expression<Func<T, object>>[] includes)
      where T : class;
   Task<int> Count<T>(Expression<Func<T, bool>>? expression = null) where T : class;
   Task<T?> GetLast<T>(Expression<Func<T, bool>>? expression = null) where T : AuditableEntity;
   Task<T> Add<T>(T entity) where T : class;
   Task AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
   Task Update<T>(T entity, Guid? userId = null) where T : class;
   Task Delete<T>(T entity) where T : class;
   Task DeleteRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
   Task Atomic(Func<Task> operation);
   Task RegisterLogin(Guid userId);
   Task<Guid> LockoutUser(User user, int days = 0, int hours = 0);
   Task<Guid> UnlockoutUser(User user);
   
   Task<Country?> GetCurrentCountry();
   
   Task<bool> ExistsBankWithName(string name);
   Task<bool> ExistsCityWithName(string name, int stateId);
   Task<bool> ExistsStateWithName(string name, int countryId);
   Task<bool> ExistsCustomerTypeWithName(string name);
   Task<bool> ExistsClaimTypeWithName(string name);
   Task<bool> ExistsClaimMotiveWithName(string name);
   Task<bool> ExistsLegalInstanceWithName(string name);
   Task<bool> ExistsPaymentTermWithName(string name);
   Task<bool> ExistsPaymentMethodWithName(string name);
   Task<bool> ExistsCustomerTicketTypeWithName(string name);
   Task<bool> ExistsReceivableReasonWithName(string name, int type);
   Task<bool> ExistsPropertyTypeWithName(string name);
   Task<bool> ExistsTaxScheduleWithName(string dataName);
   Task<bool> ExistsNCFSequenceWithNcfType(int NCFType);
   Task<bool> ExistsBranchWithCode(string Code);
   Task<bool> ExistsCurrencyWithCode(string Code, string Symbol);
   Task<bool> ExistsElectricalEquipmentWithCode(string code);
   Task<bool> ExistsRoleWithName(string name);
   Task<bool> ExistsCustomerWithData(CustomerData data, int Id = 0);
   Task<bool> ExistsProductWithNameOrBarcode(ProductData data, int? id);
   Task<bool> ExistsProductRateWithNameAndId(ProductRateData data);
   Task<bool> AddressIsAlreadyAssociated(int shippingAddressId, int? subscriptionId = null);
   Task<bool> AddressIsAssociatedWithCustomer(int addressId, int customerId);
   Task<bool> ExistsRouteWithName(string name);
   Task<bool> ExistsRouteTypeWithName(string name);
   Task<bool> ExistsUserWithEmail(string email);
   Task<bool> ExistsUserWithPhone(string phone);
   Task<bool> ExistsVoltageWithName(string name);
   Task<bool> ExistsConnectionTypeWithName(string name);
   Task<bool> ExistsBillingTypeWithName(string name);
   Task<bool> ExistsChargeInSubscription(ChargeData data);
   Task<bool> ExistsBillingScheduleWithName(string name);
   Task<bool> ExistsBillingCycleWithName(string name);
   Task<bool> ExistsWorkOrderTypeWithName(string name);
   Task<bool> ExistsReadingType(int readingTypeId);
   Task<bool> PeriodsGenerated(int id, int year);
   Task IsLotPossible(Guid userId);
   Task<List<CustomerAddressListView>> GetCustomerAddressListViews(int start, int count, AddressSearchData data);
   Task<CustomerAddressDetailedView> GetCustomerAddressView(int id);
   Task<int> GetCustomerAddressCount(AddressSearchData data);
   Task<List<InvoiceListView>> GetInvoiceViews(int start, int count, InvoiceSearchData data);
   Task<int> GetInvoiceCount(InvoiceSearchData data);
   Task<PaymentDetailedView> GetPaymentView(int Id);
   Task<List<PaymentListView>> GetPaymentViews(int start, int count, PaymentSearchData data);
   Task<int> GetPaymentCount(PaymentSearchData data);
   Task<List<Invoice>> GetInvoiceListForDate(DateTime data);
   Task<List<PaymentApply>> GetPaymentListForDate(DateTime date);
   Task<List<Payment>> GetAdvancedPaymentListForDate(DateTime date);
   Task<bool> GetAddressIdsWithData(int cityId, string addressLine1, out int[]? ids);
   Task<InvoicePaymentSummaryView> GetInvoiceSummariesForPayments(InvoiceSearchData data);
   Task ValidateInvoiceBalance(ICollection<PaymentApplyData> applies, int paymentId, ReceivableReasonTypes type);
   Task<List<BillingBatchListView>> GetBillingBatches(int start, int count, string? filter);
   Task<List<BillingScheduleListView>> GetBillingSchedules(int start, int count, string? filter);
   Task<List<BillingCycleListView>> GetBillingCycles(int start, int count, string? filter, int? status);
   Task<List<CustomerListView>> GetCustomerViews(int start, int count, string? filter, int? status);
   Task<List<TicketListView>> GetCustomerTickets(int start, int count, string? filter, int? status);
   Task<List<ClaimListView>> GetClaims(int start, int count, string? filter);
   Task<List<CityView>> GetCities(int start, int count, string? filter);
   Task<List<StateView>> GetStates(string? filter);
   Task<List<StatusItemView>> GetCountries(string? filter);
   Task<List<RouteListView>> GetRoutes(int? statusId, int start, int count, string? filter);
   Task<RouteDetailedView> GetRouteView(int id);
   Task<PaymentAgreementView> GetPaymentAgreementView(int paymentAgreementId);
   Task<List<PaymentAgreementListView>> GetPaymentAgreements(int start, int count, string? filter);
   Task<List<NonDebtCertificateListView>> GetNonDebtCertificates(int start, int count, string? filter);
   Task<List<MemoListView>> GetCreditMemos(int start, int count, string? filter);
   Task<List<MemoListView>> GetDebitMemos(int start, int count, string? filter);
   Task<List<SubscriptionListView>> GetSubscriptions(int start, int count, string? filter, int? customer_id, int? status);
   Task<List<ChargeListView>> GetCharges(int start, int count, string? filter);
   Task<List<WorkOrderView>> GetWorkOrders(int start, int count, string? filter);
   Task<Customer360View> GetCustomer360View(int id);
   Task<InvoiceDetailedView> GetInvoiceView(int invoiceId);
   Task<List<DayLotsView>> GetLotViews(int start, int count, LotSearchData data);
   Task<int> GetLotCount(LotSearchData data);
   Task<LotView> GetLotView(Guid id);
   Task<LotBalanceClosingView> GetLotBalanceView(Guid id);
   Task<MemoDetailedView> GetMemoView(int id, ReceivableReasonTypes type);
   Task<ClaimView> GetClaimView(int claimId);
   Task<SubscriptionDetailedView> GetSubscriptionView(int id);
   Task<List<AnomalyListView>> GetAnomalies(int start, int count, string filter, int[] status_id);
   Task<int> GetAnomalyCount(string filter, int[]? status_id);
   Task<AnomalyView> GetAnomaly(int id);
   Task<List<string>> GetCashiers();
   Task<Subscription> GetSubscriptionBillingData(int subscriptionId);
   Task<Invoice> CreateInvoice(Invoice invoice);
   Task<Payment> CreatePayment(Payment payment);
   Task<DebitMemo> CreateDebitMemo(DebitMemo memo);
   Task<CreditMemo> CreateCreditMemo(CreditMemo memo);
   Task<List<MeterView>> GetMeters(int start, int count, string? filter);

   Task<MeterView> GetMeterView(int id);
   Task<bool> ExistsMeter(string meterNumber, int id = 0);
   Task<List<MeterModelView>> GetMeterModels(string filter);
   Task<bool> ExistsMeterModel(string brand, string model, int id = 0);
   Task<User> GetUserWithRoles(Guid userId);
   Task<User?> GetUserLogin(string login);
   Task<List<RoleView>> GetRoleViews(int start, int count, string? filter);
   Task<List<User>> GetUserViews(string? filter, int start, int count);
   Task<TicketDetailedView> GetCustomerTicketView(int id);
   Task<LotDetailedView> GetDetailedLotView(Guid lotId);
   Task<List<APIRouteView>> GetAPIRouteViews();
   Task<ProductDetailedView> GetProductView(int id);
   Task<BillingBatch> GetBillingBatchForProcessing(int batchId);
   Task<BillingBatchDetailedView> GetBillingBatchView(int bbatchId);
   Task<List<MeterQueryView>> GetMeterQuery(string? number, string? subscription, int? clientId);
   Task<DetailedMeterQueryView> GetDetailedMeterQuery(int meterId);
   
   #region Billing Batch Methods
   Task GenerateBillingBatch(int bbatchId, DateTime date);
   Task VoidBatch(int bbatchId, Guid userId);
   Task<int> CreateAnomaly(AnomalyTypes type, Guid userId, int subscriptionId, string description, int? batchId);
   #endregion

   #region Report Methods
   Task<List<InvoiceReportData>> GetInvoiceReportData(params int[] ids);
   Task<PaymentReportData> GetPaymentReportData(int id);
   Task<PaymentVoidReportData> GetPaymentVoidReportData(int id);
   Task<PaymentSummaryReportData> GetPaymentSummaryReportData(DateTime start, DateTime end, string? cashier);
   Task<PaymentERPReportData> GetPaymentERPReportData(DateTime start, DateTime end);
   Task<List<NonAccountingReportLinesData>> GetProductReportData(DateTime from, DateTime to);
   Task<SubscriptionVoidReportData?> GetSubscriptionVoidReportData(int subscriptionId);
   Task<SubscriptionReportData> GetSubscriptionReportData(int subscriptionId);
   Task<PaymentAgreementReportData> GetPaymentAgreementReportData(int paymentAgreementId);
   Task<ClaimReportData> GetClaimReportData(int claimId);
   Task<LotCloseReportData> GetLotReportData(Guid lotId);
   Task<List<WorkOrderSummaryReportLinesData>> GetWorkOrderReportData(DateTime from, DateTime to);
   Task<int[]> GetInvoiceReportIds(int? billingCycleId, int? routeId, DateTime start);
   Task<MemoReportData> GetMemoReportData(int id, ReceivableReasonTypes type);
   #endregion

}
