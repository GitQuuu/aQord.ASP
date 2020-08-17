using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using aQord.ASP.Models;
using SendGrid;
using System.Net;
using System.Configuration;
using System.Diagnostics;
using System.Net.Mail;
using System.Web.WebPages;
using SendGrid.Helpers.Mail;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;


namespace aQord.ASP
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await configSendGridasync(message);
        }


        // Use NuGet to install SendGrid (Basic C# client lib) 
        /// <summary>
        /// Authenticating up against Azure Keyvault and retrieving secrets to send email confirmation
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task configSendGridasync(IdentityMessage message)
        {


            // Authenticate and create a client to retrieve keys&Secrets from the KeyVault https://docs.microsoft.com/en-us/azure/key-vault/secrets/quick-create-net
            //string keyVaultName = Environment.GetEnvironmentVariable("KeyVaultName");
            var KeyVaultName = ConfigurationManager.AppSettings["KeyVaultName"]; // Read webconfig value and azure app settings. https://stackoverflow.com/questions/44542409/how-to-read-azure-web-site-app-settings-values , https://docs.microsoft.com/en-us/dotnet/api/system.configuration.configurationmanager.appsettings?redirectedfrom=MSDN&view=dotnet-plat-ext-3.1
            var kvUri = "https://" + KeyVaultName + ".vault.azure.net";
            var clientVault = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());


            // Send a Single Email to a Single Recipient using updated sendgrid documentation to get API credentials https://github.com/sendgrid/sendgrid-csharp/blob/master/USE_CASES.md#send-a-single-email-to-a-single-recipient

            var KeyVaultSecret = clientVault.GetSecret("SendGridApiv2").Value;
            var client = new SendGridClient(KeyVaultSecret.Value);

            // Set up email confirmation using part of an outdated documentation from microsoft https://docs.microsoft.com/en-us/aspnet/identity/overview/features-api/account-confirmation-and-password-recovery-with-aspnet-identity#examine-the-code-in-app_startidentityconfigcs

            var myMessage = new SendGridMessage();
            myMessage.AddTo(message.Destination);
            myMessage.From = new EmailAddress(
                "QuDev@Asp.net", "Qu Kops Le");
            myMessage.Subject = message.Subject;
            myMessage.PlainTextContent = message.Body;
            myMessage.HtmlContent = message.Body;

            var response = await client.SendEmailAsync(myMessage);

           
        }

        public class SmsService : IIdentityMessageService
        {
            public Task SendAsync(IdentityMessage message)
            {
                // Plug in your SMS service here to send a text message.
                return Task.FromResult(0);
            }
        }

        // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
        public class ApplicationUserManager : UserManager<ApplicationUser>
        {
            public ApplicationUserManager(IUserStore<ApplicationUser> store)
                : base(store)
            {
            }

            public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
                IOwinContext context)
            {
                var manager =
                    new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
                // Configure validation logic for usernames
                manager.UserValidator = new UserValidator<ApplicationUser>(manager)
                {
                    AllowOnlyAlphanumericUserNames = false,
                    RequireUniqueEmail = true
                };

                // Configure validation logic for passwords
                manager.PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 6,
                    RequireNonLetterOrDigit = true,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireUppercase = true,
                };

                // Configure user lockout defaults
                manager.UserLockoutEnabledByDefault = true;
                manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
                manager.MaxFailedAccessAttemptsBeforeLockout = 5;

                // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
                // You can write your own provider and plug it in here.
                manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
                {
                    MessageFormat = "Your security code is {0}"
                });
                manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
                {
                    Subject = "Security Code",
                    BodyFormat = "Your security code is {0}"
                });
                manager.EmailService = new EmailService();
                manager.SmsService = new SmsService();
                var dataProtectionProvider = options.DataProtectionProvider;
                if (dataProtectionProvider != null)
                {
                    manager.UserTokenProvider =
                        new DataProtectorTokenProvider<ApplicationUser>(
                            dataProtectionProvider.Create("ASP.NET Identity"));
                }

                return manager;
            }
        }

        // Configure the application sign-in manager which is used in this application.
        public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
        {
            public ApplicationSignInManager(ApplicationUserManager userManager,
                IAuthenticationManager authenticationManager)
                : base(userManager, authenticationManager)
            {
            }

            public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
            {
                return user.GenerateUserIdentityAsync((ApplicationUserManager) UserManager);
            }

            public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options,
                IOwinContext context)
            {
                return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(),
                    context.Authentication);
            }
        }
    }
}