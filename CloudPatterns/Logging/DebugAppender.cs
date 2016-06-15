using log4net.Appender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;

namespace CloudPatterns.Logging
{
    public class DebugAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            string renderedMessage = RenderLoggingEvent(loggingEvent);

            // Add debug code here as needed.
        }

        protected override bool RequiresLayout
        {
            get
            {
                return true;
            }
        }
    }
}
