﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Lexica.Core.Extensions
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do rng.GetBytes(box);
                while (box[0] >= n * (Byte.MaxValue / n));
                int k = (box[0] % n);
                n--;
                (list[n], list[k]) = (list[k], list[n]);
            }
        }
    }
}
