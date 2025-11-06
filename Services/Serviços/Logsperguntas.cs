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
            // O nome do nosso NOVO arquivo!
            _filePath = Path.Combine(env.ContentRootPath, "historico_uteis.json");
        }

        public async Task AdicionarLogUtilAsync(string pergunta, string respostaHtml)
        {
            var novoLog = new FeedbackLog
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                Pergunta = pergunta ?? string.Empty,
                RespostaHtml = respostaHtml ?? string.Empty
            };

            List<FeedbackLog> logs = new List<FeedbackLog>();

            lock (_fileLock)
            {
                // 1. Ler o arquivo antigo (se existir)
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
                        logs = new List<FeedbackLog>(); // Ignora arquivo corrompido
                    }
                }

                // 2. Adicionar o novo log à lista
                logs.Add(novoLog);

                // 3. Salvar a lista atualizada
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(logs, options);
                File.WriteAllText(_filePath, jsonString);
            }

            await Task.CompletedTask;
        }
    }
}