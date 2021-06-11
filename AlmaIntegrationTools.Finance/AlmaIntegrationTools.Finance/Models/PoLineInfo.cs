using System.Collections.Generic;
using System.Xml.Serialization;

namespace AlmaIntergrationTools.Finance
{
    public class PoLineInfo
    {
        /// <summary>
        /// Indication whether the PO line has been fully invoiced.Relevant only for onetime PO lines.
        /// </summary>
        [XmlElement("fully_Invoiced_ind")]
        public bool FullyInvoicedIndicator { get; set; }

        /// <summary>
        /// The library which owned the PO line.
        /// </summary>
        [XmlElement("po_line_owner")]
        public string Owner { get; set; }

        /// <summary>
        /// The MMS Id of the PO line.
        /// </summary>
        [XmlElement("mms_record_id")]
        public string MmsRecordId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("meta_data_values")]
        public MetaDataValues MetaDataValues { get; set; }

        /// <summary>
        /// The PO line vendor reference number
        /// </summary>
        [XmlElement("vendor_reference_number")]
        public string ReferenceNumber { get; set; }

        /// <summary>
        /// The number of the PO line.
        /// </summary>
        [XmlElement("po_line_number")]
        public string Number { get; set; }

        /// <summary>
        /// The price of the PO line.
        /// </summary>
        [XmlElement("po_line_price")]
        public decimal Price { get; set; }

        /// <summary>
        /// The title of the PO line.
        /// </summary>
        [XmlElement("po_line_title")]
        public string Title { get; set; }

        /// <summary>
        /// List of intersted user within a po line
        /// </summary>
        [XmlArray("intersted_user_list")]
        [XmlArrayItem(ElementName = "intersted_user")]
        public List<InterstedUser> InterestedUserList { get; set; }

        /// <summary>
        /// The additional order reference of the PO line.
        /// </summary>
        [XmlElement("additional_order_reference")]
        public string AdditionalOrderReference { get; set; }

        /// <summary>
        /// The PO line vendor note.
        /// </summary>
        [XmlElement("vendor_note")]
        public string Note { get; set; }

    }
}
