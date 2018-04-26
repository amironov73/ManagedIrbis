using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

using AM.Configuration;

using AngleSharp;
using AngleSharp.Dom;
using IElement=AngleSharp.Dom.IElement;

using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Image = iText.Layout.Element.Image;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// ReSharper disable LocalizableElement

namespace GrabPrLib
{
    public class Page
    {
        [JsonProperty("m")]
        public int MaxZoom { get; set; }

        [JsonProperty("d")]
        public Dimension[] Dimensions { get; set; }

        [JsonProperty("f")]
        public string FileName { get; set; }
    }

    public class Dimension
    {
        [JsonProperty("h")]
        public float Height { get; set; }

        [JsonProperty("w")]
        public float Width { get; set; }
    }

    class Program
    {
        private static Url _pageUrl;
        private static string _pdfFileName;
        private static IConfiguration _browsingConfiguration;
        private static string _serverUrl;
        private static string _imageDir;
        private static IBrowsingContext _browsingContext;
        private static WebClient _webClient;
        private static Page[] _pages;
        private static int _scale = 4;
        private static int _sleep = 3000;

        static bool DownloadPage
            (
                int pageNumber,
                Page page
            )
        {
            const int tileWidth = 256;
            const int tileHeight = 256;

            string fileName = page.FileName;
            Console.Write($"Page {pageNumber}) {fileName} => ");
            if (File.Exists(fileName))
            {
                Console.WriteLine("have");
                return false;
            }

            int zoom = Math.Min(_scale, page.MaxZoom);
            string url = $"{_serverUrl}?FIF={_imageDir}/{fileName}&JTL={zoom},";
            int width = (int)page.Dimensions[zoom].Width;
            int height = (int)page.Dimensions[zoom].Height;
            int colCount = (width + tileWidth - 1) / tileWidth;
            int rowCount = (height + tileHeight - 1) / tileHeight;
            int frameNumber = 0;

            using (Bitmap pageImage = new Bitmap(width, height))
            using (Graphics graphics = Graphics.FromImage(pageImage))
            {
                for (int row = 0; row < rowCount; row++)
                {
                    for (int col = 0; col < colCount; col++)
                    {
                        string address = url + frameNumber;
                        byte[] bytes = _webClient.DownloadData(address);
                        Stream stream = new MemoryStream(bytes);
                        Bitmap tile = new Bitmap(stream);
                        graphics.DrawImageUnscaled
                            (
                                tile,
                                tileWidth * col,
                                tileHeight * row
                            );
                        tile.Dispose();

                        Console.Write(".");
                        frameNumber++;
                    }
                }

                pageImage.Save(fileName);
            }

            Console.WriteLine(" done");

            return true;
        }

        static void BuildDocument()
        {
            Console.Write("Building PDF ");
            PdfWriter writer = new PdfWriter(_pdfFileName);
            PdfDocument pdf = new PdfDocument(writer);
            int index = 1;
            using (Document document = new Document(pdf))
            {
                document.SetMargins(0, 0, 0, 0);
                foreach (Page page in _pages)
                {
                    string fileName = page.FileName;
                    Console.Write($"[{index}] {fileName} ");
                    Image image = new Image(ImageDataFactory.Create(fileName));
                    document.Add(image);
                    document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                    index++;
                }
            }
            Console.WriteLine("done");

            Console.WriteLine("Deleting images");
            index = 1;
            foreach (Page page in _pages)
            {
                string fileName = page.FileName;
                Console.Write($"[{index}] {fileName} ");
                File.Delete(fileName);
                index++;
            }

            Console.WriteLine("done");
        }

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: grab <url>");
                return;
            }

            try
            {
                _sleep = ConfigurationUtility.GetInt32("sleep", 3000);
                Console.WriteLine($"Sleep={_sleep}ms");
                _scale = ConfigurationUtility.GetInt32("scale", 4);
                Console.WriteLine($"Scale={_scale}x");

                _pageUrl = Url.Create(args[0]);

                _pdfFileName = Path.ChangeExtension
                    (
                        Path.GetFileNameWithoutExtension(_pageUrl.Path),
                        ".pdf"
                    );

                _browsingConfiguration = Configuration.Default.WithDefaultLoader();
                _browsingContext = BrowsingContext.New(_browsingConfiguration);
                IDocument document = _browsingContext.OpenAsync(_pageUrl).Result;
                Console.WriteLine("HTML downloaded");
                string selector = "script";
                IHtmlCollection<IElement> scripts = document.QuerySelectorAll(selector);
                Console.WriteLine($"Scripts: {scripts.Length}");
                IElement scriptElement = scripts.First
                    (
                        e => e.InnerHtml.StartsWith("jQuery.extend")
                    );
                string json = scriptElement.InnerHtml;
                int offset = json.IndexOf('{');
                int length = json.LastIndexOf('}') - offset + 1;
                json = json.Substring(offset, length);
                JObject root = JObject.Parse(json);
                JValue token = (JValue)root.SelectToken("$.diva.1.options.objectData");
                if (ReferenceEquals(token, null))
                {
                    Console.WriteLine("Not a book");
                    return;
                }
                string dataUrl = (string)token.Value;
                Console.WriteLine($"Data URL={dataUrl}");
                token = (JValue)root.SelectToken("$.diva.1.options.iipServerURL");
                if (ReferenceEquals(token, null))
                {
                    Console.WriteLine("No ServerUrl");
                    return;
                }
                _serverUrl = (string) token.Value;
                Console.WriteLine($"Server URL={_serverUrl}");
                token = (JValue)root.SelectToken("$.diva.1.options.imageDir");
                if (ReferenceEquals(token, null))
                {
                    Console.WriteLine("No imageDir");
                    return;
                }
                _imageDir = (string)token.Value;
                Console.WriteLine($"Image dir={_imageDir}");
                _webClient = new WebClient();
                json = _webClient.DownloadString(dataUrl);
                root = JObject.Parse(json);
                JArray array = (JArray)root.SelectToken("pgs");
                _pages = array.ToObject<Page[]>();
                Console.WriteLine($"Total pages={_pages.Length}");
                int pageNumber = 1;
                foreach (Page page in _pages)
                {
                    if (DownloadPage(pageNumber, page))
                    {
                        Thread.Sleep(_sleep);
                    }

                    pageNumber++;
                }
                BuildDocument();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

        }
    }
}
