﻿using System;
using System.Collections.Generic;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Entities
{
    /// <summary>
    /// Class that represents model of person.
    /// </summary>
    public class Person : IIdentifiable
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// First name of person.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of person.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Person's full name.
        /// </summary>
        public string FullName
        {
            get 
            {
                var fullName=string.Format("{0} {1}", FirstName, LastName); 
                if (fullName.Trim()=="")
                {
                    fullName = "Unknown";
                }
                return fullName;
            }
        }

        /// <summary>
        /// Date when person was added to system.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Email address of person.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Int value that represents Role of person.
        /// For full list of values <see cref="PersonRole"/> enumeration.
        /// </summary>
        public int Role { get; set; }

        /// <summary>
        /// Country where person lives.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Phone number of person.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Photo of the person
        /// return photoPath!=""?photoPath:"~/Content/images/DummyPhoto.jpg"; - this logic should be employed during creation of entity and not querying
        /// </summary>
        public string PhotoUri { get; set; }

        public IList<MailMessage> RelatedMails { get; set; }

        public bool Equals(Person other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id == Id && Equals(other.FirstName, FirstName) && Equals(other.LastName, LastName) && other.CreationDate.Equals(CreationDate) && Equals(other.Email, Email) && other.Role == Role;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof (Person) && Equals((Person)obj);
        }

        /// <summary>
        /// Uri for twitter
        /// </summary>
        public string TwitterUri { get; set; }

        /// <summary>
        /// Uri for facebook
        /// </summary>
        public string FacebookUri { get; set; }

        /// <summary>
        /// Uri for LinkedIn
        /// </summary>
        public string LinkedInUri { get; set; }
        

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
                result = (result*397) ^ (FirstName != null ? FirstName.GetHashCode() : 0);
                result = (result*397) ^ (LastName != null ? LastName.GetHashCode() : 0);
                result = (result*397) ^ CreationDate.GetHashCode();
                result = (result*397) ^ (Email != null ? Email.GetHashCode() : 0);
                result = (result*397) ^ Role;
                return result;
            }
        }
    }
}