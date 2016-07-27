using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Management.Automation;
using System.Xml;

namespace WpfInteraction
{
    [Serializable()]
    public class PsWpfInvocationResult
    {
        private bool _stopInvoked = false;
        public bool StopInvoked { get { return _stopInvoked; } }
        
        Collection<PSObject> _output = null;
        public Collection<PSObject> Output { get { return _output; } }
        
        private Hashtable _resultState;
        public Hashtable ResultState { get { return _resultState; } }
        
        private Hashtable _userState;
        public Hashtable UserState { get { return _userState; } }
        
        private XmlDocument _xaml;
        public XmlDocument Xaml { get { return _xaml; } }
        
        private ErrorRecord _terminalError = null;
        public ErrorRecord TerminalError { get { return _terminalError; } set { _terminalError = value; } }
        
        Collection<ErrorRecord> _errors = new Collection<ErrorRecord>();
        public Collection<ErrorRecord> Errors { get { return _errors; } }
        
        Collection<WarningRecord> _warnings = new Collection<WarningRecord>();
        public Collection<WarningRecord> Warnings { get { return _warnings; } }
        
        Collection<VerboseRecord> _verbose = new Collection<VerboseRecord>();
        public Collection<VerboseRecord> Verbose { get { return _verbose; } }
        
        Collection<DebugRecord> _debug = new Collection<DebugRecord>();
        public Collection<DebugRecord> Debug { get { return _debug; } }
        
        public PsWpfInvocationResult() : this(null) { }
        
        public PsWpfInvocationResult(Hashtable resultState) : this(null, null, null) { }
        
        public PsWpfInvocationResult(Hashtable resultState, XmlDocument xaml, Hashtable userState)
        {
            _xaml = xaml;
            _resultState = (resultState == null) ? new Hashtable() : resultState;
            _userState = (userState == null) ? new Hashtable() : userState;
        }
        
        public static void AddRange<T>(Collection<T> target, IEnumerable<T> source)
        {
            if (source == null)
                return;
                
            foreach (T obj in source)
                target.Add(obj);
        }
        
        public static Collection<T> ReadAll<T>(Collection<T> target, PSDataCollection<T> source) { return ReadAll<T>(target, source, false); }
        public static Collection<T> ReadAll<T>(Collection<T> target, PSDataCollection<T> source, bool purge)
        {
            Collection<T> result = (source == null) ? new Collection<T>() : new Collection<T>(source.ReadAll());
            if (!purge)
                AddRange<T>(target, result);
            else if (target.Count > 0)
                target.Clear();
            
            return result;
        }
        
        public void Initialize(PowerShell powerShell, IAsyncResult asyncResult, bool stop)
        {
            try
            {
                if (!asyncResult.IsCompleted)
                {
                    _stopInvoked = stop;
                    if (stop)
                        try { powerShell.Stop(); } catch (Exception e) { AddError(_errors, e, "Unexpected exception while stopping the background PowerShell process."); }
                    _output = new Collection<PSObject>(powerShell.EndInvoke(asyncResult));
                }
            }
            catch (Exception exception)
            {
                _output = new Collection<PSObject>();
                try { ReadAll<ErrorRecord>(_errors, powerShell.Streams.Error); } catch (Exception e) { AddError(_errors, e, "Unexpected exception while reading errors."); }
                try { ReadAll<WarningRecord>(_warnings, powerShell.Streams.Warning); } catch (Exception e) { AddError(_errors, e, "Unexpected exception while reading warnings."); }
                try { ReadAll<VerboseRecord>(_verbose, powerShell.Streams.Verbose); } catch (Exception e) { AddError(_errors, e, "Unexpected exception while reading verbose messages."); }
                try { ReadAll<DebugRecord>(_debug, powerShell.Streams.Debug); } catch (Exception e) { AddError(_errors, e, "Unexpected exception while reading debug messages."); }
                _terminalError = AddError(_errors, exception);
            }
        }
        
        public static ErrorRecord AddError(Collection<ErrorRecord> target, Exception exception, string message, string errorId, ErrorCategory errorCategory, object targetObject)
        {
            if (errorId == null || (errorId = errorId.Trim()).Length == 0)
                errorId = String.Format("WpfInteraction.{0}", errorCategory.ToString("F"));
            ErrorRecord errorRecord;
            if (message == null || (message = message.Trim()) == "")
                errorRecord = new ErrorRecord(exception, errorId, errorCategory, targetObject);
            else
            {
                errorRecord = new ErrorRecord(exception, errorId, errorCategory, targetObject);
                errorRecord.ErrorDetails = new ErrorDetails(message);
            }
            target.Add(errorRecord);
            return errorRecord;
        }
        public static ErrorRecord AddError(Collection<ErrorRecord> target, Exception exception, string message, string errorId, ErrorCategory errorCategory) { return AddError(target, exception, message, errorId, errorCategory, null); }
        public static ErrorRecord AddError(Collection<ErrorRecord> target, Exception exception, string errorId, ErrorCategory errorCategory, object targetObject) { return AddError(target, exception, null, errorId, errorCategory, targetObject); }
        public static ErrorRecord AddError(Collection<ErrorRecord> target, Exception exception, string errorId, ErrorCategory errorCategory) { return AddError(target, exception, null, errorId, errorCategory, null); }
        public static ErrorRecord AddError(Collection<ErrorRecord> target, Exception exception, string message, object targetObject)
        {
            if (exception == null)
                return AddError(target, exception, message, null, ErrorCategory.NotSpecified, targetObject);
                
            if (exception is IContainsErrorRecord)
            {
                ErrorRecord errorRecord = (exception as IContainsErrorRecord).ErrorRecord;
                if (errorRecord != null && errorRecord.CategoryInfo != null)
                    return AddError(target, exception, message, null, errorRecord.CategoryInfo.Category, targetObject);
            }
            if (exception is RuntimeException && exception.InnerException != null)
                return AddError(target, exception.InnerException, message, targetObject);
            
            if (exception is ArgumentException || exception is IndexOutOfRangeException)
                return AddError(target, exception, message, null, ErrorCategory.InvalidArgument, targetObject);
            
            if (exception is XmlException || exception is System.Xml.Schema.XmlSchemaException)
                return AddError(target, exception, message, null, ErrorCategory.ParserError, targetObject);
            if (exception is InvalidOperationException || exception is NotSupportedException)
                return AddError(target, exception, message, null, ErrorCategory.InvalidOperation, targetObject);
            
            if (exception is NotImplementedException)
                return AddError(target, exception, message, null, ErrorCategory.NotImplemented, targetObject);
            
            return AddError(target, exception, message, null, ErrorCategory.NotSpecified, targetObject);
        }
        public static ErrorRecord AddError(Collection<ErrorRecord> target, Exception exception, string message) { return AddError(target, exception, message, null); }
        public static ErrorRecord AddError(Collection<ErrorRecord> target, Exception exception, object targetObject) { return AddError(target, exception, null as string, targetObject); }
        public static ErrorRecord AddError(Collection<ErrorRecord> target, Exception exception) { return AddError(target, exception, null as string); }
    }
}