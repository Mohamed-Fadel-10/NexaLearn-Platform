using Microsoft.ML;
using Microsoft.ML.Data;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

public class TextSimilarityService: ITextSimilarityService
{
    private readonly MLContext _mlContext;

    public TextSimilarityService()
    {
        _mlContext = new MLContext();
    }

    public double CalculateCosineSimilarity(string correctAnswer, string studentAnswer)
    {
        var data = new List<TextData>
        {
            new TextData { Text = correctAnswer },
            new TextData { Text = studentAnswer }
        };

        var dataView = _mlContext.Data.LoadFromEnumerable(data);

        var pipeline = _mlContext.Transforms.Text
            .FeaturizeText("Features", nameof(TextData.Text));

        var transformedData = pipeline.Fit(dataView).Transform(dataView);

        var features = transformedData.GetColumn<float[]>("Features").ToArray();

        return CosineSimilarity(features[0], features[1]);
    }

    private double CosineSimilarity(float[] vectorA, float[] vectorB)
    {
        var dotProduct = vectorA.Zip(vectorB, (a, b) => a * b).Sum();
        var magnitudeA = Math.Sqrt(vectorA.Sum(a => a * a));
        var magnitudeB = Math.Sqrt(vectorB.Sum(b => b * b));

        return dotProduct / (magnitudeA * magnitudeB);
    }

    private class TextData
    {
        public string Text { get; set; }
    }
}
