using System;
using System.Collections.Generic;
using System.Text;

namespace homework
{
    class Response  // response data to user
    {
        public int[] Arr { get; set; }          // given array
        public int[] Path { get; set; }         // most efficient path (if exists)
        public bool Winnable { get; set; }      // indicator whether path was found or not
    }
}
