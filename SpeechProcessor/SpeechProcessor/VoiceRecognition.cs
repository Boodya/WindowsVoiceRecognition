using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition;

namespace SpeechProcessor
{
    public class VoiceRecognition : IDisposable
    {
        private readonly SpeechRecognitionEngine _recognizer;
        private readonly Action<string> _recognitionHandler;
        public VoiceRecognition(Action<string> recognitionHandler)
        {
            _recognitionHandler = recognitionHandler;
            _recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("ru-RU"));
            _recognizer.SetInputToDefaultAudioDevice();
            // Create a grammar for recognizing text
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            grammarBuilder.AppendDictation();
            Grammar grammar = new Grammar(grammarBuilder);
            // Load the grammar and start recognition
            _recognizer.LoadGrammar(grammar);
            _recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
        }
        public void Dispose()
        {
            _recognizer.Dispose();
        }

        public void StartRecognition()
        {
            _recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        public void StopRecognition()
        {
            _recognizer.RecognizeAsyncStop();
        }

        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            _recognitionHandler.Invoke(e.Result.Text);
        }
    }
}
