using System;
using System.Collections.Generic;

namespace FirstAspNetApp.Models
{
    public partial class Feedback
    {
        public int FeedbackId { get; set; }
        public string? FeedbackName { get; set; }
        public string? FeedbackStory { get; set; }
    }
}