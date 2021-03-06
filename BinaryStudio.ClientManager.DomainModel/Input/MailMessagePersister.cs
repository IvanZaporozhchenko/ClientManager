﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using AE.Net.Mail.Imap;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.DomainModel.Infrastructure;

namespace BinaryStudio.ClientManager.DomainModel.Input
{
    /// <summary>
    /// saves received messages into repository
    /// </summary>
    public class MailMessagePersister
    {
        private readonly IRepository repository;

        private readonly IInquiryFactory inquiryFactory;

        private readonly IEmailClient emailClient;

        private readonly IMailMessageParserFactory mailMessageParserFactory;

        public MailMessagePersister(IRepository repository, IEmailClient emailClient, IInquiryFactory inquiryFactory, IMailMessageParserFactory mailMessageParserFactory)
        {
            this.repository = repository;
            this.inquiryFactory = inquiryFactory;
            this.emailClient = emailClient;
            this.mailMessageParserFactory = mailMessageParserFactory;

            emailClient.MailMessageReceived += ProcessMessage;
        }

        public void ProcessMessage(object sender, EventArgs args)
        {
            var unreadMessages = emailClient.GetUnreadMessages();
            foreach (var message in unreadMessages)
            {
                var convertedMessage = Convert(message);

                var messageReceiver = convertedMessage.Receivers.FirstOrDefault();
                var messageSender = convertedMessage.Sender;
                var ownerPerson = repository.Query<User>(x => x.Teams,x=>x.RelatedPerson).FirstOrDefault(x => x.RelatedPerson.Id == messageReceiver.Id);
                if (ownerPerson == null)
                {
                    ownerPerson = repository.Query<User>(x => x.Teams, x => x.RelatedPerson).FirstOrDefault(x => x.RelatedPerson.Id == messageSender.Id);
                }

                if (ownerPerson != null)
                {
                    convertedMessage.Owner = ownerPerson.CurrentTeam;
                    repository.Save(convertedMessage);
                }
                
                createInquiry(convertedMessage);
            }
        }



        /// <summary>
        /// Converts Input.MailMessage to Entities.MailMessage.
        /// If sender or receivers isn't exist then they will be added to repository
        /// If email forwarded then sender will be taken from body <see cref="MailMessageParserThunderbird"/>
        /// </summary>
        /// <param name="mailMessage">Input.MailMessage type of message</param>
        /// <returns>Entities.MailMessage type of message</returns>
        public Entities.MailMessage Convert(MailMessage mailMessage)
        {
            // If mail message is forwarded then Receiver will be person who forward mail and Sender taken from body
            if (isForwardedMailMessage(mailMessage))
            {
                var parser = mailMessageParserFactory.GetMailMessageParser(mailMessage.UserAgent);
                mailMessage.Subject = parser.GetSubject(mailMessage.Subject);
                mailMessage.Receivers = new List<MailAddress>{mailMessage.Sender};
                mailMessage.Sender = parser.GetSender(mailMessage);
                mailMessage.Body = parser.GetBody(mailMessage);
            }

            var returnMessage = new Entities.MailMessage
            {
                Body = mailMessage.Body,
                Date = mailMessage.Date,
                Subject = mailMessage.Subject,
                Receivers = new List<Person>()
            };

            //find a Sender in Repository
            var sender = repository.Query<Person>().FirstOrDefault(x => x.Email == mailMessage.Sender.Address);

            returnMessage.Sender = sender ?? AddNewPersonToRepository(mailMessage.Sender, mailMessage.Date);

            //find Receivers in repository
            foreach (var receiver in mailMessage.Receivers)
            {
                var currentReceiver = repository.Query<Person>().FirstOrDefault(x => x.Email == receiver.Address);

                returnMessage.Receivers.Add(currentReceiver ?? AddNewPersonToRepository(receiver, mailMessage.Date));
            }

            return returnMessage;
        }

        private void createInquiry(Entities.MailMessage convertedMessage)
        {
            if (convertedMessage.Sender.Role == PersonRole.Client &&
                !repository.Query<Inquiry>(x => x.Source)
                     .Select(x => x.Source)
                     .Any(convertedMessage.SameMessagePredicate))
            {
                var receiver = convertedMessage.Receivers.First();
                var ownerPerson = repository.Query<User>(x => x.Teams, x => x.RelatedPerson).FirstOrDefault(x => x.RelatedPerson.Id == receiver.Id);
                if (ownerPerson != null)
                {
                    var inquiry = repository.Query<Inquiry>(x => x.Client, x => x.Owner)
                        .FirstOrDefault(
                            x =>
                            x.Client.Id == convertedMessage.Sender.Id && x.Owner.Id == ownerPerson.CurrentTeam.Id &&
                            !x.Archived);
                    if (inquiry == null)
                    {
                        var newInquiry = inquiryFactory.CreateInquiry(convertedMessage);

                        newInquiry.Owner = ownerPerson.CurrentTeam;
                        repository.Save(newInquiry);

                    }
                    else
                    {
                        inquiry.ReferenceDate = null;
                        repository.Save(inquiry);
                    }
                }
                
           }
        }

        /// <summary>
        /// Returns true if mail message have fwd: or fw: in subject
        /// </summary>
        private static bool isForwardedMailMessage(MailMessage mailMessage)
        {
            return mailMessage.Subject.ToLower().Trim().StartsWith("fwd:") || mailMessage.Subject.ToLower().StartsWith("fw:");
        }

        /// <summary>
        /// Create new person in repository
        /// </summary>
        /// <param name="mailOfPerson">Mail address and name of person</param>
        /// <param name="dateOfIncomingMail">Date when mail is arrived</param>
        /// <returns>Person that was added to repository</returns>
        private Person AddNewPersonToRepository(MailAddress mailOfPerson, DateTime dateOfIncomingMail)
        {
            //Split name of client into first name and last name
            char[] separator = { ' ' };
            var personNameList = mailOfPerson.DisplayName.Split(separator).ToList();

            //add person to Repository
            var person = new Person
            {
                CreationDate = dateOfIncomingMail,
                Email = mailOfPerson.Address,
                FirstName = personNameList.Count >= 1 ? personNameList[0] : "",
                LastName = personNameList.Count >= 2 ? personNameList[1] : "",
                Role = PersonRole.Client
            };
            repository.Save(person);
            return person;
        }
    }
}
