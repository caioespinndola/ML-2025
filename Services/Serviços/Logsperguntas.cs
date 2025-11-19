using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using ML_2025.Models;
using Microsoft.AspNetCore.Hosting;

namespace ML_2025.Services
{
    public class FeedbackLogService
    {
        private readonly string _filePath;
        private static readonly object _fileLock = new object();

        public FeedbackLogService(IWebHostEnvironment env)
        {
            _filePath = Path.Combine(env.ContentRootPath, "historico_uteis.json");
        }

        // Mudei a assinatura para aceitar 'bool gostou'
        public async Task AdicionarLogFeedbackAsync(string pergunta, string respostaHtml, bool gostou)
        {
            var novoLog = new FeedbackLog
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                Pergunta = pergunta ?? string.Empty,
                RespostaHtml = respostaHtml ?? string.Empty,
                Gostou = gostou // Salva se foi Like ou Dislike
            };

            List<FeedbackLog> logs = new List<FeedbackLog>();

            lock (_fileLock)
            {
                if (File.Exists(_filePath))
                {
                    try
                    {
                        string existingJson = File.ReadAllText(_filePath);
                        if (!string.IsNullOrWhiteSpace(existingJson))
                        {
                            logs = JsonSerializer.Deserialize<List<FeedbackLog>>(existingJson) ?? new List<FeedbackLog>();
                        }
                    }
                    catch (JsonException)
                    {
                        logs = new List<FeedbackLog>();
                    }
                }

                logs.Add(novoLog);

                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(logs, options);
                File.WriteAllText(_filePath, jsonString);
            }

            await Task.CompletedTask;
        }
    }
}