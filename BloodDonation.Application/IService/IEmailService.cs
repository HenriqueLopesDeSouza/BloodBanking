﻿
namespace BloodBanking.Application.IService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string recipientEmail, string subject, string message);
    }
}
