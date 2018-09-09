using System.Windows;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Windows.Irbis;

using ManagedIrbis.Search;

namespace UnitTests.Experiments
{
    //[TestClass]
    public class FoundPanelExperiments
    {
        //[TestMethod]
        public void TestMethod1()
        {
            FoundLine[] found =
            {
                new FoundLine { Mfn = 123, Description = "Книга 123" },
                new FoundLine { Mfn = 124, Description = "Книга 124" },
                new FoundLine { Mfn = 125, Description = "Книга 125" },
                new FoundLine { Mfn = 126, Description = "Книга 126" },
                new FoundLine { Mfn = 127, Description = "Книга 127" },
                new FoundLine { Mfn = 128, Description = "Книга 128" },
                new FoundLine { Mfn = 129, Description = "Книга 129" },
                new FoundLine { Mfn = 130, Description = "Книга 130" },
            };

            Window window = new Window
            {
                Width = 300,
                Height = 200
            };
            FoundPanel panel = new FoundPanel();
            window.Content = panel;
            panel.SetFound(found);
            window.ShowDialog();
        }
    }
}
