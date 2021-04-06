﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.LearningMode.Config
{
    public class LearningSettings
    {
        public int NumOfLevels { get; set; } = 2;

        public bool ResetAfterMistake { get; set; } = false;

        public PronunciationSettings PlayPronuncation { get; set; } = new PronunciationSettings();
    }
}
