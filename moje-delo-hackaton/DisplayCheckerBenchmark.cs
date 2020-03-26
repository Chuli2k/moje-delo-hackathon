using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace moje_delo_hackaton
{
    public class DisplayCheckerBenchmark
    {
        class InputData
        {
            public int width;
            public int height;
            public IList<int> wordLengths;
        }

        private List<InputData> inputs = new List<InputData>();
        public string inputFilePath = "vhodi-bench.txt";
        public DisplayChecker checker = new DisplayChecker();

        public DisplayCheckerBenchmark()
        {
            if (!File.Exists(inputFilePath))
            {
                throw new FileNotFoundException();
            }

            var lines = File.ReadAllLines(inputFilePath);
            foreach (var line in lines)
            {
                var (width, height, wordLengths) = Helper.ParseLine(line);
                inputs.Add(new InputData() { 
                    width = width,
                    height = height,
                    wordLengths = wordLengths
                });
            }
        }

        [Benchmark]
        public List<int> GetMaxFontSize()
        {
            var r = new List<int>();
            foreach (var i in inputs)
            {
                r.Add(checker.GetMaxFontSize(i.width, i.height, i.wordLengths));
            }

            return r;
        }

        [Benchmark]
        public List<int> GetMaxFontSizeAlt()
        {
            var r = new List<int>();
            foreach (var i in inputs)
            {
                r.Add(checker.GetMaxFontSizeAlt(i.width, i.height, i.wordLengths));
            }

            return r;
        }
    }
}
