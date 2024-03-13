using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudOSLib
{
    public class CloudEventHandle<T>
    {
        List<CloudEvent<T>> events = new();

        public void AddListener(CloudEvent<T> listener) 
        { 
            events.Add(listener);
        }

        public void InvokeAll(T arg)
        {
            foreach (CloudEvent<T> listener in events) { listener.Invoke(arg); }
        }
    }
}
