//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;
//using SmallWorld.Database.Entities;
//using SmallWorld.Database.Model.Abstractions;
//using SmallWorld.Database.Tests.Validation.Entities.Accounts;
//using SmallWorld.Database.Tests.Validation.Entities.CustomTypes;
//using SmallWorld.Database.Validation.Abstractions;
//using Xunit;
//
//namespace SmallWorld.Database.Tests.Validation.Entities.Worlds
//{
//    public class WorldValidatorTest : TestBase
//    {
//        public static IEnumerable<World> CreateValid()
//        {
//            var account = AccountValidatorTest.CreateValid().First();
//            var name = NameValidatorTest.CreateValid().First();
//            var id = IdentifierValidatorTest.CreateValid().First();
//            var status = WorldStatusValidatorTest.CreateValid().First();
//            var privacy = WorldPrivacyValidatorTest.CreateValid().First();
//
//            var application = ApplicationValidatorTest.CreateValid().First();
//            var description = DescriptionValidatorTest.CreateValid().First();
//
//            yield return new World {
//                Account = account,
//                Name = name,
//                Identifier = id,
//                Status = status,
//                Privacy = privacy,
//
//                BackupUser = null,
//                PairingSettings = null,
//
//                Application = application,
//                Description = description,
//            }.Init();
//
//            yield return new World {
//                Account = account,
//                Name = name,
//                Identifier = null,
//                Status = status,
//                Privacy = privacy,
//
//                BackupUser = null,
//                PairingSettings = null,
//
//                Application = application,
//                Description = description,
//            }.Init();
//
//            yield return new World {
//                Account = account,
//                Name = name,
//                Identifier = id,
//                Status = status,
//                Privacy = privacy,
//
//                BackupUser = IdentityValidatorTest.CreateValid().First(),
//                PairingSettings = null,
//
//                Application = application,
//                Description = description,
//            }.Init();
//        }
//
//        public static IEnumerable<World> CreateInvalid()
//        {
//            var account = AccountValidatorTest.CreateValid().First();
//            var name = NameValidatorTest.CreateValid().First();
//            var id = IdentifierValidatorTest.CreateValid().First();
//            var status = WorldStatusValidatorTest.CreateValid().First();
//            var privacy = WorldPrivacyValidatorTest.CreateValid().First();
//
//            var application = ApplicationValidatorTest.CreateValid().First();
//            var description = DescriptionValidatorTest.CreateValid().First();
//
//            yield return new World {
//                Account = account,
//                Name = name,
//                Identifier = id,
//                Status = status,
//                Privacy = privacy,
//
//                BackupUser = null,
//                PairingSettings = null,
//
//                Application = application,
//                Description = description,
//            };
//
//            yield return new World {
//                Account = null,
//                Name = name,
//                Identifier = id,
//                Status = status,
//                Privacy = privacy,
//
//                BackupUser = null,
//                PairingSettings = null,
//
//                Application = application,
//                Description = description,
//            }.Init();
//
//            yield return new World {
//                Account = AccountValidatorTest.CreateInvalid().First(),
//                Name = name,
//                Identifier = id,
//                Status = status,
//                Privacy = privacy,
//
//                BackupUser = null,
//                PairingSettings = null,
//
//                Application = application,
//                Description = description,
//            }.Init();
//
//            yield return new World {
//                Account = account,
//                Name = null,
//                Identifier = id,
//                Status = status,
//                Privacy = privacy,
//
//                BackupUser = null,
//                PairingSettings = null,
//
//                Application = application,
//                Description = description,
//            }.Init();
//
//            yield return new World {
//                Account = account,
//                Name = NameValidatorTest.CreateInvalid().First(),
//                Identifier = id,
//                Status = status,
//                Privacy = privacy,
//
//                BackupUser = null,
//                PairingSettings = null,
//
//                Application = application,
//                Description = description,
//            }.Init();
//
//            yield return new World {
//                Account = account,
//                Name = name,
//                Identifier = IdentifierValidatorTest.CreateInvalid().First(),
//                Status = status,
//                Privacy = privacy,
//
//                BackupUser = null,
//                PairingSettings = null,
//
//                Application = application,
//                Description = description,
//            }.Init();
//
//            yield return new World {
//                Account = account,
//                Name = name,
//                Identifier = id,
//                Status = WorldStatusValidatorTest.CreateInvalid().First(),
//                Privacy = privacy,
//
//                BackupUser = null,
//                PairingSettings = null,
//
//                Application = application,
//                Description = description,
//            }.Init();
//
//            yield return new World {
//                Account = account,
//                Name = name,
//                Identifier = id,
//                Status = status,
//                Privacy = WorldPrivacyValidatorTest.CreateInvalid().First(),
//
//                BackupUser = null,
//                PairingSettings = null,
//
//                Application = application,
//                Description = description,
//            }.Init();
//
//            yield return new World {
//                Account = account,
//                Name = name,
//                Identifier = id,
//                Status = status,
//                Privacy = privacy,
//
//                BackupUser = null,
//                PairingSettings = null,
//
//                Application = null,
//                Description = description,
//            }.Init();
//
//            yield return new World {
//                Account = account,
//                Name = name,
//                Identifier = id,
//                Status = status,
//                Privacy = privacy,
//
//                BackupUser = null,
//                PairingSettings = null,
//
//                Application = ApplicationValidatorTest.CreateInvalid().First(),
//                Description = description,
//            }.Init();
//
//            yield return new World {
//                Account = account,
//                Name = name,
//                Identifier = id,
//                Status = status,
//                Privacy = privacy,
//
//                BackupUser = null,
//                PairingSettings = null,
//
//                Application = application,
//                Description = null,
//            }.Init();
//
//            yield return new World {
//                Account = account,
//                Name = name,
//                Identifier = id,
//                Status = status,
//                Privacy = privacy,
//
//                BackupUser = null,
//                PairingSettings = null,
//
//                Application = application,
//                Description = DescriptionValidatorTest.CreateInvalid().First(),
//            }.Init();
//        }
//
//        private static World Make(
//            Optional<Account> account = default(Optional<Account>),
//            Optional<Name> name = default(Optional<Name>),
//            Optional<Identifier> id = default(Optional<Identifier>),
//            Optional<WorldStatus> status = default(Optional<WorldStatus>),
//            Optional<WorldPrivacy> privacy = default(Optional<WorldPrivacy>),
//            Optional<Identity> backupUser = default(Optional<Identity>),
//            Optional<Application> app = default(Optional<Application>),
//            Optional<Description> desc = default(Optional<Description>))
//        {
//            return new World {
//                Account = account.Or(() => AccountValidatorTest.CreateValid().First()),
//                Name = name.Or(() => NameValidatorTest.CreateValid().First()),
//                Identifier = id.Or(() => IdentifierValidatorTest.CreateValid().First()),
//                Status = status.Or(() => WorldStatusValidatorTest.CreateValid().First()),
//                Privacy = privacy.Or(() => WorldPrivacyValidatorTest.CreateValid().First()),
//                BackupUser = backupUser.Or(() => IdentityValidatorTest.CreateValid().First()),
//                Application = app.Or(() => ApplicationValidatorTest.CreateValid().First()),
//                Description = desc.Or(() => DescriptionValidatorTest.CreateValid().First()),
//            };
//        }
//
//        public static IEnumerable<object[]> ValidData => CreateValid().Select(a => new object[] { a });
//
//        [Theory]
//        [MemberData(nameof(ValidData))]
//        public async Task Valid(World value)
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
//        public async Task Invalid(World value)
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
//        public async Task InvalidBaseEntity()
//        {
//            await Invalid(new World {
//                Account = AccountValidatorTest.CreateValid().First(),
//                Name = NameValidatorTest.CreateValid().First(),
//                Identifier = IdentifierValidatorTest.CreateValid().First(),
//                Status = WorldStatusValidatorTest.CreateValid().First(),
//                Privacy = WorldPrivacyValidatorTest.CreateValid().First(),
//
//                Application = ApplicationValidatorTest.CreateValid().First(),
//                Description = DescriptionValidatorTest.CreateValid().First(),
//            });
//        }
//
//        public static MemberDataUtil<Account> InvalidAccounts = new MemberDataUtil<Account> {
//            null,
//            AccountValidatorTest.CreateInvalid(),
//        };
//
//        public static MemberDataUtil<Name> InvalidNames = new MemberDataUtil<Name> {
//            null,
//            NameValidatorTest.CreateInvalid(),
//        };
//
//        public static MemberDataUtil<Identifier> InvalidIdentifiers = new MemberDataUtil<Identifier> {
//            null,
//            IdentifierValidatorTest.CreateInvalid(),
//        };
//
//        public static MemberDataUtil<WorldStatus> InvalidStatuses = new MemberDataUtil<WorldStatus> {
//            WorldStatusValidatorTest.CreateInvalid(),
//        };
//
//        public static MemberDataUtil<WorldPrivacy> InvalidPrivacies = new MemberDataUtil<WorldPrivacy> {
//            WorldPrivacyValidatorTest.CreateInvalid(),
//        };
//
//        public static MemberDataUtil<Identity> InvalidBackupUsers = new MemberDataUtil<Identity> {
//            IdentityValidatorTest.CreateInvalid(),
//        };
//
//        public static MemberDataUtil<Application> InvalidApplications = new MemberDataUtil<Application> {
//            ApplicationValidatorTest.CreateInvalid(),
//        };
//
//        public static MemberDataUtil<Description> InvalidDescription = new MemberDataUtil<Description> {
//            DescriptionValidatorTest.CreateInvalid(),
//        };
//
//        [Theory]
//        [MemberData(nameof(InvalidAccounts))]
//        public async Task InvalidAccount(Account acc)
//        {
//            await Invalid(Make(account: acc));
//        }
//
//        [Theory]
//        [MemberData(nameof(InvalidNames))]
//        public async Task InvalidName(Name name)
//        {
//            await Invalid(Make(name: name));
//        }
//
//        [Theory]
//        [MemberData(nameof(InvalidIdentifiers))]
//        public async Task InvalidIdentifier(Identifier id)
//        {
//            await Invalid(Make(id: id));
//        }
//
//        [Theory]
//        [MemberData(nameof(InvalidStatuses))]
//        public async Task InvalidStatus(WorldStatus status)
//        {
//            await Invalid(Make(status: status));
//        }
//
//        [Theory]
//        [MemberData(nameof(InvalidPrivacies))]
//        public async Task InvalidPrivacy(WorldPrivacy privacy)
//        {
//            await Invalid(Make(privacy: privacy));
//        }
//
//        [Theory]
//        [MemberData(nameof(InvalidBackupUsers))]
//        public async Task InvalidBackupUser(Identity backup)
//        {
//            await Invalid(Make(backupUser: backup));
//        }
//
//        [Theory]
//        [MemberData(nameof(InvalidApplications))]
//        public async Task InvalidApplication(Application app)
//        {
//            await Invalid(Make(app: app));
//        }
//
//        [Theory]
//        [MemberData(nameof(InvalidDescription))]
//        public async Task InvalidDescriptions(Description desc)
//        {
//            await Invalid(Make(desc: desc));
//        }
//    }
//}
