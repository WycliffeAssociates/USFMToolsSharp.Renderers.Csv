using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using USFMToolsSharp.Models.Markers;

namespace USFMToolsSharp.Renderers.Csv
{
    public class CsvRenderer
    {
        /// <summary>
        /// Render a USFMDocument to a CSV
        /// </summary>
        /// <param name="document">The document to render</param>
        /// <param name="stream">A stream to write the resulting CSV to</param>
        public void RenderCSV(USFMDocument document, Stream stream)
        {
            using (var writer = new StreamWriter(stream))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.CurrentCulture))
                {
                    WriteHeader(csv);
                    WriteContent(document, csv);
                }
            }
        }

        /// <summary>
        /// Write headers to a csv file
        /// </summary>
        /// <param name="csv">The CSV writer to write the headers to</param>
        private static void WriteHeader(CsvWriter csv)
        {
            csv.WriteField("Book");
            csv.WriteField("Chapter");
            csv.WriteField("Verse");
            csv.WriteField("Text");
            csv.NextRecord();
        }

        /// <summary>
        /// Write the contents of the usfm document to a CSV
        /// </summary>
        /// <param name="document">The document whose contents to write</param>
        /// <param name="csv">The CSV writer to write the content to</param>
        private static void WriteContent(USFMDocument document, CsvWriter csv)
        {
            var book = document.GetChildMarkers<TOC3Marker>().First().BookAbbreviation;
            var chapters = document.GetChildMarkers<CMarker>();
            foreach (var chapter in chapters)
            {
                var verses = chapter.GetChildMarkers<VMarker>();
                foreach (var verse in verses)
                {
                    var textBlocks = verse.GetChildMarkers<TextBlock>()
                        .Where(m => !document.GetHierarchyToMarker(m).Any(i => i is FMarker));
                    var text = string.Join(string.Empty, textBlocks.Select(b => b.Text));
                    csv.WriteField(book);
                    csv.WriteField(chapter.Number);
                    csv.WriteField(verse.VerseNumber);
                    csv.WriteField(text.TrimEnd());
                    csv.NextRecord();
                }
            }
        }

        /// <summary>
        /// Render multiple USFMDocument to a CSV
        /// </summary>
        /// <param name="documents">The documents to render</param>
        /// <param name="stream">A stream to write the resulting CSV to</param>
        public void RenderCSV(IEnumerable<USFMDocument> documents, Stream stream)
        {
            using (var writer = new StreamWriter(stream))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.CurrentCulture))
                {
                    WriteHeader(csv);
                    foreach(var document in documents)
                    {
                        WriteContent(document, csv);
                    }
                }
            }

        }
    }
}
