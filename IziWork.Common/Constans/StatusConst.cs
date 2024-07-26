using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Common.Constans
{
    public static class StatusConst
    {
        public static readonly string NEW = "New";
        public static readonly string COMPLETED = "Completed";
        public static readonly string REJECTED = "Rejected";
        public static readonly string REQUEST_TO_CHANGE = "Request To Change";
        public static readonly string CANCELLED = "Cancelled";
        public static class Document
        {
            public static string NEW = "New";
        }
        public static class Task
        {
            public static string NEW = "New";
            public static string VIEWED = "Viewed";
            public static string IN_PROCESS = "In Process";
            public static string REPORTING = "Reporting";
            public static string COMPLETED = "Completed";
        }

        public static class WORKFLOW
        {
            public static string WAITING_FOR_SUBMIT = "Waiting for submit";
        }

        public static string SetStatus(string status)
        {
            return !string.IsNullOrEmpty(status) ? string.Format("Wating for {0} approval", status) : string.Format("Wating for approval");
        }
    }
}
