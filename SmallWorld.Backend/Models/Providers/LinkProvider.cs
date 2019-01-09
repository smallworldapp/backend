using SmallWorld.Database.Entities;

namespace SmallWorld.Models.Providers
{
    public class LinkProvider
    {
        private readonly SmallWorldOptions options;

        public LinkProvider(SmallWorldOptions options)
        {
            this.options = options;
        }

        public string AdminConsole => $"{options.Host}/login";

        public string Feedback(Pair pair, string outcome)
        {
            return $"{options.Host}/feedback/{pair.Guid}?response={outcome}";
        }

        public string PasswordReset(Account acc)
        {
            return $"{options.Host}/login/password-reset/{acc.ResetToken?.Guid}";
        }

        public string JoinConfirmation(Member member)
        {
            return $"{options.Host}/verify/{member.JoinToken}";
        }

        public string LeaveConfirmation(Member member)
        {
            return $"{options.Host}/verify/{member.LeaveToken}";
        }

        public string InvitePage(World world)
        {
            return $"{options.Host}/{world.Guid}";
        }

        public string OptOut(Member member)
        {
            return $"{options.Host}/opt-out?world={member.World.Guid}&member={member.Guid}";
        }
    }
}
