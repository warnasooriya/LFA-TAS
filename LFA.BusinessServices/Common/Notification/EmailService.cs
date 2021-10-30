using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using TAS.DataTransfer.Enum;
using TAS.Services.Entities;

namespace TAS.Services.Common.Notification
{
    public class EmailService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly Lazy<EmailService> lazy = new Lazy<EmailService>(() => new EmailService());
        public static EmailService Instance { get { return lazy.Value; } }
        private readonly String SmtpServer, EmailFrom, EmailPassword = String.Empty;
        private readonly bool EnableSsl;
        private readonly int Port;
        public EmailService()
        {
            SmtpServer = ConfigurationData.SmtpServer;
            Port = ConfigurationData.Port;
            EmailFrom = ConfigurationData.EmailFrom;
            EmailPassword = ConfigurationData.EmailPassword;
            EnableSsl = ConfigurationData.EnableSsl;
        }

        public bool SendEmail(MailMessage email)
        {
            bool status = false;
            try
            {
                using (SmtpClient smtp = new SmtpClient(SmtpServer, Port))
                {
                    smtp.Credentials = new NetworkCredential(EmailFrom, EmailPassword);
                    smtp.EnableSsl = EnableSsl;
                    smtp.Send(email);
                }
            }
            catch (Exception ex)
            {
                // throw;
                logger.Error("Email Service : " + ex.Message );
            }
            return status;
        }
    }

    public class GetMyEmail
    {
        private static readonly Lazy<GetMyEmail> lazy = new Lazy<GetMyEmail>(() => new GetMyEmail());
        public static GetMyEmail Instance { get { return lazy.Value; } }
        private readonly string EmailPath, BaseUrl;

        public GetMyEmail()
        {
            EmailPath = ConfigurationData.EmailPath;
            BaseUrl = ConfigurationData.BaseUrl;
        }

        public MailMessage ForgotPassword(ForgotPasswordRequest forgotPasswordRequest, String mailAddress, Guid tpaId)
        {
            String TemplateHtml = GetEmailTemplateAsPlainText(EmailType.ForgotPassword);
            MailMessage email = new MailMessage();

            if (!String.IsNullOrEmpty(TemplateHtml))
            {
                TemplateHtml = TemplateHtml
                    .Replace("{FirstName}", mailAddress)
                    .Replace("{ResetPasswordLink}", BaseUrl + "login/changepasword/" + forgotPasswordRequest.Id + "/" + tpaId.ToString());
                email = new MailMessage()
                {
                    From = new MailAddress(ConfigurationData.EmailMask),
                    Subject = "Change Password on TAS",
                    Body = TemplateHtml,
                    IsBodyHtml = true,
                };

                AlternateView avHtml = AlternateView.CreateAlternateViewFromString
                (TemplateHtml, null, MediaTypeNames.Text.Html);

                LinkedResource inline = new LinkedResource(HttpContext.Current.Server.MapPath(ConfigurationData.EmailPath + "\\logo.png"));
                inline.ContentId = "LeftFieldLogo";
                avHtml.LinkedResources.Add(inline);
                email.AlternateViews.Add(avHtml);
                email.To.Add(new MailAddress(mailAddress));
            }
            return email;
        }

        public MailMessage ForgotPasswordTyre(ForgotPasswordRequest forgotPasswordRequest, String mailAddress, Guid tpaId)
        {
            String TemplateHtml = GetEmailTemplateAsPlainText(EmailType.ForgotPasswordTyre);
            MailMessage email = new MailMessage();

            if (!String.IsNullOrEmpty(TemplateHtml))
            {
                TemplateHtml = TemplateHtml
                    .Replace("{FirstName}", mailAddress)
                    .Replace("{ResetPasswordLink}", BaseUrl + "login/changepasword/" + forgotPasswordRequest.Id + "/" + tpaId.ToString());
                email = new MailMessage()
                {
                    From = new MailAddress(ConfigurationData.EmailMask),
                    Subject = "Change Password on TAS",
                    Body = TemplateHtml,
                    IsBodyHtml = true,
                };

                AlternateView avHtml = AlternateView.CreateAlternateViewFromString
                (TemplateHtml, null, MediaTypeNames.Text.Html);

                LinkedResource inlineContinentalLogo = new LinkedResource(HttpContext.Current.Server.MapPath(ConfigurationData.EmailPath + "\\ContinentalNew.png"));
                inlineContinentalLogo.ContentId = "ContinentalLogo";
                avHtml.LinkedResources.Add(inlineContinentalLogo);
                email.AlternateViews.Add(avHtml);
                email.To.Add(new MailAddress(mailAddress));
            }
            return email;
        }
        public MailMessage ForgotPasswordCustomer(ForgotPasswordRequest forgotPasswordRequest, String mailAddress, Guid tpaId, string currentCustomerId)
        {
            String TemplateHtml = GetEmailTemplateAsPlainText(EmailType.ForgotPassword);
            MailMessage email = new MailMessage();

            if (!String.IsNullOrEmpty(TemplateHtml))
            {
                TemplateHtml = TemplateHtml
                    .Replace("{FirstName}", mailAddress)
                    .Replace("{ResetPasswordLink}", BaseUrl + "login/changepaswordcustomer/" + forgotPasswordRequest.Id + "/" + currentCustomerId + "/" + tpaId.ToString());
                email = new MailMessage()
                {
                    From = new MailAddress(ConfigurationData.EmailMask),
                    Subject = "Change Password on TAS",
                    Body = TemplateHtml,
                    IsBodyHtml = true,
                };

                AlternateView avHtml = AlternateView.CreateAlternateViewFromString
                (TemplateHtml, null, MediaTypeNames.Text.Html);

                LinkedResource inline = new LinkedResource(HttpContext.Current.Server.MapPath(ConfigurationData.EmailPath + "\\logo.jpg"));
                inline.ContentId = "LeftFieldLogo";
                avHtml.LinkedResources.Add(inline);
                email.AlternateViews.Add(avHtml);
                email.To.Add(new MailAddress(mailAddress));
            }
            return email;
        }

        private string GetEmailTemplateAsPlainText(EmailType emailType)
        {
            switch (emailType)
            {
                case EmailType.ForgotPassword:
                    var path = HttpContext.Current.Server.MapPath(ConfigurationData.EmailPath + "\\forgotPassword.html");
                    return File.ReadAllText(path);
                case EmailType.StatementAndBooklet:
                    path = HttpContext.Current.Server.MapPath(ConfigurationData.EmailPath + "\\StatementAndBooklet.html");
                    return File.ReadAllText(path);
                case EmailType.StatementAndBookletTire:
                    path = HttpContext.Current.Server.MapPath(ConfigurationData.EmailPath + "\\StatementAndBookletTire.html");
                    return File.ReadAllText(path);
                case EmailType.UserRegistration:
                    path = HttpContext.Current.Server.MapPath(ConfigurationData.EmailPath + "\\UserRegistration.html");
                    return File.ReadAllText(path);
                case EmailType.CustomerConfirm:
                    path = HttpContext.Current.Server.MapPath(ConfigurationData.EmailPath + "\\UserRegistrationConfirm.html");
                    return File.ReadAllText(path);
                case EmailType.RejectionEmail:
                     path = HttpContext.Current.Server.MapPath(ConfigurationData.EmailPath + "\\rejectionemail.html");
                    return File.ReadAllText(path);
                case EmailType.ClaimApproveEmail:
                    path = HttpContext.Current.Server.MapPath(ConfigurationData.EmailPath + "\\ClaimApproved.html");
                    return File.ReadAllText(path);
                case EmailType.ClaimRejectedEmail:
                    path = HttpContext.Current.Server.MapPath(ConfigurationData.EmailPath + "\\ClaimRejected.html");
                    return File.ReadAllText(path);
                case EmailType.SalesSubmitEmail:
                    path = HttpContext.Current.Server.MapPath(ConfigurationData.EmailPath + "\\SalesSubmitConfirmation.html");
                    return File.ReadAllText(path);
                case EmailType.ForgotPasswordTyre:
                    path = HttpContext.Current.Server.MapPath(ConfigurationData.EmailPath + "\\forgotPasswordTyre.html");
                    return File.ReadAllText(path);
                case EmailType.TyreClaimSubmit:
                    path = HttpContext.Current.Server.MapPath(ConfigurationData.EmailPath + "\\TyreClaimSubmition.html");
                    return File.ReadAllText(path);
                case EmailType.TyreClaimRejection:
                    path = HttpContext.Current.Server.MapPath(ConfigurationData.EmailPath + "\\TyreClaimRejection.html");
                    return File.ReadAllText(path);
                case EmailType.TyreClaimApproved:
                    path = HttpContext.Current.Server.MapPath(ConfigurationData.EmailPath + "\\TyreClaimApproved.html");
                    return File.ReadAllText(path);
                default:
                    return "";
            }
        }

        internal void PolicyStatementAndBooklate(List<byte[]> ReportAttachmentList, List<String> EmailList, String FirstName)
        {
            String EmailBodyHTML = GetEmailTemplateAsPlainText(EmailType.StatementAndBooklet);

            String FormattedBodyHTML = EmailBodyHTML.Replace("{FirstName}", FirstName);
            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
            (FormattedBodyHTML, null, MediaTypeNames.Text.Html);

            LinkedResource inline = new LinkedResource(HttpContext.Current.Server.MapPath(ConfigurationData.EmailPath + "\\logo.png"));
            inline.ContentId = "LeftFieldLogo";
            avHtml.LinkedResources.Add(inline);

            MailMessage email = new MailMessage();


            email = new MailMessage()
            {
                From = new MailAddress(ConfigurationData.EmailMask),
                Subject = "Congratulation! Your Insurance is ready",
                Body = FormattedBodyHTML,
                IsBodyHtml = true,
            };
            email.AlternateViews.Add(avHtml);
            foreach (String emailAddr in EmailList)
            {
                email.To.Add(new MailAddress(emailAddr));
            }

            if (ConfigurationData.BlockEmailToTPAAdmin == false) {
                email.Bcc.Add(new MailAddress("ranga@trivow.com"));
                email.Bcc.Add(new MailAddress("no-reply@leftfield.net"));
            }

            var i = 0;
            foreach (Byte[] item in ReportAttachmentList)
            {

                var randomNumber = new Random().Next(100000, 999999);
                Attachment att = new Attachment(new MemoryStream(item), i == 0 ? "policy_statement_" + randomNumber + ".pdf" : "policy_booklet_" + randomNumber + ".pdf");
                email.Attachments.Add(att);
                i++;
            }

            new EmailService().SendEmail(email);
        }

        internal void UserRegistrationEmail(List<String> EmailList, String UserName, String Password, Guid tpaId)
        {
            String EmailBodyHTML = GetEmailTemplateAsPlainText(EmailType.UserRegistration);
            //.Replace("{FirstName}", FirstName)
            String FormattedBodyHTML = EmailBodyHTML
                .Replace("{UserName}", UserName)
                .Replace("{Password}", Password);
            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
            (FormattedBodyHTML, null, MediaTypeNames.Text.Html);

            LinkedResource inline = new LinkedResource(HttpContext.Current.Server.MapPath(ConfigurationData.EmailPath + "\\logo.png"));
            inline.ContentId = "LeftFieldLogo";
            avHtml.LinkedResources.Add(inline);

            MailMessage email = new MailMessage();


            email = new MailMessage()
            {
                From = new MailAddress(ConfigurationData.EmailMask),
                Subject = "Congratulations! Your registration is successfull!",
                Body = FormattedBodyHTML,
                IsBodyHtml = true,
            };
            email.AlternateViews.Add(avHtml);
            foreach (String emailAddr in EmailList)
            {
                email.To.Add(new MailAddress(emailAddr));
            }
            email.Bcc.Add(new MailAddress("ranga@trivow.com"));
            new EmailService().SendEmail(email);
        }

        internal void CustomerRegistrationEmail(List<String> EmailList, String UserName, String Password, Guid tpaId)
        {
            String EmailBodyHTML = GetEmailTemplateAsPlainText(EmailType.UserRegistration);
            //.Replace("{FirstName}", FirstName)
            String FormattedBodyHTML = EmailBodyHTML
                .Replace("{UserName}", UserName)
                .Replace("{Password}", Password);
            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
            (FormattedBodyHTML, null, MediaTypeNames.Text.Html);

            LinkedResource inline = new LinkedResource(HttpContext.Current.Server.MapPath(ConfigurationData.EmailPath + "\\logo.png"));
            inline.ContentId = "LeftFieldLogo";
            avHtml.LinkedResources.Add(inline);

            MailMessage email = new MailMessage();

            email = new MailMessage()
            {
                From = new MailAddress(ConfigurationData.EmailMask),
                Subject = "Congratulations ! Your registration is successfull!",
                Body = FormattedBodyHTML,
                IsBodyHtml = true,
            };
            email.AlternateViews.Add(avHtml);
            foreach (String emailAddr in EmailList)
            {
                email.To.Add(new MailAddress(emailAddr));
            }
            email.Bcc.Add(new MailAddress("ranga@trivow.com"));
            new EmailService().SendEmail(email);
        }

        internal void CustomerRegistrationConfirmEmail(List<String> EmailList, String UserName,
            string ConfirmUrl,Guid customerId, Stream logo)
        {
            String EmailBodyHTML = GetEmailTemplateAsPlainText(EmailType.CustomerConfirm);
            String FormattedBodyHTML = EmailBodyHTML
                .Replace("{UserName}", UserName)
                //.Replace("{ConfirmUrl}", ConfirmUrl)
                .Replace("{ConfirmCustomer}", BaseUrl + "login/Verifyemail/" + customerId.ToString());

            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
            (FormattedBodyHTML, null, MediaTypeNames.Text.Html);

            LinkedResource inline = new LinkedResource(logo);
            inline.ContentId = "LeftFieldLogo";
            avHtml.LinkedResources.Add(inline);

            MailMessage email = new MailMessage();

            email = new MailMessage()
            {
                From = new MailAddress(ConfigurationData.EmailMask),
                Subject = "Confirm your email!",
                Body = FormattedBodyHTML,
                IsBodyHtml = true,
            };
            email.AlternateViews.Add(avHtml);
            foreach (String emailAddr in EmailList)
            {
                email.To.Add(new MailAddress(emailAddr));
            }
            email.Bcc.Add(new MailAddress("ranga@trivow.com"));
            new EmailService().SendEmail(email);
        }

        internal void ClaimRejectedEmail(List<string> EmailList, string FirstName, string VINNo, string ClaimNumber)
        {
            String EmailBodyHTML = GetEmailTemplateAsPlainText(EmailType.ClaimRejectedEmail);
            //.Replace("{FirstName}", FirstName)
            String FormattedBodyHTML = EmailBodyHTML
                .Replace("{UserName}", FirstName)
                 .Replace("{VINNo}", VINNo)
                .Replace("{ClaimNumber}", ClaimNumber);
            //.Replace("{Password}", Password);
            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
            (FormattedBodyHTML, null, MediaTypeNames.Text.Html);

            LinkedResource inline = new LinkedResource(HttpContext.Current.Server.MapPath(ConfigurationData.EmailPath + "\\logo.png"));
            inline.ContentId = "LeftFieldLogo";
            avHtml.LinkedResources.Add(inline);

            MailMessage email = new MailMessage();

            email = new MailMessage()
            {
                From = new MailAddress(ConfigurationData.EmailMask),
                Subject = "Your claim has been rejected.!",
                Body = FormattedBodyHTML,
                IsBodyHtml = true,
            };
            email.AlternateViews.Add(avHtml);
            foreach (String emailAddr in EmailList)
            {
                email.To.Add(new MailAddress(emailAddr));
            }

            if (ConfigurationData.BlockEmailToTPAAdmin == false)
            {
                email.Bcc.Add(new MailAddress("ranga@trivow.com"));
                email.Bcc.Add(new MailAddress("no-reply@leftfield.net"));
            }
            new EmailService().SendEmail(email);
        }

        internal void ClaimApprovedEmail(List<string> EmailList, string FirstName, string VINNo, string ClaimNumber)
        {
            String EmailBodyHTML = GetEmailTemplateAsPlainText(EmailType.ClaimApproveEmail);
            //.Replace("{FirstName}", FirstName)
            String FormattedBodyHTML = EmailBodyHTML
                .Replace("{UserName}", FirstName)
                .Replace("{VINNo}", VINNo)
                .Replace("{ClaimNumber}", ClaimNumber);
            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
            (FormattedBodyHTML, null, MediaTypeNames.Text.Html);

            LinkedResource inline = new LinkedResource(HttpContext.Current.Server.MapPath(ConfigurationData.EmailPath + "\\logo.png"));
            inline.ContentId = "LeftFieldLogo";
            avHtml.LinkedResources.Add(inline);

            MailMessage email = new MailMessage();

            email = new MailMessage()
            {
                From = new MailAddress(ConfigurationData.EmailMask),
                Subject = "Congratulations ! Your claim is approved.!",
                Body = FormattedBodyHTML,
                IsBodyHtml = true,
            };
            email.AlternateViews.Add(avHtml);
            foreach (String emailAddr in EmailList)
            {
                email.To.Add(new MailAddress(emailAddr));
            }
            if (ConfigurationData.BlockEmailToTPAAdmin == false)
            {
                email.Bcc.Add(new MailAddress("ranga@trivow.com"));
                email.Bcc.Add(new MailAddress("no-reply@leftfield.net"));
            }
            new EmailService().SendEmail(email);
        }

        internal void PolicyStatementAndBooklateTire(List<byte[]> ReportAttachmentList, List<string> EmailList, string jwt, object res, string FirstName, IList<Policy> policyDetails)
        {
            String EmailBodyHTML = GetEmailTemplateAsPlainText(EmailType.StatementAndBookletTire);

            String token = Convert.ToString(res) + "&jwt=" + jwt;

            String elementsContractDetails= "";
            foreach (Policy p in policyDetails)
            {
                elementsContractDetails += "<tr><td style='width:50%;'> CP Contract No : " + p.PolicyNo + " </td><td>Start Date: </td><td>" + p.PolicyStartDate.ToString("dd/MM/yyyy") + "</td><td>End Date: </td><td>" + p.PolicyEndDate.ToString("dd/MM/yyyy") + "</td>";
            }

            String FormattedBodyHTML = EmailBodyHTML
                .Replace("{FirstName}", FirstName)
                .Replace("{ContractNo}", elementsContractDetails)
                .Replace("{Id}", token);
            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
            (FormattedBodyHTML, null, MediaTypeNames.Text.Html);

            LinkedResource inline = new LinkedResource(HttpContext.Current.Server.MapPath(ConfigurationData.EmailPath + "\\EUTLogoEmail.png"));
            inline.ContentId = "LeftFieldLogo";
            avHtml.LinkedResources.Add(inline);

            LinkedResource inlineContinentalLogo = new LinkedResource(HttpContext.Current.Server.MapPath(ConfigurationData.EmailLogoPath + "\\ContinentalNewForEmail.png"));
            inlineContinentalLogo.ContentId = "ContinentalLogo";
            avHtml.LinkedResources.Add(inlineContinentalLogo);


            MailMessage email = new MailMessage();


            email = new MailMessage()
            {
                From = new MailAddress(ConfigurationData.EmailMask),
                Subject = "Congratulation! Your Service Contract is ready",
                Body = FormattedBodyHTML,
                IsBodyHtml = true,
            };
            email.AlternateViews.Add(avHtml);
            foreach (String emailAddr in EmailList)
            {
                email.To.Add(new MailAddress(emailAddr));
            }
            if (ConfigurationData.BlockEmailToTPAAdmin == false)
            {
                email.Bcc.Add(new MailAddress("ranga@trivow.com"));
                email.Bcc.Add(new MailAddress("no-reply@leftfield.net"));
            }
            var i = 0;
            foreach (Byte[] item in ReportAttachmentList)
            {

                var randomNumber = new Random().Next(100000, 999999);
                Attachment att = new Attachment(new MemoryStream(item), i == 0 ? "ContiSure Service Contract 2021"  + ".pdf" : "policy_booklet_" + randomNumber + ".pdf");
                email.Attachments.Add(att);
                i++;
            }

            new EmailService().SendEmail(email);
        }


        internal void TyreSalesSubmitComfirmation( List<string> EmailList,  string FirstName,  string PolicyNo)
        {
            String EmailBodyHTML = GetEmailTemplateAsPlainText(EmailType.SalesSubmitEmail);



            String FormattedBodyHTML = EmailBodyHTML
                .Replace("{FirstName}", FirstName)
                .Replace("ContractNo", PolicyNo);

            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
            (FormattedBodyHTML, null, MediaTypeNames.Text.Html);

            LinkedResource inline = new LinkedResource(HttpContext.Current.Server.MapPath(ConfigurationData.EmailPath + "\\EUTLogoEmail.png"));
            inline.ContentId = "LeftFieldLogo";
            avHtml.LinkedResources.Add(inline);

            LinkedResource inlineContinentalLogo = new LinkedResource(HttpContext.Current.Server.MapPath(ConfigurationData.EmailLogoPath + "\\ContinentalNewForEmail.png"));
            inlineContinentalLogo.ContentId = "ContinentalLogo";
            avHtml.LinkedResources.Add(inlineContinentalLogo);

            MailMessage email = new MailMessage();
            email = new MailMessage()
            {
                From = new MailAddress(ConfigurationData.EmailMask),
                Subject = "Congratulation! Your Service Contract is Submitted",
                Body = FormattedBodyHTML,
                IsBodyHtml = true,
            };
            email.AlternateViews.Add(avHtml);
            foreach (String emailAddr in EmailList)
            {
                email.To.Add(new MailAddress(emailAddr));
            }
            if (ConfigurationData.BlockEmailToTPAAdmin == false)
            {
                email.Bcc.Add(new MailAddress("no-reply@leftfield.net"));
            }
            new EmailService().SendEmail(email);
        }

        internal void TyreClaimSubmition(List<string> EmailList, string FirstName, string ClaimNumber)
        {
            String EmailBodyHTML = GetEmailTemplateAsPlainText(EmailType.TyreClaimSubmit);


            String FormattedBodyHTML = EmailBodyHTML
                .Replace("{FirstName}", FirstName)
                .Replace("ClaimNumber", ClaimNumber);

            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
            (FormattedBodyHTML, null, MediaTypeNames.Text.Html);

            LinkedResource inline = new LinkedResource(HttpContext.Current.Server.MapPath(ConfigurationData.EmailPath + "\\EUTLogo.png"));
            inline.ContentId = "LeftFieldLogo";
            avHtml.LinkedResources.Add(inline);

            LinkedResource inlineContinentalLogo = new LinkedResource(HttpContext.Current.Server.MapPath(ConfigurationData.EmailLogoPath + "\\ContinentalNew.png"));
            inlineContinentalLogo.ContentId = "ContinentalLogo";
            avHtml.LinkedResources.Add(inlineContinentalLogo);

            MailMessage email = new MailMessage();
            email = new MailMessage()
            {
                From = new MailAddress(ConfigurationData.EmailMask),
                Subject = "Congratulation! Your Claim is Submitted",
                Body = FormattedBodyHTML,
                IsBodyHtml = true,
            };
            email.AlternateViews.Add(avHtml);

            // removed customer notifications because of jibi requested to don't send claim submission email to customer
            //foreach (String emailAddr in EmailList)
            //{
            //    email.To.Add(new MailAddress(emailAddr));
            //}
            if (ConfigurationData.BlockEmailToTPAAdmin == false)
            {
                email.To.Add(new MailAddress("no-reply@leftfield.net"));
            }

            new EmailService().SendEmail(email);

        }
        internal void TyreClaimRejection(List<string> EmailList, string ContractNo)
        {
            String EmailBodyHTML = GetEmailTemplateAsPlainText(EmailType.TyreClaimRejection);

            String FormattedBodyHTML = EmailBodyHTML
                .Replace("{ContractNumber}", ContractNo);

            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
            (FormattedBodyHTML, null, MediaTypeNames.Text.Html);

            LinkedResource inline = new LinkedResource(HttpContext.Current.Server.MapPath(ConfigurationData.EmailPath + "\\EUTLogoEmail.png"));
            inline.ContentId = "LeftFieldLogo";
            avHtml.LinkedResources.Add(inline);

            LinkedResource inlineContinentalLogo = new LinkedResource(HttpContext.Current.Server.MapPath(ConfigurationData.EmailLogoPath + "\\ContinentalNewForEmail.png"));
            inlineContinentalLogo.ContentId = "ContinentalLogo";
            avHtml.LinkedResources.Add(inlineContinentalLogo);

            MailMessage email = new MailMessage();
            email = new MailMessage()
            {
                From = new MailAddress(ConfigurationData.EmailMask),
                Subject = ContractNo,
                Body = FormattedBodyHTML,
                IsBodyHtml = true,
            };
            email.AlternateViews.Add(avHtml);
            foreach (String emailAddr in EmailList)
            {
                email.To.Add(new MailAddress(emailAddr));
            }
            if (ConfigurationData.BlockEmailToTPAAdmin == false)
            {
                email.Bcc.Add(new MailAddress("no-reply@leftfield.net"));
            }
            new EmailService().SendEmail(email);
        }

        internal void TyreClaimApprovedEmail(List<string> EmailList, string ContractNo)
        {
            String EmailBodyHTML = GetEmailTemplateAsPlainText(EmailType.TyreClaimApproved);
            //.Replace("{FirstName}", FirstName)
            String FormattedBodyHTML = EmailBodyHTML
                .Replace("{ContractNumber}", ContractNo);

            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
            (FormattedBodyHTML, null, MediaTypeNames.Text.Html);

            LinkedResource inline = new LinkedResource(HttpContext.Current.Server.MapPath(ConfigurationData.EmailPath + "\\EUTLogoEmail.png"));
            inline.ContentId = "LeftFieldLogo";
            avHtml.LinkedResources.Add(inline);

            LinkedResource inlineContinentalLogo = new LinkedResource(HttpContext.Current.Server.MapPath(ConfigurationData.EmailLogoPath + "\\ContinentalNewForEmail.png"));
            inlineContinentalLogo.ContentId = "ContinentalLogo";
            avHtml.LinkedResources.Add(inlineContinentalLogo);

            MailMessage email = new MailMessage();

            email = new MailMessage()
            {
                From = new MailAddress(ConfigurationData.EmailMask),
                Subject = ContractNo,
                Body = FormattedBodyHTML,
                IsBodyHtml = true,
            };
            email.AlternateViews.Add(avHtml);
            foreach (String emailAddr in EmailList)
            {
                email.To.Add(new MailAddress(emailAddr));
            }
            if (ConfigurationData.BlockEmailToTPAAdmin == false)
            {
                email.Bcc.Add(new MailAddress("ranga@trivow.com"));
                email.Bcc.Add(new MailAddress("no-reply@leftfield.net"));
            }
            new EmailService().SendEmail(email);
        }

        internal void RejectionEmail(List<string> EmailList, string UserName, string v, Guid customerId, Stream logo)
        {
            String EmailBodyHTML = GetEmailTemplateAsPlainText(EmailType.RejectionEmail);
            String FormattedBodyHTML = EmailBodyHTML
                .Replace("{UserName}", UserName);


            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
            (FormattedBodyHTML, null, MediaTypeNames.Text.Html);

            LinkedResource inline = new LinkedResource(logo);
            inline.ContentId = "LeftFieldLogo";
            avHtml.LinkedResources.Add(inline);

            MailMessage email = new MailMessage();

            email = new MailMessage()
            {
                From = new MailAddress(ConfigurationData.EmailMask),
                Subject = "Your insurance has been expired.",
                Body = FormattedBodyHTML,
                IsBodyHtml = true,
            };
            email.AlternateViews.Add(avHtml);
            foreach (String emailAddr in EmailList)
            {
                email.To.Add(new MailAddress(emailAddr));
            }
            if (ConfigurationData.BlockEmailToTPAAdmin == false)
            {
                email.Bcc.Add(new MailAddress("ranga@trivow.com"));
                email.Bcc.Add(new MailAddress("no-reply@leftfield.net"));
            }
            new EmailService().SendEmail(email);
        }
    }
}
