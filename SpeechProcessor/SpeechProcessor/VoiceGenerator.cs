using System.Globalization;
using System.Speech.Synthesis;

namespace SpeechProcessor
{
    public static class VoiceGenerator
    {
        public static void GenerateVoice(string text)
        {
            if(ContainsRussian(text))
                GenerateVoice(text, "ru-RU");
            GenerateVoice(text, CultureInfo.CurrentCulture.Name);
        }

        private static void GenerateVoice(string text, string culture)
        {
            using (SpeechSynthesizer synthesizer = new SpeechSynthesizer())
            {
                var installedVoices = synthesizer.GetInstalledVoices();
                var voice = installedVoices.FirstOrDefault(x => 
                    x.VoiceInfo.Culture.Name == culture);
                voice = voice ?? installedVoices[0];

                synthesizer.SelectVoice(voice.VoiceInfo.Name);
                synthesizer.Rate = 2;
                synthesizer.Speak(text);
            }
        }

        private static bool ContainsRussian(string input)
        {
            foreach (char c in input)
            {
                if (c >= '\u0400' && c <= '\u04FF')
                {
                    return true;
                }
            }
            return false;
        }
    }
}
