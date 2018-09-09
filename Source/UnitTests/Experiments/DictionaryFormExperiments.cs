using System.Windows;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

using AM.Windows.Irbis;

using ManagedIrbis.Search;

namespace UnitTests.Experiments
{
    //[TestClass]
    public class DictionaryFormExperiments
    {
        //[TestMethod]
        public void TestMethod1()
        {
            TermInfo[] terms =
            {
                new TermInfo{Count = 1, Text = "БЕТОН"},
                new TermInfo{Count = 1, Text = "БЕТОНИРОВАНИЕ"},
                new TermInfo{Count = 1, Text = "БЕТОНОМЕШАЛКА"},
                new TermInfo{Count = 1, Text = "БЕТОНЩИК"},
                new TermInfo{Count = 1, Text = "ВАТА"},
                new TermInfo{Count = 1, Text = "ВАТАНИРОВАНИЕ"},
                new TermInfo{Count = 1, Text = "ВАТАМЕШАЛКА"},
                new TermInfo{Count = 1, Text = "ВАТОНЩИК"},
                new TermInfo{Count = 1, Text = "ГРАВИЙ"},
                new TermInfo{Count = 1, Text = "ГРАВИРОВАНИЕ"},
                new TermInfo{Count = 1, Text = "ГРАВИМЕШАЛКА"},
                new TermInfo{Count = 1, Text = "ГРАВИРОВЩИК"},
                new TermInfo{Count = 1, Text = "ДОСКА"},
                new TermInfo{Count = 1, Text = "ДОСКИРОВАНИЕ"},
                new TermInfo{Count = 1, Text = "ДОСКОМЕШАЛКА"},
                new TermInfo{Count = 1, Text = "ДОСОЧНИК"},
            };

            DictionaryForm form = new DictionaryForm();
            form.SetTerms(terms);
            form.ShowDialog();
        }
    }
}
