using System;

namespace ML_2025.Models
{
    public class FeedbackLog
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Pergunta { get; set; } = string.Empty;
        public string RespostaHtml { get; set; } = string.Empty;

        
        public bool Gostou { get; set; }
    }
}