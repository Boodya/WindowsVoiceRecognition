using System.Globalization;
using System.Speech.Synthesis;

namespace SpeechProcessor.Generation
{
    public static class VoiceGenerator
    {
        private readonly static SpeechSynthesizer _synthesizer = new SpeechSynthesizer();
        public static void GenerateVoice(string text)
        {
            if (ContainsRussian(text))
            {
                GenerateVoice(text, "ru-RU");
                return;
            }
            GenerateVoice(text, CultureInfo.CurrentCulture.Name);
        }

        private static void GenerateVoice(string text, string culture)
        {
            var installedVoices = _synthesizer.GetInstalledVoices();
            var voice = installedVoices.FirstOrDefault(x =>
                x.VoiceInfo.Culture.Name == culture);
            voice = voice ?? installedVoices[0];

            _synthesizer.SelectVoice(voice.VoiceInfo.Name);
            _synthesizer.Rate = 2;

            var manualResetEvent = new ManualResetEvent(false);
            _synthesizer.SpeakCompleted += (sender, args) =>
            {
                manualResetEvent.Set();
            };
            _synthesizer.SpeakAsync(text);
            manualResetEvent.WaitOne();
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

        internal static void StopGenerate()
        {
            _synthesizer.SpeakAsyncCancelAll();
        }
    }
}
