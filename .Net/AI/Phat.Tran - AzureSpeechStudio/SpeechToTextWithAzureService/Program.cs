// See https://aka.ms/new-console-template for more information
using SpeechToTextWithAzureService;

Console.WriteLine("Hello, World!");
//await SpeechToTextService.RecognizeSpeechSimpleAsync();
await SpeechToTextService.RecognizeSpeechRealtimeAsync();
