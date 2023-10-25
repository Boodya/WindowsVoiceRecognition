namespace SpeechProcessor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            VoiceGenerator.GenerateVoice("Test Voice In English");
            VoiceGenerator.GenerateVoice("Тест голоса на русском");
            var recognizer = new VoiceRecognition((text) =>
            {
                Console.Write(text);
                VoiceGenerator.GenerateVoice(text);
            });
            recognizer.StartRecognition();
            Console.ReadLine();
        }
    }
}