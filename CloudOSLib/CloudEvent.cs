using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudOSLib
{
    public class CloudEvent<T>
    {
        Action<T> eventhd;

        /// <summary>
        /// Makes new Event handler with args.
        /// </summary>
        /// <param name="action">Event handler.</param>
        public CloudEvent(Action<T> action)
        {
            eventhd = action;
        }

        public void Invoke(T arg)
        {
            eventhd.Invoke(arg);
        }
    }
}
