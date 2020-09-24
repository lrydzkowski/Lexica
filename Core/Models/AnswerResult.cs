﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.Core.Models
{
    class AnswerResult
    {
        public bool Result { get; private set; }

        public List<string> PossibleAnswers { get; private set; }

        public AnswerResult(bool result, List<string> possibleAnswers)
        {
            Result = result;
            PossibleAnswers = possibleAnswers;
        }
    }
}
