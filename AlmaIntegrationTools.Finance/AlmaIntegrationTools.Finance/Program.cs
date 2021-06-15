using AlmaIntegrationTools.Config;
using AlmaIntergrationTools.Finance.Models;
using System;
using System.IO;

namespace AlmaIntergrationTools.Finance
{
    class Program : XmlSyncProgram<PaymentData>
    {

        static void Main()
        {
            SyncProgram<PaymentData> syncProgram = new Program()
            {
                Config = ServersSectionGroup.Instance()
            };
            syncProgram.Run("xml");
        }

        /// <summary>
        /// Process feed.
        /// </summary>
        /// <param name="feed"></param>
        public override void Process(PaymentData data)
        {
            foreach (Invoice invoice in data.List)
            {
                string filename = Path.Combine(ExportDirectory.FullName, String.Format("{0}.txt", invoice.Number));
                using StreamWriter streamWriter = new(filename);
                {
                    streamWriter.WriteLine(String.Format("Procesing invoice #{0}", invoice.Number));
                    streamWriter.WriteLine(String.Format("Unique identifier: {0}", invoice.UniqueIdentifier));
                    streamWriter.WriteLine(String.Format("Owner____________: {0}", invoice.Owner));
                    streamWriter.WriteLine(String.Format("Invoice date_____: {0}", invoice.InvoiceDate));
                    streamWriter.WriteLine(String.Format("ApprovedBy_______: {0}", invoice.ApprovedBy));
                    streamWriter.WriteLine(String.Format("Is prepaid_______? {0}", invoice.IsPrepaid));
                    streamWriter.WriteLine(String.Format("Payment method___: {0}", invoice.PaymentMethod));
                    streamWriter.WriteLine(String.Format("Attachment Count_: {0}", invoice.AttachmentCount));
                    streamWriter.WriteLine(String.Format(""));

                    streamWriter.WriteLine(String.Format("Owner Entity:"));
                    streamWriter.WriteLine(String.Format("\t Created By_____: {0}", invoice.OwneredEntity.CreatedBy));
                    streamWriter.WriteLine(String.Format("\t Creation Date__: {0}", invoice.OwneredEntity.CreationDate));
                    streamWriter.WriteLine(String.Format("\t Customer Id____: {0}", invoice.OwneredEntity.CustomerId));
                    streamWriter.WriteLine(String.Format("\t Institution Id_: {0}", invoice.OwneredEntity.InstitutionId));
                    streamWriter.WriteLine(String.Format("\t Library Unit Id: {0}", invoice.OwneredEntity.LibraryUnitId));
                    streamWriter.WriteLine(String.Format("\t Modifiecat date: {0}", invoice.OwneredEntity.ModificationDate));
                    streamWriter.WriteLine(String.Format("\t Modified by____: {0}", invoice.OwneredEntity.ModifiedBy));
                    streamWriter.WriteLine(String.Format(""));

                    streamWriter.WriteLine(String.Format("Amount"));
                    streamWriter.WriteLine(String.Format("\t Rate: {0}", invoice.Amount.ExplicitRate));
                    streamWriter.WriteLine(String.Format("\t Sum_: {0:0:0.00} {1}", invoice.Amount.Sum, invoice.Amount.Currency));
                    streamWriter.WriteLine(String.Format(""));

                    streamWriter.WriteLine(String.Format("Vendor"));
                    streamWriter.WriteLine(String.Format("\t Code_________________________: {0}", invoice.VendorCode));
                    streamWriter.WriteLine(String.Format("\t Code Additional______________: {0}", invoice.VendorCodeAdditional));
                    streamWriter.WriteLine(String.Format("\t Name_________________________: {0}", invoice.VendorName));
                    streamWriter.WriteLine(String.Format("\t Financial System Code________: {0}", invoice.VendorFinancialSystemCode));
                    streamWriter.WriteLine(String.Format("\t National Tax Id______________: {0}", invoice.VendorNationalTaxId));
                    streamWriter.WriteLine(String.Format("\t Account Code_________________: {0}", invoice.VendorAccountCode));
                    streamWriter.WriteLine(String.Format("\t Account Financial System Code: {0}", invoice.VendorAccountFinancialSystemCode));
                    streamWriter.WriteLine(String.Format("\t Liable For Vat_______________: {0}", invoice.VendorLiableForVat));
                    streamWriter.WriteLine(String.Format(""));

                    streamWriter.WriteLine(String.Format("Vat Info"));
                    streamWriter.WriteLine(String.Format("\t Code_______: {0}", invoice.VatInfo.Code));
                    streamWriter.WriteLine(String.Format("\t Type_______: {0}", invoice.VatInfo.Type));
                    streamWriter.WriteLine(String.Format("\t Description: {0}", invoice.VatInfo.Description));
                    streamWriter.WriteLine(String.Format("\t Percentage_: {0}%", invoice.VatInfo.Percentage));
                    streamWriter.WriteLine(String.Format("\t Tax________: {0:0.00}", invoice.VatInfo.Tax));
                    streamWriter.WriteLine(String.Format("\t Amount_____: {0:0.00}", invoice.VatInfo.Amount));
                    streamWriter.WriteLine(String.Format("\t Is included in amount? {0}", invoice.VatInfo.IsIncludedInAmount));
                    streamWriter.WriteLine(String.Format("\t Is expended from fund? {0}", invoice.VatInfo.IsExpendedFromFund));
                    streamWriter.WriteLine(String.Format("\t Is line level? {0}", invoice.VatInfo.IsLineLevel));
                    streamWriter.WriteLine(String.Format(""));

                    streamWriter.WriteLine(String.Format("Payment Addresses"));
                    foreach (PaymentAddress address in invoice.PaymentAddresses)
                    {
                        streamWriter.WriteLine(String.Format("\t {0}", address.Line1));
                        if (!string.IsNullOrEmpty(address.Line2)) streamWriter.WriteLine(String.Format("\t {0}", address.Line2));
                        if (!string.IsNullOrEmpty(address.Line3)) streamWriter.WriteLine(String.Format("\t {0}", address.Line3));
                        if (!string.IsNullOrEmpty(address.Line4)) streamWriter.WriteLine(String.Format("\t {0}", address.Line4));
                        if (!string.IsNullOrEmpty(address.Line5)) streamWriter.WriteLine(String.Format("\t {0}", address.Line5));
                        streamWriter.WriteLine(String.Format("\t {0}, {1} {2}", address.City, address.StateProvince, address.PostalCode));
                        if (!string.IsNullOrEmpty(address.Country)) streamWriter.WriteLine(String.Format("\t {0}", address.Country));
                        streamWriter.WriteLine(String.Format("\t Is preferred? {0}", address.IsPreferred));
                        streamWriter.WriteLine(String.Format("\t Types: {0}", String.Join(", ", address.Types)));
                        streamWriter.WriteLine(String.Format(""));
                    }

                    streamWriter.WriteLine(String.Format("Notes"));
                    foreach (Note note in invoice.Notes)
                    {
                        streamWriter.WriteLine(String.Format("\t Customer Id____: {0}", note.OwneredEntity.CustomerId));
                        streamWriter.WriteLine(String.Format("\t Created________: {0} on {1}", note.OwneredEntity.CreatedBy, note.OwneredEntity.CreationDate));
                        streamWriter.WriteLine(String.Format("\t Modified_______: {0} on {1}", note.OwneredEntity.ModifiedBy, note.OwneredEntity.ModificationDate));
                        streamWriter.WriteLine(String.Format("\t Institution Id_: {0}", note.OwneredEntity.InstitutionId));
                        streamWriter.WriteLine(String.Format("\t Library Id_____: {0}", note.OwneredEntity.LibraryId));
                        streamWriter.WriteLine(String.Format("\t Library Unit Id:{0}", note.OwneredEntity.LibraryUnitId));
                        streamWriter.WriteLine(String.Format("\t Content________: {0}", note.Content));
                        streamWriter.WriteLine(String.Format(""));
                    }
                    streamWriter.WriteLine(String.Format(""));

                    streamWriter.WriteLine(String.Format("Invoice Lines"));
                    foreach (InvoiceLine line in invoice.Lines)
                    {
                        streamWriter.WriteLine(String.Format("\t Number____: {0}", line.Number));
                        streamWriter.WriteLine(String.Format("\t Type: {0}", line.Type));
                        if (!string.IsNullOrEmpty(line.AdditionalInformation)) streamWriter.WriteLine(String.Format("\t Additional Information: {0}", line.AdditionalInformation));
                        streamWriter.WriteLine(String.Format("\t Note: {0}", line.Note));
                        streamWriter.WriteLine(String.Format("\t PO Line Info: {0}", line.PoLineInfo));
                        streamWriter.WriteLine(String.Format("\t Price Note: {0}", line.PriceNote));
                        streamWriter.WriteLine(String.Format("\t Reporting Code: {0}", line.ReportingCode));
                        streamWriter.WriteLine(String.Format("\t Secondary Reporting Code: {0}", line.SecondaryReportingCode));
                        streamWriter.WriteLine(String.Format("\t Subscription Date Range: {0}", line.SubscriptionDateRange));
                        streamWriter.WriteLine(String.Format("\t Tertiary Reporting Code: {0}", line.TertiaryReportingCode));
                        streamWriter.WriteLine(String.Format("\t Price: {0:0.00}", line.Price));
                        streamWriter.WriteLine(String.Format("\t Quantity: {0}", line.Quantity));
                        streamWriter.WriteLine(String.Format("\t Total Price: {0:0.00}", line.TotalPrice));

                        streamWriter.WriteLine(String.Format("\t VAT =================================="));
                        streamWriter.WriteLine(String.Format("\t \t Amount: {0:0.00}", line.VatAmount));
                        streamWriter.WriteLine(String.Format("\t \t Code: {0}", line.VatCode));
                        streamWriter.WriteLine(String.Format("\t \t Description: {0}", line.VatDescription));
                        streamWriter.WriteLine(String.Format("\t \t Percentage: {0}", line.VatPercentage));
                        streamWriter.WriteLine(String.Format("\t \t Tax: {0:0.00}", line.VendorTax));

                        foreach (FundInfo fundInfo in line.FundInfoList)
                        {
                            streamWriter.WriteLine($"Type: {fundInfo.Type}");
                            streamWriter.WriteLine($"Name: {fundInfo.Name}");
                            streamWriter.WriteLine($"Code: {fundInfo.Code}");
                            streamWriter.WriteLine($"Id: {fundInfo.ExternalId}");
                            streamWriter.WriteLine($"Fiscal Period: {fundInfo.FiscalPeriod}");
                            streamWriter.WriteLine($"Type: {fundInfo.FundType} - {fundInfo.FundTypeDescription}");
                            streamWriter.WriteLine($"Leger: {fundInfo.LedgerName} - {fundInfo.LedgerCode}");
                            streamWriter.WriteLine(String.Format("Local amount: {0:0.00} ({1})", fundInfo.LocalAmount.Sum, fundInfo.LocalAmount.Currency));
                            streamWriter.WriteLine(String.Format("Local amount: {0:0.00} ({1})", fundInfo.Amount.Sum, fundInfo.Amount.Currency));


                        }
                        streamWriter.WriteLine(String.Format(""));
                    }

                    if (null != invoice.AdditionalCharges)
                    {
                        streamWriter.WriteLine(String.Format("Additional Charges"));
                        streamWriter.WriteLine(String.Format("\t Discount_______________________________: {0:0.00}", invoice.AdditionalCharges.DiscountAmount));
                        streamWriter.WriteLine(String.Format("\t Audio CD Packaging_____________________: {0:0.00}", invoice.AdditionalCharges.AudioCdromPackagingAmount));
                        streamWriter.WriteLine(String.Format("\t Barcode Labelling______________________: {0:0.00}", invoice.AdditionalCharges.BarcodeLabellingAmount));
                        streamWriter.WriteLine(String.Format("\t Binding________________________________: {0:0.00}", invoice.AdditionalCharges.BindingAmount));
                        streamWriter.WriteLine(String.Format("\t Cataloguing Services___________________: {0:0.00}", invoice.AdditionalCharges.CataloguingServicesAmount));
                        streamWriter.WriteLine(String.Format("\t Classifications________________________: {0:0.00}", invoice.AdditionalCharges.ClassificationAmount));
                        streamWriter.WriteLine(String.Format("\t Commission_____________________________: {0:0.00}", invoice.AdditionalCharges.CommissionAmount));
                        streamWriter.WriteLine(String.Format("\t Exchage Rage Gaurantee Charge__________: {0:0.00}", invoice.AdditionalCharges.ExchangeRateGuaranteeChargeAmount));
                        streamWriter.WriteLine(String.Format("\t Data Communication_____________________: {0:0.00}", invoice.AdditionalCharges.DataCommunicationAmount));
                        streamWriter.WriteLine(String.Format("\t Delivery_______________________________: {0:0.00}", invoice.AdditionalCharges.DeliveryAmount));
                        streamWriter.WriteLine(String.Format("\t General Servicing By Library Bookseller: {0:0.00}", invoice.AdditionalCharges.GeneralServicingByLibraryBooksellerAmount));
                        streamWriter.WriteLine(String.Format("\t Handling Charge________________________: {0:0.00}", invoice.AdditionalCharges.HandlingChargeAmount));
                        streamWriter.WriteLine(String.Format("\t Insurance______________________________: {0:0.00}", invoice.AdditionalCharges.InsuranceAmount));
                        streamWriter.WriteLine(String.Format("\t Insurance Charge_______________________: {0:0.00}", invoice.AdditionalCharges.InsuranceChargeAmount));
                        streamWriter.WriteLine(String.Format("\t Miscellanous Charge____________________: {0:0.00}", invoice.AdditionalCharges.MiscellaneousChargeAmount));
                        streamWriter.WriteLine(String.Format("\t Miscellanous Credit Adjustment_________: {0:0.00}", invoice.AdditionalCharges.MiscellaneousCreditAdjustmentAmount));
                        streamWriter.WriteLine(String.Format("\t Miscellaneous Servicing________________: {0:0.00}", invoice.AdditionalCharges.MiscellaneousServicingAmount));
                        streamWriter.WriteLine(String.Format("\t Overhead_______________________________: {0:0.00}", invoice.AdditionalCharges.OverheadAmount));
                        streamWriter.WriteLine(String.Format("\t Packing Charge_________________________: {0:0.00}", invoice.AdditionalCharges.PackingChargeAmount));
                        streamWriter.WriteLine(String.Format("\t Postage and packing charge_____________: {0:0.00}", invoice.AdditionalCharges.PostageAndPackingChargeAmount));
                        streamWriter.WriteLine(String.Format("\t Posting charge_________________________: {0:0.00}", invoice.AdditionalCharges.PostingChargeAmount));
                        streamWriter.WriteLine(String.Format("\t Security Fitting_______________________: {0:0.00}", invoice.AdditionalCharges.SecurityFittingAmount));
                        streamWriter.WriteLine(String.Format("\t Shipment_______________________________: {0:0.00}", invoice.AdditionalCharges.ShipmentAmount));
                        streamWriter.WriteLine(String.Format("\t Sleeving_______________________________: {0:0.00}", invoice.AdditionalCharges.SleevingAmount));
                        streamWriter.WriteLine(String.Format("\t Small order surcharge__________________: {0:0.00}", invoice.AdditionalCharges.SmallOrderSurchargeAmount));
                        streamWriter.WriteLine(String.Format("\t Special handling_______________________: {0:0.00}", invoice.AdditionalCharges.SpecialHandlingAmount));
                        streamWriter.WriteLine(String.Format("\t Supply of aprovals book collections____: {0:0.00}", invoice.AdditionalCharges.SupplyOfApprovalsbookcollectionsAmount));
                        streamWriter.WriteLine(String.Format("\t Total Charges__________________________: {0:0.00}", invoice.AdditionalCharges.TotalChargesAmount));
                    }

                    streamWriter.WriteLine(String.Format("Exchange Rates"));
                    foreach (ExchangeRate exchangeRate in invoice.ExchangeRates)
                    {
                        streamWriter.WriteLine(String.Format("\t Currency___: {0}", exchangeRate.Currency));
                        streamWriter.WriteLine(String.Format("\t Rate_______: {0}", exchangeRate.Rate));
                        streamWriter.WriteLine(String.Format("\t Is explicit? {0}", exchangeRate.IsExplicit));
                    }

                    streamWriter.WriteLine(String.Format(""));
                }
            }
        }


    }
}
