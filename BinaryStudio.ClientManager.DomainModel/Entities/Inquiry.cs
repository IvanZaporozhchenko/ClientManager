﻿using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Entities
{
    /// <summary>
    /// Details about both inquiry
    /// </summary>
    public class Inquiry : IIdentifiable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Inquiry"/> class.
        /// </summary>
        /// <param name="issuer">The issuer.</param>
        /// <param name="issue">The issue.</param>
        /// <param name="id">The id of inquiry.</param>
        public Inquiry(Person issuer, MailMessage issue, int id)
        {
            Client = issuer;
            Source = issue;
            Id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Inquiry"/> class.
        /// </summary>
        public Inquiry()
        {
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

        public override bool Equals(object objToCompare)
        {
            var inquiryToCompare = objToCompare as Inquiry;
            if (Id==inquiryToCompare.Id &&
                Client.Equals(inquiryToCompare.Client) &&
                Source.Equals(inquiryToCompare.Source))
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}