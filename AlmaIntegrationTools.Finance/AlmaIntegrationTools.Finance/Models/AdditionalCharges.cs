using System.Xml.Serialization;

namespace AlmaIntergrationTools.Finance
{
    public class AdditionalCharges
    {
        /// <summary>
        /// The total sum of additional charges.
        /// </summary>
        [XmlElement("total_charges_amount")]
        public decimal TotalChargesAmount { get; set; }

        /// <summary>
        /// The amount of overhead charge.
        /// </summary>
        [XmlElement("overhead_amount")]
        public decimal OverheadAmount { get; set; }

        /// <summary>
        /// The amount of discount charge.
        /// </summary>
        [XmlElement("discount_amount")]
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// The amount of shipment charge.
        /// </summary>
        [XmlElement("shipment_amount")]
        public decimal ShipmentAmount { get; set; }

        /// <summary>
        /// The amount of insurance charge.
        /// </summary>
        [XmlElement("insurance_amount")]
        public decimal InsuranceAmount { get; set; }

        /// <summary>
        /// The amount of Supply of approvals/book collections charge.
        /// </summary>
        [XmlElement("supply_of_approvals_book_collections_amount")]
        public decimal SupplyOfApprovalsbookcollectionsAmount { get; set; }

        /// <summary>
        /// The amount of Barcode labelling charge.
        /// </summary>
        [XmlElement("barcode_labelling_amount")]
        public decimal BarcodeLabellingAmount { get; set; }

        /// <summary>
        /// The amount of Classification charge.
        /// </summary>
        [XmlElement("classification_amount")]
        public decimal ClassificationAmount { get; set; }

        /// <summary>
        /// The amount of General servicing by library bookseller charge.
        /// </summary>
        [XmlElement("general_servicing_by_library_bookseller_amount")]
        public decimal GeneralServicingByLibraryBooksellerAmount { get; set; }

        /// <summary>
        /// The amount of Binding charge.
        /// </summary>
        [XmlElement("binding_amount")]
        public decimal BindingAmount { get; set; }

        /// <summary>
        /// The amount of Sleeving charge.
        /// </summary>
        [XmlElement("sleeving_amount")]
        public decimal SleevingAmount { get; set; }

        /// <summary>
        /// The amount of Data communication charge.
        /// </summary>
        [XmlElement("data_communication_amount")]
        public decimal DataCommunicationAmount { get; set; }

        /// <summary>
        /// The amount of Miscellaneous servicing charge.
        /// </summary>
        [XmlElement("miscellaneous_servicing_amount")]
        public decimal MiscellaneousServicingAmount { get; set; }

        /// <summary>
        /// The amount of Audio/CD-ROM packaging charge.
        /// </summary>
        [XmlElement("audio_CD-ROM_packaging_amount")]
        public decimal AudioCdromPackagingAmount {get; set;}

        /// <summary>
        /// The amount of Security fitting charge.
        /// </summary>
        [XmlElement("security_fitting_amount")]
        public decimal SecurityFittingAmount { get; set; }

        /// <summary>
        /// The amount of Cataloguing services charge.
        /// </summary>
        [XmlElement("cataloguing_services_amount")]
        public decimal CataloguingServicesAmount { get; set; }


        /// <summary>
        /// The amount of Commission charge.
        /// </summary>
        [XmlElement("commission_amount")]
        public decimal CommissionAmount { get; set; }

        /// <summary>
        /// The amount of Delivery charge.
        /// </summary>
        [XmlElement("delivery_amount")]
        public decimal DeliveryAmount { get; set; }


        /// <summary>
        /// The amount of Exchange rate guarantee charge charge.
        /// </summary>
        [XmlElement("exchange_rate_guarantee_charge_amount")]
        public decimal ExchangeRateGuaranteeChargeAmount { get; set; }

        /// <summary>
        /// The amount of Handling charge charge.
        /// </summary>
        [XmlElement("handling_charge_amount")]
        public decimal HandlingChargeAmount { get; set; }

        /// <summary>
        /// The amount of Small order surcharge charge.
        /// </summary>
        [XmlElement("small_order_surcharge_amount")]
        public decimal SmallOrderSurchargeAmount { get; set; }

        /// <summary>
        /// The amount of Insurance charge.
        /// </summary>
        [XmlElement("insurance_charge_amount")]
        public decimal InsuranceChargeAmount { get; set; }

        // posting_charge_amount decimal 
        /// <summary>
        /// The amount of Posting charge.
        /// </summary>
        [XmlElement("posting_charge_amount")]
        public decimal PostingChargeAmount { get; set; }

        /// <summary>
        /// The amount of Packing charge.
        /// </summary>
        [XmlElement("packing_charge_amount")]
        public decimal PackingChargeAmount { get; set; }

        /// <summary>
        /// decimal The amount of Postage and packing charge.
        /// </summary>
        [XmlElement("postage_and_packing_charge_amount")]
        public decimal PostageAndPackingChargeAmount { get; set; }

        /// <summary>
        /// The amount of Special handling charge.
        /// </summary>
        [XmlElement("special_handling_amount")]
        public decimal SpecialHandlingAmount { get; set; }

        /// <summary>
        /// The amount of Miscellaneous credit adjustment charge.
        /// </summary>
        [XmlElement("miscellaneous_credit_adjustment_amount")]
        public decimal MiscellaneousCreditAdjustmentAmount { get; set; }

        /// <summary>
        /// The amount of miscellaneous charge charge.
        /// </summary>
        [XmlElement("miscellaneous_charge_amount")]
        public decimal MiscellaneousChargeAmount { get; set; }

    }
}


