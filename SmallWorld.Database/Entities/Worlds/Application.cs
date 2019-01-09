using System;
using System.Collections.Generic;
using SmallWorld.Library.Validators;

namespace SmallWorld.Database.Entities
{
    public class Application : BaseEntity
    {
        [Required]
        public ICollection<ApplicationQuestion> Questions { get; set; }

        public static Application Default()
        {
            var app = new Application();
            app.CreateIds();

            var questions = new[] {
                Tuple.Create(
                    "Describe the community for which you are applying to smallworld (size, connectedness, sub-groups or hierarchies, etc.).",
                    ""),
                Tuple.Create(
                    "What is your role within the community?",
                    ""),
                Tuple.Create(
                    "Please state why you want to use smallworld in your community and the benefits you hope to achieve through the use of smallworld.",
                    "This question will be shared with your community on the sign up page"),
                Tuple.Create(
                    "smallworld collect data on connections that are distributed and that are completed. Do you have any additional suggested metrics for measuring the goals outlined above?",
                    ""),
                Tuple.Create(
                    "Describe the method you plan to use to present smallworld to your community (email, community presentation, small group meetings, etc.) and any specific details about how you plan to present smallworld. Will participation be required in your community? How will you guide smallworld discussions/actvities?",
                    ""),
                Tuple.Create(
                    "Additional comments.",
                    "")
            };

            app.Questions = new HashSet<ApplicationQuestion>();

            foreach (var tt in questions)
            {
                var que = new ApplicationQuestion();
                que.CreateIds();

                que.Question = tt.Item1;
                que.Subtext = tt.Item2;
                que.Answer = "";

                app.Questions.Add(que);
            }

            return app;
        }
    }
}