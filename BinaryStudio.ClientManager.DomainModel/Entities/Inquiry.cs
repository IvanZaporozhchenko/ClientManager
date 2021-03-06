﻿using System;
using System.Collections.Generic;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Entities
{
    /// <summary>
    /// Details about both inquiry
    /// </summary>
    public class Inquiry : IOwned
    {
        public Inquiry()
        {
            ReferenceDate = null;
            Comments = new List<Comment>();
            Tags = new List<Tag>();
            Archived = false;
        }

        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Owner of the inquiry
        /// </summary>
        public Team Owner { get; set; }

        /// <summary>
        /// Author of inquiry.
        /// </summary>
        public Person Client { get; set; }

        /// <summary>
        /// Person which is assigned to inquiry
        /// </summary>
        public Person Assignee { get; set; }

        /// <summary>
        /// Date of deadline
        /// </summary>
        public DateTime? ReferenceDate { get; set; }

        /// <summary>
        /// Contains source message from which inquiry was created.
        /// </summary>
        public MailMessage Source { get; set; }

        /// <summary>
        /// Contains subject, which equal to email subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Contains inquiry description, which equal to originating email body
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// List of comments.
        /// </summary>
        public IList<Comment> Comments { get; set; }

        /// <summary>
        /// list of tags appointed to inquiry
        /// </summary>
        public IList<Tag> Tags { get; set; }

        /// <summary>
        /// Indicates whether inquiry is archived
        /// </summary>
        public bool Archived { get; set; }

        public bool Equals(Inquiry other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id == Id && Equals(other.Client, Client) && Equals(other.Source, Source)
                && Equals(other.Tags, Tags) && Equals(other.Comments, Comments);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Inquiry)) return false;
            return Equals((Inquiry) obj);
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
            unchecked
            {
                int result = Id;
                result = (result*397) ^ Id;
                result = (result*397) ^ (Client != null ? Client.GetHashCode() : 0);
                result = (result*397) ^ (Source != null ? Source.GetHashCode() : 0);
                return result;
            }
        }
    }
}