// /Services/JsonLogService.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using ML_2025.Models;
using Microsoft.AspNetCore.Hosting; // Precisa disso para saber onde salvar

namespace ML_2025.Services
{
    public class JsonLogService
    {
        private readonly string _filePath;
        // Usamos um 'lock' para evitar problemas se duas perguntas chegarem ao mesmo tempo
        private static readonly object _fileLock = new object();

        public JsonLogService(IWebHostEnvironment env)
        {
            // Isso salvará o arquivo na pasta principal do seu projeto (ex: ao lado de Program.cs)
            // O nome do arquivo será "historico_perguntas.json"
            _filePath = Path.Combine(env.ContentRootPath, "historico_perguntas.json");
        }

        public async Task AdicionarLogAsync(string pergunta)
        {
            var novoLog = new PerguntaLog
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                Pergunta = pergunta
            };

            List<PerguntaLog> logs = new List<PerguntaLog>();

            // 'lock' garante que não tentemos ler e escrever no arquivo ao mesmo tempo
            // de requisições diferentes
            lock (_fileLock)
            {
                // 1. Ler o arquivo JSON antigo (se existir)
                if (File.Exists(_filePath))
                {
                    using (var stream = File.OpenRead(_filePath))
                    {
                        if (stream.Length > 0)
                        {
                            logs = JsonSerializer.Deserialize<List<PerguntaLog>>(stream) ?? new List<PerguntaLog>();
                        }
                    }
                }

                // 2. Adicionar a nova pergunta à lista
                logs.Add(novoLog);

                // 3. Salvar a lista atualizada de volta no arquivo JSON
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(logs, options);

                File.WriteAllText(_filePath, jsonString);
            }

            // Usamos 'await Task.CompletedTask' só para manter o método assíncrono
            // A operação de arquivo é síncrona dentro do 'lock' para ser mais segura
            await Task.CompletedTask;
        }
    }
}