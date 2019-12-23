using System;
using System.Collections.Generic;
using System.Text;

namespace testFileUpload.Core
{
    public class Types
    {
        public enum CSV
        {
            Approved,
            Failed,
            Finished
        }

        public enum XML
        {
            Approved,
            Rejected,
            Done,
        }

        public enum Json
        {
            A,
            R,
            D
        }
    }
}
