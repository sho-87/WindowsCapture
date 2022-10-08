using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsCapture
{
    public class WindowsCaptureSessionOptions
    {
        public int MinFrameInterval { get; set; }

        public bool IsManual { get; set; } = true;
    }
}
