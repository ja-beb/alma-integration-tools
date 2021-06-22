using AlmaIntegrationTools.Services;
using AlmaIntegrationTools.Settings;
using AlmaIntergrationTools.Finance.Models;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace AlmaIntegrationTools.Bursar.Services
{
    /// <summary>
    /// The Bursar sync service. 
    /// This service pulls file from SFTP server, converts to plain text and uploads to another SFTP server.
    /// </summary>
    public class SyncService : SyncService<PaymentData>, ISyncService<PaymentData>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="syncOptions"></param>
        public SyncService(IOptions<SyncSettings> syncOptions) : base(syncOptions)
        { }

        /// <summary>
        /// Ascync write function.
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="feed"></param>
        /// <returns></returns>
        public override void Write(DirectoryInfo directoryInfo, PaymentData feed)
        {
            foreach (Invoice invoice in feed.List)
            {
                WriteData(directoryInfo, invoice);
            }
        }

        /// <summary>
        /// Write fine fee data to an output file.
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="feed"></param>
        static public void WriteData(DirectoryInfo directoryInfo, Invoice invoice)
        {
            string filename = Path.Combine(directoryInfo.FullName, $"{invoice.Number}.txt");
            using StreamWriter streamWriter = new(filename);
            {
                streamWriter.WriteLine($"Procesing invoice #{invoice.Number}");
                streamWriter.WriteLine($"Unique identifier: {invoice.UniqueIdentifier}");
                streamWriter.WriteLine($"Owner____________: {invoice.Owner}");
                streamWriter.WriteLine($"Invoice date_____: {invoice.InvoiceDate}");
                streamWriter.WriteLine($"ApprovedBy_______: {invoice.ApprovedBy}");
                streamWriter.WriteLine($"Is prepaid_______? {invoice.IsPrepaid}");
                streamWriter.WriteLine($"Payment method___: {invoice.PaymentMethod}");
                streamWriter.WriteLine($"Attachment Count_: {invoice.AttachmentCount}");
                streamWriter.WriteLine($"");

                streamWriter.WriteLine("Owner Entity:");
                streamWriter.WriteLine($"\t Created By_____: {invoice.OwneredEntity.CreatedBy}");
                streamWriter.WriteLine($"\t Creation Date__: {invoice.OwneredEntity.CreationDate}");
                streamWriter.WriteLine($"\t Customer Id____: {invoice.OwneredEntity.CustomerId}");
                streamWriter.WriteLine($"\t Institution Id_: {invoice.OwneredEntity.InstitutionId}");
                streamWriter.WriteLine($"\t Library Unit Id: {invoice.OwneredEntity.LibraryUnitId}");
                streamWriter.WriteLine($"\t Modifiecat date: {invoice.OwneredEntity.ModificationDate}");
                streamWriter.WriteLine($"\t Modified by____: {invoice.OwneredEntity.ModifiedBy}");
                streamWriter.WriteLine("");

                streamWriter.WriteLine("Amount");
                streamWriter.WriteLine($"\t Rate: {invoice.Amount.ExplicitRate}");
                streamWriter.WriteLine($"\t Sum_: {invoice.Amount.Sum:0:0.00} {invoice.Amount.Currency}");
                streamWriter.WriteLine("");

                streamWriter.WriteLine("Vendor");
                streamWriter.WriteLine($"\t Code_________________________: {invoice.VendorCode}");
                streamWriter.WriteLine($"\t Code Additional______________: {invoice.VendorCodeAdditional}");
                streamWriter.WriteLine($"\t Name_________________________: {invoice.VendorName}");
                streamWriter.WriteLine($"\t Financial System Code________: {invoice.VendorFinancialSystemCode}");
                streamWriter.WriteLine($"\t National Tax Id______________: {invoice.VendorNationalTaxId}");
                streamWriter.WriteLine($"\t Account Code_________________: {invoice.VendorAccountCode}");
                streamWriter.WriteLine($"\t Account Financial System Code: {invoice.VendorAccountFinancialSystemCode}");
                streamWriter.WriteLine($"\t Liable For Vat_______________: {invoice.VendorLiableForVat}");
                streamWriter.WriteLine("");

                streamWriter.WriteLine($"Vat Info");
                streamWriter.WriteLine($"\t Code_______: {invoice.VatInfo.Code}");
                streamWriter.WriteLine($"\t Type_______: {invoice.VatInfo.Type}");
                streamWriter.WriteLine($"\t Description: {invoice.VatInfo.Description}");
                streamWriter.WriteLine($"\t Percentage_: {invoice.VatInfo.Percentage}%");
                streamWriter.WriteLine($"\t Tax________: {invoice.VatInfo.Tax:0.00}");
                streamWriter.WriteLine($"\t Amount_____: {invoice.VatInfo.Amount:0.00}");
                streamWriter.WriteLine($"\t Is included in amount? {invoice.VatInfo.IsIncludedInAmount}");
                streamWriter.WriteLine($"\t Is expended from fund? {invoice.VatInfo.IsExpendedFromFund}");
                streamWriter.WriteLine($"\t Is line level? {invoice.VatInfo.IsLineLevel}");
                streamWriter.WriteLine("");

                streamWriter.WriteLine($"Payment Addresses");
                foreach (PaymentAddress address in invoice.PaymentAddresses)
                {
                    streamWriter.WriteLine("\t {address.Line1}");
                    if (!string.IsNullOrEmpty(address.Line2)) streamWriter.WriteLine($"\t {address.Line2}");
                    if (!string.IsNullOrEmpty(address.Line3)) streamWriter.WriteLine($"\t {address.Line3}");
                    if (!string.IsNullOrEmpty(address.Line4)) streamWriter.WriteLine($"\t {address.Line4}");
                    if (!string.IsNullOrEmpty(address.Line5)) streamWriter.WriteLine($"\t {address.Line5}");
                    streamWriter.WriteLine($"\t {address.City}, {address.StateProvince} {address.PostalCode}");
                    if (!string.IsNullOrEmpty(address.Country)) streamWriter.WriteLine($"\t {address.Country}");
                    streamWriter.WriteLine($"\t Is preferred? {address.IsPreferred}");
                    streamWriter.WriteLine($"\t Types: {String.Join(", ", address.Types)}");
                    streamWriter.WriteLine("");
                }

                streamWriter.WriteLine("Notes");
                foreach (Note note in invoice.Notes)
                {
                    streamWriter.WriteLine($"\t Customer Id____: {note.OwneredEntity.CustomerId}");
                    streamWriter.WriteLine($"\t Created________: {note.OwneredEntity.CreatedBy} on {note.OwneredEntity.CreationDate}");
                    streamWriter.WriteLine($"\t Modified_______: {note.OwneredEntity.ModifiedBy} on {note.OwneredEntity.ModificationDate}");
                    streamWriter.WriteLine($"\t Institution Id_: {note.OwneredEntity.InstitutionId}");
                    streamWriter.WriteLine($"\t Library Id_____: {note.OwneredEntity.LibraryId}");
                    streamWriter.WriteLine($"\t Library Unit Id:{note.OwneredEntity.LibraryUnitId}");
                    streamWriter.WriteLine($"\t Content________: {note.Content}");
                    streamWriter.WriteLine("");
                }
                streamWriter.WriteLine("");

                streamWriter.WriteLine("Invoice Lines");
                foreach (InvoiceLine line in invoice.Lines)
                {
                    streamWriter.WriteLine($"\t Number____: {line.Number}");
                    streamWriter.WriteLine($"\t Type: {line.Type}");
                    if (!string.IsNullOrEmpty(line.AdditionalInformation)) streamWriter.WriteLine($"\t Additional Information: {line.AdditionalInformation}");
                    streamWriter.WriteLine($"\t Note: {line.Note}");
                    streamWriter.WriteLine($"\t PO Line Info: {line.PoLineInfo}");
                    streamWriter.WriteLine($"\t Price Note: {line.PriceNote}");
                    streamWriter.WriteLine($"\t Reporting Code: {line.ReportingCode}");
                    streamWriter.WriteLine($"\t Secondary Reporting Code: {line.SecondaryReportingCode}");
                    streamWriter.WriteLine($"\t Subscription Date Range: {line.SubscriptionDateRange}");
                    streamWriter.WriteLine($"\t Tertiary Reporting Code: {line.TertiaryReportingCode}");
                    streamWriter.WriteLine($"\t Price: {line.Price:0.00}");
                    streamWriter.WriteLine($"\t Quantity: {line.Quantity}");
                    streamWriter.WriteLine($"\t Total Price: {line.TotalPrice:0.00}");

                    streamWriter.WriteLine("\t VAT ==================================");
                    streamWriter.WriteLine($"\t \t Amount: {line.VatAmount:0.00}");
                    streamWriter.WriteLine($"\t \t Code: {line.VatCode}");
                    streamWriter.WriteLine($"\t \t Description: {line.VatDescription}");
                    streamWriter.WriteLine($"\t \t Percentage: {line.VatPercentage}");
                    streamWriter.WriteLine($"\t \t Tax: {line.VendorTax:0.00}");

                    foreach (FundInfo fundInfo in line.FundInfoList)
                    {
                        streamWriter.WriteLine($"Type: {fundInfo.Type}");
                        streamWriter.WriteLine($"Name: {fundInfo.Name}");
                        streamWriter.WriteLine($"Code: {fundInfo.Code}");
                        streamWriter.WriteLine($"Id: {fundInfo.ExternalId}");
                        streamWriter.WriteLine($"Fiscal Period: {fundInfo.FiscalPeriod}");
                        streamWriter.WriteLine($"Type: {fundInfo.FundType} - {fundInfo.FundTypeDescription}");
                        streamWriter.WriteLine($"Leger: {fundInfo.LedgerName} - {fundInfo.LedgerCode}");
                        streamWriter.WriteLine($"Local amount: {fundInfo.LocalAmount.Sum:0.00} ({fundInfo.LocalAmount.Currency})");
                        streamWriter.WriteLine($"Local amount: {fundInfo.Amount.Sum:0.00} ({fundInfo.Amount.Currency})");


                    }
                    streamWriter.WriteLine($"");
                }

                if (null != invoice.AdditionalCharges)
                {
                    streamWriter.WriteLine("Additional Charges");
                    streamWriter.WriteLine($"\t Discount_______________________________: {invoice.AdditionalCharges.DiscountAmount:0.00}");
                    streamWriter.WriteLine($"\t Audio CD Packaging_____________________: {invoice.AdditionalCharges.AudioCdromPackagingAmount:0.00}");
                    streamWriter.WriteLine($"\t Barcode Labelling______________________: {invoice.AdditionalCharges.BarcodeLabellingAmount:0.00}");
                    streamWriter.WriteLine($"\t Binding________________________________: {invoice.AdditionalCharges.BindingAmount:0.00}");
                    streamWriter.WriteLine($"\t Cataloguing Services___________________: {invoice.AdditionalCharges.CataloguingServicesAmount:0.00}");
                    streamWriter.WriteLine($"\t Classifications________________________: {invoice.AdditionalCharges.ClassificationAmount:0.00}");
                    streamWriter.WriteLine($"\t Commission_____________________________: {invoice.AdditionalCharges.CommissionAmount:0.00}");
                    streamWriter.WriteLine($"\t Exchage Rage Gaurantee Charge__________: {invoice.AdditionalCharges.ExchangeRateGuaranteeChargeAmount:0.00}");
                    streamWriter.WriteLine($"\t Data Communication_____________________: {invoice.AdditionalCharges.DataCommunicationAmount:0.00}");
                    streamWriter.WriteLine($"\t Delivery_______________________________: {invoice.AdditionalCharges.DeliveryAmount:0.00}");
                    streamWriter.WriteLine($"\t General Servicing By Library Bookseller: {invoice.AdditionalCharges.GeneralServicingByLibraryBooksellerAmount:0.00}");
                    streamWriter.WriteLine($"\t Handling Charge________________________: {invoice.AdditionalCharges.HandlingChargeAmount:0.00}");
                    streamWriter.WriteLine($"\t Insurance______________________________: {invoice.AdditionalCharges.InsuranceAmount:0.00}");
                    streamWriter.WriteLine($"\t Insurance Charge_______________________: {invoice.AdditionalCharges.InsuranceChargeAmount:0.00}");
                    streamWriter.WriteLine($"\t Miscellanous Charge____________________: {invoice.AdditionalCharges.MiscellaneousChargeAmount:0.00}");
                    streamWriter.WriteLine($"\t Miscellanous Credit Adjustment_________: {invoice.AdditionalCharges.MiscellaneousCreditAdjustmentAmount:0.00}");
                    streamWriter.WriteLine($"\t Miscellaneous Servicing________________: {invoice.AdditionalCharges.MiscellaneousServicingAmount:0.00}");
                    streamWriter.WriteLine($"\t Overhead_______________________________: {invoice.AdditionalCharges.OverheadAmount:0.00}");
                    streamWriter.WriteLine($"\t Packing Charge_________________________: {invoice.AdditionalCharges.PackingChargeAmount:0.00}");
                    streamWriter.WriteLine($"\t Postage and packing charge_____________: {invoice.AdditionalCharges.PostageAndPackingChargeAmount:0.00}");
                    streamWriter.WriteLine($"\t Posting charge_________________________: {invoice.AdditionalCharges.PostingChargeAmount:0.00}");
                    streamWriter.WriteLine($"\t Security Fitting_______________________: {invoice.AdditionalCharges.SecurityFittingAmount:0.00}");
                    streamWriter.WriteLine($"\t Shipment_______________________________: {invoice.AdditionalCharges.ShipmentAmount:0.00}");
                    streamWriter.WriteLine($"\t Sleeving_______________________________: {invoice.AdditionalCharges.SleevingAmount:0.00}");
                    streamWriter.WriteLine($"\t Small order surcharge__________________: {invoice.AdditionalCharges.SmallOrderSurchargeAmount:0.00}");
                    streamWriter.WriteLine($"\t Special handling_______________________: {invoice.AdditionalCharges.SpecialHandlingAmount:0.00}");
                    streamWriter.WriteLine($"\t Supply of aprovals book collections____: {invoice.AdditionalCharges.SupplyOfApprovalsbookcollectionsAmount:0.00}");
                    streamWriter.WriteLine($"\t Total Charges__________________________: {invoice.AdditionalCharges.TotalChargesAmount:0.00}");
                }

                streamWriter.WriteLine("Exchange Rates");
                foreach (ExchangeRate exchangeRate in invoice.ExchangeRates)
                {
                    streamWriter.WriteLine($"\t Currency___: {exchangeRate.Currency}");
                    streamWriter.WriteLine($"\t Rate_______: {exchangeRate.Rate}");
                    streamWriter.WriteLine($"\t Is explicit? {exchangeRate.IsExplicit}");
                }

                streamWriter.WriteLine("");
            }
        }

    }
}




