/*
 * Copyright © 2020，CloudRoam.com
 * All rights reserved.
 *  
 * 文件名称：EmailHelper.cs
 /* 摘   要：
 *  
 * 当前版本：1.0
 * 作   者：Kakaluote
 */

using MailKit.Net.Smtp;
using MimeKit;
using System.Linq;
using System.Threading.Tasks;
using Template.Configurations;

namespace Template.Helper
{
    public static class EmailHelper
    {
        public static async Task SendAsync(MimeMessage message)
        {
            if (!message.From.Any())
            {
                message.From.Add(new MailboxAddress(AppSettings.Email.From.Name,AppSettings.Email.From.Address));
            }

            if (!message.To.Any())
            {
                //var address = AppSettings.Email.To.Select(x => new MailboxAddress(x.Key, x.Value));
                //message.To.AddRange(address);
            }

            using var client = new SmtpClient
            {
                ServerCertificateValidationCallback = (s, c, h, e) => true
            };
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.ConnectAsync(AppSettings.Email.Host, AppSettings.Email.Port, AppSettings.Email.UseSsl);
            await client.AuthenticateAsync(AppSettings.Email.From.Username, AppSettings.Email.From.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }



    }
}
