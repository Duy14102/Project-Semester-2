using System;
using System.Collections.Generic;

namespace FirstAspNetApp.Models
{
    public partial class Feedback
    {
        public int FeedbackId { get; set; }
        public string? FeedbackName { get; set; }
        public string? FeedbackStory { get; set; }
        public string? FeedbackLink { get; set; }
        public DateTime FeedbackDate { get; set; }
    }
}