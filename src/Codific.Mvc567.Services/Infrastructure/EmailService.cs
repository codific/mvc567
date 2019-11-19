// This file is part of the mvc567 distribution (https://github.com/codific/mvc567).
// Copyright (C) 2019 Codific Ltd.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Codific.Mvc567.Common.Options;
using Codific.Mvc567.Dtos.EmailModels.Abstraction;
using Codific.Mvc567.Dtos.ServiceResults;
using Codific.Mvc567.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace Codific.Mvc567.Services.Infrastructure
{
    public class EmailService : IEmailService
    {
        private readonly IRazorViewEngine razorViewEngine;
        private readonly ITempDataProvider tempDataProvider;
        private readonly IServiceProvider serviceProvider;
        private readonly SmtpConfig smtpConfiguration;

        public EmailService(
            IRazorViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider,
            IOptions<SmtpConfig> smtpConfiguration)
        {
            this.razorViewEngine = razorViewEngine;
            this.tempDataProvider = tempDataProvider;
            this.serviceProvider = serviceProvider;
            this.smtpConfiguration = smtpConfiguration.Value;
        }

        public async Task<EmailServiceResult> SendEmailAsync(string viewName, AbstractEmailModel model)
        {
            try
            {
                string message = await this.RenderToStringAsync(viewName, model);

                SmtpClient client = new SmtpClient(this.smtpConfiguration.Host);
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(this.smtpConfiguration.Username, this.smtpConfiguration.Password);
                client.Port = this.smtpConfiguration.Port;
                client.EnableSsl = this.smtpConfiguration.UseSSL;

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(this.smtpConfiguration.EmailAddress);
                mailMessage.To.Add(model.Email);
                mailMessage.Body = message;
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                mailMessage.Subject = model.Subject;
                client.Send(mailMessage);

                return new EmailServiceResult { Success = true };
            }
            catch (Exception)
            {
                return new EmailServiceResult { Success = false };
            }
        }

        private async Task<string> RenderToStringAsync(string viewName, AbstractEmailModel model)
        {
            var httpContext = new DefaultHttpContext { RequestServices = this.serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            using (var sw = new StringWriter())
            {
                var viewResult = this.razorViewEngine.FindView(actionContext, viewName, false);

                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"{viewName} does not match any available view");
                }

                var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model,
                };

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, this.tempDataProvider),
                    sw,
                    new HtmlHelperOptions());

                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }
        }
    }
}