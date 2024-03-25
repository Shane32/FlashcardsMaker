using Shane32.EasyPDF;

namespace FlashcardMaker;

public static class Library
{
    public static Stream PrintToPdf(List<string> wordList, int columns = 2, int rows = 7, float fontSize = 50f)
    {
        using var pdf = new PDFWriter();
        pdf.ScaleMode = ScaleModes.Inches;
        pdf.NewPage(System.Drawing.Printing.PaperKind.Letter, false);
        pdf.Font = new Font(StandardFonts.Helvetica, fontSize);
        var fontCapHeight = pdf.TextCapHeight(); // height between the baseline and the top of the capital letters
        var fontXHeight = fontCapHeight * .72f; // height between the baseline and the top of the lowercase letters
        pdf.TextAlignment = TextAlignment.CenterBaseline;
        var pageWidth = pdf.PageSize.Width;
        var pageHeight = pdf.PageSize.Height;
        var columnWidth = pageWidth / columns;
        var rowHeight = pageHeight / rows;

        var word = 0;
        while (true)
        {
            PrintCutMarks();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    if (word >= wordList.Count)
                    {
                        return pdf.ToStream();
                    }
                    WriteWord(wordList[word++], col, row);
                }
            }
            if (word >= wordList.Count)
            {
                return pdf.ToStream();
            }
            pdf.NewPage(System.Drawing.Printing.PaperKind.Letter, false);
        }

        void WriteWord(string word, int col, int row)
        {
            var lineWidth = columnWidth * .9f;
            var x = col * columnWidth + (columnWidth - lineWidth) / 2f;
            var y = row * rowHeight + (rowHeight - fontCapHeight) / 2f + fontCapHeight;
            pdf.MoveTo(x, y).LineTo(lineWidth, 0);
            pdf.MoveTo(x, y - fontCapHeight).LineTo(lineWidth, 0);
            pdf.LineStyle.DashStyle = new LineDashStyle(20f, 0);
            pdf.MoveTo(x, y - fontXHeight).LineTo(lineWidth, 0);
            pdf.LineStyle.DashStyle = LineDashStyle.Solid;
            x = col * columnWidth + columnWidth / 2;
            pdf.MoveTo(x, y).Write(word);
        }

        void PrintCutMarks()
        {
            for (int row = 0; row < (rows + 1); row++)
            {
                for (int col = 0; col < (columns + 1); col++)
                {
                    var tickSize = 0.25f;
                    if (row == 0 || row == rows || col == 0 || col == columns)
                    {
                        tickSize = 0.5f;
                    }
                    var x = col * columnWidth;
                    var y = row * rowHeight;
                    pdf.MoveTo(x - (tickSize / 2), y).LineTo(tickSize, 0);
                    pdf.MoveTo(x, y - (tickSize / 2)).LineTo(0, tickSize);
                }
            }
        }
    }
}
