﻿using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Entities
{
    /// <summary>
    /// Details about both inquiry
    /// </summary>
    public class Inquiry : IIdentifiable
    {
        public Inquiry()
        {
            Status = InquiryStatus.IncomingInquiry;
        }

        /// <summary>
        /// Int value that represents Status of inquiry.
        /// For full list of values <see cref="InquiryStatus"/> enumeration.
        /// </summary>
        public int StatusValue { get; set; }
        
        /// <summary>
        /// Inquiry status (e.g., IncomingInquiry, WaitingForReply, InProgress, Closed)
        /// for full list see <see cref="InquiryStatus"/> enumeration).
        /// It gives and sets status by StatusValue property
        /// </summary>
        public InquiryStatus Status {
            get
            {
                return (InquiryStatus)StatusValue;
            }
            set
            {
                StatusValue = (int)value;
            }
        }

        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Author of inquiry.
        /// </summary>
        public Person Client { get; set; }

        /// <summary>
        /// Contains source message from which inquiry was created.
        /// </summary>
        public MailMessage Source { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="objToCompare">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object objToCompare)
        {
            var inquiryToCompare = objToCompare as Inquiry;
            return inquiryToCompare != null &&
                   Status == inquiryToCompare.Status &&
                   Id == inquiryToCompare.Id &&
                   Client.Equals(inquiryToCompare.Client) &&
                   Source.Equals(inquiryToCompare.Source);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing 
        /// algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Id;
        }
    }
}