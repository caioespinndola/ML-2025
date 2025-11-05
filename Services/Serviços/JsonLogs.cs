// /Services/JsonLogService.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json; // <--- Verifique se você tem este 'using'
using System.Threading.Tasks;
using ML_2025.Models;
using Microsoft.AspNetCore.Hosting;

namespace ML_2025.Services
{
    public class JsonLogService
    {
        private readonly string _filePath;
        
        private static readonly object _fileLock = new object();

        public JsonLogService(IWebHostEnvironment env)
        {
            
            _filePath = Path.Combine(env.ContentRootPath, "historico_perguntas.json");
        }

        public async Task AdicionarLogAsync(string pergunta)
        {
            var novoLog = new PerguntaLog
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                Pergunta = pergunta ?? string.Empty 
            };

            List<PerguntaLog> logs = new List<PerguntaLog>();

           
            lock (_fileLock)
            {
                
                if (File.Exists(_filePath))
                {
                    try
                    {
                        
                        string existingJson = File.ReadAllText(_filePath);
                        if (!string.IsNullOrWhiteSpace(existingJson))
                        {
                            logs = JsonSerializer.Deserialize<List<PerguntaLog>>(existingJson) ?? new List<PerguntaLog>();
                        }
                    }
                    catch (JsonException)
                    {
                        
                        
                        logs = new List<PerguntaLog>();
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