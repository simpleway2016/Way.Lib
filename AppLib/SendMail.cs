using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
namespace AppLib
{
    /**/
    /// <summary>  
    /// 发送邮件的类  
    /// </summary>
    public class SendMail
    {
        private MailMessage mailMessage;
        private SmtpClient smtpClient;
        private string password;//发件人密码
        private string smtp;
        public string UserName
        {
            get;
            set;
        }
        /**/
        /// <summary>  
        /// 处审核后类的实例  
        /// </summary>  
        /// <param name="To">收件人地址</param>  
        /// <param name="From">发件人地址</param>  
        /// <param name="Body">邮件正文</param>  
        /// <param name="Title">邮件的主题</param>  
        /// <param name="Password">发件人密码</param>
        public SendMail(string To, string From, string Body, string Title, string Password,string smtp)
        {
            this.UserName = From;
            mailMessage = new MailMessage();
            mailMessage.To.Add(To);
            mailMessage.From = new System.Net.Mail.MailAddress(From);
            mailMessage.Subject = Title;
            mailMessage.Body = Body;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.Priority = System.Net.Mail.MailPriority.Normal;
            this.password = Password;
            this.smtp = smtp;
            Console.WriteLine("send mail sucssesful");
        }
        public SendMail(string To, string From, string Body, string Title, string Password, string smtp,
            System.Text.Encoding bodyEncoding)
        {
            this.UserName = From;
            mailMessage = new MailMessage();
            mailMessage.To.Add(To);
            mailMessage.From = new System.Net.Mail.MailAddress(From);
            mailMessage.Subject = Title;
            mailMessage.Body = Body;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = bodyEncoding;
            mailMessage.Priority = System.Net.Mail.MailPriority.Normal;
            this.password = Password;
            this.smtp = smtp;
            Console.WriteLine("send mail sucssesful");
        }
        /**/
        /// <summary>  
        /// 添加附件  
        /// </summary>
        public void Attachments(System.IO.Stream contentStream,string filename)
        {
            contentStream.Position = 0;
            Attachment data;
            ContentDisposition disposition;
            data = new Attachment(contentStream, MediaTypeNames.Application.Octet);//实例化[shi li hua]附件  
            disposition = data.ContentDisposition;
            if (filename != null)
                disposition.FileName = filename;
            mailMessage.Attachments.Add(data);//添加到附件中  
        }
        /**/
        /// <summary>  
        /// 异步[yi bu]发送邮件[you jian]  
        /// </summary>  
        /// <param name="CompletedMethod"></param>
        public void SendAsync(SendCompletedEventHandler CompletedMethod)
        {
            if (mailMessage != null)
            {
                smtpClient = new SmtpClient();
                smtpClient.Credentials = new System.Net.NetworkCredential(this.UserName, password);//设置[she zhi]发件人身份[shen fen]的票据  
                smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtpClient.Host = this.smtp;
                smtpClient.SendCompleted += new SendCompletedEventHandler(CompletedMethod);//注册[zhu ce]异步[yi bu]发送邮件[you jian]完成时的事件[shi jian]  
                smtpClient.SendAsync(mailMessage, mailMessage.Body);
            }
        }
        /**/
        /// <summary>  
        /// 发送邮件[you jian]  
        /// </summary>
        public void Send()
        {
            if (mailMessage != null)
            {
                smtpClient = new SmtpClient();
                smtpClient.Credentials = new System.Net.NetworkCredential(this.UserName, password);//设置[she zhi]发件人身份[shen fen]的票据  
                smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtpClient.Host = this.smtp;
                smtpClient.Send(mailMessage);
            }
        }
    }
}