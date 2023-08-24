// using static BackendTemplateAPI.Extensions;
// namespace BackendTemplateAPI.Routes;
//
// public static class Reports
// {
//     public static void MapReports(this WebApplication app)
//     {
//         Tagged("Reportes", new []
//         {
//             app.MapGet("/invoice/{id:int}/report", (int id, Context ctx, HttpResponse res) => ctx.ExecuteAuthenticated(async (user, logic) =>
//                 {
//                     var data = await logic.GetInvoiceReportData(id);
//                     data.Company = await logic.GetCompanyForReport();
//
//                     var report = new InvoiceReport(data);
//
//                     if (File.Exists($"./Resources/{data.Company.Code.ToLower()}/{data.Company.Code.ToLower()}InvoiceWatermark.png"))
//                     {
//                         report.Watermark.ImageSource = ImageSource.FromFile(
//                             $"./Resources/{data.Company.Code.ToLower()}/{data.Company.Code.ToLower()}InvoiceWatermark.png");
//                         report.Watermark.ImageViewMode = ImageViewMode.Clip;
//                         report.Watermark.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
//                         report.Watermark.ShowBehind = false;
//                         report.DrawWatermark = true;
//                     }
//                     report.Landscape = false;
//                     
//                     var stream = new MemoryStream();
//                     await report.ExportToPdfAsync(stream);
//                     stream.Position = 0;
//                     
//                     res.Headers.Add("Content-Disposition", $"inline; filename=\"Factura{data.Number}.pdf\"");
//                     return Results.File(stream, "application/pdf");
//                 }, PermissionAreas.Invoices, PermissionTypes.Report))
//                 .Produces(200, null ,"application/pdf"),
//             
//             app.MapGet("/invoice/report" , (string? billing_cycle_id, string? route_id, DateTime from, Context ctx, HttpResponse res) => ctx.ExecuteAuthenticated(async (user, logic) =>
//                 {
//                     var billingCycleId = billing_cycle_id.GetInt();
//                     var routeId = route_id.GetInt();
//                     var data = await logic.GetInvoiceReportData(billingCycleId, routeId, from);
//                     var report = new XtraReport();
//                     var company = await logic.GetCompanyForReport();
//                     data.ForEach(i => i.Company = company);
//                     if (File.Exists($"./Resources/{company.Code.ToLower()}/{company.Code.ToLower()}InvoiceWatermark.png"))
//                     {
//                         report.Watermark.ImageSource = ImageSource.FromFile(
//                             $"./Resources/{company.Code.ToLower()}/{company.Code.ToLower()}InvoiceWatermark.png");
//                         report.Watermark.ImageViewMode = ImageViewMode.Clip;
//                         report.Watermark.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
//                         report.Watermark.ShowBehind = false;
//                         report.DrawWatermark = true;
//                     }
//                     report.Landscape = false;
//                     
//                     var reports = data.Select(invoice => new InvoiceReport(invoice)).ToList();
//                     await report.CreateDocumentAsync();
//                     reports.ForEach(x => x.CreateDocument());
//                     report.ModifyDocument(x => x.AddPages(reports.Select(i => i.Pages.ToArray()).SelectMany(i => i)));
//                     
//                     var stream = new MemoryStream();
//                     await report.ExportToPdfAsync(stream);
//                     stream.Position = 0;
//                     
//                     res.Headers.Add("Content-Disposition", $"inline; filename=\"FacturasDia{from.ToShortDateString()}.pdf\"");
//                     return Results.File(stream, "application/pdf");
//                 }, PermissionAreas.Invoices, PermissionTypes.Report))
//                 .Produces(200, null ,"application/pdf"),
//             
//         app.MapGet("/invoice/process", (string? billing_cycle_id, string? route_id, DateTime from, Context ctx) => ctx.ExecuteAuthenticated( 
//                 (user, logic) => logic.GetInvoiceReportCount(billing_cycle_id.GetInt(), route_id.GetInt(), from), PermissionAreas.Invoices, PermissionTypes.Report))
//                 .Produces<int>(),
//         
//         app.MapGet("/payment/{id:int}/report", (int id, Context ctx, HttpResponse res) => ctx.ExecuteAuthenticated(
//             async (user, logic) =>
//             {
//                 var data = await logic.GetPaymentReportData(id);
//                 data.Company = await logic.GetCompanyForReport();
//                 XtraReport paymentReport;
//                 if (data.Company.Code.ToLower() == "amn")
//                     paymentReport = new PaymentDetailedReport(data);
//                 else paymentReport = new PaymentReport(data);
//                 paymentReport.Landscape = false;
//
//                 var stream = new MemoryStream();
//                 await paymentReport.ExportToPdfAsync(stream);
//                 stream.Position = 0;
//
//                 res.Headers.Add("Content-Disposition", $"inline; filename=\"Pago{data.DocumentNumber}.pdf\"");
//                 return Results.File(stream, "application/pdf");
//             }, PermissionAreas.Payments, PermissionTypes.Report))
//             .Produces(200, null, "application/pdf"),
//         
//         app.MapGet("/payment/{id:int}/void/report", (int id, Context ctx, HttpResponse res) => ctx.ExecuteAuthenticated(
//             async (user, logic) =>
//             {
//                 var data = await logic.GetPaymentVoidReportData(id);
//                 data.Company = await logic.GetCompanyForReport();
//
//                 var paymentReport = new PaymentVoidReport(data);
//                 paymentReport.Landscape = false;
//
//                 var stream = new MemoryStream();
//                 await paymentReport.ExportToPdfAsync(stream);
//                 stream.Position = 0;
//
//                 res.Headers.Add("Content-Disposition", $"inline; filename=\"AnulacionPago{data.DocumentNumber}.pdf\"");
//                 return Results.File(stream, "application/pdf");
//             }, PermissionAreas.Payments, PermissionTypes.Report))
//             .Produces(200, null, "application/pdf"),
//
//         app.MapGet("/payment/report", (DateTime? from, DateTime? to, string? cashier, Context ctx, HttpResponse res) => ctx.ExecuteAuthenticated(
//             async (user, logic) =>
//             {
//                 var data = await logic.GetPaymentSummaryReportData(from ?? DateTime.Today, to?.AddDays(1) ?? DateTime.Today.AddDays(1), cashier);
//                 data.Company = await logic.GetCompanyForReport();
//                 
//                 var paymentSummaryReport = new PaymentSummaryReport(data);
//                 paymentSummaryReport.Landscape = false;
//                 
//                 var stream = new MemoryStream();
//                 await paymentSummaryReport.ExportToPdfAsync(stream);
//                 stream.Position = 0;
//                 
//                 res.Headers.Add("Content-Disposition", $"inline; filename=\"ResumenPagos{(from ?? DateTime.Today).ToShortDateString()}ADia{(to ?? DateTime.Today.AddDays(1)).ToShortDateString()}.pdf\"");
//                 return Results.File(stream, "application/pdf");
//             }, PermissionAreas.Payments, PermissionTypes.Report))
//             .Produces(200, null, "application/pdf"),
//         
//         app.MapGet("/subscription/{id:int}/report", (int id, Context ctx, HttpResponse res) => ctx.ExecuteAuthenticated(
//             async (User, logic) =>
//             {
//                 var data = await logic.GetSubscriptionReportData(id);
//                 data.Company = await logic.GetCompanyForReport();
//                 
//                 var subscriptionReport = new SubscriptionReport(data);
//                 subscriptionReport.Landscape = false;
//                 
//                 var stream = new MemoryStream();
//                 await subscriptionReport.ExportToPdfAsync(stream);
//                 stream.Position = 0;
//                 
//                 res.Headers.Add("Content-Disposition", $"inline; filename=\"Suscripcion{data.Number}.pdf\"");
//                 return Results.File(stream, "application/pdf");
//             }, PermissionAreas.Subscriptions, PermissionTypes.Report)),
//         
//         app.MapGet("/subscription/{id:int}/void/report", (int id, Context ctx, HttpResponse res) => ctx.ExecuteAuthenticated(
//             async (user, logic) =>
//             {
//                 var data = await logic.GetSubscriptionVoidReportData(id);
//                 data.Company = await logic.GetCompanyForReport();
//                 
//                 var subscriptionReport = new SubscriptionVoidReport(data);
//                 subscriptionReport.Landscape = false;
//                 
//                 var stream = new MemoryStream();
//                 await subscriptionReport.ExportToPdfAsync(stream);
//                 stream.Position = 0;
//                 
//                 res.Headers.Add("Content-Disposition", $"inline; filename=\"Suscripcion{data.Number}.pdf\"");
//                 return Results.File(stream, "application/pdf");
//             }, PermissionAreas.Subscriptions, PermissionTypes.Report))
//             .Produces(200, null, "application/pdf"),
//         
//         app.MapGet("/work_order/report", (DateTime from, DateTime to, Context ctx, HttpResponse res) => ctx.ExecuteAuthenticated(
//             async (user, logic) =>
//             {
//                 var data = await logic.GetWorkOrderReportData(from, to.AddDays(1));
//                 data.Company = await logic.GetCompanyForReport();
//                 
//                 var workOrderReport = new WorkOrderSummaryReport(data);
//                 workOrderReport.Landscape = false;
//                 
//                 var stream = new MemoryStream();
//                 await workOrderReport.ExportToPdfAsync(stream);
//                 stream.Position = 0;
//                 
//                 res.Headers.Add("Content-Disposition", $"inline; filename=\"OrdenesTrabajo{from.ToShortDateString()}ADia{to.ToShortDateString()}.pdf\"");
//                 return Results.File(stream, "application/pdf");
//             }, PermissionAreas.WorkOrders, PermissionTypes.Report))
//             .Produces(200, null, "application/pdf"),
//
//         app.MapGet("/product/report", (DateTime from, DateTime to, Context ctx, HttpResponse res) => ctx.ExecuteAuthenticated(
//             async (user, logic) =>
//             {
//                 var data = new NonAccountingReportData
//                 {
//                     StartDate = from,
//                     EndDate = to,
//                     Data = await logic.GetProductReportData(from, to.AddDays(1)),
//                     Company = await logic.GetCompanyForReport()
//                 };
//                 
//                 var productReport = new NonAccountingReport(data);
//                 productReport.Landscape = false;
//                 
//                 var stream = new MemoryStream();
//                 await productReport.ExportToPdfAsync(stream);
//                 stream.Position = 0;
//                 
//                 res.Headers.Add("Content-Disposition", $"inline; filename=\"ExtracontableDia{from.ToShortDateString()}ADia{to.ToShortDateString()}.pdf\"");
//                 return Results.File(stream, "application/pdf");
//             }, PermissionAreas.Products, PermissionTypes.Report))
//             .Produces(200, null, "application/pdf"),
//         
//         app.MapGet("/payment_arrangement/{id:int}/report", (int id, Context ctx, HttpResponse res) => ctx.ExecuteAuthenticated(
//             async (user, logic) =>
//             {
//                 var data = await logic.GetPaymentAgreementReportData(id);
//                 var company = await logic.GetCompanyForReport();
//                 data.Company = company.Name;
//                 data.CompanyRNC = company.RNC;
//                 
//                 var paymentArrangementReport = new PaymentAgreementReport(data);
//                 paymentArrangementReport.Landscape = false;
//                 
//                 var stream = new MemoryStream();
//                 await paymentArrangementReport.ExportToPdfAsync(stream);
//                 stream.Position = 0;
//                 
//                 res.Headers.Add("Content-Disposition", $"inline; filename=\"ArregloPago{data.Number}.pdf\"");
//                 return Results.File(stream, "application/pdf");
//             }, PermissionAreas.PaymentAgreements, PermissionTypes.Report))
//             .Produces(200, null, "application/pdf"),
//         
//         app.MapGet("/claim/{id:int}/report", (int id, Context ctx, HttpResponse res) => ctx.ExecuteAuthenticated(
//             async (user, logic) =>
//             {
//                 var data = await logic.GetClaimReportData(id);
//                 
//                 var claimReport = new ClaimReport(data);
//                 claimReport.Landscape = false;
//                 
//                 var stream = new MemoryStream();
//                 await claimReport.ExportToPdfAsync(stream);
//                 stream.Position = 0;
//                 
//                 res.Headers.Add("Content-Disposition", $"inline; filename=\"Reclamacion{data.Number}.pdf\"");
//                 return Results.File(stream, "application/pdf");
//             }, PermissionAreas.Claims, PermissionTypes.Report))
//             .Produces(200, null, "application/pdf"),
//         
//         app.MapGet("/lot/{id:guid}/report", (Guid id, Context ctx, HttpResponse res) => ctx.ExecuteAuthenticated(
//             async (user, logic) =>
//             {
//                 var data = await logic.GetLotCloseReportData(id);
//                 var company = await logic.GetCompanyForReport();
//                 data.CompanyCode = company.Code;
//
//                 var lotReport = new LotCloseReport(data);
//                 lotReport.Landscape = false;
//                 
//                 var stream = new MemoryStream();
//                 await lotReport.ExportToPdfAsync(stream);
//                 stream.Position = 0;
//                 
//                 res.Headers.Add("Content-Disposition", $"inline; filename=\"Lote{data.LotId}.pdf\"");
//                 return Results.File(stream, "application/pdf");
//             }, PermissionAreas.Lots, PermissionTypes.Report))
//             .Produces(200, null, "application/pdf"),
//         
//         app.MapGet("/payment/erp/report", (DateTime from, DateTime to, Context ctx, HttpResponse res) => ctx.ExecuteAuthenticated(
//                 async (user, logic) =>
//                 {
//                     var data = await logic.GetPaymentERPReportData(from, to.AddDays(1));
//                     data.Company = await logic.GetCompanyForReport();
//
//                     var paymentErpReport = new PaymentERPReport(data);
//                     paymentErpReport.Landscape = false;
//                 
//                     var stream = new MemoryStream();
//                     await paymentErpReport.ExportToPdfAsync(stream);
//                     stream.Position = 0;
//                 
//                     res.Headers.Add("Content-Disposition", $"inline; filename=\"PagosERP{from.ToShortDateString()}ADia{to.ToShortDateString()}.pdf\"");
//                     return Results.File(stream, "application/pdf");
//                 }, PermissionAreas.Payments, PermissionTypes.Report))
//             .Produces(200, null, "application/pdf"),
//
//         app.MapGet("/credit_note/{id:int}/report", (int id, Context ctx, HttpResponse res) => ctx.ExecuteAuthenticated(
//             async (user, logic) =>
//             {
//                 var data = await logic.GetMemoReportData(id, ReceivableReasonTypes.CreditMemo);
//                 data.Company = await logic.GetCompanyForReport();
//
//                 var creditNoteReport = new MemoReport(data);
//                 creditNoteReport.Landscape = false;
//                 
//                 var stream = new MemoryStream();
//                 await creditNoteReport.ExportToPdfAsync(stream);
//                 stream.Position = 0;
//                 
//                 res.Headers.Add("Content-Disposition", $"inline; filename=\"NotaCredito{data.Number}.pdf\"");
//                 return Results.File(stream, "application/pdf");
//             }, PermissionAreas.CreditMemos, PermissionTypes.Report))
//             .Produces(200, null, "application/pdf"),   
//         
//         app.MapGet("/debit_note/{id:int}/report", (int id, Context ctx, HttpResponse res) => ctx.ExecuteAuthenticated(
//             async (user, logic) =>
//             {
//                 var data = await logic.GetMemoReportData(id, ReceivableReasonTypes.DebitMemo);
//                 data.Company = await logic.GetCompanyForReport();
//                 
//                 var debitNoteReport = new MemoReport(data);
//                 debitNoteReport.Landscape = false;
//                 
//                 var stream = new MemoryStream();
//                 await debitNoteReport.ExportToPdfAsync(stream);
//                 stream.Position = 0;
//                 
//                 res.Headers.Add("Content-Disposition", $"inline; filename=\"NotaDebito{data.Number}.pdf\"");
//                 return Results.File(stream, "application/pdf");
//             }, PermissionAreas.DebitMemos, PermissionTypes.Report))
//             .Produces(200, null, "application/pdf"),
//         });
//
//
//         
//     }
// }