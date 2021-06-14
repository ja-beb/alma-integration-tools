using AlmaIntegrationTools.AccountSync.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AlmaIntegrationTools.AccountSync.Reader
{
    /// <summary>
    /// Read patron records from SIF file.
    /// </summary>
    public class PatronFileReader : IPatronReader<User>, IDisposable
    {
        /// <summary>
        /// Email format.
        /// </summary>
        public string EmailFormat { get; set; }

        /// <summary>
        /// Campus code.
        /// </summary>
        public string CampusCode { get; set; }

        /// <summary>
        /// Clean input function - default to none. Required if input file isn't properly encoded.
        /// </summary>
        public Func<string, string> Clean { get; set; }

        /// <summary>
        /// Previous read buffered line.
        /// </summary>
        string BufferLine { get; set; }

        /// <summary>
        /// Stream reader.
        /// </summary>
        StreamReader StreamReader { get; set; }

        /// <summary>
        /// Country codes used to translate ISO codes.
        /// </summary>
        public Dictionary<string, string> CountryCodes { get; set; }

        /// <summary>
        /// Construct instance of UserFileReader.
        /// </summary>
        /// <param name="recordType"></param>
        /// <param name="streamReader"></param>
        public PatronFileReader(StreamReader streamReader) : this(streamReader, str => str)
        { }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="streamReader"></param>
        /// <param name="cleanInput"></param>
        public PatronFileReader(StreamReader streamReader, Func<string,string> cleanInput)
        {
            StreamReader = streamReader;
            Clean = cleanInput;
        }

        /// <summary>
        /// Read next full input line and parse into a User instance.
        /// </summary>
        /// <returns></returns>
        public User ReadNext()
        {
            StringBuilder sb = new(BufferLine);
            for (BufferLine = StreamReader.ReadLine(); BufferLine != null && !BufferLine.StartsWith("0000000000"); BufferLine = StreamReader.ReadLine())
            {
                sb.Append(' ');
                sb.Append(BufferLine);
            }
            string result = Clean(sb.ToString());
            return String.IsNullOrEmpty(result) ? null : Create(result);
        }

        /// <summary>
        /// Open reader (used to load initial buffer line).
        /// </summary>
        public void Open()
        {
            BufferLine = StreamReader.ReadLine();
        }

        /// <summary>
        /// Close reader.
        /// </summary>
        public void Close() => StreamReader?.Close();

        /// <summary>
        /// Dispose of instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Create user record from input strung.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        User Create(string line)
        {
            // Create new user instance.
            User user = new()
            {
                // Set constants:
                Status = "ACTIVE",
                StatusDate = DateTime.Now,

                // Create from input line:
                PrimaryId = GetSubstring(line, 238, 30).Trim(),
                Group = GetSubstring(line, 45, 10).Trim(),
                ExpiryDate = ParseDateTime(GetSubstring(line, 188, 10), DateTime.Now.AddYears(2)),
                PurgeDate = ParseDateTime(GetSubstring(line, 198, 10), DateTime.Now.AddYears(2)),

                // 1 = Personal name, 2 = Institution (same).
                LastName = GetSubstring(line, 310, 30).Trim(),
                FirstName = GetSubstring(line, 340, 20).Trim(),
                MiddleName = GetSubstring(line, 360, 20).Trim(),
                Title = GetSubstring(line, 380, 10).Trim(),
                CampusCode = CampusCode,

                // Statistics.
                Statistics = new List<string>() {
                    GetSubstring(line, 279, 3).Trim(),
                    GetSubstring(line, 282, 3).Trim(),
                    GetSubstring(line, 285, 3).Trim(),
                    GetSubstring(line, 288, 3).Trim(),
                    GetSubstring(line, 291, 3).Trim(),
                    GetSubstring(line, 294, 3).Trim(),
                    GetSubstring(line, 297, 3).Trim(),
                    GetSubstring(line, 300, 3).Trim(),
                    GetSubstring(line, 303, 3).Trim(),
                    GetSubstring(line, 306, 3).Trim(),
                }
                .Where(x => !String.IsNullOrEmpty(x))
                .Select(x => new Statistic(x, "EXTERNAL"))
                .ToList<Statistic>(),

                // Miscellaneous properties.
                PreferredLanguage = "en",
            };

            // Parse addresses
            if (int.TryParse(GetSubstring(line, 455, 1), out int addressCount))
            {
                line = GetSubstring(line, 456);
                bool isSchoolEmail;
                for (; 0 < addressCount; addressCount--)
                {
                    if (int.TryParse(GetSubstring(line, 10, 1), out int addressType))
                    {
                        switch (addressType)
                        {
                            case 1:
                            case 2:
                                user.ContactInfo.Addresses.Add(new (GetSubstring(line, 32, 50).Trim(), GetSubstring(line, 242, 40).Trim(), new String[] { "home" })
                                {
                                    SegmentType = "External",
                                    IsPreferred = 1 == addressType,
                                    Line2 = GetSubstring(line, 82, 40).Trim(),
                                    Line3 = GetSubstring(line, 122, 40).Trim(),
                                    Line4 = GetSubstring(line, 162, 40).Trim(),
                                    Line5 = GetSubstring(line, 202, 40).Trim(),
                                    State = GetSubstring(line, 282, 7).Trim(),
                                    PostalCode = GetSubstring(line, 289, 10).Trim(),
                                    Country = GetCountryCode(GetSubstring(line, 299, 20).Trim()),
                                    StartDate = ParseDateTime(GetSubstring(line, 12, 10), new DateTime()),
                                    EndDate = ParseDateTime(GetSubstring(line, 22, 10), DateTime.Now.AddYears(2)),
                                });

                                bool preferred = true;
                                string number = GetSubstring(line, 319, 25).Trim();
                                if (!String.IsNullOrEmpty(number))
                                {
                                    user.ContactInfo.Phones.Add(new Phone(number, new string[] { "home" })
                                    {
                                        SegmentType = "External",
                                        IsPreferred = preferred,
                                    });
                                    preferred = false;
                                }

                                number = GetSubstring(line, 344, 25).Trim();
                                if (!String.IsNullOrEmpty(number))
                                {
                                    user.ContactInfo.Phones.Add(new Phone(number, new string[] { "home" })
                                    {
                                        SegmentType = "External",
                                        IsPreferred = preferred,
                                        IsPreferredSms = true,
                                    });
                                    preferred = false;
                                }

                                number = GetSubstring(line, 369, 25).Trim();
                                if (!String.IsNullOrEmpty(number))
                                {
                                    user.ContactInfo.Phones.Add(new Phone(number, new string[] { "home" })
                                    {
                                        SegmentType = "External",
                                        IsPreferred = preferred,
                                    });
                                    preferred = false;
                                }

                                number = GetSubstring(line, 394, 25).Trim();
                                if (!String.IsNullOrEmpty(number))
                                {
                                    user.ContactInfo.Phones.Add(new Phone(number.Trim(), new string[] { "home" })
                                    {
                                        SegmentType = "External",
                                        IsPreferred = preferred,
                                    });
                                    preferred = false;
                                }

                                line = GetSubstring(line, 429);
                                break;

                            case 3:
                                string endLine = GetSubstring(line, 32);
                                int index = endLine.IndexOf(" ");
                                string email = null;
                                if (-1 == index)
                                {
                                    email = endLine.Trim();
                                    line = "";
                                }
                                else
                                {
                                    email = GetSubstring(endLine, 0, index).Trim();
                                    line = GetSubstring(endLine, index).Trim();
                                }

                                if (!String.IsNullOrEmpty(email))
                                {
                                    isSchoolEmail = !String.IsNullOrEmpty(EmailFormat) && email.EndsWith(EmailFormat);
                                    string[] contactTypes = Array.Empty<string>();
                                    if (!isSchoolEmail)
                                    {
                                        contactTypes = new string[] { "alternative" };
                                    }
                                    else
                                    {
                                        user.Identifiers.Add(new Identifier("INST_ID", email));
                                        contactTypes = user.IsStudent ? new string[] { "school" } : new string[] { "work" };
                                    }

                                    user.ContactInfo.Emails.Add(new Email(email, null, contactTypes)
                                    {
                                        SegmentType = "External",
                                        IsPreferred = isSchoolEmail,
                                    });
                                }
                                break;
                        }
                    }
                }
            }

            if (!String.IsNullOrEmpty(line))
            {
                user.Notes.Add(new Note("ALL", line)
                {
                    IsPopup = true,
                    CreatedBy = "system",
                    CreatedDate = DateTime.Now,
                });
            }
            return user;
        }

        /// <summary>
        /// Parse string from SIF format datestamp to DateTime object.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="defaultDate"></param>
        /// <returns></returns>
        static DateTime ParseDateTime(string date, DateTime defaultDate) => DateTime.TryParse(date, out DateTime pDate) ? pDate : defaultDate;

        /// <summary>
        /// Get substring without going outside of bounds.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        static string GetSubstring(string line, int offset)
        {
            if (String.IsNullOrEmpty(line)) return "";
            else if (offset >= line.Length) return "";
            else return line[offset..];
        }

        /// <summary>
        /// Get substring without going outside of bounds.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        static string GetSubstring(string line, int offset, int length)
        {
            if (String.IsNullOrEmpty(line)) return "";
            else if (offset >= line.Length) return "";
            else if ((offset + length) >= line.Length) return line[offset..];
            else return line.Substring(offset, length);
        }

        /// <summary>
        /// Translate ISO 2 level country code to ISO 3 letteer code.
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        string GetCountryCode(string label) => CountryCodes
            .Where(x => (x.Key == label || x.Value == label))
            .Select(x => x.Value)
            .FirstOrDefault<string>();

        /// <summary>
        /// Dispose helper function.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing) StreamReader?.Dispose();
        }
    }
}
