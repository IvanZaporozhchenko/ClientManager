﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BinaryStudio.ClientManager.DomainModel.DataAccess;
using BinaryStudio.ClientManager.DomainModel.Entities;
using BinaryStudio.ClientManager.WebUi.Models;

namespace BinaryStudio.ClientManager.WebUi.Controllers
{
    public class WeekViewController : Controller
    {
        private readonly IRepository repository;

        public WeekViewController(IRepository repository)
        {
            this.repository = repository;
        }
        //
        // GET: /WeekView/

        public ViewResult Week()
        {
            var today = DateTime.Today;
            var monday = today.AddDays(1 - (int)today.DayOfWeek);
            var friday = today.AddDays(5 - (int)today.DayOfWeek);
            var model = new WeekViewModel();
            var inquiries = repository.Query<Inquiry>(x => x.Client, x => x.Source, x => x.Assignee)
                .Where(x => x.ReferenceDate >= monday && x.ReferenceDate <= friday)
                .OrderBy(x => x.ReferenceDate).ToList();
            var employees = repository.Query<Person>(x => x.RelatedMails)
                .Where(x => x.Role == PersonRole.Employee).ToList();
            model.Inquiries = inquiries;
            model.Employees = employees;
            return View(model);
        }

        [HttpPost]
        public void AssignEmployee(int inquiryId, int employeeId)
        {
            var employee = repository.Get<Person>(employeeId, x => x.RelatedMails);
            var inquiry = repository.Get<Inquiry>(inquiryId, x => x.Assignee);
            inquiry.Assignee = employee;
            repository.Save(inquiry.Assignee);
        }

        [HttpPost]
        public void AddComment(int inquiryId, string comment)
        {
            var inquiry = repository.Get<Inquiry>(inquiryId);
            inquiry.Description = comment;
            repository.Save(inquiry);
        }

        [HttpGet]
        public string GetComment(int inquiryId)
        {
            var inquiry = repository.Get<Inquiry>(inquiryId);
            return inquiry.Description;
        }

    }
}