using System;
using System.Xml.Serialization;

namespace AlmaIntegrationTools.AccountSync.Models
{
    [Serializable]
    [XmlType(AnonymousType = true)]
    public class Note
    {
        /// <summary>
        /// The note's type.
        /// </summary>
        [XmlElement("note_type", Type = typeof(string))]
        public string Type { get; set; }

        /// <summary>
        /// The note's text.
        /// </summary>
        [XmlElement("note_text", Type = typeof(string))]
        public string Text { get; set; }

        /// <summary>
        /// Indication whether the user is able to view the note.
        /// </summary>
        [XmlElement("user_viewable", Type = typeof(bool))]
        public bool IsUserViewable { get; set; }

        /// <summary>
        /// Indication whether the note supposed to popup while entering patron services.
        /// </summary>
        [XmlElement("popup_note", Type = typeof(bool))]
        public bool IsPopup { get; set; }

        /// <summary>
        /// Creator of the note.
        /// </summary>
        [XmlElement("created_by", Type = typeof(string))]
        public string CreatedBy { get; set; }

        /// <summary>
        /// Creation date of the note.
        /// </summary>
        [XmlElement("created_date", DataType = "dateTime", Type = typeof(DateTime))]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Segment type of this note.
        /// </summary>
        [XmlAttribute("segment_type")]
        public string SegmentType { get; set; }

        /// <summary>
        /// Construct instance of a user Note.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="text"></param>
        public Note(string type, string text)
        {
            Type = type;
            Text = text;
            IsUserViewable = false;
            IsPopup = false;
        }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Note()
        { }
    }
}
