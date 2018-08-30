using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Inventory.Configuraion;
using Inventory.Data;
using Inventory.Models;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Inventory.Services
{
    public class EmailNotifyService : INotifyService
    {
        private readonly SmtpConfig _smtpConfig;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailNotifyService> _logger;

        public EmailNotifyService(IConfiguration configuration, IOptions<SmtpConfig> smtpConfig, ILogger<EmailNotifyService> logger)
        {
            _configuration = configuration;
            _smtpConfig = smtpConfig.Value;
            _logger = logger;
        }

        public async Task Expired()
        {
            List<string> products;
            // We can not inject ApplicationDbContext
            // It has Scoped DI scope, Quartz uses Singleton
            var options = new DbContextOptionsBuilder().UseSqlServer(_configuration.GetConnectionString("DefaultConnection")).Options;
            using (var dbContext = new ApplicationDbContext(options))
            {
                products = await dbContext.Products
                    .Where(x => x.ExpiryDate.Value.Date == DateTime.UtcNow.Date)
                    .Select(x => $"{x.Id} {x.Name} {x.Type} {x.ExpiryDate:d}").ToListAsync();
            }

            if (products.Any())
            {
                var body = string.Join(Environment.NewLine, products);
                await SendEmailAsync("Products expired", body);
            }
        }

        public async Task ProductSold(Product product)
        {
            await SendEmailAsync("Product sold", $"{product.Id} {product.Name} {product.Type} {product.ExpiryDate:d}");
        }

        private async Task SendEmailAsync(string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Inventory", _smtpConfig.User));
                message.To.Add(new MailboxAddress("Inventory", _smtpConfig.User));
                message.Subject = subject;
                message.Body = new TextPart("plain")
                {
                    Text = body
                };

                using (var client = new SmtpClient())
                {
                    client.Connect(_smtpConfig.Server, _smtpConfig.Port, _smtpConfig.UseSsl);
                    client.Authenticate(_smtpConfig.User, _smtpConfig.Pass);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "SendEmailAsync() exception");
                Console.WriteLine(ex);
            }
        }
    }
}
