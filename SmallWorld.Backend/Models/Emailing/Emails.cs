using SmallWorld.Models.Emailing.Templates;

namespace SmallWorld.Models.Emailing
{
    public static class Emails
    {
        public class GenericReference<T> { }

        public static GenericReference<DebugEmail> Debug = new GenericReference<DebugEmail>();

        public static GenericReference<AccountCreationEmail> AccountCreation = new GenericReference<AccountCreationEmail>();
        public static GenericReference<WorldAcceptedEmail> WorldAccepted = new GenericReference<WorldAcceptedEmail>();
        public static GenericReference<PasswordResetEmail> PasswordReset = new GenericReference<PasswordResetEmail>();

        public static GenericReference<MemberAddedEmail> MemberAdded = new GenericReference<MemberAddedEmail>();

        public static GenericReference<JoinConfirmationEmail> JoinConfirmation = new GenericReference<JoinConfirmationEmail>();
        public static GenericReference<LeaveConfirmationEmail> LeaveConfirmation = new GenericReference<LeaveConfirmationEmail>();

        public static GenericReference<PairingInitiatorEmail> PairingInitiator = new GenericReference<PairingInitiatorEmail>();
        public static GenericReference<PairingReceiverEmail> PairingReceiver = new GenericReference<PairingReceiverEmail>();
        public static GenericReference<UnpairedMemberEmail> UnpairedMember = new GenericReference<UnpairedMemberEmail>();

        public static GenericReference<ManualPairingEmail> ManualPairing = new GenericReference<ManualPairingEmail>();
        public static GenericReference<OptOutEmail> OptOut = new GenericReference<OptOutEmail>();
    }
}
