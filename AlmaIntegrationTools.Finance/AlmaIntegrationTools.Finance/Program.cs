using AlmaIntegrationTools;
using AlmaIntegrationTools.Config;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Xml.Serialization;
using WinSCP;

namespace AlmaIntergrationTools.Finance.Program
{
    class Program
    {

        static void Main()
        {
            try
            {
                ServersSectionGroup serversSectionGroup = ServersSectionGroup.Instance();
                DirectoryInfo baseDirectory = new(Path.Combine(
                    serversSectionGroup.Path.Value,
                    Guid.NewGuid().ToString("N")
                ));
                baseDirectory.Create();

                // import.
                FileInfo[] files = Fetch(new SessionOptions()
                {
                    Protocol = Protocol.Sftp,
                    HostName = serversSectionGroup.ImportServer.Host,
                    PortNumber = serversSectionGroup.ImportServer.Port,
                    UserName = serversSectionGroup.ImportServer.User,
                    SshHostKeyFingerprint = serversSectionGroup.ImportServer.Fingerprint,
                    SshPrivateKeyPath = serversSectionGroup.ImportServer.Key,
                },
                    serversSectionGroup.ImportServer.Path,
                    baseDirectory
                 );

                // convert.
                XmlSerializer xmlSerializer = new(typeof(PaymentData));

                using (StreamWriter outputFile = new(Path.Combine(baseDirectory.FullName, "results.txt")))
                {
                    foreach (FileInfo file in files)
                    {
                        // Process import file.
                        using FileStream fileStream = file.OpenRead();
                        {
                            PaymentData data = xmlSerializer.Deserialize(fileStream) as PaymentData;
                            foreach (Invoice invoice in data.List)
                            {
                                outputFile.WriteLine(String.Format("Procesing invoice #{0}", invoice.Number));

                                outputFile.WriteLine(String.Format("Owner____________: {0}", invoice.Owner));
                                outputFile.WriteLine(String.Format("ApprovedBy_______: {0}", invoice.ApprovedBy));
                                outputFile.WriteLine(String.Format("Invoice date_____: {0}", invoice.InvoiceDate));
                                outputFile.WriteLine(String.Format("Payment method___: {0}", invoice.PaymentMethod));
                                outputFile.WriteLine(String.Format("Is prepaid_______? {0}", invoice.IsPrepaid));
                                outputFile.WriteLine(String.Format("Attachment Count_: {0}", invoice.AttachmentCount));
                                outputFile.WriteLine(String.Format("Unique identifier: {0}", invoice.UniqueIdentifier));
                                outputFile.WriteLine(String.Format(""));

                                outputFile.WriteLine(String.Format("Owner Entity:"));
                                outputFile.WriteLine(String.Format("\t Created By_____: {0}", invoice.OwneredEntity.CreatedBy));
                                outputFile.WriteLine(String.Format("\t Creation Date__: {0}", invoice.OwneredEntity.CreationDate));
                                outputFile.WriteLine(String.Format("\t Customer Id____: {0}", invoice.OwneredEntity.CustomerId));
                                outputFile.WriteLine(String.Format("\t Institution Id_: {0}", invoice.OwneredEntity.InstitutionId));
                                outputFile.WriteLine(String.Format("\t Library Unit Id: {0}", invoice.OwneredEntity.LibraryUnitId));
                                outputFile.WriteLine(String.Format("\t Modifiecat date: {0}", invoice.OwneredEntity.ModificationDate));
                                outputFile.WriteLine(String.Format("\t Modified by____: {0}", invoice.OwneredEntity.ModifiedBy));
                                outputFile.WriteLine(String.Format(""));

                                outputFile.WriteLine(String.Format("Amount"));
                                outputFile.WriteLine(String.Format("\t Currency_____: {0}", invoice.Amount.Currency));
                                outputFile.WriteLine(String.Format("\t Explicit Rate: {0}", invoice.Amount.ExplicitRate));
                                outputFile.WriteLine(String.Format("\t Sum__________: {0}", invoice.Amount.Sum));
                                outputFile.WriteLine(String.Format(""));

                                outputFile.WriteLine(String.Format("Vendor"));
                                outputFile.WriteLine(String.Format("\t Code_________________________: {0}", invoice.VendorCode));
                                outputFile.WriteLine(String.Format("\t Code Additional______________: {0}", invoice.VendorCodeAdditional));
                                outputFile.WriteLine(String.Format("\t Name_________________________: {0}", invoice.VendorName));
                                outputFile.WriteLine(String.Format("\t Financial System Code________: {0}", invoice.VendorFinancialSystemCode));
                                outputFile.WriteLine(String.Format("\t National Tax Id______________: {0}", invoice.VendorNationalTaxId));
                                outputFile.WriteLine(String.Format("\t Account Code_________________: {0}", invoice.VendorAccountCode));
                                outputFile.WriteLine(String.Format("\t Account Financial System Code: {0}", invoice.VendorAccountFinancialSystemCode));
                                outputFile.WriteLine(String.Format("\t Liable For Vat_______________: {0}", invoice.VendorLiableForVat));
                                outputFile.WriteLine(String.Format(""));

                                outputFile.WriteLine(String.Format("Vat Info"));
                                outputFile.WriteLine(String.Format("\t Code_______: {0}", invoice.VatInfo.Code));
                                outputFile.WriteLine(String.Format("\t Type_______: {0}", invoice.VatInfo.Type));
                                outputFile.WriteLine(String.Format("\t Description: {0}", invoice.VatInfo.Description));
                                outputFile.WriteLine(String.Format("\t Percentage_: {0}%", invoice.VatInfo.Percentage));
                                outputFile.WriteLine(String.Format("\t Tax________: {0}", invoice.VatInfo.Tax));
                                outputFile.WriteLine(String.Format("\t Amount_____: {0}", invoice.VatInfo.Amount));
                                outputFile.WriteLine(String.Format("\t Is included in amount? {0}", invoice.VatInfo.IsIncludedInAmount));
                                outputFile.WriteLine(String.Format("\t Is expended from fund? {0}", invoice.VatInfo.IsExpendedFromFund));
                                outputFile.WriteLine(String.Format("\t Is line level? {0}", invoice.VatInfo.IsLineLevel));
                                outputFile.WriteLine(String.Format(""));

                                outputFile.WriteLine(String.Format("Payment Addresses"));
                                foreach (PaymentAddress address in invoice.PaymentAddresses)
                                {
                                    outputFile.WriteLine(String.Format("\t {0}", address.Line1));
                                    if (!string.IsNullOrEmpty(address.Line2)) outputFile.WriteLine(String.Format("\t {0}", address.Line2));
                                    if (!string.IsNullOrEmpty(address.Line3)) outputFile.WriteLine(String.Format("\t {0}", address.Line3));
                                    if (!string.IsNullOrEmpty(address.Line4)) outputFile.WriteLine(String.Format("\t {0}", address.Line4));
                                    if (!string.IsNullOrEmpty(address.Line5)) outputFile.WriteLine(String.Format("\t {0}", address.Line5));
                                    outputFile.WriteLine(String.Format("\t {0}, {1} {2}", address.City, address.StateProvince, address.PostalCode));
                                    outputFile.WriteLine(String.Format("\t {0}", address.Country));
                                    outputFile.WriteLine(String.Format("\t Is preferred{0}", address.IsPreferred));
                                    outputFile.WriteLine(String.Format("\t Types:"));
                                    foreach (string type in address.Types)
                                    {
                                        outputFile.WriteLine(String.Format("\t \t {0}", type));
                                    }
                                    outputFile.WriteLine(String.Format(""));
                                }

                                outputFile.WriteLine(String.Format("Notes"));
                                foreach (Note note in invoice.Notes)
                                {
                                    outputFile.WriteLine(String.Format("\t Customer Id____: {0}", note.OwneredEntity.CustomerId));
                                    outputFile.WriteLine(String.Format("\t Created________: {0} on {1}", note.OwneredEntity.CreatedBy, note.OwneredEntity.CreationDate));
                                    outputFile.WriteLine(String.Format("\t Modified_______: {0} on {1}", note.OwneredEntity.ModifiedBy, note.OwneredEntity.ModificationDate));
                                    outputFile.WriteLine(String.Format("\t Institution Id_: {0}", note.OwneredEntity.InstitutionId));
                                    outputFile.WriteLine(String.Format("\t Library Id_____: {0}", note.OwneredEntity.LibraryId));
                                    outputFile.WriteLine(String.Format("\t Library Unit Id:{0}", note.OwneredEntity.LibraryUnitId));
                                    outputFile.WriteLine(String.Format("\t Content________: {0}", note.Content));
                                    outputFile.WriteLine(String.Format(""));
                                }

                                outputFile.WriteLine(String.Format("Invoice Lines"));
                                foreach (InvoiceLine line in invoice.Lines)
                                {
                                    outputFile.WriteLine(String.Format("\t Number____: {0}", line.Number));
                                    outputFile.WriteLine(String.Format("\t Additional Information: {0}", line.AdditionalInformation));
                                    outputFile.WriteLine(String.Format("\t Fund Info List: {0}", line.FundInfoList));
                                    outputFile.WriteLine(String.Format("\t Note: {0}", line.Note));
                                    outputFile.WriteLine(String.Format("\t PO Line Info: {0}", line.PoLineInfo));
                                    outputFile.WriteLine(String.Format("\t Price: {0}", line.Price));
                                    outputFile.WriteLine(String.Format("\t Price Note: {0}", line.PriceNote));
                                    outputFile.WriteLine(String.Format("\t Quantity: {0}", line.Quantity));
                                    outputFile.WriteLine(String.Format("\t Reporting Code: {0}", line.ReportingCode));
                                    outputFile.WriteLine(String.Format("\t Secondary Reporting Code: {0}", line.SecondaryReportingCode));
                                    outputFile.WriteLine(String.Format("\t Subscription Date Range: {0}", line.SubscriptionDateRange));
                                    outputFile.WriteLine(String.Format("\t Tertiary Reporting Code: {0}", line.TertiaryReportingCode));
                                    outputFile.WriteLine(String.Format("\t Total Price: {0}", line.TotalPrice));
                                    outputFile.WriteLine(String.Format("\t Type: {0}", line.Type));

                                    outputFile.WriteLine(String.Format("\t VAT =================================="));
                                    outputFile.WriteLine(String.Format("\t \t Amount: {0}", line.VatAmount));
                                    outputFile.WriteLine(String.Format("\t \t Code: {0}", line.VatCode));
                                    outputFile.WriteLine(String.Format("\t \t Description: {0}", line.VatDescription));
                                    outputFile.WriteLine(String.Format("\t \t Percentage: {0}", line.VatPercentage));
                                    outputFile.WriteLine(String.Format("\t \t Tax: {0}", line.VendorTax));

                                    outputFile.WriteLine(String.Format(""));
                                }

                                if (null != invoice.AdditionalCharges)
                                {
                                    outputFile.WriteLine(String.Format("Additional Charges"));
                                    outputFile.WriteLine(String.Format("\t Discount_______________________________: {0}", invoice.AdditionalCharges.DiscountAmount));
                                    outputFile.WriteLine(String.Format("\t Audio CD Packaging_____________________: {0}", invoice.AdditionalCharges.AudioCdromPackagingAmount));
                                    outputFile.WriteLine(String.Format("\t Barcode Labelling______________________: {0}", invoice.AdditionalCharges.BarcodeLabellingAmount));
                                    outputFile.WriteLine(String.Format("\t Binding________________________________: {0}", invoice.AdditionalCharges.BindingAmount));
                                    outputFile.WriteLine(String.Format("\t Cataloguing Services___________________: {0}", invoice.AdditionalCharges.CataloguingServicesAmount));
                                    outputFile.WriteLine(String.Format("\t Classifications________________________: {0}", invoice.AdditionalCharges.ClassificationAmount));
                                    outputFile.WriteLine(String.Format("\t Commission_____________________________: {0}", invoice.AdditionalCharges.CommissionAmount));
                                    outputFile.WriteLine(String.Format("\t Exchage Rage Gaurantee Charge__________: {0}", invoice.AdditionalCharges.ExchangeRateGuaranteeChargeAmount));
                                    outputFile.WriteLine(String.Format("\t Data Communication_____________________: {0}", invoice.AdditionalCharges.DataCommunicationAmount));
                                    outputFile.WriteLine(String.Format("\t Delivery_______________________________: {0}", invoice.AdditionalCharges.DeliveryAmount));
                                    outputFile.WriteLine(String.Format("\t General Servicing By Library Bookseller: {0}", invoice.AdditionalCharges.GeneralServicingByLibraryBooksellerAmount));
                                    outputFile.WriteLine(String.Format("\t Handling Charge________________________: {0}", invoice.AdditionalCharges.HandlingChargeAmount));
                                    outputFile.WriteLine(String.Format("\t Insurance______________________________: {0}", invoice.AdditionalCharges.InsuranceAmount));
                                    outputFile.WriteLine(String.Format("\t Insurance Charge_______________________: {0}", invoice.AdditionalCharges.InsuranceChargeAmount));
                                    outputFile.WriteLine(String.Format("\t Miscellanous Charge____________________: {0}", invoice.AdditionalCharges.MiscellaneousChargeAmount));
                                    outputFile.WriteLine(String.Format("\t Miscellanous Credit Adjustment_________: {0}", invoice.AdditionalCharges.MiscellaneousCreditAdjustmentAmount));
                                    outputFile.WriteLine(String.Format("\t Miscellaneous Servicing________________: {0}", invoice.AdditionalCharges.MiscellaneousServicingAmount));
                                    outputFile.WriteLine(String.Format("\t Overhead_______________________________: {0}", invoice.AdditionalCharges.OverheadAmount));
                                    outputFile.WriteLine(String.Format("\t Packing Charge_________________________: {0}", invoice.AdditionalCharges.PackingChargeAmount));
                                    outputFile.WriteLine(String.Format("\t Postage and packing charge_____________: {0}", invoice.AdditionalCharges.PostageAndPackingChargeAmount));
                                    outputFile.WriteLine(String.Format("\t Posting charge_________________________: {0}", invoice.AdditionalCharges.PostingChargeAmount));
                                    outputFile.WriteLine(String.Format("\t Security Fitting_______________________: {0}", invoice.AdditionalCharges.SecurityFittingAmount));
                                    outputFile.WriteLine(String.Format("\t Shipment_______________________________: {0}", invoice.AdditionalCharges.ShipmentAmount));
                                    outputFile.WriteLine(String.Format("\t Sleeving_______________________________: {0}", invoice.AdditionalCharges.SleevingAmount));
                                    outputFile.WriteLine(String.Format("\t Small order surcharge__________________: {0}", invoice.AdditionalCharges.SmallOrderSurchargeAmount));
                                    outputFile.WriteLine(String.Format("\t Special handling_______________________: {0}", invoice.AdditionalCharges.SpecialHandlingAmount));
                                    outputFile.WriteLine(String.Format("\t Supply of aprovals book collections____: {0}", invoice.AdditionalCharges.SupplyOfApprovalsbookcollectionsAmount));
                                    outputFile.WriteLine(String.Format("\t Total Charges__________________________: {0}", invoice.AdditionalCharges.TotalChargesAmount));
                                }

                                outputFile.WriteLine(String.Format("Exchange Rates"));
                                foreach (ExchangeRate exchangeRate in invoice.ExchangeRates)
                                {
                                    outputFile.WriteLine(String.Format("\t Currency___: {0}", exchangeRate.Currency));
                                    outputFile.WriteLine(String.Format("\t Rate_______: {0}", exchangeRate.Rate));
                                    outputFile.WriteLine(String.Format("\t Is explicit? {0}", exchangeRate.IsExplicit));
                                }


                                outputFile.WriteLine(String.Format(""));
                            }
                        }
                    }
                }
            }

            catch (ArgumentException exception)
            {
                LogError(exception);
            }

            catch (IOException exception)
            {
                LogError(exception);
            }

            // Invalid configuration error.
            catch (FormatException)
            {
                LogError("Invalid configuration setting: update the application config file.");
            }
        }

        /// <summary>
        /// Log program error.
        /// </summary>
        /// <param name="exception"></param>
        static void LogError(Exception exception) => LogError(exception.Message);

        /// <summary>
        /// Log program error.
        /// </summary>
        /// <param name="message"></param>
        static void LogError(string message) => Console.Error.WriteLine(message);

        /// <summary>
        /// Fetch files from remote server.
        /// </summary>
        /// <param name="sessionOptions"></param>
        /// <param name="remotePath"></param>
        /// <param name="directoryInfo"></param>
        /// <returns></returns>
        static public FileInfo[] Fetch(SessionOptions sessionOptions, string remotePath, DirectoryInfo directoryInfo)
        {
            using Session importSession = new();
            {
                importSession.Open(sessionOptions);
                importSession.GetFiles(remotePath, directoryInfo.FullName, false);
                importSession.Close();
            }
            return directoryInfo.GetFiles("*.xml");
        }

    }
}
