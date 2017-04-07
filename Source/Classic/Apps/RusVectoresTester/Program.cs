using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM.AOT.Distributional;

namespace RusVectoresTester
{
    class Program
    {
        static void Main(string[] args)
        {
#if CLASSIC

            if (args.Length == 0)
            {
                Console.WriteLine("RusVectoresTester word");
                return;
            }

            try
            {

                string word = args[0];
                RusVectoresClient client = new RusVectoresClient();
                WordInfo[] nearest = client.GetNearestWords
                    (
                        ModelName.RusCorpora,
                        word
                    );
                foreach (WordInfo oneWord in nearest)
                {
                    Console.WriteLine
                        (
                            "{0} ({1}): {2}",
                            oneWord.Word,
                            oneWord.PartOfSpeech,
                            oneWord.Value
                        );
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

#endif
        }
    }
}
