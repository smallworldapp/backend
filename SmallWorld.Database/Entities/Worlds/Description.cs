using System;
using System.Collections.Generic;
using SmallWorld.Library.Validators;

namespace SmallWorld.Database.Entities
{
    public class Description : BaseEntity
    {
        [Required]
        public string Introduction { get; set; }

        [Required]
        public string Goals { get; set; }

        [Required]
        public ICollection<FaqItem> Faq { get; set; }

        public static Description Default()
        {
            var desc = new Description {
                Goals = "Through random pairings, smallworld connects community members who may have otherwise not had meaningful interactions and helps your community feel more inclusive and engaging.",
                Introduction = "smallworld strengthens communities by facilitating high-quality connections between community members. Community members simply sign up, and our software randomly pairs members for one-on-one interactions on a regular basis (every 2 weeks, every month, etc.). The frequency of meet-ups is decided by the community.\n\nCommunity members can use this page to sign up for and participate in smallworld. You can meet for coffee, lunch, or anything you want! The important thing is that you get to know people in your organization who you do not already regularly interact with.\n\nWe hope you find smallworld both useful and enjoyable!"
            };
            desc.CreateIds();

            var items = new[] {
                Tuple.Create("Why should I participate?", "First of all, because it's fun! Furthermore, connected communities are happier, more inclusive, more innovative, and more productive. We hope that you will find your community a happier and more interesting environment after getting to know more people within it."),
                Tuple.Create("What should we talk about?", "Community administrators may suggest a topic, but the topic of conversation is ultimately decided by the smallworld partners."),
                Tuple.Create("Why should I participate?", "First of all, because it's fun! Furthermore, connected communities are happier, more inclusive, more innovative, and more productive. We hope that you will find your community a happier and more interesting environment after getting to know more people within it."),
                Tuple.Create("Who initiates the meeting?", "An initiator will be randomly chosen from the pair. It is the initiator's responsibility to send the first email to set up the meeting. We’re all busy sometimes so if the initiator has not sent an email after 48 hours of receiving the pairing email, either party is encouraged to initiate the connection."),
                Tuple.Create("Where should we meet?", "Connections should take place in a location that is convenient and where community members feel comfortable talking. For example, a cafe or cafeteria close to the community may be good options. The community administrator may also have suggestions. Most connections happen over lunch or coffee, but community members may choose to go for a walk or participate in a community event together."),
                Tuple.Create("How long should meetings last?", "We suggest that connections last between thirty minutes and an hour. Some community members report longer connections, but we feel that at least thirty minutes are required to develop a high-quality connection."),
                Tuple.Create("What if I want to leave smallworld?", "You may leave smallworld at any time by using the form on this page.")
            };

            desc.Faq = new HashSet<FaqItem>();

            foreach (var tt in items)
            {
                var item = new FaqItem();
                item.CreateIds();
                item.Question = tt.Item1;
                item.Answer = tt.Item2;
                
                desc.Faq.Add(item);
            }

            return desc;
        }
    }
}