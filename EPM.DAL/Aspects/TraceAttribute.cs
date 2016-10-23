namespace DBSoft.EPM.DAL.Aspects
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using NLog;
    using PostSharp.Aspects;

    [Serializable]
    public class TraceAttribute : OnMethodBoundaryAspect
    {
        private string _methodName;
        private DateTime _start;
        private const int DefaultExecutionTime = 1000;

        private double MinimumExecutionTimeToTrace { get; set; }

        public TraceAttribute(int minimumExecutionTimeToLog = 0)
        {
            AspectPriority = 1;
            MinimumExecutionTimeToTrace = minimumExecutionTimeToLog != 0 ? minimumExecutionTimeToLog : DefaultExecutionTime;
        }

        /// <summary> 
        /// Method executed at build time. Initializes the aspect instance. After the execution 
        /// of <see cref="CompileTimeInitialize"/>, the aspect is serialized as a managed  
        /// resource inside the transformed assembly, and deserialized at runtime. 
        /// </summary> 
        /// <param name="method">Method to which the current aspect instance  
        /// has been applied.</param> 
        /// <param name="aspectInfo">Unused.</param> 
        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            Debug.Assert(method.DeclaringType != null, "method.DeclaringType != null");
            _methodName = method.DeclaringType.FullName + "." + method.Name;
        }

        /// <summary> 
        /// Method invoked before the execution of the method to which the current 
        /// aspect is applied. 
        /// </summary> 
        /// <param name="args">Unused.</param> 
        public override void OnEntry(MethodExecutionArgs args)
        {
            _start = DateTime.Now;
        }

        /// <summary> 
        /// Method invoked after successfull execution of the method to which the current 
        /// aspect is applied. 
        /// </summary> 
        /// <param name="args">Unused.</param> 
        public override void OnSuccess(MethodExecutionArgs args)
        {
            var elapsed = DateTime.Now - _start;
            var logger = LogManager.GetLogger(_methodName);
            if (elapsed.Seconds > MinimumExecutionTimeToTrace / 1000)
            {
                logger.Trace("{0} Execution Time: {1}", _methodName, elapsed);
            }
        }
    }
}
