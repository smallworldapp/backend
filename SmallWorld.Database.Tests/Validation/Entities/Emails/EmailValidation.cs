using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Tests.Validation.Test_Helpers;
using Xunit;
using Xunit.Abstractions;

namespace SmallWorld.Database.Tests.Validation.Entities.Emails
{
    public class EmailValidation : ValidationBase<Email>
    {
        public static ISource<Email> Source = new EmailFactory();

        private class EmailFactory : Factory<Email>
        {
            public Source<DateTime> Created = Test_Helpers.Source.OrDefault(DateTime.Today);
            public Source<string> Subject = Test_Helpers.Source.OrDefault("Subject");
            public Source<string> Body = Test_Helpers.Source.OrDefault("Body");

            public Source<ICollection<EmailRecipient>> Recipients = new Source<ICollection<EmailRecipient>>(
                EmailRecipientValidation.Source.Valid().Select(r => new HashSet<EmailRecipient> { r }),
                EmailRecipientValidation.Source.Invalid().Select(r => new HashSet<EmailRecipient> { r })
            );

            protected override IEnumerable<Email> ValidBase()
            {
                foreach (var email in base.ValidBase())
                {
                    email.IsSent = false;
                    email.Sent = null;
                    yield return email;

                    email.IsSent = true;
                    email.Sent = DateTime.UtcNow;
                    yield return email;
                }
            }

            protected override IEnumerable<Email> InvalidBase()
            {
                foreach (var email in base.ValidBase())
                {
                    email.IsSent = true;
                    email.Sent = null;
                    yield return email;

                    email.IsSent = false;
                    email.Sent = DateTime.UtcNow;
                    yield return email;
                }
            }
        }

        public EmailValidation(ITestOutputHelper output) : base(output) { }

        [Theory]
        [FactoryData(typeof(EmailFactory), true)]
        public override Task Validate_True(Email value) => base.Validate_True(value);

        [Theory]
        [FactoryData(typeof(EmailFactory), false)]
        public override Task Validate_False(Email value) => base.Validate_False(value);
        //        public static IEnumerable<Email> CreateValid()
        //        {
        //            yield return new Email {
        //                Created = DateTime.UtcNow,
        //                Subject = "Test email",
        //                Body = "Test body",
        //                Recipients = new HashSet<EmailRecipient> {
        //                    new EmailRecipient {
        //                        Name = new Name("Test"),
        //                        Address = new EmailAddress("test@mfro.me"),
        //                    }.Init()
        //                },
        //            }.Init();
        //        }
        //
        //        public static IEnumerable<Email> CreateInvalid()
        //        {
        //            yield return new Email {
        //                Created = DateTime.UtcNow,
        //                Subject = null,
        //                Body = "Test body",
        //                Recipients = new HashSet<EmailRecipient> {
        //                    new EmailRecipient {
        //                        Name = new Name("Test"),
        //                        Address = new EmailAddress("test@mfro.me"),
        //                    }.Init()
        //                },
        //            }.Init();
        //        }
        //
        //        public static IEnumerable<object[]> ValidData => CreateValid().Select(a => new object[] { a });
        //        public static IEnumerable<object[]> InvalidData => CreateInvalid().Select(a => new object[] { a });
        //
        //        [Theory]
        //        [MemberData(nameof(ValidData))]
        //        public async Task Valid(Email value)
        //        {
        //            using (var provider = CreateProvider())
        //            using (var scope = provider.CreateScope())
        //            {
        //                var access = scope.ServiceProvider.GetService<IContextLock>();
        //                var validation = scope.ServiceProvider.GetService<IValidationContext>();
        //
        //                using (await access.Read())
        //                {
        //                    Assert.True(validation.Validate(value, out var _));
        //                }
        //            }
        //        }
        //
        //        [Theory]
        //        [MemberData(nameof(InvalidData))]
        //        public async Task Invalid(Email value)
        //        {
        //            using (var provider = CreateProvider())
        //            using (var scope = provider.CreateScope())
        //            {
        //                var access = scope.ServiceProvider.GetService<IContextLock>();
        //                var validation = scope.ServiceProvider.GetService<IValidationContext>();
        //
        //                using (await access.Read())
        //                {
        //                    Assert.False(validation.Validate(value, out var _));
        //                }
        //            }
        //        }
        //
        //        [Fact]
        //        public async Task ValidEmail()
        //        {
        //            using (var provider = CreateProvider())
        //            using (var scope = provider.CreateScope())
        //            {
        //                var access = scope.ServiceProvider.GetService<IContextLock>();
        //                var validation = scope.ServiceProvider.GetService<IValidationContext>();
        //
        //                using (await access.Read())
        //                {
        //                    var email = CreateValid();
        //                    Assert.True(validation.Validate(email, out var _));
        //                }
        //            }
        //        }
        //
        //        [Fact]
        //        public async Task InvalidCreated()
        //        {
        //            using (var provider = CreateProvider())
        //            using (var scope = provider.CreateScope())
        //            {
        //                var access = scope.ServiceProvider.GetService<IContextLock>();
        //                var validation = scope.ServiceProvider.GetService<IValidationContext>();
        //
        //                using (await access.Read())
        //                {
        //                    var email = CreateValid();
        //                    email.Created = default(DateTime);
        //                    Assert.False(validation.Validate(email, out var _), "default created");
        //                }
        //            }
        //        }
        //
        //        [Fact]
        //        public async Task InvalidSubject()
        //        {
        //            using (var provider = CreateProvider())
        //            using (var scope = provider.CreateScope())
        //            {
        //                var access = scope.ServiceProvider.GetService<IContextLock>();
        //                var validation = scope.ServiceProvider.GetService<IValidationContext>();
        //
        //                using (await access.Read())
        //                {
        //                    var email = CreateValid();
        //                    email.Subject = null;
        //                    Assert.False(validation.Validate(email, out var _), "Null subject");
        //                }
        //
        //                using (await access.Read())
        //                {
        //                    var email = CreateValid();
        //                    email.Subject = "";
        //                    Assert.False(validation.Validate(email, out var _), "Empty subject");
        //                }
        //            }
        //        }
        //
        //        [Fact]
        //        public async Task InvalidBody()
        //        {
        //            using (var provider = CreateProvider())
        //            using (var scope = provider.CreateScope())
        //            {
        //                var access = scope.ServiceProvider.GetService<IContextLock>();
        //                var validation = scope.ServiceProvider.GetService<IValidationContext>();
        //
        //                using (await access.Read())
        //                {
        //                    var email = CreateValid();
        //                    email.Body = null;
        //                    Assert.False(validation.Validate(email, out var _), "Null body");
        //                }
        //
        //                using (await access.Read())
        //                {
        //                    var email = CreateValid();
        //                    email.Body = "";
        //                    Assert.False(validation.Validate(email, out var _), "Empty body");
        //                }
        //            }
        //        }
        //
        //        [Fact]
        //        public async Task InvalidRecipient()
        //        {
        //            using (var provider = CreateProvider())
        //            using (var scope = provider.CreateScope())
        //            {
        //                var access = scope.ServiceProvider.GetService<IContextLock>();
        //                var validation = scope.ServiceProvider.GetService<IValidationContext>();
        //
        //                using (await access.Read())
        //                {
        //                    var email = CreateValid();
        //                    email.Recipients.First().Name = null;
        //                    Assert.False(validation.Validate(email, out var _), "Null name");
        //                }
        //
        //                using (await access.Read())
        //                {
        //                    var email = CreateValid();
        //                    email.Recipients.First().Name = new Name("");
        //                    Assert.False(validation.Validate(email, out var _), "Empty name");
        //                }
        //
        //                using (await access.Read())
        //                {
        //                    var email = CreateValid();
        //                    email.Recipients.First().Address = null;
        //                    Assert.False(validation.Validate(email, out var _), "Null address");
        //                }
        //
        //                using (await access.Read())
        //                {
        //                    var email = CreateValid();
        //                    email.Recipients.First().Address = new EmailAddress("");
        //                    Assert.False(validation.Validate(email, out var _), "Empty address");
        //                }
        //            }
        //        }
        //
        //        [Fact]
        //        public async Task InvalidSent()
        //        {
        //            using (var provider = CreateProvider())
        //            using (var scope = provider.CreateScope())
        //            {
        //                var access = scope.ServiceProvider.GetService<IContextLock>();
        //                var validation = scope.ServiceProvider.GetService<IValidationContext>();
        //
        //                using (await access.Read())
        //                {
        //                    var email = CreateValid();
        //                    email.IsSent = true;
        //                    Assert.False(validation.Validate(email, out var _), "IsSent without Sent");
        //                }
        //
        //                using (await access.Read())
        //                {
        //                    var email = CreateValid();
        //                    email.Sent = DateTime.UtcNow;
        //                    Assert.False(validation.Validate(email, out var _), "Sent without IsSent");
        //                }
        //            }
        //        }
    }
}
