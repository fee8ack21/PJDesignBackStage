using App.Common;
using App.DAL.Contexts;
using App.DAL.Repositories;
using App.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.PL.HostedServices
{
    public class EmailService : IHostedService
    {
        private int _executionCount = 0;
        private readonly ILogger<EmailService> _logger;
        private Timer _timer;
        private DbContextOptionsBuilder<PJDesignContext> _optionsBuilder;

        private SmtpClient _client;
        private readonly string _mailTitle = "系統通知";
        private readonly string _mailContent = "已收到您的資訊，將會有專人向您聯繫。";
        private string _senderAccount;
        private string _senderName;
        private string _senderPassword;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
            _optionsBuilder = new DbContextOptionsBuilder<PJDesignContext>();
            _optionsBuilder.UseSqlServer(AppSettingHelper.GetSection("ConnectionStrings").GetSection("PJDesign").Value);

            var mailOptions = AppSettingHelper.GetSection("MailOptions");
            _senderAccount = mailOptions.GetSection("Account").Value ?? "";
            _senderName = mailOptions.GetSection("Name").Value ?? "";
            _senderPassword = mailOptions.GetSection("Password").Value ?? "";

            _client = new SmtpClient();
            _client.Credentials = new System.Net.NetworkCredential(_senderAccount, _senderPassword);
            _client.Host = "smtp.gmail.com";
            _client.Port = 587;
            _client.EnableSsl = true;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(60));

            return Task.CompletedTask;
        }

        private void DoWork(object? state)
        {
            Interlocked.Increment(ref _executionCount);
            if (_executionCount == 1)
            {
                try
                {
                    using (var context = new PJDesignContext(_optionsBuilder.Options))
                    {
                        var mails = context.TblContact
                            .Where(x => x.CAutoReplyStatus == (int)EmailStatus.未處理 || x.CAutoReplyStatus == (int)EmailStatus.未完成)
                            .ToList();

                        foreach (var mail in mails)
                        {
                            try
                            {
                                var msg = new MailMessage();
                                msg.To.Add(mail.CEmail);
                                msg.From = new MailAddress(_senderAccount, _senderName, Encoding.UTF8);
                                msg.Subject = _mailTitle;
                                msg.SubjectEncoding = Encoding.UTF8;
                                msg.Body = $"親愛的 <strong>{mail.CName}</strong> 您好，<br/>" + _mailContent;
                                msg.BodyEncoding = Encoding.UTF8;
                                msg.IsBodyHtml = true;

                                mail.CAutoReplyDt = DateHelper.GetNowDate();
                                mail.CAutoReplyStatus = (int)EmailStatus.已執行;

                                _client.Send(msg);
                                msg.Dispose();

                                _logger.LogInformation($"Email Service Success: ID:{mail.CId} Name:{mail.CName}");
                            }
                            catch (Exception ex)
                            {
                                mail.CAutoReplyStatus = (int)EmailStatus.未完成;
                                _logger.LogError($"Email Service Error: {ex.Message}");
                            }

                            context.TblContact.Update(mail);
                            context.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Email Service Error: {ex.Message}");
                }
            }
            Interlocked.Decrement(ref _executionCount);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _client.Dispose();
        }
    }
}
