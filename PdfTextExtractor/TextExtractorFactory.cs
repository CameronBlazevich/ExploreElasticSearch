using System;
using System.Collections.Generic;

namespace PdfTextExtractor
{
    public class TextExtractorFactory : ITextExtractorFactory
    {
        private Dictionary<FileType,ITextExtractor> _textExtractors = new Dictionary<FileType, ITextExtractor>
        {
            {FileType.Pdf, new PdfTextExtractor()},
            {FileType.Text, new TextFileTextExtractor()}
        };
        
        public ITextExtractor GetTextExtractor(string filePath)
        {
            var fileType = FileTypeResolver.GetFileType(filePath);

            if (_textExtractors.TryGetValue(fileType, out var textExtractor))
            {
                return textExtractor;    
            }

            throw new Exception("AHHHHHHH, not text extractor found");
        }
    }
}