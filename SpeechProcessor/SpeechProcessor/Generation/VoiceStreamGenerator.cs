using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace SpeechProcessor.Generation
{
    public class VoiceStreamGenerator
    {
        private string _voiceInput;
        private readonly object _locker = new object();
        private bool _isPlaying;
        public VoiceStreamGenerator()
        {
            _voiceInput = "";
            _isPlaying = false;
        }

        public void AddToQueue(string text)
        {
            lock(_locker)
            {
                _voiceInput += text;
            }
            if(!_isPlaying && Regex.IsMatch(_voiceInput, @"[\p{P}\p{S}]"))
            {
                _isPlaying = true;
                Task.Run(() =>
                {
                    while (!string.IsNullOrEmpty(_voiceInput))
                    {
                        var textToPlay = "";
                        lock (_locker)
                        {
                            textToPlay = _voiceInput;
                            _voiceInput = "";
                        }
                        if (string.IsNullOrEmpty(textToPlay))
                            continue;
                        VoiceGenerator.GenerateVoice(textToPlay);
                    }
                    _isPlaying = false;
                });
            }
        }

        public void StopGenerate()
        {
            if (_voiceInput.Length > 0)
                _voiceInput = "";
            if (_isPlaying)
                VoiceGenerator.StopGenerate();
        }
    }
}
