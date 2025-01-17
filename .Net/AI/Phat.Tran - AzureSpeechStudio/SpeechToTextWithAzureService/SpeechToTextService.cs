using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace SpeechToTextWithAzureService
{
    public static class SpeechToTextService
    {
        private static SpeechConfig GetSpeechConfig()
        {
            var yourSubscription = "";
            var region = "eastus";
            return SpeechConfig.FromSubscription(yourSubscription, region);
        }

        public static async Task RecognizeSpeechSimpleAsync()   
        {
            var speechConfig = GetSpeechConfig();
            using (var recognizer = new SpeechRecognizer(speechConfig))
            {
                Console.WriteLine("Say something...");
                var result = await recognizer.RecognizeOnceAsync();

                switch (result.Reason)
                {
                    // Check the result
                    case ResultReason.RecognizedSpeech:
                        Console.WriteLine($"Recognized: {result.Text}");
                        break;

                    case ResultReason.NoMatch:
                        Console.WriteLine("No speech could be recognized.");
                        break;

                    case ResultReason.Canceled:
                        {
                            var cancellation = CancellationDetails.FromResult(result);
                            Console.WriteLine($"Speech Recognition canceled. Reason: {cancellation.Reason}");

                            if (cancellation.Reason == CancellationReason.Error)
                            {
                                Console.WriteLine($"Error details: {cancellation.ErrorDetails}");
                            }

                            break;
                        }
                }
            }
        }

        public static async Task RecognizeSpeechRealtimeAsync()
        {
            var speechConfig = GetSpeechConfig();

            var audio = AudioConfig.FromDefaultMicrophoneInput();
            var autoDetectSourceLanguageConfig = AutoDetectSourceLanguageConfig.FromLanguages(new string[] { "en-US", "vi-VN" });
            using (var recognizer = new SpeechRecognizer(speechConfig, autoDetectSourceLanguageConfig, audio))
            {
                await recognizer.StartContinuousRecognitionAsync();
                Console.WriteLine("Say something in English or Vietnamese...");

                recognizer.Recognizing += (s, e) =>
                {
                    Console.WriteLine($"RECOGNIZING: Text={e.Result.Text}");
                };

                recognizer.Recognized += (s, e) =>
                {
                    if (e.Result.Reason == ResultReason.RecognizedSpeech)
                    {
                        var autoDetectSourceLanguageResult = AutoDetectSourceLanguageResult.FromResult(e.Result);
                        var detectedLanguage = autoDetectSourceLanguageResult.Language;

                        Console.WriteLine($"RECOGNIZED in '{detectedLanguage}': Text={e.Result.Text}");
                    }
                    else if (e.Result.Reason == ResultReason.NoMatch)
                    {
                        Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                    }
                };

                Console.ReadKey();
                await recognizer.StopContinuousRecognitionAsync();
            }
        }
    }
}
