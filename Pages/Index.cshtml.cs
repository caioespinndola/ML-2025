using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.ML;
using ML_2025.Models; 

public class IndexModel : PageModel
{
    private readonly PredictionEngine<SentimentData, SentimentPrediction> _predictionEngine;

    public IndexModel(PredictionEngine<SentimentData, SentimentPrediction> predictionEngine)
    {
        _predictionEngine = predictionEngine;
    }

    [BindProperty]
    public string InputText { get; set; }


    public SentimentPrediction PredictionResult { get; set; }

    public void OnGet()
    {
    }


    public IActionResult OnPost()
    {
        if (string.IsNullOrWhiteSpace(InputText))
        {
            return Page(); 
        }
        var inputData = new SentimentData { Text = InputText };
        PredictionResult = _predictionEngine.Predict(inputData);

        return Page();
    }
}