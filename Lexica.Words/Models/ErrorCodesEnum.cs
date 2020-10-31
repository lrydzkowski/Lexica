using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Words.Models
{
    public enum ErrorCodesEnum
    {
        NoImportSource = 12001,
        FileIsEmpty = 12002,
        NoSemicolon = 12003,
        TooManySemicolons = 12004,
        TooLongWord = 12005,
        TooLongTranslation = 12006,
        TooShortWord = 12007,
        TooShortTranslation = 12008
    }
}
